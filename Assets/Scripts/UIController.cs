using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    public GameObject m_GameUI;
    public GameObject m_NewPlayerUI;
    public GameObject m_NicknamePanel; //Panel for nickname prompt
    public GameObject m_FearPanel; //Panel for the fear prompt
    public GameObject m_CurrentPet; //UI Element for the current pet
    public GameObject m_UpgradePanel; //Panel for the upgrades
    public GameObject m_CameraPlane; //Plane which is drawing the camera on it
    public GameObject m_RadarOverlay; //Image overlay which will have a radar with a rotating arm on it
    public GameObject m_BackgroundPlane; //Plane which is drawing the background on it
    public InputField m_NicknameIF; //Input field for the nickname prompt
    public InputField m_FearIF; //Input field for the fear prompt
    public PlayerData m_PlayerData; //Player data

    public Text m_NicknameText; //Text element for the pet's nickname;
    public Text m_TitleText; //Text element for the pet's current title
    public Text m_EnergyText; //Player's current points - UI element
    public Text m_ShieldsText; //Player's current shields = UI element
    public Text m_EnergyTimerText; //Text element for the Points - UI element
    public Slider m_EnergySlider; //Slider element for the Energy UI
    public string m_SelectedPet;

    private GameObject currPet_;
    private GameController gc_; //Game Controller script for easier access

    private bool isNewPlayer_;
    private bool scannerActive_;
    private float energyTimer_; //Timer until the player receives their next set of points, starts at 300 because the interval is 5 minutes, and there are 300 seconds in 5 minutes
    private float scannerTimer_; //This is the timer that once it reaches the max, will turn off the camera access and return to the normal screen
    private string minutes_;
    private string seconds_;

	void Start () 
    {
        gc_ = Camera.main.GetComponent<GameController>();
        isNewPlayer_ = gc_.m_FirstTimePlayer;

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
            if(scannerTimer_ >= Constants.MAX_SCANNER_TIME)
            {
                CloseScanner();
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
        m_UpgradePanel.SetActive(false);
    }

    public void OpenScanner()
    {
        scannerActive_ = true;
        m_GameUI.SetActive(false);
        m_CameraPlane.SetActive(true);
        m_RadarOverlay.SetActive(true);
        m_BackgroundPlane.SetActive(false);
        currPet_.SetActive(false);
    }

    public void CloseScanner()
    {
        scannerActive_ = false;
        m_GameUI.SetActive(true);
        m_CameraPlane.SetActive(false);
        m_RadarOverlay.SetActive(false);
        m_BackgroundPlane.SetActive(true);
        currPet_.SetActive(true);
    }
}