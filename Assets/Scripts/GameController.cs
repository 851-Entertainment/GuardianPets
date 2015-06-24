using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class GameController : MonoBehaviour
{
    public bool m_FirstTimePlayer = false; //Bool to determine whether or not we need to run the initial play segment
    public List<GameObject> m_PetChoices = new List<GameObject>(); //List of possible pets -- this needs to stay here in case they decide to buy more pets
    public PlayerData m_PlayerData; //The player data
    public List<GameObject> m_AnimalButtons;
    public List<Item> m_Items = new List<Item>(); //List of available items to purchase

    private UIController ui_;
    private string currPetName_; //Name of the current pet, used to find the right pet in the list
    private float saveTimer_; //Timer between autosaves
    private float maxSaveTime_; //Max time between autosaves
    private GameObject pet_;

    public string CurrentPet
    {
        set { currPetName_ = value; }
    }

    public void SetPet(GameObject value)
    {
        pet_ = value;
    }

    public GameObject ActivePet
    {
        get { return pet_; }
    }

	void Awake ()
    {
        Load();
        ui_ = Camera.main.GetComponent<UIController>();
	}
	
	void Update ()
    {
        if (pet_ != null)
        {
            pet_.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 5.0f));
        }

        if(Input.GetKey(KeyCode.Escape))
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
        PlayerPrefs.Save();
        Save();
    }

    //When this Save function is called, all other save functions get called. No where else should save.
    public void Save()
    {
        if (!File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "gpSaveData.dat"))
        {
            Debug.Log("Creating file");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + Path.DirectorySeparatorChar + "gpSaveData.dat");
            SaveData sData = new SaveData();
            PetData pData = new PetData();
            Pet pet = pet_.GetComponent<Pet>();

            //Player save data
            for (int i = 0; i < m_PlayerData.m_Pets.Count; ++i)
            {
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
            sData.m_Shields = m_PlayerData.m_Shields;
            sData.m_CloseDate = DateTime.Now;
            sData.m_CurrPet = pet_.name;
            sData.m_CurrPetNickname = pet.m_Nickname;
            sData.m_TimesCleaned = ui_.TimesWashed;
            sData.m_TimesPlayed = ui_.TimesPlayed;
            sData.m_TimesFed = ui_.TimesFed;

            bf.Serialize(file, sData);
            file.Close();
        }
        else
        {
            Debug.Log("Saving to " + Application.persistentDataPath);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + Path.DirectorySeparatorChar + "gpSaveData.dat", FileMode.Open);
            SaveData sData = new SaveData();
            PetData pData = new PetData();
            Pet pet = pet_.GetComponent<Pet>();

            //Player save data
            for (int i = 0; i < m_PlayerData.m_Pets.Count; ++i)
            {
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
            sData.m_Shields = m_PlayerData.m_Shields;
            sData.m_CloseDate = DateTime.Now;
            sData.m_CurrPet = pet_.name;
            sData.m_CurrPetNickname = pet.m_Nickname;
            sData.m_TimesCleaned = ui_.TimesWashed;
            sData.m_TimesPlayed = ui_.TimesPlayed;
            sData.m_TimesFed = ui_.TimesFed;

            bf.Serialize(file, sData);
            file.Close();
        }
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "gpSaveData.dat"))
        {
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
                        if(m_PetChoices[i].name == sData.m_CurrPet)
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
                        else
                        {
                            Pet pData = m_PetChoices[i].GetComponent<Pet>();
                            pData.m_Nickname = sData.m_Pets[j].m_Nickname;
                            pData.m_FearOne = sData.m_Pets[j].m_FearOne;
                            pData.m_FearTwo = sData.m_Pets[j].m_FearTwo;
                            pData.m_Bored = sData.m_Pets[j].m_Boredom;
                            pData.m_Cleanliness = sData.m_Pets[j].m_Cleanliness;
                            pData.m_Hunger = sData.m_Pets[j].m_Hunger;

                            m_PlayerData.m_Pets.Add(m_PetChoices[i]);
                        }
                    }
                }
            }
            m_PlayerData.m_Shields = sData.m_Shields;
            DateTime now = DateTime.Now;
            TimeSpan ts = now - Convert.ToDateTime(sData.m_CloseDate);
            float minutesElapsed = (float)ts.TotalMinutes / 5;
            float energyToAdd;
            if(minutesElapsed >= 1)
            {
                energyToAdd = minutesElapsed;
            }
            else
            {
                energyToAdd = 0;
            }

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
            file.Close();
            m_FirstTimePlayer = false;
        }
        else
        {
            Debug.Log("Failed to load, file doesn't exist");
            m_FirstTimePlayer = true;
            m_PlayerData.m_Energy = Constants.DEFAULT_START_ENERGY;
            m_PlayerData.m_Shields = Constants.DEFAULT_START_SHIELDS;
        }
    }
}

[Serializable]
class SaveData
{
    /// <summary>List of the pets the player owns</summary>
    public List<PetData> m_Pets = new List<PetData>();

    /// <summary>List of the items the player owns</summary>
    public List<GameObject> m_PlayerItems = new List<GameObject>();

    /// <summary>Player's currently active pet</summary>
    public string m_CurrPet; //Player's currently active pet

    /// <summary>Player's currently active pet's nickname</summary>
    public string m_CurrPetNickname;

    /// <summary>Player's current shields</summary>
    public int m_Shields;

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
