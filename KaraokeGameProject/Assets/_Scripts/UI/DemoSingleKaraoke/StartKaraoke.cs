using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace KaraokeGame
{
    public class StartKaraoke : MonoBehaviour
    {
        public event EventHandler StartKaraokeEvent;

        public VideoPlayer videoPlayer;
        private Button startButton;

        void Awake()
        {
            startButton = GetComponent<Button>();
            startButton.interactable = videoPlayer.isPrepared;
        }

        private void Start()
        {
            videoPlayer.prepareCompleted += VideoPlayerOnPrepareCompleted;
            RecordingManager.Instance.StartRecording += OnStartKaraokeRecording;
        }

        public void StartKaraokePerformance()
        {
            StartKaraokeEvent?.Invoke(this, EventArgs.Empty);
        }

        void VideoPlayerOnPrepareCompleted(VideoPlayer source)
        {
            startButton.interactable = videoPlayer.isPrepared;
        }

        private void OnEnable()
        {

        }

        private void OnStartKaraokeRecording(object sender, EventArgs e)
        {
            startButton.interactable = false;
        }

        void OnDisable()
        {
            //if (RecordingManager.Instance != null)
            //{
            //    RecordingManager.Instance.StartRecording -= OnStartKaraokeRecording;
            //}
            videoPlayer.prepareCompleted -= VideoPlayerOnPrepareCompleted;
        }
    }
}
