﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class UIController : MonoBehaviour
{
    #region Public Variables
    public List<string> m_PlayerItems = new List<string>();
    public GameObject[] m_ChangePetButtons;
    public GameObject[] m_AnimalButtons;
    public GameObject[] m_Toys;
    public List<GameObject> m_StoreButtons = new List<GameObject>();
    public GameObject m_ChangePetPanel;
    public GameObject m_GameUI;
    public GameObject m_SpeechBubble;
    public GameObject m_ButtonPage1;
    public GameObject m_ButtonPage2;
    public GameObject m_NewPlayerUI;
    public GameObject m_Radar;
    public GameObject m_InteractMenuBar;
    public GameObject m_PetMenuBar;
    public GameObject m_TrophyMenu;
    public GameObject m_ConfirmPurchasePanel;
    public int m_ScannerCost = 1;
    public float EnergyTimer { get { return energyTimer_; } set { energyTimer_ = value; } }
    public bool UpdateEnergyTimer { get { return updateEnergyTimer_; } set { updateEnergyTimer_ = value; } }

    /// <summary>Button for playing with the pet</summary>
    public GameObject m_PlayButton;

    /// <summary>List of the trophies to unlock</summary>
    public List<GameObject> m_Trophy = new List<GameObject>();

    /// <summary>Panel for the nickname prompt</summary>
    public GameObject m_NicknamePanel;

    /// <summary>Panel for the fear prompt</summary>
    public GameObject m_FearPanel;

    /// <summary>UI Element for the current pet</summary>
    public GameObject m_CurrentPet;

    /// <summary>Plane which is drawing the background image on it</summary>
    public GameObject m_BackgroundPlane;

    /// <summary>Name of selected pet</summary>
    public string m_SelectedPet;

    /// <summary>Y offset for the check mark</summary>
    public float m_CheckMarkOffset = 150.0f;

    /// <summary>The player's data</summary>
    public PlayerData m_PlayerData;

    /// <summary>Shield sprite for the upgrades panel</summary>
    public Sprite m_ShieldSprite;

    /// <summary>The amount a pet will cost if it isn't a new player</summary>
    public int m_PetCost = 1000; 

    #region Scanner Variables
    /// <summary>Plane which is drawing the camera on it</summary>
    public GameObject m_CameraPlane;

    /// <summary>Image overlay which will have the rader with a rotating arm on it</summary>
    public GameObject m_RadarOverlay;

    /// <summary>Button to close the radar scanner</summary>
    public GameObject m_CloseScannerButton;

    public Text m_ScannerText;
    #endregion

    #region Upgrades/Store Variables
    /// <summary>Panel for the upgrades</summary>
    public GameObject m_UpgradePanel;

    /// <summary>Button prefab for the items sold in the store</summary>
    public GameObject m_GoodsButtonPrefab;

    /// <summary>Parent game object for the button prefab</summary>
    public GameObject m_ButtonParent;
    #endregion

    #region Input Fields
    /// <summary>Input field for the nickname prompt</summary>
    public InputField m_NicknameIF;

    /// <summary>Input field for the fear prompt</summary>
    public InputField m_FearIF;
    #endregion

    #region Text Elements
    /// <summary>Text element for the pet's speech</summary>
    public Text m_SpeechText;

    /// <summary>Text element for the pet's nickname</summary>
    public Text m_NicknameText;

    /// <summary>Text element for the pet's current title</summary>
    public Text m_TitleText;

    /// <summary>Text element for the player's current energy</summary>
    public Text m_EnergyText;

    /// <summary>Text element for the player's current shields</summary>
    public Text m_ShieldsText;

    /// <summary>Text element for the player's energy timer</summary>
    public Text m_EnergyTimerText;
    #endregion

    #region Slider Elements
    /// <summary>Slider element for the energy UI</summary>
    public Slider m_EnergySlider;

    /// <summary>Slider element for the love UI</summary>
    public Slider m_LoveSlider;
    #endregion

    
    #endregion

    #region Audio Variables
    /// <summary>Transition sound audio clip</summary>
    public AudioClip m_TransitionSound;

    /// <summary>Upgrade sound audio clip</summary>
    public AudioClip m_UpgradeClip;

    /// <summary>Array of feed sounds</summary>
    public AudioClip[] m_FeedClip;

    /// <summary>Array of play sounds</summary>
    public AudioClip[] m_PlayClip;

    /// <summary>Array of cleaning sounds</summary>
    public AudioClip[] m_CleanClip;

    /// <summary>Click sound</summary>
    public AudioClip m_ClickClip;

    /// <summary>Excercise sound</summary>
    public AudioClip m_ExerciseClip;

    /// <summary>Transition sound volume</summary>
    public float m_TransitionVolume = 0.3f;
    #endregion

    #region Private Variables

    /// <summary>Game object of player's current pet</summary>
    private GameObject currPet_;

    private GameObject petSheildCost_;

    /// <summary>Game object for the check mark in the New Player UI</summary>
    private GameObject checkMark_;

    /// <summary>Button to go back from pick pet menu</summary>
    private GameObject returnFromPetMenu_;

    /// <summary>Panel for the achievement menu</summary>
    private GameObject achievementPanel_;

    /// <summary>Pet Data script</summary>
    private Pet petData_;

    /// <summary>Game Controller script</summary>
    private GameController gc_;
    private AudioSource audio_;

    /// <summary>Pet var that is assigned if the player is using the lion</summary>
    private GameObject lion_;
    /// <summary>Pet var that is assigned if the player is using the hippo</summary>
    private GameObject hippo;
    /// <summary>Pet var that is assigned if the player is using the elephant</summary>
    private GameObject elephant_;
    /// <summary>Pet var that is assigned if the player is using the bear</summary>
    private GameObject bear_;
    /// <summary>Pet var that is assigned if the player is using the alligator</summary>
    private GameObject alligator_;
    /// <summary>Pet var that is assigned if the player is using the monkey</summary>
    private GameObject monkey_;

    /// <summary>The item button pressed to confirm a purchase</summary>
    private GameObject tempItemButton_;

    private string bttnName_;
    private int clickCounter_ = 0;
    private int energySliderIncrease_ = 0;
    private int maxEnergy_;
    /// <summary>temp var to set the pet active on the intial run.</summary>
    private bool activatePet_ = true;
    private bool upgradeEnergy_ = false;
    private bool setFearString_ = true;
    private bool playCloseSound_ = true;
    private bool resetTimerValue_ = true;
    /// <summary>when you buy a toy increment the counter buy only do it once</summary>
    private bool increaseToyCounter_ = true; 
    ///<summary>Can update the check toy function. False if all toys have been unlocked</summary>
    private bool checkToys_ = true;
    private bool updateEnergyTimer_ = true;
    private bool isNewPlayer_;
    private bool checkLove_ = true;
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
    /// <summary>Temp string to show what item is being considered for purchase</summary>
    private string confirmItem_;
    private List<string> petsOwned_;    //list of names of the players pet 

    private int timesPlayed_;
    private int timesFed_;
    private int timesWashed_;
    private int timesLoved_;
    private int timesExercised_;
    private int toyIndex_ = 0;
    private int playerItemIndex_ = 0;
    private int numToysActive = 0;
    #endregion

    #region Attributes
    public int TimesPlayed { get { return timesPlayed_; } set { timesPlayed_ = value; } }
    public int TimesFed { get { return timesFed_; } set { timesFed_ = value; } }
    public int TimesWashed { get { return timesWashed_; } set { timesWashed_ = value; } }
    public int TimesLoved { get { return timesLoved_; } set { timesLoved_ = value; } }
    public int TimesExercised { get { return timesExercised_; } set { timesExercised_ = value; } }
    #endregion

    void Start () 
    {
        checkToys_ = true;
        achievementPanel_ = GameObject.Find("AchievementMenu");
        returnFromPetMenu_ = GameObject.Find("Back Button");
        petSheildCost_ = GameObject.Find("Sheilds ");
        returnFromPetMenu_.SetActive(false);
        petSheildCost_.SetActive(false);
        checkMark_ = GameObject.Find("Check");
        checkMark_.SetActive(false);
        audio_ = GetComponent<AudioSource>();
        gc_ = Camera.main.GetComponent<GameController>();
        isNewPlayer_ = gc_.m_FirstTimePlayer; 

        m_NicknameIF.characterLimit = Constants.CHARACTER_LIMIT;

        if (isNewPlayer_)
        {
            updateEnergyTimer_ = true;
            GuardianPetsAssets.SCANNER_CURRENCY.Give(Constants.START_SCANS);
            m_GameUI.SetActive(false);
            m_NewPlayerUI.SetActive(true);
            energyTimer_ = Constants.ENERGY_TIMER;
        }
        else
        {
            RemoveUI();
        }

        m_EnergySlider.minValue = 0;
        m_EnergySlider.maxValue = Constants.DEFAULT_MAX_ENERGY;
        m_LoveSlider.minValue = 0;
        m_LoveSlider.maxValue = Constants.DEFAULT_MAX_LOVE;
        maxEnergy_ = Constants.DEFAULT_MAX_ENERGY;
        setFearString_ = true;
        m_SpeechBubble.SetActive(false);
        LoadPets();
        activatePet_ = true;
        m_Radar.SetActive(false);
	}

    void CheckLoveFilled()
    {
        if (m_LoveSlider.value == m_LoveSlider.maxValue && checkLove_)
        {
            timesLoved_++;
            PlayGamesPlatform.Instance.IncrementAchievement(Constants.LOVE_ACHIEVEMENT_ID, 1, (bool success) =>
            {
                // handle success or failure
            });
            checkLove_ = false;
        }
        else if(m_LoveSlider.value != m_LoveSlider.maxValue)
        {
            checkLove_ = true;
        }
    }
	
	void Update ()
    {
        currPet_ = gc_.ActivePet;
        if (currPet_ != null)
        {
            if (activatePet_)
            {
                ChangePet(currPet_.name);
                Loaditems();
                activatePet_ = false;
            }
            //set the fear string once at the start of the game
            if (setFearString_ && !gc_.m_FirstTimePlayer)
            {
                setFearString_ = !setFearString_;
                m_FearIF.text = currPet_.GetComponent<Pet>().m_FearOne.ToString();
                SetFearTitle();
            }
            m_NicknameText.text = currPet_.GetComponent<Pet>().m_Nickname;
            m_EnergyText.text = "Energy: " + m_PlayerData.m_Energy.ToString() + "/" + maxEnergy_;
            m_ShieldsText.text = GuardianPetsAssets.SHIELD_CURRENCY.GetBalance().ToString();
            m_EnergySlider.value = m_PlayerData.m_Energy + energySliderIncrease_;

            petData_ = currPet_.GetComponent<Pet>();

            m_LoveSlider.value = 300.0f - petData_.m_Love;
            UpdateTimer();
        }
        m_CameraPlane.GetComponent<CameraAccess>().UpdateCamera();
        UpdateSpeech();
        m_ScannerText.text = GuardianPetsAssets.SCANNER_CURRENCY.GetBalance().ToString();
	}

    void FixedUpdate()
    {
        UpdateToys();
    }

    void UpdateTimer()
    {
        if (gc_.m_PlayerData.m_Energy < Constants.DEFAULT_MAX_ENERGY && updateEnergyTimer_)
        {
            m_EnergyTimerText.enabled = true;   
            minutes_ = Mathf.Floor(energyTimer_ / 60).ToString("00");
            seconds_ = (energyTimer_ % 60).ToString("00");
            m_EnergyTimerText.text = minutes_ + ":" + seconds_;
            energyTimer_ -= Time.deltaTime;
            if (energyTimer_ <= 0.0f)
            {
                gc_.m_PlayerData.m_Energy += Constants.ENERGY_REWARDED;
                energyTimer_ = Constants.ENERGY_TIMER;
                minutes_ = Mathf.Floor(energyTimer_ / 60).ToString("00");
                seconds_ = (energyTimer_ % 60).ToString("00");
                m_EnergyTimerText.text = minutes_ + ":" + seconds_;
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
                if (playCloseSound_)
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
        if (isNewPlayer_)
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
            else if (clickCounter_ >= 1 && bttnName_ == btn.name)
            {
                checkMark_.SetActive(false);
                m_SelectedPet = btn.name;
                gc_.CurrentPet = m_SelectedPet;
                m_NicknamePanel.SetActive(true);
                gc_.SetUpGame();
                clickCounter_ = 0;
                returnFromPetMenu_.SetActive(false);
                petSheildCost_.SetActive(false);
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
        else if(GuardianPetsAssets.SHIELD_CURRENCY.GetBalance() >= m_PetCost && !isNewPlayer_)
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
            else if (clickCounter_ >= 1 && bttnName_ == btn.name)
            {
                GuardianPetsAssets.SHIELD_CURRENCY.Take(m_PetCost);
                checkMark_.SetActive(false);
                m_SelectedPet = btn.name;
                gc_.CurrentPet = m_SelectedPet;
                m_NicknamePanel.SetActive(true);
                gc_.SetUpGame();
                clickCounter_ = 0;
                returnFromPetMenu_.SetActive(false);
                petSheildCost_.SetActive(false);
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
            //gc_.m_PlayerData.m_Pets.Add(currPet_);
            SetFearTitle();
            AssignPet(currPet_.name);
            currPet_.SetActive(true);
            gc_.Save();
            m_NewPlayerUI.SetActive(false);
            //Destroy(m_NewPlayerUI);
            m_GameUI.SetActive(true);
            gc_.m_FirstTimePlayer = false;
            AudioSource.PlayClipAtPoint(m_ClickClip, transform.position);
           
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
            PlayGamesPlatform.Instance.IncrementAchievement(Constants.FEED_ACHIEVEMENT_ID, 1, (bool success) =>
            {
                // handle success or failure
            });
            if (currPet_.GetComponent<Pet>().m_Hunger <= Constants.MIN_PET_STAT)
            {
                currPet_.GetComponent<Pet>().m_Hunger = Constants.MIN_PET_STAT;
                if (petData_.m_Love == 0)
                {
                    GuardianPetsAssets.SHIELD_CURRENCY.Give(Constants.SHIELDS_REWARDED, false);
                }
                
            }
            gc_.m_PlayerData.RemoveEnergy();
            RandomSound(1, m_FeedClip);
            if (gc_.m_PlayerData.m_Energy == 20)
            {
                energyTimer_ = Constants.ENERGY_TIMER;
            }
            CheckLoveFilled();
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
            PlayGamesPlatform.Instance.IncrementAchievement(Constants.PLAY_ACHIEVEMENT_ID, 1, (bool success) =>
            {
                // handle success or failure
            });
            if (currPet_.GetComponent<Pet>().m_Bored <= Constants.MIN_PET_STAT)
            {
                currPet_.GetComponent<Pet>().m_Bored = Constants.MIN_PET_STAT;
                if (petData_.m_Love == 0)
                {
                    GuardianPetsAssets.SHIELD_CURRENCY.Give(Constants.SHIELDS_REWARDED, false);
                }
            }
            gc_.m_PlayerData.RemoveEnergy();
            RandomSound(2, m_PlayClip);
            if(gc_.m_PlayerData.m_Energy == 20)
            {
                energyTimer_ = Constants.ENERGY_TIMER;
            }
            CheckLoveFilled();
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
            PlayGamesPlatform.Instance.IncrementAchievement(Constants.CLEAN_ACHIEVEMENT_ID, 1, (bool success) =>
            {
                // handle success or failure
            });
            if (currPet_.GetComponent<Pet>().m_Cleanliness <= Constants.MIN_PET_STAT)
            {
                currPet_.GetComponent<Pet>().m_Cleanliness = Constants.MIN_PET_STAT;
                if (petData_.m_Love == 0)
                {
                    GuardianPetsAssets.SHIELD_CURRENCY.Give(Constants.SHIELDS_REWARDED, false);
                }
            }
            gc_.m_PlayerData.RemoveEnergy();
            RandomSound(2, m_CleanClip);
            if (gc_.m_PlayerData.m_Energy == 20)
            {
                energyTimer_ = Constants.ENERGY_TIMER;
            }
            CheckLoveFilled();
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
            AudioSource.PlayClipAtPoint(m_ClickClip, transform.position);
        }
        if (achievementPanel_.activeSelf)
        {
            achievementPanel_.SetActive(false);
            AudioSource.PlayClipAtPoint(m_ClickClip, transform.position);
        }
    }

    public void OpenScanner()
    {
        if (GuardianPetsAssets.SCANNER_CURRENCY.GetBalance() >= m_ScannerCost)
        {
            GuardianPetsAssets.SCANNER_CURRENCY.Take(m_ScannerCost);
            //m_PlayerData.m_Scans -= m_ScannerCost;
            scannerActive_ = true;
            m_CameraPlane.GetComponent<CameraAccess>().m_DisableWebCam = false;
            m_GameUI.SetActive(false);
            m_CameraPlane.SetActive(true);
            m_CameraPlane.GetComponent<CameraAccess>().enabled = true;
            m_RadarOverlay.SetActive(true);
            m_BackgroundPlane.SetActive(false);
            currPet_.SetActive(false);
        }
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
        AudioSource.PlayClipAtPoint(m_ClickClip, transform.position);
    }

    public void Exercise(int upgradeAmount)
    {
        if (gc_.m_PlayerData.m_Energy >= Constants.ACTION_COST)
        {
            AudioSource.PlayClipAtPoint(m_ExerciseClip, transform.position);
            petData_.m_Exercise++;
            currPet_.GetComponent<Pet>().m_Bored -= Constants.STAT_DECREASE_VAL;
            timesExercised_++;
            PlayGamesPlatform.Instance.IncrementAchievement(Constants.EXERCISE_ACHIEVEMENT_ID, 1, (bool success) =>
            {
                // handle success or failure
            });
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
            if (gc_.m_PlayerData.m_Energy == 20)
            {
                energyTimer_ = Constants.ENERGY_TIMER;
            }
            CheckLoveFilled();
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
            //gc_.m_PlayerData.m_Pets.Add(currPet_);
            SetFearTitle();
            AssignPet(currPet_.name);
            currPet_.SetActive(true);
            gc_.Save();
            m_NewPlayerUI.SetActive(false);
           // Destroy(m_NewPlayerUI);
            m_GameUI.SetActive(true);
            gc_.m_FirstTimePlayer = false;
        }
      
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
        gc_.Save();
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

    /// <param name="Load Player Items">
    /// Unlock the items the player has bought
    /// </param>
    public void Loaditems()
    {
        if (currPet_ != null)
        {
            for (int i = 0; i < m_PlayerItems.Count; ++i)
            {
                for (int j = 0; j < gc_.m_Items.Count; ++j)
                {
                    if (m_PlayerItems[i] == gc_.m_Items[j].name)
                    {
                        UnlockItemByName(gc_.m_Items[j].name);
                    }
                }
            }
        }
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
                    m_ConfirmPurchasePanel.SetActive(true);
                    tempItemButton_ = go;
                    confirmItem_ = gc_.m_Items[i].m_ItemName;
                }
            }
        }
    }

    /// <param name="i">Confirm the item purchase for the player</param>
    public void ConfirmItemPurchase()
    {   
        for (int i = 0; i < gc_.m_Items.Count; ++i)
        {
            if (gc_.m_Items[i].m_ItemName == confirmItem_)
            {
                AudioSource.PlayClipAtPoint(m_UpgradeClip, transform.position);
                GuardianPetsAssets.SHIELD_CURRENCY.Take(gc_.m_Items[i].m_Cost);
                gc_.m_Items[i].gameObject.SetActive(true);
                if (gc_.m_Items[i].m_IsPlayerItem)
                {
                    GameObject obj = GameObject.Find(gc_.m_Items[i].m_Names[gc_.m_Items[i].m_ItemSpot]);    //get the waypoint the item will be at
                    gc_.m_Items[i].gameObject.transform.SetParent(obj.transform);
                }
                m_PlayerItems.Add(gc_.m_Items[i].name);
                tempItemButton_.GetComponent<Button>().interactable = false;
                m_ConfirmPurchasePanel.SetActive(false);
            }
        }
    }

    /// <param name="">Cancels the item purchase for the player</param>
    public void CancelItemPurchase()
    {
        m_ConfirmPurchasePanel.SetActive(false);
    }


    /// <param name="Unlock Item By Name">
    /// This function is used when the player loads a save to give the player their items
    /// </param>
    public void UnlockItemByName(string go)
    {
        for (int i = 0; i < gc_.m_Items.Count; ++i)
        {
            if (gc_.m_Items[i].m_ItemName == go)
            {
                //if (GuardianPetsAssets.SHIELD_CURRENCY.GetBalance() >= gc_.m_Items[i].m_Cost)
               // {
                    gc_.m_Items[i].gameObject.SetActive(true);
                    if (gc_.m_Items[i].m_IsPlayerItem)
                    {
                        GameObject obj = GameObject.Find(gc_.m_Items[i].m_Names[gc_.m_Items[i].m_ItemSpot]);    //get the waypoint the item will be at
                        gc_.m_Items[i].gameObject.transform.SetParent(obj.transform);
                    }
                    for(int j = 0; j < m_StoreButtons.Count; ++j)
                    {
                        if(m_StoreButtons[j].name == go)
                        {
                            m_StoreButtons[j].GetComponent<Button>().interactable = false;
                        }
                    }
               // }
            }
        }
    }
    #endregion

    #region Multiple Pets
    void DisablePetButton()
    {
        for (int i = 0; i < m_AnimalButtons.Length; ++i)
        {
            for (int j = 0; j < gc_.m_PlayerData.m_Pets.Count; ++j)
            {
                if (m_AnimalButtons[i].name == gc_.m_PlayerData.m_Pets[j].name)
                {
                    m_AnimalButtons[i].SetActive(false);
                }
            }
        }
    }

    /// <param name="Enable and Disable pet Selection Buttons">///Goes through and turns all selection buttons off and only turns the ones on that you have</param>
    void SetPetChangeButtons()
    {
        for (int i = 0; i < m_ChangePetButtons.Length; ++i)
        {
            m_ChangePetButtons[i].SetActive(false);
        }

        for (int i = 0; i < m_ChangePetButtons.Length; ++i)
        {
            for (int j = 0; j < gc_.m_PlayerData.m_Pets.Count; ++j)
            {
                if (m_ChangePetButtons[i].name == gc_.m_PlayerData.m_Pets[j].name)
                {
                    m_ChangePetButtons[i].SetActive(true);
                }
            }
        }
    }

    public void ChangePet(string name)
    {
        //set all pets off 
        if (gc_.m_PlayerData.m_Pets != null)
        {
            for (int i = 0; i < gc_.m_PlayerData.m_Pets.Count; ++i)
            {
                gc_.m_PlayerData.m_Pets[i].SetActive(false);
            }
        }

        //turn on the proper one based on the name passed in
        #region Activate Pet
        if (name == "Lion" && lion_ != null)
        {
            lion_.SetActive(true);
            setFearString_ = true;
        }
        else if (name == "Elephant" && elephant_ != null)
        {
            elephant_.SetActive(true);
            setFearString_ = true;
        }
        else if (name == "Hippo" && hippo != null)
        {
            hippo.SetActive(true);
            setFearString_ = true;
        }
        else if (name == "Bear" && bear_ != null)
        {
            bear_.SetActive(true);
            setFearString_ = true;
        }
        else if (name == "Alligator" && alligator_ != null)
        {
            alligator_.SetActive(true);
            setFearString_ = true;
        }
        else if (name == "Monkey" && monkey_ != null)
        {
            monkey_.SetActive(true);
            setFearString_ = true;
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
        }
        else if (go.name == "Elephant")
        {   
            elephant_ = go;
        }
        else if (go.name == "Hippo")
        {  
            hippo = go;
        }
        else if (go.name == "Bear")
        {  
            bear_ = go;
        }
        else if (go.name == "Alligator")
        {    
            alligator_ = go;
        }
        else if (go.name == "Monkey")
        {
            monkey_ = go;
        }
    }

    public void ChangePetMenu(bool open)
    {
        if(open)
        {
            m_ChangePetPanel.SetActive(true);
            SetPetChangeButtons();
        }
        else
        {
            m_ChangePetPanel.SetActive(false);
            AudioSource.PlayClipAtPoint(m_ClickClip, transform.position);
        }
    }

    public void PickNewPet()
    {
        if(!(m_PlayerData.m_Pets.Count >= Constants.MAX_PETS))
        {
            isNewPlayer_ = false;
            returnFromPetMenu_.SetActive(true);
            petSheildCost_.SetActive(true);
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
    }
    #endregion 

    /// <param name="obj">Sets whatever object you want to the opposite state it is in</param>
    public void OpenMenu(GameObject obj)
    {
        if (obj.activeSelf)
        {
            AudioSource.PlayClipAtPoint(m_ClickClip, transform.position);
        }
        obj.SetActive(!obj.activeSelf);
    }

    /// <param name="">Disables the new player UI to return the player to the game</param>
    public void ReturnToGame()
    {
        m_NewPlayerUI.SetActive(false);
        m_GameUI.SetActive(true);
        m_PetMenuBar.SetActive(false);
        currPet_.SetActive(true);
        AudioSource.PlayClipAtPoint(m_ClickClip, transform.position);
    }

    /// <param name="menuBar">///Turns on or off the menu bar based on if it is currently active or not and which bar is in use</param>
    public void OpenMenuBar(GameObject menuBar)
    {
       if(menuBar.name == m_InteractMenuBar.name)
       {
           m_PetMenuBar.SetActive(false);
           m_InteractMenuBar.SetActive(!m_InteractMenuBar.activeSelf);
       }
       else if(menuBar.name == m_PetMenuBar.name)
       {
           m_InteractMenuBar.SetActive(false);
           m_PetMenuBar.SetActive(!m_PetMenuBar.activeSelf);
       }
    }

    /// <param name="">Change the active state of the trophy menu</param>
    public void SetActiveTrophyMenu()
    {
        if (m_TrophyMenu.activeSelf)
        {
            AudioSource.PlayClipAtPoint(m_ClickClip, transform.position);
        }
        m_TrophyMenu.SetActive(!m_TrophyMenu.activeSelf);
    }

    /// <param name="bttn">Changes the play button source image with the button image of the passed in parameter</param>
    public void ChangePlayButtonImage(Button bttn)
    {
        m_PlayButton.GetComponent<Image>().sprite = bttn.image.sprite;
        AudioSource.PlayClipAtPoint(m_PlayClip[2], transform.position, 0.3f);
    }

    /// <param name="">Loops through the toy array and the player item array and sees if what the player currently has matches an item in the toy array. If so set it active in the scene.</param>
    void UpdateToys()
    {
        if (checkToys_)
        {
            for (toyIndex_ = 0; toyIndex_ < m_Toys.Length; ++toyIndex_)
            {
                for (int playerItemIndex_ = 0; playerItemIndex_ < m_PlayerItems.Count; ++playerItemIndex_)
                {
                    if (m_Toys[toyIndex_].name == m_PlayerItems[playerItemIndex_] && m_Toys[toyIndex_].activeSelf != true)
                    {     
                        m_Toys[toyIndex_].SetActive(true);
                        increaseToyCounter_ = true;
                        IncrementNumToys();
                    }
                }
            }

            //if not all the toys are active in the scene reset the counters so we can check again
            foreach (GameObject obj in m_Toys)
            {
                if (obj.activeSelf == false)
                {
                    toyIndex_ = 0;
                    playerItemIndex_ = 0;
                }
            }

            if (numToysActive >= m_Toys.Length)
            {
                 checkToys_ = false;
            }
        }
    }

    /// <param name="">If something is active in the toys array increment the counter</param>
    void IncrementNumToys()
    {
        //if not all the toys are active in the scene reset the counters so we can check again
        foreach (GameObject obj in m_Toys)
        {
            if (obj.activeSelf == true && increaseToyCounter_)
            {
                numToysActive++;
                increaseToyCounter_ = false;
            }
        }
    }

    public void OpenGooglePlayAchievements()
    {
        Social.ShowAchievementsUI();
        AudioSource.PlayClipAtPoint(m_ClickClip, transform.position);
    }
}