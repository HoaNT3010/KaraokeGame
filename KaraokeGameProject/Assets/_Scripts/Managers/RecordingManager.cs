using KaraokeGame.Recording.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using YoutubePlayer.Components;

namespace KaraokeGame
{
    public class RecordingManager : Singleton<RecordingManager>
    {
        public event EventHandler StartRecording;
        public event EventHandler EndRecording;

        public VoiceRecorder voiceRecorder;
        public VideoPlayer videoPlayer;
        public StartKaraoke startKaraoke;
        public InvidiousVideoPlayer invidiousVideoPlayer;
        public StopKaraoke stopKaraoke;
        public Button findVideoButton;
        public Button prepareVideoButton;
        [Header("Performances Managing")]
        public PerformanceList performanceList;
        public Button playPerformanceButton;
        public Button pausePerformanceButton;
        public Button stopPerformanceButton;
        public Button performanceButton;
        public AudioSource performanceAudioSource;

        const string Performance_Json_File_Name = "/performances.json";

        private bool isReplayPerformance = false;
        private bool isRecording = false;
        private bool isPerformancePaused = false;
        private PlayerPerformance currentPerformance;
        [SerializeField] public List<PlayerPerformance> performances = new List<PlayerPerformance>();

        private void Start()
        {
            LoadPerformances();
            stopKaraoke.GetComponent<Button>().interactable = false;
            currentPerformance = null;
            startKaraoke.StartKaraokeEvent += OnStartKaraoke;
            videoPlayer.loopPointReached += VideoPlayerEnd;
            stopKaraoke.StopKaraokeEvent += OnStopKaraoke;
            performanceList.OnPlayPerformance += HandlePlayPerformance;
            videoPlayer.prepareCompleted += OnReplayVideoPrepared;
            playPerformanceButton.interactable = false;
            pausePerformanceButton.interactable = false;
            stopPerformanceButton.interactable = false;
        }

        private void OnReplayVideoPrepared(VideoPlayer source)
        {
            if (isReplayPerformance)
            {
                playPerformanceButton.interactable = true;
                startKaraoke.GetComponent<Button>().interactable = false;
            }
        }

        public void ReplayPerformance()
        {
            videoPlayer.Play();
            performanceAudioSource.Play();

            playPerformanceButton.interactable = false;
            pausePerformanceButton.interactable = true;
            stopPerformanceButton.interactable = true;
            DisableRecordingButtons();
            Debug.Log("Play saved performance!");
        }

        public void PausePerformance()
        {
            if (isReplayPerformance)
            {
                if (!isPerformancePaused)
                {
                    isPerformancePaused = true;
                    videoPlayer.Pause();
                    performanceAudioSource.Pause();
                }
                else
                {
                    isPerformancePaused = false;
                    videoPlayer.Play();
                    performanceAudioSource.Play();
                }
                Debug.Log("Pause saved performance!");
            }
        }

        public void StopPerformance()
        {
            if (isReplayPerformance)
            {
                isReplayPerformance = false;
                videoPlayer.Stop();
                performanceAudioSource.Stop();
                playPerformanceButton.interactable = false;
                pausePerformanceButton.interactable = false;
                stopPerformanceButton.interactable = false;
                EnableRecordingButtons();
                Debug.Log("Stop playing saved performance!");
            }
        }

        private void DisableRecordingButtons()
        {
            performanceButton.interactable = false;
            findVideoButton.interactable = false;
            prepareVideoButton.interactable = false;
        }

        private void EnableRecordingButtons()
        {
            performanceButton.interactable = true;
            findVideoButton.interactable = true;
            prepareVideoButton.interactable = true;
        }

        private async void HandlePlayPerformance(object sender, PerformanceList.PlayPerformanceEventArgs e)
        {
            isReplayPerformance = true;
            // Audio
            AudioClip performanceRecording = WavUtility.ToAudioClip(e.recordingFilePath);
            performanceAudioSource.clip = performanceRecording;

            // Video
            invidiousVideoPlayer.VideoId = e.videoId;
            await invidiousVideoPlayer.PrepareVideoAsync();
        }

        private void OnStopKaraoke(object sender, EventArgs e)
        {
            OnStopRecording();
        }

        /// <summary>
        /// Logic to handle karaoke performance when the karaoke video reach the end
        /// </summary>
        /// <param name="source"></param>
        private void VideoPlayerEnd(VideoPlayer source)
        {
            if (isRecording)
            {
                OnStopRecording();
            }
            else if (isReplayPerformance)
            {
                StopPerformance();
            }
        }

        private void OnStopRecording()
        {
            if (isRecording)
            {
                EnableButtons();
                videoPlayer.Stop();
                string filePath = voiceRecorder.StopRecording();
                stopKaraoke.GetComponent<Button>().interactable = false;
                isRecording = false;
                currentPerformance.VoiceRecordingLocation = filePath;
                performances.Add(currentPerformance);
                SavePerformances();
                EndRecording?.Invoke(this, EventArgs.Empty);
            }
        }

        public void StopRecordingManually()
        {
            OnStopRecording();
        }

        private void OnStartKaraoke(object sender, System.EventArgs e)
        {
            if (!isRecording)
            {
                DisableButtons();
                voiceRecorder.StartRecording();
                videoPlayer.Play();
                voiceRecorder.fileName = DateTime.UtcNow.Ticks.ToString();
                isRecording = true;
                stopKaraoke.GetComponent<Button>().interactable = true;
                currentPerformance = new PlayerPerformance
                {
                    PerformanceId = Guid.NewGuid(),
                    PerformanceMode = PerformanceMode.Single,
                    YouTubeVideoId = invidiousVideoPlayer.VideoId,
                    PerformanceName = voiceRecorder.fileName,
                };
                StartRecording?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnDisable()
        {
            startKaraoke.StartKaraokeEvent -= OnStartKaraoke;
            videoPlayer.loopPointReached -= VideoPlayerEnd;
            stopKaraoke.StopKaraokeEvent -= OnStopKaraoke;
        }

        private void DisableButtons()
        {
            findVideoButton.interactable = false;
            prepareVideoButton.interactable = false;
        }

        private void EnableButtons()
        {
            findVideoButton.interactable = true;
            prepareVideoButton.interactable = true;
        }

        private void SavePerformances()
        {
            string savePath = Application.persistentDataPath + Performance_Json_File_Name;
            //string json = JsonUtility.ToJson(performances);
            string json = JsonConvert.SerializeObject(performances, Formatting.Indented);
            File.WriteAllText(savePath, json);
        }

        private void LoadPerformances()
        {
            string savePath = Application.persistentDataPath + Performance_Json_File_Name;
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                //performances = JsonUtility.FromJson<List<PlayerPerformance>>(json);
                performances = JsonConvert.DeserializeObject<List<PlayerPerformance>>(json);
            }
        }
    }
}
