using System.Collections;
using System.Collections.Generic;
using System.IO;
using FMOD.Studio;
using FMODUnity;
using HuggingFace.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Singing : MonoBehaviour
{
    //todo use selected microphone from settings
    
    [SerializeField] Interactable interactable;
    
    [SerializeField] private List<string> lyrics;
    [SerializeField] private TextMeshProUGUI lyricsText;
    [SerializeField] private float APIRetryDelay;
    [SerializeField] private EventReference correctSound;
    [SerializeField] private EventReference wrongSound;
    [SerializeField] private EventReference operaMusic;
    private EventInstance operaMusicInstance;
    [SerializeField] private Transform soundSource;
    [SerializeField] private int maxIncorrectWords;
    [SerializeField] private int recordingLength;
    
    private int currentIndex;
    private string currentPlayerLyrics;
    [HideInInspector] public bool singingIsFinished;
    private bool inVerse;
    
    private AudioClip clip;
    private byte[] bytes;
    private bool recording;
    
    private PlayerInput inputActions;
    

    private void Start()
    {
        currentIndex = 0;
        inputActions = GameObject.Find("Player").GetComponent<PlayerMovement>().inputActions;
        
        operaMusicInstance = RuntimeManager.CreateInstance(operaMusic);
        RuntimeManager.AttachInstanceToGameObject(operaMusicInstance, soundSource.transform);
    }
    
    private void Update()
    {
        if (recording && Microphone.GetPosition(null) >= clip.samples) {
            StopRecording();
        }

        if (interactable.objIsActive && !interactable.isMoving)
        {
            inputActions.Singing.Enable();
            if (!inVerse)
            {
                inVerse = true;
                lyricsText.text = lyrics[currentIndex];
                StartRecording();
            }
        }

        /*if (inputActions.Clocks.Cancel.triggered && interactable.objIsActive)
        {
            interactable.isMoving = true;
            inputActions.Clocks.Disable();
        }*/
    }
    
    private void CheckVerse()
    {
        if (CompareVerses())
        {
            AudioManager.Instance.PlayOneShotAttached(correctSound, soundSource);
            currentIndex++;
            if (currentIndex < lyrics.Count)
            {
                lyricsText.text = lyrics[currentIndex];
                StartCoroutine(StartVerse());
            }
            else
            {
                //todo finish logic
                singingIsFinished = true;
                GameManager.Instance.singingMiniGameCompleted = true;
            }
        }
        else
        {
            AudioManager.Instance.PlayOneShotAttached(wrongSound, soundSource);
            StartCoroutine(StartVerse());
        }
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
        
        clip = Microphone.Start(GameManager.Instance.currentMicrophone, false, recordingLength, 44100);
        recording = true;
    }
    
    private void StopRecording() {
        var position = Microphone.GetPosition(GameManager.Instance.currentMicrophone);
        Microphone.End(GameManager.Instance.currentMicrophone);
        var samples = new float[position * clip.channels];
        clip.GetData(samples, 0);
        bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);
        recording = false;
        SendRecording();
    }

    private void SendRecording() {
        HuggingFaceAPI.AutomaticSpeechRecognition(bytes, response => {
            currentPlayerLyrics = response;
            CheckVerse();
        }, error => {
            Debug.Log(error);
            StartCoroutine(RetryRecording());
        });
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
