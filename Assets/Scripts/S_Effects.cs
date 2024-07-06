using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class S_Effects : MonoBehaviour
{
    [SerializeField] private PostProcessVolume volume;
    [SerializeField] private AnimationCurve blinkingCurveClose;
    [SerializeField] private AnimationCurve blinkingCurveOpen;
    [SerializeField] private AnimationCurve blinkingCurve_2;
    [SerializeField] private AnimationCurve crossCurve;


    [SerializeField] private Image img;


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
        effectLength = blinkingCurveOpen.length;
        try
        {
            volume.profile.TryGetSettings(out vignette);
            volume.profile.TryGetSettings(out depthOfField);
            volume.profile.TryGetSettings(out grain);
            volume.profile.TryGetSettings(out backColor);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
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
        intensityTime = 0f;
        while (intensityTime < 5)
        {
            if (intensityTime > 1f)
            {
                gameObject.GetComponent<PlayerMovement>().inputActions.Moving.Move.Disable();
            }
            intensityTime += speed * Time.deltaTime;
            float alphaValue = crossCurve.Evaluate(intensityTime);;
            float intensity = blinkingCurveClose.Evaluate(intensityTime);
            float postExp = blinkingCurve_2.Evaluate(intensityTime);
            vignette.intensity.value = intensity;
            backColor.postExposure.value = postExp;
            depthOfField.focalLength.value = Math.Clamp(intensityTime * 60f, 1, 300);
            grain.intensity.value = intensityTime * 0.2f;
            img.color = new Color(255, 255, 255, 255 * alphaValue);
            grain.size.value = intensityTime * 0.8f;
            yield return null;
        }
    }

    IEnumerator Open()
    {
        intensityTime = 5f;
        // On build uncomment
        //gameObject.GetComponent<PlayerMovement>().inputActions.Moving.Move.Disable();
        while (intensityTime > 0)
        {
            if (intensityTime < 1f)
            {
                gameObject.GetComponent<PlayerMovement>().inputActions.Moving.Move.Enable();
            }
            intensityTime -= speed * Time.deltaTime;
            float intensity = blinkingCurveOpen.Evaluate(intensityTime);
            float postExp = blinkingCurve_2.Evaluate(intensityTime);
            img.color = new Color(255, 255, 255, 255);
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
