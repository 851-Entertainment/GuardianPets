using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Achievement
{
    /// <summary>Name of the achievement</summary>
    private string name_;

    /// <summary>Description of the achievement</summary>
    private string description_;

    /// <summary>Flag indicating whether the achievement is unlocked</summary>
    private bool unlocked_;

    /// <summary>Flag indicating whether the trophy has been unlocked</summary>
    private bool unlockedTrophy_;

    /// <summary>Amount of shields the achievement is worth</summary>
    private int points_;

    /// <summary>Index of the icon in the sprite array</summary>
    private int spriteIndex_;

    /// <summary>Flag indicating whether the achievement unlocks a trophy</summary>
    private bool trophy_;

    /// <summary>Reference to the achievement object</summary>
    private GameObject achievementRef_;

    /// <summary>List of achievements this achievement is dependent on</summary>
    private List<Achievement> dependencies_ = new List<Achievement>();

    /// <summary>Achievement that is dependent on this achievement</summary>
    private string child;

    #region Properties

    public string Name
    {
        get { return name_; } set { name_ = value; }
    }

    public string Description
    {
        get { return description_; } set { description_ = value; }
    }

    public bool Unlocked
    {
        get { return unlocked_; } set { unlocked_ = value; }
    }

    public bool UnlockedTrophy
    {
        get { return unlockedTrophy_; }
        set { unlockedTrophy_ = value; }
    }

    public int Points
    {
        get { return points_; } set { points_ = value; }
    }

    public int SpriteIndex
    {
        get { return spriteIndex_; } set { spriteIndex_ = value; }
    }
    public string Child
    {
        get { return child; } set { child = value; }
    }

    #endregion

    /// <param name="name">The name of the achievement</param>
    /// <param name="description">The achievement's description</param>
    /// <param name="points">The amount of points the achievement is worth</param>
    /// <param name="spriteIndex">The spriteindex</param>
    /// <param name="achievementRef">A reference to the achievement's gameobject</param>
    public Achievement(string name, string description, int points, int spriteIndex, GameObject achievementRef, bool trophy)
    {
        this.name_ = name;
        this.description_ = description;
        this.unlocked_ = false;
        this.unlockedTrophy_ = false;
        this.points_ = points;
        this.spriteIndex_ = spriteIndex;
        this.achievementRef_ = achievementRef;
       
        this.trophy_ = trophy;
       
        //Loads the achievement so that we have the correct information
        LoadAchievement();
    }

    /// <summary>Adds a dependency to the achievement</summary>
    /// <param name="dependency"></param>
    public void AddDependency(Achievement dependency)
    {
        dependencies_.Add(dependency);
    }

    /// <summary>Earns the achievements</summary>
    /// <returns>True if the achievement was earned</returns>
    public bool EarnAchievement()
    {   
        //If the achievement isn't unlocked and all the dependencies are unlocked
        if (!unlocked_ && !dependencies_.Exists(x => x.unlocked_ == false))
        {   
            //Changes the sprite to the unlocked sprite
            achievementRef_.GetComponent<Image>().sprite = AchievementManager.Instance.m_UnlockedSprite;

            //Saves the achievement
            SaveAchievement(true);     

            if (child != null) //Checks if the achievement has a child
            {   
                //Tries to earn the child achievements
                AchievementManager.Instance.EarnAchievement(child);
            }
            return true;
        }
        return false;
    }

    /// <summary>Saves the achievement</summary>
    /// <param name="value">If the achievement is earned</param>
    public void SaveAchievement(bool value)
    {
        unlocked_ = value; //Sets the value
        unlockedTrophy_ = trophy_;

        //Gets the amount of points
        int tmpPoints = PlayerPrefs.GetInt("Points");

        //Stores the amount of points 
        PlayerPrefs.SetInt("Points", tmpPoints += points_);

        //Stores the achievement's status
        PlayerPrefs.SetInt(name_, value ? 1 : 0);

        //Saves the achievement
        PlayerPrefs.Save();
    }

    /// <summary>Loads the achievement</summary>
    public void LoadAchievement()
    {
        //Loads the status
        unlocked_ = PlayerPrefs.GetInt(name_) == 1 ? true : false;
        unlockedTrophy_ = PlayerPrefs.GetInt(name_) == 1 ? true : false;

        if (unlocked_) //If the achievement is unlocked then we need to change the sprite and aquire the points
        {
            AchievementManager.Instance.m_TextPoints.text = "Points: " + PlayerPrefs.GetInt("Points");
            achievementRef_.GetComponent<Image>().sprite = AchievementManager.Instance.m_UnlockedSprite;
        }
        
    }   
}
