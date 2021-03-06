﻿using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi.SavedGame;

public class GameController : MonoBehaviour
{
    /// <summary>Flag to determine whether or not we need to run the first play segment</summary>
    public bool m_FirstTimePlayer = false;

    /// <summary>List of possible pets - DO NOT ALTER THIS LIST</summary>
    public List<GameObject> m_PetChoices = new List<GameObject>();

    /// <summary>Player data</summary>
    public PlayerData m_PlayerData;

    /// <summary></summary>
    public List<GameObject> m_AnimalButtons;

    /// <summary>List of available items to purchase</summary>
    public List<Item> m_Items = new List<Item>();

    /// <summary>The UI controller script</summary>
    private UIController ui_;

    /// <summary>Name of the current pet</summary>
    private string currPetName_;

    /// <summary>Timer between autosaves</summary>
    private float saveTimer_;

    /// <summary>Max time between autosaves</summary>
    private float maxSaveTime_;

    /// <summary>Pet game object</summary>
    private GameObject pet_;

    /// <summary>Screenshot for Google Play Cloud Saves -- DO NOT USE EXCEPT FOR CLOUD</summary>
    private Texture2D screenImage_;

    /// <summary>Cache of Save Data for use in cloud saves</summary>
    private SaveData sData_;

    /// <summary>Keep track of saving or loading during callbacks</summary>
    private bool saving_;

    public string CurrentPet
    {
        set { currPetName_ = value; }
    }

    /// <summary>Whether or not the player is logged into Google Play</summary>
    public bool Authenticated
    {
        get { return Social.Active.localUser.authenticated; }
    }

    public void SetPet(GameObject value)
    {
        pet_ = value;
    }

    public GameObject ActivePet
    {
        get { return pet_; }
    }

   void Start()
    {
        //Authenticate();
        PlayGamesPlatform.Activate();
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(config);
    }

    void Authenticate()
    {
        if(Authenticated)
        {
            Debug.LogWarning("Ignoring repeat call to Authenticate.");
            return;
        }
        PlayGamesPlatform.Activate();
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(config);
    }

	void Awake ()
    {
        ui_ = Camera.main.GetComponent<UIController>();
        Load();
        ui_.UpdateEnergyTimer = true;
	}
	
	void Update ()
    {
        if (pet_ != null)
        {
            pet_.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 5.0f));
        }

        if(Input.GetKey(KeyCode.Escape) && !m_FirstTimePlayer)
        {
            Save();
            if (Application.platform == RuntimePlatform.Android)
            {
                Application.Quit();
            }
            else
            {
                Application.Quit();
            }
        }
	}

    public void SetUpGame()
    {
        //currPetName_ = Camera.main.GetComponent<UIController>().m_SelectedPet;
        for (int i = 0; i < m_PetChoices.Count; ++i)
        {
            if (m_PetChoices[i].name == currPetName_)
            {
                pet_ = (GameObject)Instantiate(m_PetChoices[i]);
                pet_.name = m_PetChoices[i].name;
            }
            m_AnimalButtons[i].SetActive(false);
        }
        if (m_FirstTimePlayer)
        {
            pet_.GetComponent<Pet>().m_IsDancing = true;
        }
    }

    void OnApplicationQuit()
    {
        if(!m_FirstTimePlayer)
        {
            PlayerPrefs.Save();
            Save();
        }
    }

    void OnApplicationPause(bool paused)
    {
        if (!m_FirstTimePlayer)
        {
            if (paused)
            {
                PlayerPrefs.Save();
                Save();
            }
            else
            {
                LoadAfterPause();
                ui_.UpdateEnergyTimer = true;
            }
        }
    }

    /// <summary>Saves all variables to file</summary>
    public void Save()
    {
        if (!File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "gpSaveData.dat"))
        {
            Debug.Log("Creating file");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + Path.DirectorySeparatorChar + "gpSaveData.dat");
            SaveData sData = new SaveData();
            Pet pet = pet_.GetComponent<Pet>();

            sData.m_Pets.Clear();

            //Player save data
            for (int i = 0; i < m_PlayerData.m_Pets.Count; ++i)
            {
                PetData pData = new PetData();

                pData.m_PetName = m_PlayerData.m_Pets[i].GetComponent<Pet>().m_PetName;
                pData.m_Nickname = m_PlayerData.m_Pets[i].GetComponent<Pet>().m_Nickname;
                pData.m_FearOne = m_PlayerData.m_Pets[i].GetComponent<Pet>().m_FearOne;
                pData.m_FearTwo = m_PlayerData.m_Pets[i].GetComponent<Pet>().m_FearTwo;
                pData.m_Hunger = m_PlayerData.m_Pets[i].GetComponent<Pet>().m_Hunger;
                pData.m_Cleanliness = m_PlayerData.m_Pets[i].GetComponent<Pet>().m_Cleanliness;
                pData.m_Boredom = m_PlayerData.m_Pets[i].GetComponent<Pet>().m_Bored;

                sData.m_Pets.Add(pData);
            }
            sData.m_StatTimer = pet_.GetComponent<Pet>().StatTimer;
            sData.m_Energy = m_PlayerData.m_Energy;
            sData.m_Scans = m_PlayerData.m_Scans;
            sData.m_Shields = m_PlayerData.m_Shields;
            sData.m_CloseDate = DateTime.Now;
            sData.m_CurrPet = pet_.name;
            sData.m_CurrPetNickname = pet.m_Nickname;
            sData.m_TimesCleaned = ui_.TimesWashed;
            sData.m_TimesExercised = ui_.TimesExercised;
            sData.m_TimesLoved = ui_.TimesLoved;
            sData.m_TimesPlayed = ui_.TimesPlayed;
            sData.m_TimesFed = ui_.TimesFed;
            sData.m_PlayerItems = ui_.m_PlayerItems;

            bf.Serialize(file, sData);
            file.Close();
        }
        else
        {
            Debug.Log("Saving to " + Application.persistentDataPath);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + Path.DirectorySeparatorChar + "gpSaveData.dat", FileMode.Open);
            SaveData sData = new SaveData();
            Pet pet = pet_.GetComponent<Pet>();

            sData.m_Pets.Clear();

            //Player save data
            for (int i = 0; i < m_PlayerData.m_Pets.Count; ++i)
            {
                PetData pData = new PetData();

                pData.m_PetName = m_PlayerData.m_Pets[i].GetComponent<Pet>().m_PetName;
                pData.m_Nickname = m_PlayerData.m_Pets[i].GetComponent<Pet>().m_Nickname;
                pData.m_FearOne = m_PlayerData.m_Pets[i].GetComponent<Pet>().m_FearOne;
                pData.m_FearTwo = m_PlayerData.m_Pets[i].GetComponent<Pet>().m_FearTwo;
                pData.m_Hunger = m_PlayerData.m_Pets[i].GetComponent<Pet>().m_Hunger;
                pData.m_Cleanliness = m_PlayerData.m_Pets[i].GetComponent<Pet>().m_Cleanliness;
                pData.m_Boredom = m_PlayerData.m_Pets[i].GetComponent<Pet>().m_Bored;

                sData.m_Pets.Add(pData);
            }

            sData.m_Energy = m_PlayerData.m_Energy;
            sData.m_Scans = m_PlayerData.m_Scans;
            sData.m_Shields = m_PlayerData.m_Shields;
            sData.m_CloseDate = DateTime.Now;
            sData.m_CurrPet = pet_.name;
            sData.m_CurrPetNickname = pet.m_Nickname;
            sData.m_TimesCleaned = ui_.TimesWashed;
            sData.m_TimesPlayed = ui_.TimesPlayed;
            sData.m_TimesExercised = ui_.TimesExercised;
            sData.m_TimesLoved = ui_.TimesLoved;
            sData.m_TimesFed = ui_.TimesFed;
            sData.m_PlayerItems = ui_.m_PlayerItems;
            sData.m_EnergyTimer = ui_.EnergyTimer;
            sData.m_StatTimer = pet_.GetComponent<Pet>().StatTimer;

            bf.Serialize(file, sData);
            file.Close();
        }
    }

    /// <summary>Loads all variables from file</summary>
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "gpSaveData.dat"))
        {
            ui_.UpdateEnergyTimer = false;
            m_FirstTimePlayer = false;
            Debug.Log("Loading from " + Application.persistentDataPath);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + Path.DirectorySeparatorChar + "gpSaveData.dat", FileMode.Open);
            SaveData sData = (SaveData)bf.Deserialize(file);

            //Player load data
            for (int i = 0; i < m_PetChoices.Count; ++i)
            {
                for (int j = 0; j < sData.m_Pets.Count; ++j)
                {
                    if (sData.m_Pets[j].m_PetName == m_PetChoices[i].name)
                    {
                        pet_ = (GameObject)Instantiate(m_PetChoices[i]);
                     
                        Pet pData = pet_.GetComponent<Pet>();
                        pData.name = m_PetChoices[i].name;
                        pData.m_FearOne = sData.m_Pets[j].m_FearOne;
                        pData.m_FearTwo = sData.m_Pets[j].m_FearTwo;
                        pData.m_Hunger = sData.m_Pets[j].m_Hunger;
                        pData.m_Cleanliness = sData.m_Pets[j].m_Cleanliness;
                        pData.m_Bored = sData.m_Pets[j].m_Boredom;
                        pData.m_Nickname = sData.m_Pets[j].m_Nickname;
                        
                        m_PlayerData.m_Pets.Add(pet_);
                    }
                }
            }

            for (int i = 0; i < m_PlayerData.m_Pets.Count; ++i)
            {
                if (m_PlayerData.m_Pets[i].name == sData.m_CurrPet)
                {
                    pet_ = m_PlayerData.m_Pets[i];
                }
            }

            ui_.TimesFed = sData.m_TimesFed;
            ui_.TimesPlayed = sData.m_TimesPlayed;
            ui_.TimesWashed = sData.m_TimesPlayed;
            ui_.TimesExercised = sData.m_TimesExercised;
            ui_.TimesLoved = sData.m_TimesLoved;
            ui_.m_PlayerItems = sData.m_PlayerItems;
            m_PlayerData.m_Shields = sData.m_Shields;
            m_PlayerData.m_Scans = sData.m_Scans;
            DateTime now = DateTime.Now;
            TimeSpan ts = now - Convert.ToDateTime(sData.m_CloseDate);
            float minutesElapsed = (float)ts.TotalMinutes / 5;
            float threeMinutesElapsed = (float)ts.TotalMinutes / 3; //every three minutes a stat needed to be increased so get how many times this needs to happen
            float secondsElapsed = (float)ts.TotalSeconds;
            float energyToAdd = 0.0f;
            float statsToAdd = 0.0f;
            ui_.EnergyTimer = sData.m_EnergyTimer - secondsElapsed;
            float timeLeft = 0.0f;
            //basically if the timer goes past zero when the game is off or minimized this will correct the timer so it doesn't start off at 5 minutes 
            while (ui_.EnergyTimer < 0.0f)
            {
                timeLeft = ui_.EnergyTimer;
                ui_.EnergyTimer = Constants.ENERGY_TIMER;
                ui_.EnergyTimer += timeLeft;
            }
            while(ui_.EnergyTimer > Constants.ENERGY_TIMER)
            {
                timeLeft = ui_.EnergyTimer;
                ui_.EnergyTimer = timeLeft - Constants.ENERGY_TIMER;
            }
            pet_.GetComponent<Pet>().StatTimer = sData.m_StatTimer - secondsElapsed;

            if (threeMinutesElapsed >= 1)
            {
                statsToAdd = threeMinutesElapsed;
            }
            else
            {
                statsToAdd = 0.0f;
            }
            if(minutesElapsed >= 1)
            {
                energyToAdd = minutesElapsed;
            }
            else
            {
                energyToAdd = 0.0f;
            }
            UpdateStats((int)statsToAdd);
            m_PlayerData.m_Energy = sData.m_Energy + (int)energyToAdd;
            if(m_PlayerData.m_Energy > Constants.DEFAULT_MAX_ENERGY)
            {
                m_PlayerData.m_Energy = Constants.DEFAULT_MAX_ENERGY;
            }

            if (minutesElapsed >= 1)
            {
                pet_.GetComponent<Pet>().AddStats((int)minutesElapsed);
            }
            SetUpGame();
            //sData.m_LoadedTime = DateTime.Now;
            file.Close();
            //return sData;
        }
        else
        {
            Debug.Log("Failed to load, file doesn't exist");
            m_FirstTimePlayer = true;
            m_PlayerData.m_Energy = Constants.DEFAULT_START_ENERGY;
            m_PlayerData.m_Shields = Constants.DEFAULT_START_SHIELDS;
            //SaveData sData = new SaveData();
            //sData.m_LoadedTime = DateTime.Now;
            //return sData;
        }
    }

    void UpdateStats(int timesToRun)
    {
        while (timesToRun > 0)
        {
            timesToRun--;
            int randNum = UnityEngine.Random.Range(0, 3);
            if (randNum == 0)
            {
                if (pet_.GetComponent<Pet>().m_Hunger < Constants.MAX_PET_STAT)
                {
                    pet_.GetComponent<Pet>().m_Hunger += Constants.STAT_INCREASE_VAL;
                }
            }
            else if (randNum == 1)
            {
                if (pet_.GetComponent<Pet>().m_Cleanliness < Constants.MAX_PET_STAT)
                {
                    pet_.GetComponent<Pet>().m_Cleanliness += Constants.STAT_INCREASE_VAL;
                }
            }
            else if (randNum == 2)
            {
                if (pet_.GetComponent<Pet>().m_Bored < Constants.MAX_PET_STAT)
                {
                    pet_.GetComponent<Pet>().m_Bored += Constants.STAT_INCREASE_VAL;
                }
            }
        }
    }
    /// <summary>This is the function used to load the contents after a pause has occured so everything has been updated properly</summary>
    void LoadAfterPause()
    {
        if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "gpSaveData.dat"))
        {
            ui_.UpdateEnergyTimer = false;
            m_FirstTimePlayer = false;
            Debug.Log("Loading from " + Application.persistentDataPath);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + Path.DirectorySeparatorChar + "gpSaveData.dat", FileMode.Open);
            SaveData sData = (SaveData)bf.Deserialize(file);

            DateTime now = DateTime.Now;
            TimeSpan ts = now - Convert.ToDateTime(sData.m_CloseDate);
            float minutesElapsed = (float)ts.TotalMinutes / 5;
            float threeMinutesElapsed = (float)ts.TotalMinutes / 3; //every three minutes a stat needed to be increased so get how many times this needs to happen
            float secondsElapsed = (float)ts.TotalSeconds;
            float energyToAdd = 0.0f;
            float statsToAdd = 0.0f;
            ui_.EnergyTimer = sData.m_EnergyTimer - secondsElapsed;
            float timeLeft = 0.0f;
            //basically if the timer goes past zero when the game is off or minimized this will correct the timer so it doesn't start off at 5 minutes 
            while (ui_.EnergyTimer < 0.0f)
            {
                timeLeft = ui_.EnergyTimer;
                ui_.EnergyTimer = Constants.ENERGY_TIMER;
                ui_.EnergyTimer += timeLeft;
            }
            while (ui_.EnergyTimer > Constants.ENERGY_TIMER)
            {
                timeLeft = ui_.EnergyTimer;
                ui_.EnergyTimer = timeLeft - Constants.ENERGY_TIMER;
            }
            pet_.GetComponent<Pet>().StatTimer = sData.m_StatTimer - secondsElapsed;

            if (threeMinutesElapsed >= 1)
            {
                statsToAdd = threeMinutesElapsed;
            }
            else
            {
                statsToAdd = 0.0f;
            }
            if (minutesElapsed >= 1)
            {
                energyToAdd = minutesElapsed;
            }
            else
            {
                energyToAdd = 0.0f;
            }
            UpdateStats((int)statsToAdd);
            m_PlayerData.m_Energy = sData.m_Energy + (int)energyToAdd;
            if (m_PlayerData.m_Energy > Constants.DEFAULT_MAX_ENERGY)
            {
                m_PlayerData.m_Energy = Constants.DEFAULT_MAX_ENERGY;
            }

            if (minutesElapsed >= 1)
            {
                pet_.GetComponent<Pet>().AddStats((int)minutesElapsed);
            }
            //sData.m_LoadedTime = DateTime.Now;
            file.Close();
            //return sData;
        }
        /*else
        {
            SaveData sData = new SaveData();
            //sData.m_LoadedTime = DateTime.Now;
            //return sData;
        }*/
    }

    #region Google Play Save/Load
    /*void SaveToCloud(string filename)
    {
        if (Authenticated)
        {
            Debug.Log("Saving game to cloud...");
            saving_ = true;
            if (filename == null)
            {
                ((PlayGamesPlatform)Social.Active).SavedGame.ShowSelectSavedGameUI("GPSave", 1, true, true, SavedGameSelected);
            }
            else
            {
                ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, SavedGameOpened);
            }
        }
    }

    public void SavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            if (saving_)
            {
                if (screenImage_ == null)
                {
                    CaptureScreenshot();
                }
                byte[] pngData = (screenImage_ != null) ? screenImage_.EncodeToPNG() : null;
                Debug.Log("Saving to " + game);
                byte[] data = sData_.ToBytes();
                SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder().WithUpdatedPlayedTime(sData_.TotalPlayingTime).WithUpdatedDescription("Saved game at " + DateTime.Now);

                if (pngData != null)
                {
                    Debug.Log("Save image of len + " + pngData.Length);
                    builder = builder.WithUpdatedPngCoverImage(pngData);
                }
                else
                {
                    Debug.Log("No image available!");
                }
                SavedGameMetadataUpdate updatedMetaData = builder.Build();
                ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(game, updatedMetaData, data, SavedGameWritten);
            }
            else
            {
                ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(game, SavedGameLoaded);
            }
        }
    }

    public void SavedGameSelected(SelectUIStatus status, ISavedGameMetadata game)
    {
        if (status == SelectUIStatus.SavedGameSelected)
        {
            string filename = game.Filename;
            Debug.Log("Opening saved game: " + game);
            if (saving_ && (filename == null || filename.Length == 0))
            {
                filename = "save" + DateTime.Now.ToBinary();
            }

            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, SavedGameOpened);
        }
        else
        {
            Debug.LogWarning("Error selecting save game: " + status);
        }
    }

    public void SavedGameLoaded(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("SaveGameLoaded, success=" + status);
            //ProcessCloudData(data);
        }
        else
        {
            Debug.LogWarning("Error reading game: " + status);
        }
    }

    public void SavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Game " + game.Description + " written");
        }
        else
        {
            Debug.LogWarning("Error saving game: " + status);
        }
    }

    void LoadFromCloud()
    {
        Debug.Log("Loading game progress from the cloud.");
        saving_ = false;
        ((PlayGamesPlatform)Social.Active).SavedGame.ShowSelectSavedGameUI("Select saved game to load", 1, false, false, SavedGameSelected);
    }

    void ProcessCloudData(byte[] cloudData)
    {
        if (cloudData == null)
        {
            Debug.Log("No data saved to the cloud yet...");
            return;
        }
        Debug.Log("Decoding cloud data from bytes.");
        //SaveData sData = SaveData
    }

    public void CaptureScreenshot()
    {
        screenImage_ = new Texture2D(Screen.width, Screen.height);
        screenImage_.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage_.Apply();
        Debug.Log("Captured screen: " + screenImage_);
    }*/
    #endregion
}

[Serializable]
class SaveData
{

    /*public byte[] ToBytes()
    {
        return System.Text.ASCIIEncoding.Default.GetBytes(ToString());
    }

    public TimeSpan TotalPlayingTime
    {
        get
        {
            TimeSpan delta = DateTime.Now.Subtract(m_LoadedTime);
            return m_PlayTime.Add(delta);
        }
    }

    public override string ToString()
    {
        string s = "SDv3:";
        //int i;
        
        return s;
    }

    public static SaveData FromBytes(byte[] b)
    {
        return SaveData.FromString(System.Text.ASCIIEncoding.Default.GetString(b));
    }

    public static SaveData FromString(string s)
    {
        SaveData sd = new SaveData();
        string[] p = s.Split(new char[] { ':' });
        if(!p[0].StartsWith("GPv"))
        {
            Debug.LogError("Failed to parse game progress from " + s);
            return sd;
        }

        return sd;
    }*/

    //public DateTime m_LoadedTime;
    //public TimeSpan m_PlayTime;
    /// <summary>List of the pets the player owns</summary>
    public List<PetData> m_Pets = new List<PetData>();

    /// <summary>List of the items the player owns</summary>
    public List<string> m_PlayerItems = new List<string>();

    /// <summary>Player's currently active pet</summary>
    public string m_CurrPet; //Player's currently active pet

    /// <summary>Player's currently active pet's nickname</summary>
    public string m_CurrPetNickname;

    /// <summary>Player's current shields</summary>
    public int m_Shields;

    public int m_Scans;

    /// <summary>Player's energy at the time of the save</summary>
    public int m_Energy;

    /// <summary>Date the player stopped playing </summary>
    public DateTime m_CloseDate; //Date the player stopped playing

    /// <summary>Amount of times the player has fed their pet</summary>
    public int m_TimesFed;

    /// <summary>Amount of times the player has played with their pet</summary>
    public int m_TimesPlayed;

    /// <summary>Amount of times the player has washed their pet</summary>
    public int m_TimesCleaned;

    /// <summary>Amount of times the player has washed their pet</summary>
    public int m_TimesExercised;

    /// <summary>Amount of times the player has washed their pet</summary>
    public int m_TimesLoved;

    /// <summary>Player's Energy Timer when saved</summary>
    public float m_EnergyTimer;

    /// <summary>Player's Stat Timer for increasing stats</summary>
    public float m_StatTimer;
}

[Serializable]
class PetData
{
    /// <summary>Pet's name</summary>
    public string m_PetName;

    /// <summary>Pet's nickname</summary>
    public string m_Nickname;

    /// <summary>Pet's first fear</summary>
    public string m_FearOne;

    /// <summary>Pet's second fear</summary>
    public string m_FearTwo;

    /// <summary>Pet's hunger value</summary>
    public int m_Hunger;

    /// <summary>Pet's cleanliness value</summary>
    public int m_Cleanliness;

    /// <summary>Pet's boredom value</summary>
    public int m_Boredom;
}
