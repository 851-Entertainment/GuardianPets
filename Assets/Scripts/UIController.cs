using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Soomla.Store;

public class UIController : MonoBehaviour
{
    #region Upgrades
    public Button m_BowTieButton;
    public Button m_BallButton;
    public Button m_HatButton;
    public Button m_SunGlassesButton;
    public Button m_GloveButton;
    public Button m_TrophyButton;
    public Button m_ScannerButton;

    public GameObject m_Scanner;
    public GameObject m_BowTie;
    public GameObject m_Ball;
    public GameObject m_Hat;
    public GameObject m_SunGlasses;
    public GameObject m_Glove;
    public GameObject m_Trophy;

    public int m_BowTieCost;
    public int m_BallCost;
    public int m_HatCost;
    public int m_SunGlassesCost;
    public int m_GloveCost;
    public int m_TrophyCost;
    public int m_ScannerCost;

    public Text m_BowTieText;
    public Text m_BallText;
    public Text m_HatText;
    public Text m_SunGlassesText;
    public Text m_GloveText;
    public Text m_TrophyText;
    public Text m_ScannerText;

    public AudioClip m_UpgradeClip;
    #endregion

    public GameObject m_GameUI;
    public GameObject m_NewPlayerUI;
    public GameObject m_NicknamePanel; //Panel for nickname prompt
    public GameObject m_FearPanel; //Panel for the fear prompt
    public GameObject m_CurrentPet; //UI Element for the current pet
    public GameObject m_UpgradePanel; //Panel for the upgrades
    public GameObject m_CameraPlane; //Plane which is drawing the camera on it
    public GameObject m_RadarOverlay; //Image overlay which will have a radar with a rotating arm on it
    public GameObject m_BackgroundPlane; //Plane which is drawing the background on it
    public GameObject m_StorePanel; //Panel for the market
    public GameObject m_GoodsButtonPrefab; //Button prefab for the items sold in the store
    public GameObject m_ButtonParent; //Parent game object for the button prefab
    public InputField m_NicknameIF; //Input field for the nickname prompt
    public InputField m_FearIF; //Input field for the fear prompt
    public PlayerData m_PlayerData; //Player data

    public Text m_NicknameText; //Text element for the pet's nickname;
    public Text m_TitleText; //Text element for the pet's current title
    public Text m_EnergyText; //Player's current points - UI element
    public Text m_ShieldsText; //Player's current shields = UI element
    public Text m_EnergyTimerText; //Text element for the Points - UI element
    public Slider m_EnergySlider; //Slider element for the Energy UI
    public GameObject m_CloseScannerButton;
    public string m_SelectedPet;
    public float m_TransitionVolume = 0.3f;

    public AudioClip m_TransitionSound; 

    private GameObject currPet_;
    private GameController gc_; //Game Controller script for easier access
    private AudioSource audio_;
    private PlayerData playerData_;

    private bool playCloseSound_ = true;
    private bool isNewPlayer_;
    private bool scannerActive_;
    private float energyTimer_; //Timer until the player receives their next set of points, starts at 300 because the interval is 5 minutes, and there are 300 seconds in 5 minutes
    private float scannerTimer_; //This is the timer that once it reaches the max, will turn off the camera access and return to the normal screen
    private string minutes_;
    private string seconds_;

	void Start () 
    {
        audio_ = GetComponent<AudioSource>();
        gc_ = Camera.main.GetComponent<GameController>();
        isNewPlayer_ = gc_.m_FirstTimePlayer;
        playerData_ = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        if (isNewPlayer_)
        {
            m_GameUI.SetActive(false);
            m_NewPlayerUI.SetActive(true);
        }
        else
        {
            RemoveUI();
        }

        energyTimer_ = Constants.ENERGY_TIMER;
        m_EnergySlider.minValue = 0;
        m_EnergySlider.maxValue = Constants.DEFAULT_MAX_ENERGY;
	}
	
	void Update ()
    {
        currPet_ = gc_.ActivePet;
        if (currPet_ != null)
        {
            m_NicknameText.text = currPet_.GetComponent<Pet>().m_Nickname;
            m_TitleText.text = "The " + currPet_.GetComponent<Pet>().m_FearOne + " Destroyer";
            m_EnergyText.text = "Energy: " + m_PlayerData.m_Energy.ToString() + "/" + Constants.DEFAULT_MAX_ENERGY;
            m_ShieldsText.text = m_PlayerData.m_Shields.ToString();
            m_EnergySlider.value = m_PlayerData.m_Energy;
            if(m_PlayerData.m_Energy == 0)
            {
                m_EnergySlider.fillRect.GetComponent<Image>().color = Color.black;
            }
            UpdateTimer();
        }
        m_CameraPlane.GetComponent<CameraAccess>().UpdateCamera();
	}

    void UpdateTimer()
    {
        if (gc_.m_PlayerData.m_Energy < Constants.DEFAULT_MAX_ENERGY)
        {
            m_EnergyTimerText.enabled = true;
            energyTimer_ -= Time.deltaTime;
            minutes_ = Mathf.Floor(energyTimer_ / 60).ToString("00");
            seconds_ = (energyTimer_ % 60).ToString("00");
            m_EnergyTimerText.text = minutes_ + ":" + seconds_;

            if (energyTimer_ <= 0.0f)
            {
                gc_.m_PlayerData.m_Energy += Constants.ENERGY_REWARDED;
                energyTimer_ = Constants.ENERGY_TIMER;
            }
        }
        else
        {
            m_EnergyTimerText.enabled = false;
        }

        if(scannerActive_)
        {
            scannerTimer_ += Time.deltaTime;
            if (scannerTimer_ >= Constants.MAX_SCANNER_TIME - 1.0f)
            {              
                if (!audio_.isPlaying && playCloseSound_)
                {
                    playCloseSound_ = false;
                    m_CloseScannerButton.SetActive(true);
                    audio_.PlayOneShot(m_TransitionSound, m_TransitionVolume);
                }
            }
            if(scannerTimer_ >= Constants.MAX_SCANNER_TIME)
            {
                scannerTimer_ = 0.0f;
            }
        }
    }

    void RemoveUI()
    {
        Destroy(m_NewPlayerUI);
    }

    //Button function -- If the player is new, this button will be used to select their first pet
    //                -- After the player has selected their pet, it will prompt to give them a nickname
    public void SelectPet(GameObject btn)
    {
        m_SelectedPet = btn.name;
        gc_.CurrentPet = m_SelectedPet;
        gc_.m_PlayerData.AddPet(m_SelectedPet);
        m_NicknamePanel.SetActive(true);
        gc_.SetUpGame();
    }

    //Button function -- When the player presses the "Done" button after typing in a nickname, this function fires
    //                -- The function will set the pet's nickname and activate the fear panel
    public void GiveNickname()
    {
        if(!string.IsNullOrEmpty(m_NicknameIF.text) && currPet_ != null)
        {
            currPet_.GetComponent<Pet>().m_Nickname = m_NicknameIF.text;
        }
        else if (string.IsNullOrEmpty(m_NicknameIF.text))
        {
            currPet_.GetComponent<Pet>().m_Nickname = currPet_.GetComponent<Pet>().m_PetName;
        }

        m_NicknamePanel.SetActive(false);
        m_FearPanel.SetActive(true);
    }

    //Button function -- When the player presses the "Done" button after typing in a fear, this function fires
    //                -- The function will remove the new player UI from the game, turn on the Game UI and start setting up the game itself
    public void AssignFear()
    {
        if(!string.IsNullOrEmpty(m_FearIF.text) && currPet_ != null)
        {
            currPet_.GetComponent<Pet>().m_FearOne = m_FearIF.text;
            currPet_.GetComponent<Pet>().m_Bored = Constants.DEFAULT_START_STATS;
            currPet_.GetComponent<Pet>().m_Cleanliness = Constants.DEFAULT_START_STATS;
            currPet_.GetComponent<Pet>().m_Hunger = Constants.DEFAULT_START_STATS;
            gc_.Save();
            Destroy(m_NewPlayerUI);
            m_GameUI.SetActive(true);
        }
    }

    //Button function -- When the player presses the Shield, it will fire this function
    //                -- This function will give a visual display of the Upgrades available to them
    public void Upgrades()
    {
        m_UpgradePanel.SetActive(true);
    }

    //Button function -- When the player presses the feed button on the UI it will call this function
    //                -- This function will decrease the hunger level of the pet, remove the appropriate points from the player, and award them shields
    public void Feed()
    {
        if (gc_.m_PlayerData.m_Energy >= Constants.ACTION_COST)
        {
            currPet_.GetComponent<Pet>().m_Hunger -= Constants.STAT_DECREASE_VAL;
            if (currPet_.GetComponent<Pet>().m_Hunger <= Constants.MIN_PET_STAT)
            {
                currPet_.GetComponent<Pet>().m_Hunger = Constants.MIN_PET_STAT;
                if(currPet_.GetComponent<Pet>().CheckShieldConditions())
                {
                    gc_.m_PlayerData.m_Shields += Constants.SHIELDS_REWARDED;
                }
                
            }
            gc_.m_PlayerData.RemoveEnergy();
        }
    }

    //Button function -- When the player presses the play button on the UI it will call this function
    //                -- This function will decrease the boredom level of the pet, remove the appropriate points from the player, and award them shields
    public void Play()
    {
        if (gc_.m_PlayerData.m_Energy >= Constants.ACTION_COST)
        {
            currPet_.GetComponent<Pet>().m_Bored -= Constants.STAT_DECREASE_VAL;
            if (currPet_.GetComponent<Pet>().m_Bored <= Constants.MIN_PET_STAT)
            {
                currPet_.GetComponent<Pet>().m_Bored = Constants.MIN_PET_STAT;
                if (currPet_.GetComponent<Pet>().CheckShieldConditions())
                {
                    gc_.m_PlayerData.m_Shields += Constants.SHIELDS_REWARDED;
                }
            }
            gc_.m_PlayerData.RemoveEnergy();
        }
    }

    //Button function -- When the player presses the clean button on the UI it will call this function
    //                -- This function will decrease the cleanliness level of the pet, remove the appropriate points from the player, and award them shields
    public void Clean()
    {
        if (gc_.m_PlayerData.m_Energy >= Constants.ACTION_COST)
        {
            currPet_.GetComponent<Pet>().m_Cleanliness -= Constants.STAT_DECREASE_VAL;
            if (currPet_.GetComponent<Pet>().m_Cleanliness <= Constants.MIN_PET_STAT)
            {
                currPet_.GetComponent<Pet>().m_Cleanliness = Constants.MIN_PET_STAT;
                if (currPet_.GetComponent<Pet>().CheckShieldConditions())
                {
                    gc_.m_PlayerData.m_Shields += Constants.SHIELDS_REWARDED;
                }
            }
            gc_.m_PlayerData.RemoveEnergy();
        }
    }

    //Button function -- When the player presses the 'X' button on the UI it will call this function
    //                -- This function will close the currently active window
    //                -- As of right now it's only being used for the upgrade panel
    public void CloseMenu()
    {
        if (m_UpgradePanel.activeSelf)
        {
            m_UpgradePanel.SetActive(false);
        }
        else if (m_StorePanel.activeSelf)
        {
            m_StorePanel.SetActive(false);
            CleanUpMenu();
        }
    }

    public void OpenScanner()
    {
        scannerActive_ = true;
        m_CameraPlane.GetComponent<CameraAccess>().m_DisableWebCam = false;
        m_GameUI.SetActive(false);
        m_CameraPlane.SetActive(true);
        m_CameraPlane.GetComponent<CameraAccess>().enabled = true;
        m_RadarOverlay.SetActive(true);
        m_BackgroundPlane.SetActive(false);
        currPet_.SetActive(false);
       
    }

    public void CloseScanner()
    {
        scannerActive_ = false;
        m_CameraPlane.GetComponent<CameraAccess>().m_DisableWebCam = true;
        m_GameUI.SetActive(true);
        m_CameraPlane.GetComponent<CameraAccess>().enabled = false;
        m_CameraPlane.SetActive(false);
        m_RadarOverlay.SetActive(false);
        m_BackgroundPlane.SetActive(true);
        currPet_.SetActive(true);
        m_CloseScannerButton.SetActive(false);
        playCloseSound_ = true;
    }

    void CleanUpMenu()
    {
        foreach (Transform child in m_ButtonParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void PopulateStore()
    {
        float buttonWidth = m_GoodsButtonPrefab.GetComponent<RectTransform>().sizeDelta.x * 4;
        float buttonHeight = m_GoodsButtonPrefab.GetComponent<RectTransform>().sizeDelta.y;
        float startXPos = -300.0f;
        float startYPos = 0.0f;
        int row = 0;
        int col = 0;
        int maxCol = 2;
        int maxRow = 3;

        m_StorePanel.SetActive(true);
        Debug.Log(StoreInfo.Goods.Count);
        foreach (VirtualCurrencyPack vcp in StoreInfo.CurrencyPacks)
        {
            GameObject go = (GameObject)Instantiate(m_GoodsButtonPrefab, new Vector3(startXPos, startYPos, 0.0f), Quaternion.identity);
            go.gameObject.transform.SetParent(m_ButtonParent.transform, false);
            go.GetComponentInChildren<Button>().onClick.AddListener(delegate { StoreInventory.BuyItem(vcp.ItemId); });

            if (col < maxCol)
            {
                col++;
                startXPos += buttonWidth;
                if (col >= maxCol)
                {
                    if (row < maxRow)
                    {
                        col = 0;
                        startXPos = -300.0f;
                        row++;
                        startYPos -= buttonHeight;
                    }
                }
            }
        }

        Text[] buttonText = m_StorePanel.GetComponentsInChildren<Text>();

        int i = 1;
        foreach (VirtualCurrencyPack vcp in StoreInfo.CurrencyPacks)
        {
            buttonText[i].text = vcp.Name;
            buttonText[i + 1].text = vcp.Description;
            i += 2;
        }
    }

    public void onMarketPurchaseStarted(PurchasableVirtualItem pvi)
    {
        //Implement stuff
    }

    #region Upgrades
    public void UnlockItem(string name)
    {
        //check what upgrade to apply, if the player has enough money then disable and enable all the proper components 
        if (name == "Ball" && playerData_.m_Shields >= m_BallCost)
        {
            AudioSource.PlayClipAtPoint(m_UpgradeClip, transform.position);
            playerData_.m_Shields -= m_BallCost;
            m_Ball.SetActive(true);
            m_BallButton.interactable = false;
            m_BallText.text = "";
        }
        else if (name == "BowTie" && playerData_.m_Shields >= m_BowTieCost)
        {
            AudioSource.PlayClipAtPoint(m_UpgradeClip, transform.position);
            playerData_.m_Shields -= m_BowTieCost;
            m_BowTie.SetActive(true);
            m_BowTieButton.interactable = false;
            m_BowTieText.text = "";
        }
        else if (name == "Hat" && playerData_.m_Shields >= m_HatCost)
        {
            AudioSource.PlayClipAtPoint(m_UpgradeClip, transform.position);
            playerData_.m_Shields -= m_HatCost;
            m_Hat.SetActive(true);
            m_HatButton.interactable = false;
            m_HatText.text = "";
        }
        else if (name == "SunGlasses" && playerData_.m_Shields >= m_SunGlassesCost)
        {
            AudioSource.PlayClipAtPoint(m_UpgradeClip, transform.position);
            playerData_.m_Shields -= m_SunGlassesCost;
            m_SunGlasses.SetActive(true);
            m_SunGlassesButton.interactable = false;
            m_SunGlassesText.text = "";
        }
        else if (name == "Glove" && playerData_.m_Shields >= m_GloveCost)
        {
            AudioSource.PlayClipAtPoint(m_UpgradeClip, transform.position);
            playerData_.m_Shields -= m_GloveCost;
            m_Glove.SetActive(true);
            m_GloveButton.interactable = false;
            m_GloveText.text = "";
        }
        else if (name == "Trophy" && playerData_.m_Shields >= m_TrophyCost)
        {
            AudioSource.PlayClipAtPoint(m_UpgradeClip, transform.position);
            playerData_.m_Shields -= m_TrophyCost;
            m_Trophy.SetActive(true);
            m_TrophyButton.interactable = false;
            m_TrophyText.text = "";
        }
        else if(name == "Scanner")
        {
            m_Scanner.SetActive(true);  //enable the scanner
            AudioSource.PlayClipAtPoint(m_UpgradeClip, transform.position);
            playerData_.m_Shields -= m_TrophyCost;
            m_Scanner.SetActive(true);
            m_ScannerButton.interactable = false;
            m_ScannerText.text = "";
        }
    }
    #endregion
}