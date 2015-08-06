using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour 
{
    /// <summary>Max time for autosave timer</summary>
    public const float AUTO_SAVE_TIME = 100.0f;
    /// <summary>Radar arm offset in degrees</summary>
    public const float CLOCK_DEGREES_AND_OFFSET = 360.0f;
    /// <summary>Max amount of time the scanner runs</summary>
    public const float MAX_SCANNER_TIME = 10.0f;

    /// <summary>Maximum amount of pets you can own</summary>
    public const int MAX_PETS = 6;
    /// <summary>Highest possible stat point</summary>
    public const int MAX_PET_STAT = 100;
    /// <summary>Lowest possible stat point</summary>
    public const int MIN_PET_STAT = 0;
    /// <summary>Amount a stat increases when the stat timer elapses</summary>
    public const int STAT_INCREASE_VAL = 5;
    /// <summary>Amount a stat decreases when the player does an action</summary>
    public const int STAT_DECREASE_VAL = 10;
    /// <summary>Amount of energy a new player starts with</summary>
    public const int DEFAULT_START_ENERGY = 25;
    /// <summary>Amount of shields a new player will start with</summary>
    public const int DEFAULT_START_SHIELDS = 100;
    /// <summary>Stats that new pets automatically start with</summary>
    public const int DEFAULT_START_STATS = 50;
    /// <summary>Amount of time to wait to get more points - seconds</summary>
    public const int ENERGY_TIMER = 300;
    /// <summary>Amount of time for a stat to be increased randomly - seconds</summary>
    public const int STAT_TIMER = 180;
    /// <summary>Amount of time for a stat to be increased randomly - seconds</summary>
    public const int STAT_TIMER_2 = 300;
    /// <summary>Amount of time for a stat to be increased randomly - seconds</summary>
    public const int STAT_TIMER_3 = 360;
    /// <summary>Maximum amount of energy the player can have</summary>
    public const int DEFAULT_MAX_ENERGY = 25;
    /// <summary>Maximum amount of energy the player can have</summary>
    public const int DEFAULT_MAX_LOVE = 300;
    /// <summary>Cost per action</summary>
    public const int ACTION_COST = 1;
    /// <summary>Energy obtained from waiting 5 minutes</summary>
    public const int ENERGY_REWARDED = 1;
    /// <summary>Amount of shields the player is awarded for filling the love meter</summary>
    public const int SHIELDS_REWARDED = 1;
    /// <summary>Base upgrade cost that gets multiplied with the upgrade level</summary>
    public const int BASE_UPGRADE_COST = 10;
    /// <summary>Time in seconds for the radar to rotate around once</summary>
    public const int RADAR_TIME = 60;
    /// <summary>Max amount of characters in any given input field</summary>
    public const int CHARACTER_LIMIT = 10;
    /// <summary>Number of scans the player starts with</summary>
    public const int START_SCANS = 5;

    /// <summary>Feed VIII Achievement ID on Google Play Services</summary>
    public const string FEED_ACHIEVEMENT_ID = "CggI-Of8-WIQAhAB";
    /// <summary>Play VIII Achievement ID on Google Play Services</summary>
    public const string PLAY_ACHIEVEMENT_ID = "CggI-Of8-WIQAhAC";
    /// <summary>Wash VIII Achievement ID on Google Play Services</summary>
    public const string CLEAN_ACHIEVEMENT_ID = " CggI-Of8-WIQAhAD";
    /// <summary>Exercise VIII Achievement ID on Google Play Services</summary>
    public const string EXERCISE_ACHIEVEMENT_ID = "CggI-Of8-WIQAhAE";
    /// <summary>Fill Love V Achievement ID on Google Play Services</summary>
    public const string LOVE_ACHIEVEMENT_ID = "CggI-Of8-WIQAhAF";
}