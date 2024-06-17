using FMODUnity;
using UnityEngine;

public class DrumRadio : MonoBehaviour
{
        [SerializeField] private EventReference radioDrumSound;
        [SerializeField] private EventReference radioWrongSound;
        [SerializeField] private EventReference radioRightSound;
        [SerializeField] private EventReference radioFinishedSound;

        public void PlayDrumSound()
        {
            AudioManager.Instance.PlayOneShotAttached(radioDrumSound, gameObject);
        }

        public void PlayWrongSound()
        {
            AudioManager.Instance.PlayOneShotAttached(radioWrongSound, gameObject);
        }
        
        public void PlayRightSound()
        {
            AudioManager.Instance.PlayOneShotAttached(radioRightSound, gameObject);
        }
        
        public void PlayFinishedSound()
        {
            AudioManager.Instance.PlayOneShotAttached(radioFinishedSound, gameObject);
        }
}