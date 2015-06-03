using UnityEngine;
using System.Collections;

public class RotateScript : MonoBehaviour
{
    private float timer_;
    private const float time_ = Constants.CLOCK_DEGREES_AND_OFFSET / Constants.RADAR_TIME;
    void Update()
    {
        timer_ += (Time.deltaTime * 150);
        transform.localRotation = Quaternion.Euler(Constants.CLOCK_DEGREES_AND_OFFSET, 0.0f, -((timer_ - time_) - Constants.CLOCK_DEGREES_AND_OFFSET));
    }
}
