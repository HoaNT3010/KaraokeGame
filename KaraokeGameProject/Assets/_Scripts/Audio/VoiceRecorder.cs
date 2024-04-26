using System;
using System.IO;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VoiceRecorder : MonoBehaviour
{
    [SerializeField] private string fileName;

    private static AudioSource audioSource;
    private static float[] samplesData;

    private string currentMicrophone = string.Empty;
    private int defaultSampleRate = 44100;

    const string WAV_FILE_EXTENSION = ".wav";
    const int HEADER_SIZE = 44;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        InitializeMicrophone();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ListMicrophones();
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            StartRecording();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            StopRecording();
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

    private void ListMicrophones()
    {
        for (int i = 0; i < Microphone.devices.Length; i++)
        {
            Debug.Log("Microphone #" + (i + 1) + ": " + Microphone.devices[i]);
            Microphone.GetDeviceCaps(Microphone.devices[i], out int minFrequency, out int maxFrequency);
            Debug.Log($"Min freq: {minFrequency} - Max freq: {maxFrequency}");
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
        audioSource.clip = Microphone.Start(currentMicrophone, true, 5, defaultSampleRate);
    }

    public void StopRecording()
    {
        if (!IsCurrentMicrophoneAvailable())
        {
            Debug.Log("Failed to stop recording. Current microphone is not available!");
            return;
        }
        Debug.Log("Start saving file!");
        SaveRecording();
        Debug.Log("Stop recording!");
        Microphone.End(currentMicrophone);
    }

    private bool IsCurrentMicrophoneAvailable()
    {
        if (currentMicrophone == string.Empty || currentMicrophone == null)
        {
            return false;
        }
        return true;
    }

    private void SaveRecording()
    {
        // Wait until the microphone starts recording,
        // ensures that the subsequent code (such as retrieving audio data) runs only after recording has begun
        while (!(Microphone.GetPosition(null) > 0)) { }

        samplesData = new float[audioSource.clip.samples * audioSource.clip.channels];
        audioSource.clip.GetData(samplesData, 0);
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName + WAV_FILE_EXTENSION);

        // Delete the file if already exists
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        try
        {
            WriteWAVFile(audioSource.clip, filePath);
            Debug.Log("File Saved Successfully at: " + filePath);
        }
        catch (DirectoryNotFoundException)
        {
            Debug.LogError("Please, Create a StreamingAssets Directory in the Assets Folder");
        }
        catch (Exception e)
        {
            //check for other Exceptions
            Debug.Log(e.Message);
        }
    }

    private static void WriteWAVFile(AudioClip clip, string filePath)
    {
        float[] clipData = new float[clip.samples];

        // Create the file
        using (Stream fs = File.Create(filePath))
        {
            int frequency = clip.frequency;
            int numOfChannels = clip.channels;
            int samples = clip.samples;

            // Move the file pointer to the beginning of a file
            fs.Seek(0, SeekOrigin.Begin);

            //Header

            // Chunk ID
            byte[] riff = Encoding.ASCII.GetBytes("RIFF");
            fs.Write(riff, 0, 4);

            // ChunkSize
            byte[] chunkSize = BitConverter.GetBytes((HEADER_SIZE + clipData.Length) - 8);
            fs.Write(chunkSize, 0, 4);

            // Format
            byte[] wave = Encoding.ASCII.GetBytes("WAVE");
            fs.Write(wave, 0, 4);

            // Subchunk1ID
            byte[] fmt = Encoding.ASCII.GetBytes("fmt ");
            fs.Write(fmt, 0, 4);

            // Subchunk1Size
            byte[] subChunk1 = BitConverter.GetBytes(16);
            fs.Write(subChunk1, 0, 4);

            // AudioFormat
            byte[] audioFormat = BitConverter.GetBytes(1);
            fs.Write(audioFormat, 0, 2);

            // NumChannels
            byte[] numChannels = BitConverter.GetBytes(numOfChannels);
            fs.Write(numChannels, 0, 2);

            // SampleRate
            byte[] sampleRate = BitConverter.GetBytes(frequency);
            fs.Write(sampleRate, 0, 4);

            // ByteRate
            byte[] byteRate = BitConverter.GetBytes(frequency * numOfChannels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2
            fs.Write(byteRate, 0, 4);

            // BlockAlign
            ushort blockAlign = (ushort)(numOfChannels * 2);
            fs.Write(BitConverter.GetBytes(blockAlign), 0, 2);

            // BitsPerSample
            ushort bps = 16;
            byte[] bitsPerSample = BitConverter.GetBytes(bps);
            fs.Write(bitsPerSample, 0, 2);

            // Subchunk2ID
            byte[] datastring = Encoding.ASCII.GetBytes("data");
            fs.Write(datastring, 0, 4);

            // Subchunk2Size
            byte[] subChunk2 = BitConverter.GetBytes(samples * numOfChannels * 2);
            fs.Write(subChunk2, 0, 4);

            // Data

            clip.GetData(clipData, 0);
            short[] intData = new short[clipData.Length];
            byte[] bytesData = new byte[clipData.Length * 2];

            int convertionFactor = 32767;

            for (int i = 0; i < clipData.Length; i++)
            {
                intData[i] = (short)(clipData[i] * convertionFactor);
                byte[] byteArr = new byte[2];
                byteArr = BitConverter.GetBytes(intData[i]);
                byteArr.CopyTo(bytesData, i * 2);
            }

            fs.Write(bytesData, 0, bytesData.Length);
        }
    }

    public static byte[] ConvertWAVtoByteArray(string filePath)
    {
        //Open the stream and read it back.
        byte[] bytes = new byte[audioSource.clip.samples + HEADER_SIZE];
        using (FileStream fs = File.OpenRead(filePath))
        {
            fs.Read(bytes, 0, bytes.Length);
        }
        return bytes;
    }
}
