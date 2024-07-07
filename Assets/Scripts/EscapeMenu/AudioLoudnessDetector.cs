using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoudnessDetector : MonoBehaviour
{
    public int sampleWindow = 64;

    private AudioClip _microphoneClip;
    private string _microphoneName;

    private void Start()
    {
        MicrophoneToAudioClio(0);
    }

    private void OnEnable()
    {
        MicrophoneSelector.OnMicrophoneChoiceChanged += ChangeMicrophoneSource;
    }

    private void OnDisable()
    {
        MicrophoneSelector.OnMicrophoneChoiceChanged -= ChangeMicrophoneSource;
    }

    private void ChangeMicrophoneSource(int index)
    {
        MicrophoneToAudioClio(index);
    }
    private void MicrophoneToAudioClio(int micIndex)
    {
        _microphoneName = Microphone.devices[micIndex];
        _microphoneClip = Microphone.Start(_microphoneName, true, 15, AudioSettings.outputSampleRate);
    }

    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(_microphoneName), _microphoneClip);

    }

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;
        if (startPosition < 0) return 0;

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);
        float totalLoudness = 0;
        foreach (var sample in waveData)
        {
            totalLoudness = Mathf.Abs(sample);
        }

        return totalLoudness / sampleWindow;
    }
}
