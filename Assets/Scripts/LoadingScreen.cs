using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Soomla.Store;

public class LoadingScreen : MonoBehaviour
{
    public Text m_Text;
    public GameObject m_ProgressBar;

    private int loadProgress_ = 0;
    private float timer_ = 0.0f;

    void Start()
    {
        SoomlaStore.Initialize(new GuardianPetsAssets());
        SoomlaStore.StartIabServiceInBg();
        SoomlaStore.StopIabServiceInBg();
        Screen.orientation = ScreenOrientation.Landscape;
    }

	void Update () 
    {
        StartCoroutine(DisplayLoadingScreen());
	}

    IEnumerator DisplayLoadingScreen()
    {
        m_ProgressBar.transform.localScale = new Vector3(loadProgress_, m_ProgressBar.transform.localScale.y, m_ProgressBar.transform.localScale.z);

        m_Text.text = "Loading..." + loadProgress_ + "%";

        AsyncOperation async = Application.LoadLevelAsync("Game");

        while(!async.isDone)
        {
            loadProgress_ = (int)(async.progress * 100);
            m_Text.text = "Loading..." + loadProgress_ + "%";
            m_ProgressBar.transform.localScale = new Vector3(async.progress, m_ProgressBar.transform.localScale.y, m_ProgressBar.transform.localScale.z);
            yield return null;
        }
    }
}
