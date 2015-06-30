using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrophyManager : MonoBehaviour
{
    /*/// <summary>All of the unlockable trophies the player can get</summary>
    public List<GameObject> m_UnlockableTrophies = new List<GameObject>();

    /// <summary>///Achievement Manager in the scene</summary>
    private AchievementManager achievementManager_;
    /// <summary>///Trophies the player has</summary>
    private List<Achievement> trophies_ = new List<Achievement>();

	void Start ()
    {
        achievementManager_ = GameObject.Find("AchievementManager").GetComponent<AchievementManager>();

        for (int i = 0; i < achievementManager_.m_Achievements.Count; ++i)
        {
            if(achievementManager_.GetComponent<Achievement>().Unlocked)
            {
                trophies_.Add(achievementManager_.m_Achievements[i.ToString()]);
                SetTrophyActive();
            }  
        }
	}
	
	void Update () 
    {
        IncrementTrophy();
	}

    /// <param name="Add to the list of trophies">///If an anchievement has been unlocked, add it to the trophy list</param>
    void IncrementTrophy()
    {
        for (int i = 0; i < achievementManager_.m_Achievements.Count; ++i)
        {      
            //if there has been an unlocked achievement then it's time to unlock a trophy
            if (achievementManager_.m_Achievements.Count > trophies_.Count && !trophies_.Contains(achievementManager_.m_Achievements[i.ToString()]))
            {
                trophies_.Add(achievementManager_.m_Achievements[i.ToString()]);
                //add the unlocked achievement to the trophy list
                SetTrophyActive();
            }        
        }
    }

    /// <param name="Activate Unlocked Trophy">//If a trophy has been unlocked set the trophy active in the scene</param>
    void SetTrophyActive()
    {
        for (int i = 0; i < trophies_.Count; ++i)
        {
            for (int j = 0; j < m_UnlockableTrophies.Count; ++j)
            {
                if(trophies_[i].Unlocked)
                {
                    m_UnlockableTrophies[j].SetActive(true);
                }
            }
        }
    }*/
}
