using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Soomla.Store;

public class UIController : MonoBehaviour
{
    public GameObject m_GameUI;
    public GameObject m_ButtonPage1;
    public GameObject m_ButtonPage2;
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
    public Sprite m_ShieldSprite; //Shield sprite for upgrades panel

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
    public AudioClip m_UpgradeClip;

    private GameObject currPet_;
    private GameObject checkMark_;
    private Pet petData_;
    private GameController gc_; //Game Controller script for easier access
    private AudioSource audio_;
    private PlayerData playerData_;

    private string bttnName_;
    private int clickCounter_ = 0;
    private bool setFearString_ = true;
    private bool playCloseSound_ = true;
    private bool isNewPlayer_;
    private bool scannerActive_;
    private float energyTimer_; //Timer until the player receives their next set of points, starts at 300 because the interval is 5 minutes, and there are 300 seconds in 5 minutes
    private float scannerTimer_; //This is the timer that once it reaches the max, will turn off the camera access and return to the normal screen
    private string minutes_;
    private string seconds_;
    private string fearTitle_; 

	void Start () 
    {
        checkMark_ = GameObject.Find("Check");
        checkMark_.SetActive(false);
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
        setFearString_ = true;
	}
	
	void Update ()
    {
        currPet_ = gc_.ActivePet;
        if (currPet_ != null)
        {
            //set the fear string once at the start of the game
            if (setFearString_)
            {
                setFearString_ = !setFearString_;
                m_FearIF.text = currPet_.GetComponent<Pet>().m_FearOne.ToString();
                SetFearTitle();
            }
            m_NicknameText.text = currPet_.GetComponent<Pet>().m_Nickname;
            m_EnergyText.text = "Energy: " + m_PlayerData.m_Energy.ToString() + "/" + Constants.DEFAULT_MAX_ENERGY;
            m_ShieldsText.text = m_PlayerData.m_Shields.ToString();
            m_EnergySlider.value = m_PlayerData.m_Energy;
            if(m_PlayerData.m_Energy == 0)
            {
                m_EnergySlider.fillRect.GetComponent<Image>().color = Color.black;
            }
            else
            {
                m_EnergySlider.fillRect.GetComponent<Image>().color = Color.yellow;
            }
            petData_ = currPet_.GetComponent<Pet>();
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
        Transform tempPos;
        if (clickCounter_ == 0)
        {
            bttnName_ = btn.name;
            tempPos = btn.GetComponentInChildren<Transform>();
            checkMark_.transform.position = tempPos.transform.position;
            checkMark_.SetActive(true);
            clickCounter_++;
        }
        else if(clickCounter_ >= 1 && bttnName_ == btn.name)
        {
            checkMark_.SetActive(false);
            m_SelectedPet = btn.name;
            gc_.CurrentPet = m_SelectedPet;
            gc_.m_PlayerData.AddPet(m_SelectedPet);
            m_NicknamePanel.SetActive(true);
            gc_.SetUpGame();
            clickCounter_ = 0;
        }
        else
        {
            bttnName_ = btn.name;
            tempPos = btn.GetComponentInChildren<Transform>();
            checkMark_.transform.position = tempPos.transform.position;
            checkMark_.SetActive(true);
            clickCounter_++;
        }
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
            petData_.m_IsDancing = false;
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
            petData_.m_IsDancing = true;
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
            petData_.m_IsDancing = true;
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
            petData_.m_IsDancing = true;
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
        float buttonWidth = m_GoodsButtonPrefab.GetComponent<RectTransform>().sizeDelta.x * 3;
        float buttonHeight = m_GoodsButtonPrefab.GetComponent<RectTransform>().sizeDelta.y;
        float startXPos = 0.0f - (buttonWidth * 1.25f);
        float startYPos = 0.0f + (buttonHeight * 1.25f);
        int row = 0;
        int col = 0;
        int maxCol = 3;
        int maxRow = 100;

        m_UpgradePanel.SetActive(true);

        foreach(Item item in gc_.m_Items)
        {
            GameObject go = (GameObject)Instantiate(m_GoodsButtonPrefab, new Vector3(startXPos, startYPos, 0.0f), Quaternion.identity);
            go.gameObject.transform.SetParent(m_UpgradePanel.transform, false);
            go.name = item.m_ItemName;
            go.GetComponentInChildren<Image>().sprite = item.gameObject.GetComponent<Image>().sprite;
            go.GetComponentInChildren<Button>().onClick.AddListener(delegate { UnlockItem(go); });
            go.GetComponentInChildren<Text>().text = item.m_Description + " This costs " + item.m_Cost + " shields.";

            if (col < maxCol)
            {
                col++;
                startXPos += buttonWidth;
                if (col >= maxCol)
                {
                    if (row < maxRow)
                    {
                        col = 0;
                        startXPos = 0.0f - (buttonWidth * 1.25f);
                        row++;
                        startYPos -= buttonHeight;
                    }
                }
            }
        }

        /*foreach (VirtualCurrencyPack vcp in StoreInfo.CurrencyPacks)
        {
            GameObject go = (GameObject)Instantiate(m_GoodsButtonPrefab, new Vector3(startXPos, startYPos, 0.0f), Quaternion.identity);
            go.gameObject.transform.SetParent(m_UpgradePanel.transform, false);
            go.GetComponentInChildren<Image>().sprite = m_ShieldSprite;
            go.GetComponentInChildren<Button>().onClick.AddListener(delegate { StoreInventory.BuyItem(vcp.ItemId); });
            go.GetComponentInChildren<Text>().text = vcp.Description;

            if (col < maxCol)
            {
                col++;
                startXPos += buttonWidth;
                if (col >= maxCol)
                {
                    if (row < maxRow)
                    {
                        col = 0;
                        startXPos = 0.0f - (buttonWidth * 1.25f);
                        row++;
                        startYPos -= buttonHeight;
                    }
                }
            }
        }*/
    }

    public void onMarketPurchaseStarted(PurchasableVirtualItem pvi)
    {
        //Implement stuff
    }

    public void SwitchPage(bool page1)
    {
        if(page1)
        {
            m_ButtonPage1.SetActive(false);
            m_ButtonPage2.SetActive(true);    
        }
        else
        {
            m_ButtonPage1.SetActive(true);
            m_ButtonPage2.SetActive(false);  
        } 
    }

    public void SetFearByName(string name)
    {
        m_FearIF.text = name;
        if (!string.IsNullOrEmpty(m_FearIF.text) && currPet_ != null)
        {
            currPet_.GetComponent<Pet>().m_FearOne = m_FearIF.text;
            currPet_.GetComponent<Pet>().m_Bored = Constants.DEFAULT_START_STATS;
            currPet_.GetComponent<Pet>().m_Cleanliness = Constants.DEFAULT_START_STATS;
            currPet_.GetComponent<Pet>().m_Hunger = Constants.DEFAULT_START_STATS;
            petData_.m_IsDancing = false;
            gc_.Save();
            Destroy(m_NewPlayerUI);
            m_GameUI.SetActive(true);
        }
        SetFearTitle();
    }

    void SetFearTitle()
    {   
        //checks what the fear is and make a fear title
        if (m_FearIF.text == "Noises" || m_FearIF.text == "Strangers")
        {
            m_TitleText.text = "The " + currPet_.GetComponent<Pet>().m_FearOne + " Vanquisher";
        }
        else if (m_FearIF.text == "Monsters" || m_FearIF.text == "Ghosts")
        {
            m_TitleText.text = "The " + currPet_.GetComponent<Pet>().m_FearOne + " Eradicator";
        }
        else if (m_FearIF.text == "Darkness" || m_FearIF.text == "Storms")
        {
            m_TitleText.text = "The " + currPet_.GetComponent<Pet>().m_FearOne + " Destroyer";
        }
        else if (m_FearIF.text == "Animals" || m_FearIF.text == "Insects" || m_FearIF.text == "Snakes" || m_FearIF.text == "Spiders")
        {
            m_TitleText.text = "The " + currPet_.GetComponent<Pet>().m_FearOne + " Exterminator";
        }
        else if (m_FearIF.text == "Separation" || m_FearIF.text == "Failure")
        {
            m_TitleText.text = "The " + currPet_.GetComponent<Pet>().m_FearOne + " Terminator";
        }
        else if (m_FearIF.text == "Teachers" || m_FearIF.text == "Injury")
        {
            m_TitleText.text = "The " + currPet_.GetComponent<Pet>().m_FearOne + " Abolisher";
        }
        else
        {
            m_TitleText.text = "The " + currPet_.GetComponent<Pet>().m_FearOne + " Master";
        }     
    }

    #region Upgrades
    public void UnlockItem(GameObject go)
    {
        for(int i = 0; i < gc_.m_Items.Count; ++i)
        {
            if (gc_.m_Items[i].m_ItemName == go.name)
            {
                if (playerData_.m_Shields >= gc_.m_Items[i].m_Cost)
                {
                    AudioSource.PlayClipAtPoint(m_UpgradeClip, transform.position);
                    playerData_.RemoveShields(gc_.m_Items[i].m_Cost);
                    gc_.m_Items[i].gameObject.SetActive(true);
                    go.GetComponent<Button>().interactable = false;
                    go.GetComponentInChildren<Text>().text = "";
                }
            }
        }
    }
    #endregion
}