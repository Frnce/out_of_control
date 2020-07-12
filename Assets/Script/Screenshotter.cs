using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshotter : MonoBehaviour
{
    [SerializeField]
    private float maxScreenshotTimer = 0f;

    private float screenshotTime;
    // Use this for initialization
    void Start()
    {
        screenshotTime = maxScreenshotTimer;
    }
    private void FixedUpdate()
    {
        if (screenshotTime <= 0)
        {
            //take screenshot
            string date = DateTime.Now.ToString();
            date = date.Replace("/", "-");
            date = date.Replace(" ", "_");
            date = date.Replace(":", "-");
            ScreenCapture.CaptureScreenshot("Screenshots/Screenshot_" + date + ".png", 4);
            screenshotTime = maxScreenshotTimer;
        }
        else
        {
            screenshotTime -= Time.deltaTime;
        }
    }
}
