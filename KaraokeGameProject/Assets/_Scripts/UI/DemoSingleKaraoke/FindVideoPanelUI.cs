using KaraokeGame.Utilities;
using TMPro;
using UnityEngine;
using YoutubePlayer.Components;

namespace KaraokeGame
{
    public class FindVideoPanelUI : MonoBehaviour
    {
        private string videoUrl;
        public InvidiousVideoPlayer invidiousVideoPlayer;
        public TMP_InputField urlInputField;
        public GameObject notificationPanel;

        private void Awake()
        {
            videoUrl = string.Empty;
            urlInputField.text = videoUrl;
        }

        public void CancelFindVideo()
        {
            gameObject.SetActive(false);
        }

        public void CloseFindVideo()
        {
            gameObject.SetActive(false);
        }

        public void ConfirmFindVideo()
        {
            videoUrl = urlInputField.text.Trim();
            string videoId = YouTubeUrlHelper.ExtractVideoIdFromUrl(videoUrl);
            if (string.IsNullOrEmpty(videoId))
            {
                SetNotification("Invalid YouTube video ID!");
                return;
            }

            bool isIdValid = YouTubeUrlHelper.ValidateVideoId(videoId);
            // Id format is not valid
            if (!isIdValid)
            {
                SetNotification($"The video ID {videoId} is not valid!");
                return;
            }
            // Id format is valid
            else
            {
                SetNotification($"Success! Video with ID {videoId} is set.");
                invidiousVideoPlayer.VideoId = videoId;
                return;
            }
        }

        private void OnEnable()
        {
            urlInputField.text = videoUrl;
            notificationPanel.SetActive(false);
        }

        private void OnDisable()
        {
            notificationPanel.SetActive(false);
        }

        private void SetNotification(string message)
        {
            notificationPanel.GetComponentInChildren<TextMeshProUGUI>().text = message;
            notificationPanel.SetActive(true);
        }
    }
}
