using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace KaraokeGame
{
    public class FindVideoUI : MonoBehaviour
    {
        public GameObject FindVideoPanel;
        public VideoPlayer VideoPlayer;

        private void Awake()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            FindVideoPanel.SetActive(false);
        }

        public void ShowFindVideoPanel()
        {
            FindVideoPanel.SetActive(!FindVideoPanel.activeSelf);
        }

        private void OnEnable()
        {
            VideoPlayer.started += OnVideoPlayerStarted;
        }

        private void OnVideoPlayerStarted(VideoPlayer source)
        {
            FindVideoPanel.SetActive(false);
            gameObject.GetComponent<Button>().interactable = false;
        }

        private void OnDisable()
        {
            VideoPlayer.started -= OnVideoPlayerStarted;
        }
    }
}
