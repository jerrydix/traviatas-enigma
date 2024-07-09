using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioOcclusion : MonoBehaviour
{
    [Header("FMOD Event")]
    [SerializeField]
    private EventReference SelectAudio;
    private EventInstance Audio;
    private EventDescription AudioDes;
    private StudioListener Listener;
    private PLAYBACK_STATE pb;

    [SerializeField] private bool isSimple;
    [SerializeField] private bool isGrandpaClock;
    [SerializeField] private bool isRadioNormal;
    [SerializeField] private bool isRadioSwitcher;
    [SerializeField] private GrandpaClock grandpaClock;
    [SerializeField] private S_Radio radio;

    [SerializeField] private Transform clockTransform;
    [SerializeField] private Transform radioTransform;
    private Transform currentTransform;

    [Header("Occlusion Options")]
    [SerializeField]
    [Range(0f, 10f)]
    private float SoundOcclusionWidening = 5.5f;
    [SerializeField]
    [Range(0f, 10f)]
    private float PlayerOcclusionWidening = 5.5f;
    private LayerMask OcclusionLayer;

    private bool AudioIsVirtual;
    private float MaxDistance;
    private float ListenerDistance;
    private float lineCastHitCount = 0f;
    private Color colour;

    private void Start()
    {
        //change accordingly to sound type
        if (isSimple)
        {
            Audio = RuntimeManager.CreateInstance(SelectAudio);
            RuntimeManager.AttachInstanceToGameObject(Audio, GetComponent<Transform>(), GetComponent<Rigidbody>());
            Audio.start();
            Audio.release();
        } 
        else if (isGrandpaClock)
        {
            Audio = grandpaClock.instance;
            SelectAudio = grandpaClock.chimeSound;
        } 
        else if (isRadioNormal)
        {
            Audio = radio.radioInstance;
            SelectAudio = radio.radioSound;
        } 
        else if (isRadioSwitcher)
        {
            Audio = radio.switchInstance;
            SelectAudio = radio.switchSound;
        }
        //
        
        if (isGrandpaClock && !isRadioNormal && !isRadioSwitcher)
        {
            currentTransform = clockTransform;
        }
        else if (!isGrandpaClock && isRadioNormal || isRadioSwitcher)
        {
            currentTransform = radioTransform;
        }
        else
        {
            currentTransform = transform;
        }

        OcclusionLayer = LayerMask.GetMask("Default");
        
        AudioDes = RuntimeManager.GetEventDescription(SelectAudio);
        AudioDes.getMinMaxDistance(out var _, out MaxDistance);

        Listener = FindObjectOfType<StudioListener>();
    }
    
    private void Update()
    {
        if (isGrandpaClock && !isRadioNormal && !isRadioSwitcher)
        {
            currentTransform = clockTransform;
        }
        else if (!isGrandpaClock && isRadioNormal || isRadioSwitcher)
        {
            currentTransform = radioTransform;
        }
        else
        {
            currentTransform = transform;
        }
        Audio.isVirtual(out AudioIsVirtual);
        Audio.getPlaybackState(out pb);
        ListenerDistance = Vector3.Distance(currentTransform.position, Listener.transform.position);

        if (!AudioIsVirtual && pb == PLAYBACK_STATE.PLAYING && ListenerDistance <= MaxDistance)
            OccludeBetween(currentTransform.position, Listener.transform.position);

        lineCastHitCount = 0f;
    }

    private void OccludeBetween(Vector3 sound, Vector3 listener)
    {
        Vector3 SoundLeft = CalculatePoint(sound, listener, SoundOcclusionWidening, true);
        Vector3 SoundRight = CalculatePoint(sound, listener, SoundOcclusionWidening, false);

        Vector3 SoundAbove = new Vector3(sound.x, sound.y + SoundOcclusionWidening, sound.z);
        Vector3 SoundBelow = new Vector3(sound.x, sound.y - SoundOcclusionWidening, sound.z);

        Vector3 ListenerLeft = CalculatePoint(listener, sound, PlayerOcclusionWidening, true);
        Vector3 ListenerRight = CalculatePoint(listener, sound, PlayerOcclusionWidening, false);

        Vector3 ListenerAbove = new Vector3(listener.x, listener.y + PlayerOcclusionWidening * 0.5f, listener.z);
        Vector3 ListenerBelow = new Vector3(listener.x, listener.y - PlayerOcclusionWidening * 0.5f, listener.z);

        CastLine(SoundLeft, ListenerLeft);
        CastLine(SoundLeft, listener);
        CastLine(SoundLeft, ListenerRight);

        CastLine(sound, ListenerLeft);
        CastLine(sound, listener);
        CastLine(sound, ListenerRight);

        CastLine(SoundRight, ListenerLeft);
        CastLine(SoundRight, listener);
        CastLine(SoundRight, ListenerRight);
        
        CastLine(SoundAbove, ListenerAbove);
        CastLine(SoundBelow, ListenerBelow);

        if (PlayerOcclusionWidening == 0f || SoundOcclusionWidening == 0f)
        {
            colour = Color.blue;
        }
        else
        {
            colour = Color.green;
        }

        SetParameter();
    }

    private Vector3 CalculatePoint(Vector3 a, Vector3 b, float m, bool posOrneg)
    {
        float x;
        float z;
        float n = Vector3.Distance(new Vector3(a.x, 0f, a.z), new Vector3(b.x, 0f, b.z));
        float mn = (m / n);
        if (posOrneg)
        {
            x = a.x + (mn * (a.z - b.z));
            z = a.z - (mn * (a.x - b.x));
        }
        else
        {
            x = a.x - (mn * (a.z - b.z));
            z = a.z + (mn * (a.x - b.x));
        }
        return new Vector3(x, a.y, z);
    }

    private void CastLine(Vector3 Start, Vector3 End)
    {
        RaycastHit hit;
        Physics.Linecast(Start, End, out hit, OcclusionLayer);

        if (hit.collider)
        {
            lineCastHitCount++;
            Debug.DrawLine(Start, End, Color.red);
        }
        else
            Debug.DrawLine(Start, End, colour);
    }

    private void SetParameter()
    {
        Audio.setParameterByName("Occlusion", lineCastHitCount / 11);
    }
}