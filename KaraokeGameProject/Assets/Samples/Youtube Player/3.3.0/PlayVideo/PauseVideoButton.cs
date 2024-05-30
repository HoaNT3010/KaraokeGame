using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace YoutubePlayer.Samples.PlayVideo
{
    [RequireComponent(typeof(Button))]
    public class PauseVideoButton : MonoBehaviour
    {
        public VideoPlayer videoPlayer;

        Button m_Button;

        private TextMeshProUGUI buttonText;

        void Awake()
        {
            m_Button = GetComponent<Button>();
            buttonText = GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = "Pause Video";
            m_Button.interactable = videoPlayer.isPrepared;
            videoPlayer.prepareCompleted += VideoPlayerOnPrepareCompleted;
        }

        void VideoPlayerOnPrepareCompleted(VideoPlayer source)
        {
            m_Button.interactable = videoPlayer.isPrepared;
        }

        public void Pause()
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
                buttonText.text = "Resume Video";
            }
            else if (videoPlayer.isPaused)
            {
                videoPlayer.Play();
                buttonText.text = "Pause Video";
            }
        }

        void OnDestroy()
        {
            videoPlayer.prepareCompleted -= VideoPlayerOnPrepareCompleted;
        }
    }
}

