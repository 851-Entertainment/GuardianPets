using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Soomla.Store;

public class AchievementManager : MonoBehaviour 
{
    /// <summary>A prefab used for creating a new achievement</summary>
    public GameObject m_AchievementPrefab;

    /// <summary>The UI controller script</summary>
    public UIController m_UIController;

    /// <summary> An array containing the icons used for the achievements</summary>
    public Sprite[] m_Sprites;

    /// <summary>A reference to the active button(category)</summary>
    private AchievementButton activeButton_;

    /// <summary>A reference to the scrollrect that controls the achievements in the menu</summary>
    public ScrollRect m_ScrollRect;

    /// <summary>A reference to the achievement menu, this is used for hiding and showing the menu</summary>
    public GameObject m_AchievementMenu;

    /// <summary>This is a prefab for the achievement that we are showing when an achievement has been earned</summary>
    public GameObject m_VisualAchievement;

    /// <summary>This dictionary contains all achievements</summary>
    public Dictionary<string, Achievement> m_Achievements = new Dictionary<string, Achievement>();

    /// <summary>List of the trophies to unlock</summary>
    public List<GameObject> m_Trophy = new List<GameObject>();

    /// <summary>Index for where in the trophy list you are at for unlocks</summary>
    private int trophyIndex = 0;

    /// <summary>This sprite is used for indicating if an achievement is unlocked</summary>
    public Sprite m_UnlockedSprite;

    /// <summary>A reference to the text that shows the points inside the menu</summary>
    public Text m_TextPoints;

    /// <summary>The time it takes for the inventory to fade in and out in seconds</summary>
    private int fadeTime_ = 2;

    /// <summary>An instance for the AchievementManager, this is used for the singleton pattern</summary>
    private static AchievementManager instance_;

    /// <summary>A property for accesing the singleton</summary>
    public static AchievementManager Instance
    {
        get 
        {
            if (instance_ == null) //If the instance isn't instantiated we need to find it
            {
                instance_ = GameObject.FindObjectOfType<AchievementManager>();
            }
            return AchievementManager.instance_; 
        }
    }

	void Start () 
    {
        //Sets the active button to as the general button, so that we have something to show the first time we open the inventory
        activeButton_ = GameObject.Find("GeneralBtn").GetComponent<AchievementButton>();

        #region Creating Achievements
        //Creates the general achievements
        CreateAchievement("General", "Feed I", "Feed your pet 25 times.", 1, 0);
        CreateAchievement("General", "Feed II", "Feed your pet 50 times.", 1, 0);
        CreateAchievement("General", "Feed III", "Feed your pet 75 times.", 2, 0);
        CreateAchievement("General", "Feed IV", "Feed your pet 100 times.", 3, 0);
        CreateAchievement("General", "Feed V", "Feed your pet 250 times.", 4, 0);
        CreateAchievement("General", "Feed VI", "Feed your pet 500 times.", 5, 0);
        CreateAchievement("General", "Feed VII", "Feed your pet 750 times.", 7, 0);
        CreateAchievement("General", "Feed VIII", "Feed your pet 1000 times.", 10, 0);

        CreateAchievement("General", "Play I", "Play with your pet 25 times.", 1, 0);
        CreateAchievement("General", "Play II", "Play with your pet 50 times.", 1, 0);
        CreateAchievement("General", "Play III", "Play with your pet 75 times.", 2, 0);
        CreateAchievement("General", "Play IV", "Play with your pet 100 times.", 3, 0);
        CreateAchievement("General", "Play V", "Play with your pet 250 times.", 4, 0);
        CreateAchievement("General", "Play VI", "Play with your pet 500 times.", 5, 0);
        CreateAchievement("General", "Play VII", "Play with your pet 750 times.", 7, 0);
        CreateAchievement("General", "Play VIII", "Play with your pet 1000 times.", 10, 0);

        CreateAchievement("General", "Wash I", "Wash your pet 25 times.", 1, 0);
        CreateAchievement("General", "Wash II", "Wash your pet 50 times.", 1, 0);
        CreateAchievement("General", "Wash III", "Wash your pet 75 times.", 2, 0);
        CreateAchievement("General", "Wash IV", "Wash your pet 100 times.", 3, 0);
        CreateAchievement("General", "Wash V", "Wash your pet 250 times.", 4, 0);
        CreateAchievement("General", "Wash VI", "Wash your pet 500 times.", 5, 0);
        CreateAchievement("General", "Wash VII", "Wash your pet 750 times.", 7, 0);
        CreateAchievement("General", "Wash VIII", "Wash your pet 1000 times.", 10, 0);

        CreateAchievement("General", "Exercise I", "Exercise your pet 25 times.", 1, 0);
        CreateAchievement("General", "Exercise II", "Exercise your pet 50 times.", 1, 0);
        CreateAchievement("General", "Exercise III", "Exercise your pet 75 times.", 2, 0);
        CreateAchievement("General", "Exercise IV", "Exercise your pet 100 times.", 3, 0);
        CreateAchievement("General", "Exercise V", "Exercise your pet 250 times.", 4, 0);
        CreateAchievement("General", "Exercise VI", "Exercise your pet 500 times.", 5, 0);
        CreateAchievement("General", "Exercise VII", "Exercise your pet 750 times.", 7, 0);
        CreateAchievement("General", "Exercise VIII", "Exercise your pet 1000 times.", 10, 0);

        CreateAchievement("General", "Fill Love I", "Fill the love meter once.", 1, 0);
        CreateAchievement("General", "Fill Love II", "Fill the love mever 25 times.", 2, 0);
        CreateAchievement("General", "Fill Love III", "Fill the love meter 50 times.", 3, 0);
        CreateAchievement("General", "Fill Love IV", "Fill the love meter 75 times.", 4, 0);
        CreateAchievement("General", "Fill Love V", "Fill the love meter 100 times.", 5, 0);

        CreateAchievement("General", "Unlock Item I", "Unlock your first item.", 1, 0);
        CreateAchievement("General", "Unlock Item II", "Unlock five items.", 2, 0);
        CreateAchievement("General", "Unlock Item III", "Unlock 10 items.", 3, 0);
        #endregion

        //Makes sure that the achievements are disabled when we start        
        foreach (GameObject achievementList in GameObject.FindGameObjectsWithTag("AchievementList"))
        {
            achievementList.SetActive(false);
        }

        activeButton_.Click(); //Clicks the active button

        m_AchievementMenu.SetActive(false); //Hides the achievement menu
	}
	
	// Update is called once per frame
	void Update () 
    {   
        if (Input.GetKeyDown(KeyCode.I))
        {
            m_AchievementMenu.SetActive(!m_AchievementMenu.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerPrefs.DeleteAll();
        }

        #region Check Times Played
        if (m_UIController.TimesPlayed == 25)
        {
            EarnAchievement("Play I");
        }
        else if(m_UIController.TimesPlayed == 50)
        {
            EarnAchievement("Play II");
        }
        else if (m_UIController.TimesPlayed == 75)
        {
            EarnAchievement("Play III");
        }
        else if (m_UIController.TimesPlayed == 100)
        {
            EarnAchievement("Play IV");
        }
        else if (m_UIController.TimesPlayed == 250)
        {
            EarnAchievement("Play V");
        }
        else if (m_UIController.TimesPlayed == 500)
        {
            EarnAchievement("Play VI");
        }
        else if (m_UIController.TimesPlayed == 750)
        {
            EarnAchievement("Play VII");
        }
        else if (m_UIController.TimesPlayed == 1000)
        {
            EarnAchievement("Play VIII");
        }
        #endregion

        #region Check Times Fed
        if (m_UIController.TimesFed == 25)
        {
            EarnAchievement("Feed I");
        }
        else if (m_UIController.TimesFed == 50)
        {
            EarnAchievement("Feed II");
        }
        else if (m_UIController.TimesFed == 75)
        {
            EarnAchievement("Feed III");
        }
        else if (m_UIController.TimesFed == 100)
        {
            EarnAchievement("Feed IV");
        }
        else if (m_UIController.TimesFed == 250)
        {
            EarnAchievement("Feed V");
        }
        else if (m_UIController.TimesFed == 500)
        {
            EarnAchievement("Feed VI");
        }
        else if (m_UIController.TimesFed == 750)
        {
            EarnAchievement("Feed VII");
        }
        else if (m_UIController.TimesFed == 1000)
        {
            EarnAchievement("Feed VIII");
        }
        #endregion

        #region Check Times Washed
        if (m_UIController.TimesWashed == 25)
        {
            EarnAchievement("Wash I");
        }
        else if (m_UIController.TimesWashed == 50)
        {
            EarnAchievement("Wash II");
        }
        else if (m_UIController.TimesWashed == 75)
        {
            EarnAchievement("Wash III");
        }
        else if (m_UIController.TimesWashed == 100)
        {
            EarnAchievement("Wash IV");
        }
        else if (m_UIController.TimesWashed == 250)
        {
            EarnAchievement("Wash V");
        }
        else if (m_UIController.TimesWashed == 500)
        {
            EarnAchievement("Wash VI");
        }
        else if (m_UIController.TimesWashed == 750)
        {
            EarnAchievement("Wash VII");
        }
        else if (m_UIController.TimesWashed == 1000)
        {
            EarnAchievement("Wash VIII");
        }
        #endregion
    }

    /// <summary>Attempts to earn an achievement</summary>
    /// <param name="title">The name of the achievement</param>
    public void EarnAchievement(string title)
    {   
        //Checks if its the first time we try to unlock the achievement
        if (m_Achievements[title].EarnAchievement())
        {
            //Instantiates the visual achievement on the screen, so that the user can see what he/she just earned
            GameObject achievement = (GameObject)Instantiate(m_VisualAchievement);

            //Makes the visual achievement a child of the EarnCanvas, so that it is visible for the user
            SetAchievementInfo("EarnCanvas", achievement, title);

            //Updates the points in the top left corner of the menu
            m_TextPoints.text = "Points: " + PlayerPrefs.GetInt("Points");
            
            //Makes the achievement fad in and out of the screen
            StartCoroutine(FadeAchievement(achievement));

            //Give player shields equal to the number of points the achievement is worth
            GuardianPetsAssets.SHIELD_CURRENCY.Give(m_Achievements[title].Points);   
        }
    }

    /// <summary>Creates an achievement</summary>
    /// <param name="parent">The achievement's parent</param>
    /// <param name="title">The title of the achievement</param>
    /// <param name="description">The achievement's description</param>
    /// <param name="points">The amount of points the achievement is worth</param>
    /// <param name="spriteIndex">The index used to finde an icon inside the sprites array</param>
    /// <param name="dependencies"></param>
    public void CreateAchievement(string parent, string title, string description ,int points, int spriteIndex, string[] dependencies = null)
    {   
        //Creates the achievement gameobject
        GameObject achievement = (GameObject)Instantiate(m_AchievementPrefab);

        //Creates the achievement
        Achievement newAchievement = new Achievement(title, description, points, spriteIndex, achievement);

        //Adds the achievement to our dictionary
        m_Achievements.Add(title, newAchievement);
       
        //Makes sure that the achievement contains the correct info
        SetAchievementInfo(parent, achievement,title);

        if (dependencies != null) //Checks if we need to make the achievement dependent on other acievments
        {
            foreach (string achievementTitle in dependencies) //Creates the dependencies
            {
                Achievement dependency = m_Achievements[achievementTitle];
                dependency.Child = title;
                newAchievement.AddDependency(dependency);
            }
        }

        
    }

    /// <summary>Fills the onscreen achievement with information</summary>
    /// <param name="parent">The achievement's parent</param>
    /// <param name="achievement">The achievement to set the information for</param>
    /// <param name="title">The achievement's title</param>
    public void SetAchievementInfo(string parent, GameObject achievement, string title)
    {
        //Sets the parent of the achievements
        achievement.transform.SetParent(GameObject.Find(parent).transform);

        //Makes sure that it has the correct size
        achievement.transform.localScale = new Vector3(1, 1, 1);

        //Sets the information of the achievement
        achievement.transform.GetChild(0).GetComponent<Text>().text = title;
        achievement.transform.GetChild(1).GetComponent<Text>().text = m_Achievements[title].Description;
        achievement.transform.GetChild(2).GetComponent<Text>().text = m_Achievements[title].Points.ToString();
        achievement.transform.GetChild(3).GetComponent<Image>().sprite = m_Sprites[m_Achievements[title].SpriteIndex];
    }

    /// <summary>Changes the category</summary>
    /// <param name="button">The button we just clicked</param>
    public void ChangeCategory(GameObject button)
    {
        //Creates a reference to buttonScript on the button we just clicked
        AchievementButton achievementButton = button.GetComponent<AchievementButton>();

        //Changes the content, that the scroll rect controls
        m_ScrollRect.content = achievementButton.m_AchievementList.GetComponent<RectTransform>();

        //Clicks in the current button
        achievementButton.Click();

        //Clicks out the active button
        activeButton_.Click();

        //Changes the active button
        activeButton_ = achievementButton;
    }

    /// <summary>Fades the achievement in and out</summary>
    /// <param name="achievement">The achievement to fade</param>
    /// <returns></returns>
    private IEnumerator FadeAchievement(GameObject achievement)
    {
        //Creates a reference to the CanvasGroup Script
        CanvasGroup canvasGroup = achievement.GetComponent<CanvasGroup>();

        //Calculates the fade rate
        float rate = 1.0f / fadeTime_;

        //Sets the starting value
        int startAlpha = 0;
        int endAlpha = 1;

        for (int i = 0; i < 2; i++) //Runs the code 2 times to make sure that we fade in and out
        {
            float progress = 0.0f; //Resets the progress

            while (progress < 1.0) //Runs the fading as long as the progress is less than 1
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress); //Lerps the alpha

                progress += rate * Time.deltaTime; //Calculates the progress based on the time

                yield return null;
            }
            yield return new WaitForSeconds(2); //Waits for 2 seconds so that we can read the achievement
            
            //Switches around the values
            startAlpha = 1; 
            endAlpha = 0;
        }

        //Destroys the visual object
        Destroy(achievement);
    }
    /// <param name="achievement">Activate a trophy if it has been unlocked</param>
    void CheckTrophy(Achievement achievement)
    {
        if(achievement.UnlockedTrophy)
        {
            m_Trophy[trophyIndex].SetActive(true);
            trophyIndex++;
        }
    }
}