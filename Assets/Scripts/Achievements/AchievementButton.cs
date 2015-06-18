using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AchievementButton : MonoBehaviour
{
    public GameObject m_AchievementList;
    public Sprite m_Neutral;
    public Sprite m_Highlight;

    private Image sprite_;

    void Awake()
    {   
        //Sets a reference to the sprite component
        sprite_ = GetComponent<Image>();
    }

    public void Click()
    {
        //Checks if the button is selected and changes the sprite according to it. 
        if (sprite_.sprite == m_Neutral) 
        {
            sprite_.sprite = m_Highlight;
            m_AchievementList.SetActive(true); //Shows the associated category
        }
        else
        {
            sprite_.sprite = m_Neutral;
            m_AchievementList.SetActive(false);
        }
    }
}
