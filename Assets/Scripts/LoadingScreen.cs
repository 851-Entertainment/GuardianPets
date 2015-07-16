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
    private bool loadDone_ = false;
    AsyncOperation async = null;

    void Start()
    {
        SoomlaStore.Initialize(new GuardianPetsAssets());
        SoomlaStore.StartIabServiceInBg();
        SoomlaStore.StopIabServiceInBg();
        Screen.orientation = ScreenOrientation.Landscape;
        async = Application.LoadLevelAsync("Game");
    }

	void Update () 
    {
        StartCoroutine(DisplayLoadingScreen());

        if(async != null)
        {
            if(async.progress >= 0.9f)
            {
                while (loadProgress_ < 99.9f)
                {
                    loadProgress_++;
                    m_Text.text = "Loading..." + loadProgress_ + "%";
                    m_ProgressBar.transform.localScale = new Vector3((loadProgress_ / 100), m_ProgressBar.transform.localScale.y, m_ProgressBar.transform.localScale.z);
                }
                if (loadProgress_ >= 100.0f)
                {
                    async.allowSceneActivation = true;
                }
            }
        }
	}

    IEnumerator DisplayLoadingScreen()
    {
        m_ProgressBar.transform.localScale = new Vector3((loadProgress_ / 100), m_ProgressBar.transform.localScale.y, m_ProgressBar.transform.localScale.z);

        m_Text.text = "Loading..." + loadProgress_ + "%";
        
        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
        {
            loadProgress_ = (int)(async.progress * 100);
            m_Text.text = "Loading..." + loadProgress_ + "%";
            m_ProgressBar.transform.localScale = new Vector3(async.progress, m_ProgressBar.transform.localScale.y, m_ProgressBar.transform.localScale.z);
            yield return null;
        }
    }
}