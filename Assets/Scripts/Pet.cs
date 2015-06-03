using UnityEngine;
using System.Collections;

public class Pet : MonoBehaviour
{
    public string m_PetName; //The name of the pet
    public string m_Nickname; //Nickname of the pet -- created by the player
    public int m_Hunger; //The pet's hunger level -- 100 is max, 0 is min. Max is starving, min is full. Max = bad, Min = good.
    public int m_Cleanliness; //The pet's cleanliness level -- 100 is max, 0 is min. Max is filthy, min is spotless. Max = bad, Min = good.
    public int m_Bored; //The pet's bored level -- 100 is max, 0 is min. Max is extremely bored, min is entertained. Max = bad, Min = good.
    public string m_FearOne; //The fear the player chose when they picked the pet
    public string m_FearTwo; //This fear is only used if the player pays for a second fear slot

    private GameController gc_;
    private float statTimer_;

	void Start ()
    {
        gc_ = Camera.main.GetComponent<GameController>();
        statTimer_ = Constants.STAT_TIMER;
	}
	
	void Update () 
    {
        UpdateStats();
	}

    //This function runs a timer, every minute a stat is increased at random
    void UpdateStats()
    {
        statTimer_ -= Time.deltaTime;
        if(statTimer_ <= 0.0)
        {
            int randNum = Random.Range(0, 3);
            if(randNum == 0)
            {
                if (m_Hunger < Constants.MAX_PET_STAT)
                { 
                    m_Hunger += Constants.STAT_INCREASE_VAL; 
                }
            }
            else if(randNum == 1)
            {
                if(m_Cleanliness < Constants.MAX_PET_STAT)
                {
                    m_Cleanliness += Constants.STAT_INCREASE_VAL;
                }
            }
            else if(randNum == 2)
            {
                if (m_Bored < Constants.MAX_PET_STAT)
                {
                    m_Bored += Constants.STAT_INCREASE_VAL;
                }
            }
            statTimer_ = Constants.STAT_TIMER;
        }
    }

    //This function is only called from places where stats need to be added off of the timer, such as when the game loads again after being closed
    //numStats is the number of times a stat needs to be added - to clarify, NOT the amount of stats to be added(30) but the amount of times a stat is to be added(3)
    public void AddStats(int numStats)
    {
        int counter = 1;
        while(counter <= numStats)
        {
            int randNum = Random.Range(0, 3);
            if (randNum == 0)
            {
                if (m_Hunger < Constants.MAX_PET_STAT)
                {
                    m_Hunger += Constants.STAT_INCREASE_VAL;
                }
            }
            else if (randNum == 1)
            {
                if (m_Cleanliness < Constants.MAX_PET_STAT)
                {
                    m_Cleanliness += Constants.STAT_INCREASE_VAL;
                }
            }
            else if (randNum == 2)
            {
                if (m_Bored < Constants.MAX_PET_STAT)
                {
                    m_Bored += Constants.STAT_INCREASE_VAL;
                }
            }
            counter++;
        }
    }

    //This function is called whenever the player presses one of the three action buttons and returns a true or false value
    //True = all conditions are met to get a shield
    //False = not all conditions are met to get a shield
    public bool CheckShieldConditions()
    {
        bool retVal;
        if (m_Hunger == Constants.MIN_PET_STAT && m_Cleanliness == Constants.MIN_PET_STAT && m_Bored == Constants.MIN_PET_STAT)
        {
            retVal = true;
        }
        else
        {
            retVal = false;
        }
        return retVal;
    }
}