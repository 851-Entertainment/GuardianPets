using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData : MonoBehaviour
{
    public List<GameObject> m_Pets = new List<GameObject>();
    public int m_Energy;
    public int m_Shields;

    private int counter_ = 0;

	void Start ()
    {

	}
	
	void Update ()
    {
	
	}

    //This function converts the pet name (string) into the actual game object and adds it to the player's pet list
    public void AddPet(string petName)
    {
        GameController tempGC = Camera.main.GetComponent<GameController>();

        for(int i = 0; i < tempGC.m_PetChoices.Count; ++i)
        {
            if(tempGC.m_PetChoices[i].name == petName)
            {
                m_Pets.Add(tempGC.m_PetChoices[i]);
            }
        }
    }

    //This function is called by the button functions from the pets, this removes the points from the player (it's always the same) and increments a counter
    //Once this counter reaches a certain number, it will add shields to the player's account
    public void RemoveEnergy()
    {
        m_Energy -= Constants.ACTION_COST;
    }
}
