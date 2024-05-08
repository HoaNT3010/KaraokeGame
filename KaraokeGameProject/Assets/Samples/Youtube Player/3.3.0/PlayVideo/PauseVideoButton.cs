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

        void Awake()
        {
            m_Button = GetComponent<Button>();
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
            }
            else if (videoPlayer.isPaused)
            {
                videoPlayer.Play();
            }
        }

        void OnDestroy()
        {
            videoPlayer.prepareCompleted -= VideoPlayerOnPrepareCompleted;
        }
    }
}

