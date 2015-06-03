using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour 
{
    public const float AUTO_SAVE_TIME = 100.0f; //Max time for the autosave timer
    public const float CLOCK_DEGREES_AND_OFFSET = 360.0f;
    public const float MAX_SCANNER_TIME = 10.0f; //Max time the scanner can run for

    public const int MAX_PETS = 6; //Maximum amount of pets you can own
    public const int MAX_PET_STAT = 100; //Highest any of the pet stats - hunger, boredom, cleanliness - can go
    public const int MIN_PET_STAT = 0; //Lowest any of the pet stats - hunger, boredom, cleanliness - can go
    public const int STAT_INCREASE_VAL = 10; //Amount a stat increases by when the stat timer elapses
    public const int STAT_DECREASE_VAL = 10; //Amount a stat decreases when the player does an action
    public const int DEFAULT_START_ENERGY = 20; //Amount of energy a new player will start with
    public const int DEFAULT_START_SHIELDS = 100; //Amount of shields a new player will start with
    public const int DEFAULT_START_STATS = 50; //Stats that new pets automatically start at
    public const int ENERGY_TIMER = 300; //Amount of time to wait to get more points - in seconds
    public const int STAT_TIMER = 180; //Amount of time for a stat to be increased randomly - in seconds
    public const int DEFAULT_MAX_ENERGY = 20; //Maximum amount of energy the player can have
    public const int ACTION_COST = 1; //Cost per action - play, wash, feed.
    public const int ENERGY_REWARDED = 1; //This is the energy you get from waiting the 5 minutes
    public const int SHIELDS_REWARDED = 10; //This is the amount of shields the player is awarded for meeting/maintaining the 3 conditions of the pet
    public const int BASE_UPGRADE_COST = 10; //This is the base upgrade cost that gets multiplied with the upgrade level
    public const int RADAR_TIME = 60; //Time in seconds for radar to rotate around once
}