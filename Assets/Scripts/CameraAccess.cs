using UnityEngine;
using System.Collections;

public class CameraAccess : MonoBehaviour 
{
    private WebCamTexture cam_;
    public Renderer m_Renderer;

	void Start ()
    {
        cam_ = new WebCamTexture();
        m_Renderer.material.mainTexture = cam_;
        cam_.Play();
	}
}