using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData : MonoBehaviour
{
    public List<GameObject> m_Pets = new List<GameObject>();
    public int m_Energy;
    public int m_Shields;
    public int m_Scans;

    //This function is called by the button functions from the pets, this removes the points from the player (it's always the same) and increments a counter
    //Once this counter reaches a certain number, it will add shields to the player's account
    public void RemoveEnergy()
    {
        m_Energy -= Constants.ACTION_COST;
    }

    public void RemoveShields(int amt)
    {
        m_Shields -= amt;
    }
}
