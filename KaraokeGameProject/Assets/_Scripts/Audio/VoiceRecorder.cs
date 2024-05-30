using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VoiceRecorder : MonoBehaviour
{
    public string fileName;

    private static AudioSource audioSource;
    private bool isRecording;
    private string currentMicrophone = string.Empty;
    private float beginRecordTime;
    private float recordingDuration;

    const int DEFAULT_SAMPLE_RATES = 44100;
    const int MAX_RECORDING_DURATION_SECONDS = 600;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        isRecording = false;
        InitializeMicrophone();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            StartRecording();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            StopRecording();
        }
        AutomaticallyStopRecording();
    }

    /// <summary>
    /// Automatically stop and save the voice recording when the audio clip reach maximum duration.
    /// </summary>
    private void AutomaticallyStopRecording()
    {
        // Check if only is recording player voice
        if (isRecording)
        {
            recordingDuration -= Time.deltaTime;
            if (recordingDuration <= 0f)
            {
                Debug.Log("Recording has exceeds maximum length. Automatically stop and save recording!");
                StopRecording();
            }
        }
    }

    private void InitializeMicrophone()
    {
        if (Microphone.devices.Length <= 0)
        {
            currentMicrophone = string.Empty;
            Debug.Log("No microphone found. Please check your computer's devices");
            return;
        }
        currentMicrophone = Microphone.devices[0];
    }

    [ContextMenu("List Available Microphones")]
    public void ListMicrophones()
    {
        for (int i = 0; i < Microphone.devices.Length; i++)
        {
            Debug.Log("Microphone #" + (i + 1) + ": " + Microphone.devices[i]);
            Microphone.GetDeviceCaps(Microphone.devices[i], out int minFrequency, out int maxFrequency);
            Debug.Log($"Min frequency: {minFrequency} - Max frequency: {maxFrequency}");
        }
    }

    public void StartRecording()
    {
        if (!IsCurrentMicrophoneAvailable())
        {
            Debug.Log("Failed to start recording. Current microphone is not available!");
            return;
        }
        Debug.Log("Start recording!");
        beginRecordTime = Time.time;
        recordingDuration = MAX_RECORDING_DURATION_SECONDS;
        isRecording = true;
        Debug.Log("Begin recording at: " + beginRecordTime);
        audioSource.clip = Microphone.Start(currentMicrophone, false, MAX_RECORDING_DURATION_SECONDS, DEFAULT_SAMPLE_RATES);
    }

    public string StopRecording()
    {
        if (!IsCurrentMicrophoneAvailable())
        {
            Debug.Log("Failed to stop recording. Current microphone is not available!");
            return null;
        }
        isRecording = false;
        Debug.Log("Start saving file!");
        string filePath = SavingWavFile();
        Debug.Log("Stop recording!");
        Microphone.End(currentMicrophone);
        return filePath;
    }

    public string SavingWavFile()
    {
        string filePath;
        WavUtility.FromAudioClip(ExtractRecordedSound(audioSource.clip), out filePath, "", fileName);
        return filePath;
    }

    private bool IsCurrentMicrophoneAvailable()
    {
        if (currentMicrophone == string.Empty || currentMicrophone == null)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Extracts a portion of the original audio clip based on the elapsed recording time.
    /// </summary>
    /// <param name="originalClip">The original audio clip to extract from.</param>
    /// <returns>A new AudioClip containing the recorded sound.</returns>
    private AudioClip ExtractRecordedSound(AudioClip originalClip)
    {
        // Check if the input clip is null
        if (originalClip == null)
        {
            Debug.LogError("Input clip is null. Cannot extract recorded sound.");
            return null;
        }
        // Calculate the time elapsed since recording started
        float timeSinceRecordStarted = Time.time - beginRecordTime;
        // Calculate the samples per second (sampling rate)
        float samplesPerSec = originalClip.samples / originalClip.length;

        // Determine the number of recorded samples based on elapsed time
        int recordedSampleCount = Mathf.FloorToInt(samplesPerSec * timeSinceRecordStarted);

        // Create an array to store the recorded audio samples
        float[] recordedSamples = new float[recordedSampleCount];
        originalClip.GetData(recordedSamples, 0);

        // Create a new AudioClip to hold the recorded sound
        AudioClip recordedClip = AudioClip.Create("RecordedSound", recordedSampleCount, 1, DEFAULT_SAMPLE_RATES, false);
        recordedClip.SetData(recordedSamples, 0);

        // Return the newly created recorded clip
        return recordedClip;
    }
}
