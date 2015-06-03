using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fear : MonoBehaviour
{
    public string m_SelectedFear;
    private string[] fears_;

    public void Start()
    {
        fears_[0] = "Heights";
        fears_[1] = "Monsters";
        fears_[2] = "Ghosts";
        fears_[3] = "Dark";
        fears_[4] = "Storms";
    }

    //Button function -- When the player presses any of the fear buttons it will call this function, passing in the name of the button to get the appropriate fear
    public void ChooseFear(Button btn)
    {
        m_SelectedFear = btn.name;
    }
}
