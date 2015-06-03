using UnityEngine;
using System.Collections;

public class FillScreen : MonoBehaviour
{
    public float m_PlaneSize = 10.0f; // 10 for a Unity3d plane
    void Update()
    {
        Camera cam = Camera.main;

        if (cam.orthographic)
        {
            float sizeY = cam.orthographicSize * 2.0f;
            float sizeX = sizeY * cam.aspect;
            transform.localScale = new Vector3(sizeX / m_PlaneSize, 1, sizeY / m_PlaneSize);
        }
        else
        {
            float pos = (cam.farClipPlane - 0.5f);
            transform.position = cam.transform.position + cam.transform.forward * pos;
            transform.LookAt(cam.transform);
            transform.Rotate(90.0f, 0.0f, 0.0f);

            float h = (Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad * 0.5f) * pos * 2f) / m_PlaneSize;
            transform.localScale = new Vector3(h * cam.aspect, 1.0f, h);
        }
    }
}