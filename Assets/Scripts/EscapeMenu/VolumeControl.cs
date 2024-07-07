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
      RuntimeManager.GetBus("bus:/Background Music").setVolume(volume);
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
