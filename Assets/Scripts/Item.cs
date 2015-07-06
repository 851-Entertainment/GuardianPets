using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item : MonoBehaviour
{
    public bool m_IsPlayerItem;
    public string m_ItemName;
    public List<string> m_Names = new List<string>() { "Head", "Tie", "Chest", "Right Arm", "Left Arm", "Right Leg", "Left Leg", "Back", "Eyes" };
    public string m_Category;
    public string m_Description;
    public int m_ItemSpot;  //set the number to match the string index you want
    public int m_Cost;
    public int m_Quantity = 1;

    private GameObject obj_;
   
    void Update()
    {
        if (this.transform.parent != null && m_IsPlayerItem)
        {
            transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y, -5.5f);
        }
    }
}
