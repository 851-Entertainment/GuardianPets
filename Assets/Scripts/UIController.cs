﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

public class UIController : MonoBehaviour
{
    public List<GameObject> m_PlayerItems;
    public GameObject[] m_AnimalButtons;
    public GameObject m_ChangePetPanel;
    public GameObject m_GameUI;
    public GameObject m_SpeechBubble;
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

    public Text m_SpeechText;
    public Text m_NicknameText; //Text element for the pet's nickname;
    public Text m_TitleText; //Text element for the pet's current title
    public Text m_EnergyText; //Player's current points - UI element
    public Text m_ShieldsText; //Player's current shields = UI element
    public Text m_EnergyTimerText; //Text element for the Points - UI element
    public Slider m_EnergySlider; //Slider element for the Energy UI
    public Slider m_LoveSlider;
    public GameObject m_CloseScannerButton;
    public string m_SelectedPet;
    public float m_TransitionVolume = 0.3f;
    public float m_CheckMarkOffset = 150.0f;

    public AudioClip m_TransitionSound;
    public AudioClip m_UpgradeClip;
    public AudioClip[] m_FeedClip;
    public AudioClip[] m_PlayClip;
    public AudioClip[] m_CleanClip;
    public AudioClip m_ClickClip;
    public AudioClip m_ExerciseClip;

    public List<GameObject> pets_; //all the pets the player has
    public GameObject currPet_;
    private GameObject checkMark_;
    private Pet petData_;
    private GameController gc_; //Game Controller script for easier access
    private AudioSource audio_;
    private PlayerData playerData_;

    public GameObject lion_;
    public GameObject hippo;
    public GameObject elephant_;
    public GameObject bear_;
    public GameObject alligator_;
    public GameObject monkey_;

    private string bttnName_;
    private int clickCounter_ = 0;
    private int energySliderIncrease_ = 0;
    private int maxEnergy_;
    private bool upgradeEnergy_ = false;
    private bool setFearString_ = true;
    private bool playCloseSound_ = true;
    private bool isNewPlayer_;
    private bool scannerActive_;
    private float setSpeechTimer_;
    private float energyTimer_; //Timer until the player receives their next set of points, starts at 300 because the interval is 5 minutes, and there are 300 seconds in 5 minutes
    private float scannerTimer_; //This is the timer that once it reaches the max, will turn off the camera access and return to the normal screen
    private string minutes_;
    private string seconds_;
    private string fearTitle_;
    private string feedMessage_ = "I'm hungry!!!";
    private string playMessage_ = "Play with me!";
    private string cleanMessage_ = "I need a bath";
    private List<string> petsOwned_;    //list of names of the players pet 

    private int timesPlayed_;
    private int timesFed_;
    private int timesWashed_;
    public int TimesPlayed { get { return timesPlayed_; } set { timesPlayed_ = value; } }
    public int TimesFed { get { return timesFed_; } set { timesFed_ = value; } }
    public int TimesWashed { get { return timesWashed_; } set { timesWashed_ = value; } }

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
        m_LoveSlider.minValue = 0;
        m_LoveSlider.maxValue = Constants.DEFAULT_MAX_LOVE;
        maxEnergy_ = Constants.DEFAULT_MAX_ENERGY;
        setFearString_ = true;
        m_SpeechBubble.SetActive(false);
        LoadPets();
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
            m_EnergyText.text = "Energy: " + m_PlayerData.m_Energy.ToString() + "/" + maxEnergy_;
            m_ShieldsText.text = GuardianPetsAssets.SHIELD_CURRENCY.GetBalance().ToString();
            m_EnergySlider.value = m_PlayerData.m_Energy + energySliderIncrease_;

            if(m_PlayerData.m_Energy == 0)
            {
                m_EnergySlider.fillRect.GetComponent<Image>().color = Color.black;
            }
            else
            {
                m_EnergySlider.fillRect.GetComponent<Image>().color = Color.yellow;
            }
            petData_ = currPet_.GetComponent<Pet>();

            m_LoveSlider.value = petData_.m_Love;
            if (petData_.m_Love == 300)
            {
                m_LoveSlider.fillRect.GetComponent<Image>().color = Color.red;
            }
            else
            {
                m_LoveSlider.fillRect.GetComponent<Image>().color = Color.black;
            }

            UpdateTimer();
        }
        m_CameraPlane.GetComponent<CameraAccess>().UpdateCamera();
        UpdateSpeech();
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
        m_NewPlayerUI.SetActive(false);
        //Destroy(m_NewPlayerUI);
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
            checkMark_.transform.position = new Vector3(tempPos.transform.position.x, tempPos.transform.position.y + m_CheckMarkOffset, tempPos.transform.position.z);       
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
           // petsOwned_.Add(bttnName_);
        }
        else
        {
            bttnName_ = btn.name;
            tempPos = btn.GetComponentInChildren<Transform>();
            checkMark_.transform.position = new Vector3(tempPos.transform.position.x, tempPos.transform.position.y + m_CheckMarkOffset, tempPos.transform.position.z);
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
            m_NewPlayerUI.SetActive(false);
            //Destroy(m_NewPlayerUI);
            m_GameUI.SetActive(true);
            SetFearTitle();
            AudioSource.PlayClipAtPoint(m_ClickClip, transform.position);
            pets_.Add(currPet_);
            AssignPet(currPet_.name);
            AssignPet(currPet_.name);
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
            HealPet();
            currPet_.GetComponent<Pet>().m_Hunger -= Constants.STAT_DECREASE_VAL;
            timesFed_++;
            if (currPet_.GetComponent<Pet>().m_Hunger <= Constants.MIN_PET_STAT)
            {
                currPet_.GetComponent<Pet>().m_Hunger = Constants.MIN_PET_STAT;
                if (/*currPet_.GetComponent<Pet>().CheckShieldConditions()*/ petData_.m_Love == 0)
                {
                    GuardianPetsAssets.SHIELD_CURRENCY.Give(Constants.SHIELDS_REWARDED, false);
                }
                
            }
            gc_.m_PlayerData.RemoveEnergy();
            RandomSound(1, m_FeedClip);
        }
    }

    public void HealPet()
    {
        if(petData_.m_IsSick)
        {
            petData_.m_IsSick = false;
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
            timesPlayed_++;
            if (currPet_.GetComponent<Pet>().m_Bored <= Constants.MIN_PET_STAT)
            {
                currPet_.GetComponent<Pet>().m_Bored = Constants.MIN_PET_STAT;
                if (/*currPet_.GetComponent<Pet>().CheckShieldConditions()*/ petData_.m_Love == 0)
                {
                    GuardianPetsAssets.SHIELD_CURRENCY.Give(Constants.SHIELDS_REWARDED, false);
                }
            }
            gc_.m_PlayerData.RemoveEnergy();
            RandomSound(2, m_PlayClip);
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
            timesWashed_++;
            if (currPet_.GetComponent<Pet>().m_Cleanliness <= Constants.MIN_PET_STAT)
            {
                currPet_.GetComponent<Pet>().m_Cleanliness = Constants.MIN_PET_STAT;
                if (/*currPet_.GetComponent<Pet>().CheckShieldConditions()*/ petData_.m_Love == 0)
                {
                    GuardianPetsAssets.SHIELD_CURRENCY.Give(Constants.SHIELDS_REWARDED, false);
                }
            }
            gc_.m_PlayerData.RemoveEnergy();
            RandomSound(2, m_CleanClip);
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
            AudioSource.PlayClipAtPoint(m_ClickClip, transform.position);
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

    #region Store/Micro transaction stuff
    public void PopulateStore()
    {
        float buttonWidth = m_GoodsButtonPrefab.GetComponent<RectTransform>().sizeDelta.x * 3;
        float buttonHeight = m_GoodsButtonPrefab.GetComponent<RectTransform>().sizeDelta.y * 1.25f;
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
            go.GetComponentInChildren<Image>().sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
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

        foreach (VirtualCurrencyPack vcp in StoreInfo.CurrencyPacks)
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
        }
    }

    public void onMarketPurchaseStarted(PurchasableVirtualItem pvi)
    {
        //Implement stuff
    }
    #endregion

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
        AudioSource.PlayClipAtPoint(m_ClickClip, transform.position);
    }

    public void Exercise(int upgradeAmount)
    {
        if (gc_.m_PlayerData.m_Energy >= Constants.ACTION_COST)
        {
            AudioSource.PlayClipAtPoint(m_ExerciseClip, transform.position);
            petData_.m_Exercise++;
            currPet_.GetComponent<Pet>().m_Bored -= Constants.STAT_DECREASE_VAL;
            if (currPet_.GetComponent<Pet>().m_Bored <= Constants.MIN_PET_STAT)
            {
                currPet_.GetComponent<Pet>().m_Bored = Constants.MIN_PET_STAT;
                if (petData_.m_Exercise >= upgradeAmount)
                {
                    petData_.m_Exercise = 0;
                    energySliderIncrease_++;
                    maxEnergy_ += energySliderIncrease_;    //var used to display how much energy the player has        
                }
            }
            gc_.m_PlayerData.RemoveEnergy();
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
            m_NewPlayerUI.SetActive(false);
           // Destroy(m_NewPlayerUI);
            m_GameUI.SetActive(true);
        }
        SetFearTitle();
        pets_.Add(currPet_);
        AssignPet(currPet_.name);
        AssignPet(currPet_.name);
        AudioSource.PlayClipAtPoint(m_ClickClip, transform.position);
       
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

    void SetSpeechText()
    {
        setSpeechTimer_ += Time.deltaTime;
        if (setSpeechTimer_ >= 60.0f)
        {
            setSpeechTimer_ = 0.0f;
            int randNum = Random.Range(0, 2);
            if (randNum == 0)
            {
                feedMessage_ = "I'm hungry!!!";
                playMessage_ = "Play with me!";
                cleanMessage_ = "I need a bath";
            }
            else if (randNum == 1)
            {
                feedMessage_ = "Feed me!!!";
                playMessage_ = "Let's Play!";
                cleanMessage_ = "Bath time!";
            }
        }
    }

    void UpdateSpeech()
    {
        if (currPet_ != null)
        {
            if (currPet_.GetComponent<Pet>().m_IsHungry)
            {
                m_SpeechBubble.SetActive(true);
                m_SpeechText.text = feedMessage_;
            }
            else if (currPet_.GetComponent<Pet>().m_IsBored)
            {
                m_SpeechBubble.SetActive(true);
                m_SpeechText.text = playMessage_;
            }
            else if (currPet_.GetComponent<Pet>().m_NeedsCleaning)
            {
                m_SpeechBubble.SetActive(true);
                m_SpeechText.text = cleanMessage_;
            }
            else
            {
                m_SpeechBubble.SetActive(false);
                m_SpeechText.text = "";
            }
        }
    }

    void RandomSound(int numSounds, AudioClip[] clips)
    {
        int randNum = Random.Range(0, numSounds + 1);
        AudioSource.PlayClipAtPoint(clips[randNum], transform.position);
    }

    #region Upgrades
    public void UnlockItem(GameObject go)
    {
        for(int i = 0; i < gc_.m_Items.Count; ++i)
        {
            if (gc_.m_Items[i].m_ItemName == go.name)
            {
                if (GuardianPetsAssets.SHIELD_CURRENCY.GetBalance() >= gc_.m_Items[i].m_Cost)
                {
                    AudioSource.PlayClipAtPoint(m_UpgradeClip, transform.position);
                    GuardianPetsAssets.SHIELD_CURRENCY.Take(gc_.m_Items[i].m_Cost);
                    gc_.m_Items[i].gameObject.SetActive(true);
                    if (gc_.m_Items[i].m_IsPlayerItem)
                    {
                        GameObject obj = GameObject.Find(gc_.m_Items[i].m_Names[gc_.m_Items[i].m_ItemSpot]);    //get the waypoint the item will be at
                        gc_.m_Items[i].gameObject.transform.SetParent(obj.transform);
                    }
                    go.GetComponent<Button>().interactable = false;
                    go.GetComponentInChildren<Text>().text = "";

                    m_PlayerItems.Add(go);
                }
            }
        }
    }
    #endregion

    #region Multiple Pets
    void DisablePetButton()
    {
        for (int i = 0; i < m_AnimalButtons.Length; ++i)
        {
            for (int j = 0; j < pets_.Count; ++j)
            {
                if (m_AnimalButtons[i].name == pets_[j].name)
                {
                    m_AnimalButtons[i].SetActive(false);
                }
            }
        }
    }

    public void ChangePet(string name)
    {
        //set all pets off 
        if (pets_ != null)
        {
            for (int i = 0; i < pets_.Count; ++i)
            {
                pets_[i].SetActive(false);
            }
        }
        //turn on the proper one based on the name passed in
        #region Activate Pet
        if (name == "Lion" && lion_ != null)
        {
            lion_.SetActive(true);
        }
        else if (name == "Elephant" && elephant_ != null)
        {
            elephant_.SetActive(true);
        }
        else if (name == "Hippo" && hippo != null)
        {
            hippo.SetActive(true);
        }
        else if (name == "Bear" && bear_ != null)
        {
            bear_.SetActive(true);
        }
        else if (name == "Alligator" && alligator_ != null)
        {
            alligator_.SetActive(true);
        }
        else if (name == "Monkey" && monkey_ != null)
        {
            monkey_.SetActive(true);
        }
        AssignPet(name);
        #endregion
    }

    void AssignPet(string name)
    {
        //check the name passed in, check if the animal game object is empty, if yes assign it, then always set the current pet to the one passed into the function
        if (name == "Lion")
        {      
            if(lion_ == null)
            {
                lion_ = currPet_;
                gc_.m_PlayerData.m_Pets.Add(lion_);
            }
            gc_.SetPet(lion_);
        }
        else if (name == "Elephant")
        {
            if (elephant_ == null)
            {
                elephant_ = currPet_;
                gc_.m_PlayerData.m_Pets.Add(elephant_);
            }
            gc_.SetPet(elephant_);
        }
        else if (name == "Hippo")
        {    
            if (hippo == null)
            {
                hippo = currPet_;
                gc_.m_PlayerData.m_Pets.Add(hippo);
            }
            gc_.SetPet(hippo);
        }
        else if (name == "Bear")
        {  
            if (bear_ == null)
            {
                bear_ = currPet_;
                gc_.m_PlayerData.m_Pets.Add(bear_);
            }
            gc_.SetPet(bear_);
        }
        else if (name == "Alligator")
        {        
            if (alligator_ == null)
            {
                alligator_ = currPet_;
                gc_.m_PlayerData.m_Pets.Add(alligator_);
            }
            gc_.SetPet(alligator_);
        }
        else if (name == "Monkey")
        {  
            if (monkey_ == null)
            {
                monkey_ = currPet_;
                gc_.m_PlayerData.m_Pets.Add(monkey_);
            }
            gc_.SetPet(monkey_);
        }
    }

    void LoadPets()
    {
        foreach(GameObject go in gc_.m_PlayerData.m_Pets)
        {
            SetSavedPets(go);
        }
    }

    void SetSavedPets(GameObject go)
    {
        //check the name passed in, check if the animal game object is empty, if yes assign it, then always set the current pet to the one passed into the function
        if (go.name == "Lion")
        {    
            lion_ = go;
            pets_.Add(lion_);
        }
        else if (go.name == "Elephant")
        {   
            elephant_ = go;
            pets_.Add(elephant_);
        }
        else if (go.name == "Hippo")
        {  
            hippo = go;
            pets_.Add(hippo);
        }
        else if (go.name == "Bear")
        {  
            bear_ = go;
            pets_.Add(bear_);
        }
        else if (go.name == "Alligator")
        {    
            alligator_ = go;
            pets_.Add(alligator_);
        }
        else if (go.name == "Monkey")
        {
            monkey_ = go;
            pets_.Add(monkey_);
        }
    }

    public void ChangePetMenu(bool open)
    {
        if(open)
        {
            m_ChangePetPanel.SetActive(true);
        }
        else
        {
            m_ChangePetPanel.SetActive(false);
        }
    }

    public void PickNewPet()
    {
        //Sets up the new layer UI again and turns off the game UI 
        m_GameUI.SetActive(false);
        currPet_.SetActive(false);
        m_NewPlayerUI.SetActive(true);
        foreach (Transform child in m_NewPlayerUI.transform)
        {
            child.gameObject.SetActive(true);
        }
        checkMark_.SetActive(false);
        m_FearPanel.SetActive(false);
        m_NicknamePanel.SetActive(false);
        DisablePetButton();
    }
    #endregion 
}