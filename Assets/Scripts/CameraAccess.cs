using UnityEngine;
using System.Collections;

public class CameraAccess : MonoBehaviour 
{
    private WebCamTexture cam_;
    public Renderer m_Renderer;
    public bool m_DisableWebCam = false;

	void Start ()
    {
        cam_ = new WebCamTexture();
        m_Renderer.material.mainTexture = cam_;
        cam_.Play();
	}
    public void UpdateCamera()
    {
        if (m_DisableWebCam)
        {
            cam_.Stop();
        }
        else
        {
            cam_.Play();
        }
    }
    void Update()
    {
       
    }
}