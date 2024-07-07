using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillFromMicrophone : MonoBehaviour
{
    public Image audioBar;
    public AudioLoudnessDetector detector;
    public Slider sensitivitySlider;

    public float threshold = 0.1f;
    public float maxSensibility = 1000;
    public float minSensibility = 100;
    public float currLoudnessSensibility = 100f;

    private void Start()
    {
        if (sensitivitySlider == null) return;

        sensitivitySlider.value = .5f;
        SetLoudnessSensibility(sensitivitySlider.value);
    }

    private void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * currLoudnessSensibility;
        if (loudness < threshold) loudness = 0.05f;
        audioBar.fillAmount = loudness;
    }

    public void SetLoudnessSensibility(float t)
    {
        currLoudnessSensibility = Mathf.Lerp(minSensibility, maxSensibility, t);
    }
}
