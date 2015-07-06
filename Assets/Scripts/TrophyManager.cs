using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TrophyManager : MonoBehaviour
{
    /// <param name="Trophy Description">Displays what the trophy was</param>
    public string m_Description = "";
    /// <param name="Description Textfield">Where the description lives</param>
    public Text m_TextField;

    private Color startColor;
    private Color highlighColor_;
    private Button button_;
    private bool firstClick_ = true;
    private bool updateTimer_ = false;
    private float timer_ = 0.0f;

    void Start()
    {
        button_ = GetComponent<Button>();
        startColor = Color.white;
        highlighColor_ = Color.red;
    }

    void Update()
    {
        if(updateTimer_)
        {
            DisableTrophy(2.0f);
        }
    }

    public void Highlight()
    {
        if(firstClick_)
        {
            firstClick_ = !firstClick_;
            button_.image.color = highlighColor_;
            m_TextField.text = m_Description;
            updateTimer_ = true;
        }
        else
        {
            firstClick_ = !firstClick_;
            button_.image.color = startColor;
            m_TextField.text = "";
        }
    }

    /// <param name="maxTime">After a set time the trophy will go back to its default state</param>
    void DisableTrophy(float maxTime)
    {
        timer_ += Time.deltaTime;
        if (timer_ >= maxTime)
        {
            firstClick_ = false;
            Highlight();
            updateTimer_ = false;
            timer_ = 0.0f;
        }
    }
}
