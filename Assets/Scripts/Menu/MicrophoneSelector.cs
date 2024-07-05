using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MicrophoneSelector : MonoBehaviour
{
    public TMP_Dropdown sourceDropdown;
    public int deviceIndex;
    public static UnityAction<int> OnMicrophoneChoiceChanged;
    private List<string> microphones;
    void Start()
    {
       microphones = new List<string>();
       PopulateSourceDropDown();
    }

    private void PopulateSourceDropDown()
    {
        var options = new List<TMP_Dropdown.OptionData>();

        foreach (var mic in Microphone.devices)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(mic, null);
            options.Add(optionData);
            microphones.Add(mic);
        }

        sourceDropdown.options = options;
    }

    public void ChooseMicrophone(int index)
    {
        deviceIndex = index;
        OnMicrophoneChoiceChanged?.Invoke(deviceIndex);
        AudioManager.Instance.currentMicrophone = microphones[deviceIndex];
    }
}
