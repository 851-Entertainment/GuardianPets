using UnityEngine;
using System.Collections;

public class AspectRatio : MonoBehaviour 
{
	void Start () 
    {
        //Set the desired aspect ratio to 16:9
        float targetAspect = 16.0f / 9.0f;
        //Determine the game window's current aspect ratio
        float windowAspect = (float)Screen.width / (float)Screen.height;
        //Scale the viewport height by this amount
        float scaleHeight = windowAspect / targetAspect;

        //Get the camera to modify the viewport
        Camera cam = GetComponent<Camera>();

        //If the scaled height is less than the current height add a letterbox
        if(scaleHeight < 1.0f)
        {
            Rect rect = cam.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            cam.rect = rect;
        }
        //Otherwise add a pillarbox
        else
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = cam.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            cam.rect = rect;
        }
	}
}
