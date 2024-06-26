using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class S_Effects : MonoBehaviour
{
    [SerializeField] private PostProcessVolume volume;
    [SerializeField] private AnimationCurve blinkingCurve;
    [SerializeField] private AnimationCurve blinkingCurve_2;


    private float intensityTime;
    public float effectLength;
    [SerializeField] private float speed;
    private bool isBlinking;
    private Coroutine blinking;

    private Vignette vignette;
    private DepthOfField depthOfField;
    private Grain grain;
    private ColorGrading backColor;

    
    // Start is called before the first frame update
    void Awake()
    {
        effectLength = blinkingCurve.length;
        volume.profile.TryGetSettings(out vignette);
        volume.profile.TryGetSettings(out depthOfField);
        volume.profile.TryGetSettings(out grain);
        volume.profile.TryGetSettings(out backColor);

    }

    public void CloseEyes()
    { 
        StartCoroutine(Close());
    }

    public void OpenEyes()
    {
        StartCoroutine(Open());
    }

    IEnumerator Close()
    {
        while (intensityTime < 5)
        {
            intensityTime += speed * Time.deltaTime;
            float intensity = blinkingCurve.Evaluate(intensityTime);
            float postExp = blinkingCurve_2.Evaluate(intensityTime);
            vignette.intensity.value = intensity;
            backColor.postExposure.value = postExp;
            depthOfField.focalLength.value = Math.Clamp(intensityTime * 60f, 1, 300);
            grain.intensity.value = intensityTime * 0.2f;
            grain.size.value = intensityTime * 0.8f;
            yield return null;
        }
    }

    IEnumerator Open()
    {
        while (intensityTime > 0)
        {
            intensityTime -= speed * Time.deltaTime;
            float intensity = blinkingCurve.Evaluate(intensityTime);
            float postExp = blinkingCurve_2.Evaluate(intensityTime);
            vignette.intensity.value = intensity;
            depthOfField.focalLength.value = Math.Clamp(intensityTime * 60f, 1, 300);
            grain.intensity.value = intensityTime * 0.2f;
            grain.size.value = intensityTime * 0.8f;
            backColor.postExposure.value = postExp;
            yield return null;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            CloseEyes();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            OpenEyes();
        }
    }
}
