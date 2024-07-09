using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
   public Slider masterVolumeSlider;
   public Slider musicVolumeSlider;
   public Slider sfxVolumeSlider;

   private void Start()
   {
      
      RuntimeManager.GetBus("bus:/").setVolume(0.75f);
      
      RuntimeManager.GetBus("bus:/").getVolume(out var masterVolume);
      RuntimeManager.GetBus("bus:/Music").getVolume(out var musicVolume);
      RuntimeManager.GetBus("bus:/SFX").getVolume(out var sfxVolume);

      masterVolumeSlider.value = masterVolume;
      musicVolumeSlider.value = musicVolume;
      sfxVolumeSlider.value = sfxVolume;
      
      masterVolumeSlider.onValueChanged.AddListener(HandleMasterVolumeChange);
      musicVolumeSlider.onValueChanged.AddListener(HandleMusicVolumeChange);
      sfxVolumeSlider.onValueChanged.AddListener(HandleSFXVolumeChange);
   }
   
   private void HandleMasterVolumeChange(float volume)
   {
      RuntimeManager.GetBus("bus:/").setVolume(volume); 
   }

   private void HandleMusicVolumeChange(float volume)
   {
      RuntimeManager.GetBus("bus:/Music").setVolume(volume);
   }

   private void HandleSFXVolumeChange(float volume)
   {
      RuntimeManager.GetBus("bus:/SFX").setVolume(volume);
   }

   private void OnDestroy()
   {
      masterVolumeSlider.onValueChanged.RemoveListener(HandleMasterVolumeChange);
      musicVolumeSlider.onValueChanged.RemoveListener(HandleMusicVolumeChange);
      sfxVolumeSlider.onValueChanged.RemoveListener(HandleSFXVolumeChange);
   }
  }
