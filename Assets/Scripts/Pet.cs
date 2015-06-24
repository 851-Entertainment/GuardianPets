using UnityEngine;
using System.Collections;

public class Pet : MonoBehaviour
{
    public bool m_IsSick = false;
    public bool m_IsHungry;
    public bool m_NeedsCleaning;
    public bool m_IsBored;
    public string m_PetName; //The name of the pet
    public string m_Nickname; //Nickname of the pet -- created by the player
    public int m_Hunger; //The pet's hunger level -- 100 is max, 0 is min. Max is starving, min is full. Max = bad, Min = good.
    public int m_Cleanliness; //The pet's cleanliness level -- 100 is max, 0 is min. Max is filthy, min is spotless. Max = bad, Min = good.
    public int m_Bored; //The pet's bored level -- 100 is max, 0 is min. Max is extremely bored, min is entertained. Max = bad, Min = good.
    public int m_Love;
    public int m_Exercise;
    public float m_SickTimerMaxVal = 100.0f;
    public string m_FearOne; //The fear the player chose when they picked the pet
    public string m_FearTwo; //This fear is only used if the player pays for a second fear slot

    public bool m_IsDancing;
    public bool m_IsPlaying;
    private float playingTimer_ = 0.0f;

    private GameController gc_;
    private Animator animator_;
    private float statTimer_;
    private float sickTimer_;
    private float ranNumTimer_ = 0.0f;
    private float bubbleTimer_ = 0.0f;
    private bool updateSpeech_ = false;

	void Start ()
    {
        gc_ = Camera.main.GetComponent<GameController>();
        statTimer_ = Constants.STAT_TIMER;
        animator_ = GetComponent<Animator>();
      
	}
	
	void Update () 
    {
        UpdateStats();
        UpdateAnim();
        UpdateSpeechBubble();
        SpeechBubbleTimer();
        MakePetSick();
        m_Love = m_Hunger + m_Cleanliness + m_Bored;
	}

    void MakePetSick()
    {
        sickTimer_ += Time.deltaTime;
        if(sickTimer_ >= m_SickTimerMaxVal)
        {
            sickTimer_ = 0.0f;
            int randNum = Random.Range(0, 100);
            if(randNum == 50 && !m_IsSick)
            {
                m_IsSick = true;
            }
        }
    }


    void UpdateAnim()
    {
        if (m_IsDancing)
        {
            playingTimer_ += Time.deltaTime;
        }
        if(playingTimer_ >= 2.0f)
        {
            m_IsDancing = !m_IsDancing;
            playingTimer_ = 0.0f;
        }
        animator_.SetBool("Playing", m_IsPlaying);
        animator_.SetBool("Dancing ", m_IsDancing);
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

    /// <summary>Adds to the pet's current stat values</summary>
    /// <param name="numStats">Number of times to add to the pet's current stat values</param>
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

    void UpdateSpeechBubble()
    {
        ranNumTimer_ += Time.deltaTime;
        if (ranNumTimer_ >= 45.0f)
        {
            ranNumTimer_ = 0.0f;
            int randNum = Random.Range(0, 3);
            if (randNum == 0)
            {
                m_IsHungry = true;
            }
            else if (randNum == 1)
            {
                m_IsBored = true;
            }
            else if (randNum == 2)
            {
                m_NeedsCleaning = true;
            }
        }
      
    }

    void SpeechBubbleTimer()
    {
        if(m_NeedsCleaning || m_IsBored || m_IsHungry)
        {
            updateSpeech_ = true;
        }

        if(updateSpeech_)
        {
            bubbleTimer_ += Time.deltaTime;
        }

        if(bubbleTimer_ >= 4.0f)
        {
            m_NeedsCleaning = false;
            m_IsBored = false;
            m_IsHungry = false;
            updateSpeech_ = false;
            bubbleTimer_ = 0.0f;
        }
    }
}