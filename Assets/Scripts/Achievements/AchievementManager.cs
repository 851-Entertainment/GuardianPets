using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AchievementManager : MonoBehaviour 
{
    /// <summary>
    /// A prefab used for creating a new achievement
    /// </summary>
    public GameObject m_AchievementPrefab;

    /// <summary>
    /// An array containing the icons used for the achievements
    /// </summary>
    public Sprite[] m_Sprites;

    /// <summary>
    /// A reference to the active button(category)
    /// </summary>
    private AchievementButton activeButton_;

    /// <summary>
    /// A reference to the scrollrect that controls the achievements in the menu
    /// </summary>
    public ScrollRect m_ScrollRect;

    /// <summary>
    /// A reference to the achievement menu, this is used for hiding and showing the menu
    /// </summary>
    public GameObject m_AchievementMenu;

    /// <summary>
    /// This is a prefab for the achievement that we are showing when an achievement has been earned
    /// </summary>
    public GameObject m_VisualAchievement;

    /// <summary>
    /// This dictionary contains all achievements
    /// </summary>
    public Dictionary<string, Achievement> m_Achievements = new Dictionary<string, Achievement>();

    /// <summary>
    /// This sprite is used for indicating if an achievement is unlocked
    /// </summary>
    public Sprite m_UnlockedSprite;

    /// <summary>
    /// A reference to the text that shows the points inside the menu
    /// </summary>
    public Text m_TextPoints;

    /// <summary>
    /// The time it takes for the inventory to fade in and out in seconds
    /// </summary>
    private int fadeTime_ = 2;

    /// <summary>
    /// An instance for the AchievementManager, this is used for the singleton pattern
    /// </summary>
    private static AchievementManager instance_;

    /// <summary>
    /// A property for accesing the singleton
    /// </summary>
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
        
        //Creates the general achievements
        CreateAchievement("General", "Press W", "Press W to unlock this achievement", 5, 0);
        CreateAchievement("General", "Press A", "Press A to unlock this achievement", 5, 0);
        CreateAchievement("General", "Press S", "Press S to unlock this achievement", 5, 0);
        CreateAchievement("General", "Press D", "Press D to unlock this achievement", 5, 0);
        CreateAchievement("General", "All keys", "Press all keys to unlock", 10, 1, new string[] { "Press W", "Press A", "Press S", "Press D" });

        //Creates the other achievements
        CreateAchievement("Other", "I love strawberries", "Press the strawberry to unlock this achievement", 5, 7);
        CreateAchievement("Other", "I love apple", "Press the apple to unlock this achievement", 5, 2);
        CreateAchievement("Other", "I love banana", "Press the banana to unlock this achievement", 5, 3);
        CreateAchievement("Other", "I love grapefruit", "Press the grapefruit to unlock this achievement", 5, 4);
        CreateAchievement("Other", "I love kiwi", "Press the kiwi to unlock this achievement", 5, 5);
        CreateAchievement("Other", "I love pineapple", "Press the pineapple to unlock this achievement", 5, 6);
        CreateAchievement("Other", "Fruit Salad", "Press all the fruits to unlock this achievement", 10, 8, new string[] { "I love apple", "I love banana", "I love grapefruit", "I love kiwi", "I love pineapple", "I love strawberries" });
        
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
        if (Input.GetKeyDown(KeyCode.W))
        {
            EarnAchievement("Press W");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            EarnAchievement("Press A");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            EarnAchievement("Press S");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            EarnAchievement("Press D");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerPrefs.DeleteAll();
        }
	}

    /// <summary>
    /// Attempts to earn an achievement
    /// </summary>
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
        }
    }

    /// <summary>
    /// Creates an achievement
    /// </summary>
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

    /// <summary>
    /// Filles the onscreen achievement with information
    /// </summary>
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

    /// <summary>
    /// Changes the category
    /// </summary>
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

    /// <summary>
    /// Fades the achievement in and out
    /// </summary>
    /// <param name="achievement">The achievement to fade</param>
    /// <returns></returns>
    private IEnumerator FadeAchievement(GameObject achievement)
    {
        //Creates a reference to the CanvasGroup Script
        CanvasGroup canvasGroup = achievement.GetComponent<CanvasGroup>();

        //Calculats the fade rate
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

        //Destroyes the visual object
        Destroy(achievement);


    }

}
