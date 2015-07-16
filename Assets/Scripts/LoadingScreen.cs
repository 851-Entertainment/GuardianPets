﻿using UnityEngine;
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
            Debug.Log("async not null");
            if(async.progress >= 0.9f)
            {
                Debug.Log("Async is done");
                timer_ += Time.deltaTime;
                if(timer_ >= 3.0f)
                {
                    async.allowSceneActivation = true;
                }
            }
        }
	}

    IEnumerator DisplayLoadingScreen()
    {
        m_ProgressBar.transform.localScale = new Vector3(loadProgress_, m_ProgressBar.transform.localScale.y, m_ProgressBar.transform.localScale.z);

        m_Text.text = "Loading..." + loadProgress_ + "%";

        
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            Debug.Log("Loading");
            loadProgress_ = (int)(async.progress * 100);
            m_Text.text = "Loading..." + loadProgress_ + "%";
            m_ProgressBar.transform.localScale = new Vector3(async.progress, m_ProgressBar.transform.localScale.y, m_ProgressBar.transform.localScale.z);
            yield return null;
        }
        //StartScene(async);
        
        
    }

    void StartScene(AsyncOperation scene)
    {
        if (scene.isDone)
        {
            scene.allowSceneActivation = true;
        }
    }
}