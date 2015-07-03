using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TrophyManager : MonoBehaviour
{
    /// <param name="Trophy Description">Displays what the trophy was</param>
    public string m_Description = "";
    /// <param name="Where the description lives"></param>
    public Text m_TextField;

    private Color startColor;
    private Color highlighColor_;
    private Button button_;
    private bool firstClick_ = true; 

    void Start()
    {
        button_ = GetComponent<Button>();
        startColor = Color.white;
        highlighColor_ = Color.red;
    }

    public void Highlight()
    {
        if(firstClick_)
        {
            firstClick_ = !firstClick_;
            button_.image.color = highlighColor_;
            m_TextField.text = m_Description;
        }
        else
        {
            firstClick_ = !firstClick_;
            button_.image.color = startColor;
            m_TextField.text = "";
        }
    }
}
