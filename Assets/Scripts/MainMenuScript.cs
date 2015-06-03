using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Soomla.Highway;
using Soomla.Levelup;
using Soomla.Profile;
using Soomla.Store;

public class MainMenuScript : MonoBehaviour 
{
	void Start ()
    {
        SoomlaHighway.Initialize();
        SoomlaStore.Initialize(new GuardianPetsAssets());
        SoomlaProfile.Initialize();
        Screen.orientation = ScreenOrientation.Landscape;
	}
	
	void Update () 
    {
	
	}

    //Button function -- When the player presses the "Play" button this function will fire
    public void Play()
    {
        Application.LoadLevel("Game");
    }

    //Button function -- When the player presses the "Options" button this function will fire
    public void Options()
    {

    }
}