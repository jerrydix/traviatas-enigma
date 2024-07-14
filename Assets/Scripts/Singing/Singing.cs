using System.Collections;
using System.Collections.Generic;
using System.IO;
using FMOD.Studio;
using FMODUnity;
using HuggingFace.API;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Singing : MonoBehaviour
{
    
    [SerializeField] Interactable interactable;
    
    [SerializeField] private List<string> lyrics;
    [SerializeField] private TextMeshProUGUI lyricsText;
    [SerializeField] private float APIRetryDelay;
    [SerializeField] private EventReference correctSound;
    [SerializeField] private EventReference wrongSound;
    [SerializeField] private EventReference operaMusic;
    private EventInstance operaMusicInstance;
    [SerializeField] private GameObject soundSource;
    [SerializeField] private int maxIncorrectWords;
    [SerializeField] private int recordingLength;
    [SerializeField] private S_Bulb bulb;
    
    [SerializeField] private S_Effects effects;
    
    private int currentIndex;
    private string currentPlayerLyrics;
    [HideInInspector] public bool singingIsFinished;
    
    private bool inOrchestra;
    private bool inVerse;
    
    private AudioClip clip;
    private byte[] bytes;
    private bool recording;
    
    private PlayerInput inputActions;
    

    private void Start()
    {
        bulb.StandBy();
        currentIndex = 0;
        inputActions = GameObject.Find("Player").GetComponent<PlayerMovement>().inputActions;
        
        operaMusicInstance = RuntimeManager.CreateInstance(operaMusic);
        RuntimeManager.AttachInstanceToGameObject(operaMusicInstance, soundSource.transform);
    }
    
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.L))
        {
            singingIsFinished = true;
            GameManager.Instance.singingMiniGameCompleted = true;
            lyricsText.text = "";
            operaMusicInstance.setParameterByName("inEnd", 1);
            StartCoroutine(EyeClose());
        }
        
        if (recording && Microphone.GetPosition(null) >= clip.samples) {
            StopRecording();
        }

        if (interactable.objIsActive && !interactable.isMoving)
        {
            if (!inOrchestra)
            {
                inOrchestra = true;
                operaMusicInstance.start();
            }
            inputActions.Moving.Disable();
            if (!inVerse)
            {
                inVerse = true;
                lyricsText.text = lyrics[currentIndex];
                StartRecording();
            }
        }
    }
    
    private void CheckVerse()
    {
        if (CompareVerses())
        {
            Debug.Log("Correct verse");
            Debug.Log("Current Index: " + currentIndex);
            currentIndex++;
            if (currentIndex < lyrics.Count)
            {
                Debug.Log("Next verse");
                lyricsText.text = lyrics[currentIndex];
                StartCoroutine(StartVerse());
            }
            else
            {
                Debug.Log("Singing is finished");
                singingIsFinished = true;
                GameManager.Instance.singingMiniGameCompleted = true;
                lyricsText.text = "";
                operaMusicInstance.setParameterByName("inEnd", 1);
                StartCoroutine(EyeClose());
            }
        }
        else
        {
            
            Debug.Log("Incorrect verse");
            Debug.Log("Current Index: " + currentIndex);
            AudioManager.Instance.PlayOneShotAttached(correctSound, soundSource);
            StartCoroutine(StartVerse());
        }
    }

    private IEnumerator EyeClose()
    {
        Debug.Log("test");
        yield return new WaitForSeconds(5f);
        effects.CloseEyes();
        yield return new WaitForSeconds(12f);
        SceneManager.LoadScene("Intro");
    }
    
    private bool CompareVerses()
    {
        Debug.Log("Player Verse (unedited): \"" + currentPlayerLyrics+ "\"");

        var playerVerse = currentPlayerLyrics.Trim().ToLower().Replace("\n", "").Replace("\r", "").Replace(".", "")
            .Replace(",", "").Replace("!", "").Replace("?", "").Replace(";", "").Replace(":", "").Replace("(", "")
            .Replace(")", "").Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "").Replace("\"", "")
            .Replace("'", "");
        var correctVerse = lyrics[currentIndex].Trim().ToLower().Replace("\n", "").Replace("\r", "").Replace(".", "")
            .Replace(",", "").Replace("!", "").Replace("?", "").Replace(";", "").Replace(":", "").Replace("(", "")
            .Replace(")", "").Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "").Replace("\"", "")
            .Replace("'", "");
        
        Debug.Log("Player Verse: \"" + playerVerse + "\"");
        Debug.Log("Correct Verse:\"" + correctVerse + "\"");
        
        var playerVerseWords = playerVerse.Split(" ");
        var correctVerseWords = correctVerse.Split(" ");
        Debug.Log("Player Verse Words: " + playerVerseWords.Length);
        if (playerVerseWords.Length == 0)
        {
            return false;
        }
        
        var incorrectWords = 0;
        
        var lengthDifference = Mathf.Abs(playerVerseWords.Length - correctVerseWords.Length);
        
        if (lengthDifference > maxIncorrectWords)
        {
            return false;
        }

        for (var i = 0; i < playerVerseWords.Length; i++)
        {
            Debug.Log("Player Word: " + playerVerseWords[i]);
            if (i >= correctVerseWords.Length)
            {
                break;
            }
            if (playerVerseWords[i] != correctVerseWords[i])
            {
                Debug.Log("Incorrect word: " + playerVerseWords[i]);
                incorrectWords++;
            }

            if (incorrectWords > maxIncorrectWords)
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator StartVerse()
    {
        yield return new WaitForSeconds(0.5f);
        inVerse = true;
        StartRecording();
    }
    
    private void StartRecording() {
        bulb.StartBlinking();
        clip = Microphone.Start(AudioManager.Instance.currentMicrophone, false, recordingLength, 44100);
        recording = true;
    }
    
    private void StopRecording() {
        var position = Microphone.GetPosition(AudioManager.Instance.currentMicrophone);
        Microphone.End(AudioManager.Instance.currentMicrophone);
        var samples = new float[position * clip.channels];
        clip.GetData(samples, 0);
        bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);
        recording = false;
        SendRecording();
    }

    private void SendRecording() {
        bulb.StandBy();
        Debug.Log("Sending recording");
        HuggingFaceAPI.AutomaticSpeechRecognition(bytes, response => {
            currentPlayerLyrics = response;
            CheckVerse();
        }, error => {
            Debug.Log(error);
            StartCoroutine(RetryRecording());
        });
        Debug.Log("Recording sent");
    }
    
    private IEnumerator RetryRecording()
    {
        yield return new WaitForSeconds(APIRetryDelay);
        SendRecording();
    }

    private byte[] EncodeAsWAV(float[] samples, int frequency, int channels) {
        using (var memoryStream = new MemoryStream(44 + samples.Length * 2)) {
            using (var writer = new BinaryWriter(memoryStream)) {
                writer.Write("RIFF".ToCharArray());
                writer.Write(36 + samples.Length * 2);
                writer.Write("WAVE".ToCharArray());
                writer.Write("fmt ".ToCharArray());
                writer.Write(16);
                writer.Write((ushort)1);
                writer.Write((ushort)channels);
                writer.Write(frequency);
                writer.Write(frequency * channels * 2);
                writer.Write((ushort)(channels * 2));
                writer.Write((ushort)16);
                writer.Write("data".ToCharArray());
                writer.Write(samples.Length * 2);

                foreach (var sample in samples) {
                    writer.Write((short)(sample * short.MaxValue));
                }
            }
            return memoryStream.ToArray();
        }
    }

}
