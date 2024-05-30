using KaraokeGame.Recording.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KaraokeGame
{
    public static class ButtonExtension
    {
        public static void AddEventListener<T>(this Button button, T param, Action<T> OnClick)
        {
            button.onClick.AddListener(() =>
            {
                OnClick(param);
            });
        }
    }

    public class PerformanceList : MonoBehaviour
    {
        public class PlayPerformanceEventArgs : EventArgs
        {
            public string recordingFilePath;
            public string videoId;
        }

        public event EventHandler<PlayPerformanceEventArgs> OnPlayPerformance;


        private List<PlayerPerformance> performancesList;
        [SerializeField] private GameObject contentPanel;
        [SerializeField] private GameObject performanceTemplate;

        public GameObject emptyNotification;


        private void Awake()
        {
            performancesList = new List<PlayerPerformance>();
        }

        private void OnEnable()
        {
            emptyNotification.SetActive(false);
            if (RecordingManager.Instance == null)
            {
                return;
            }
            UpdatePerformanceContainer();
        }

        private void UpdatePerformanceContainer()
        {
            if (contentPanel.transform.childCount > 0)
            {
                performancesList.Clear();
                while (contentPanel.transform.childCount > 0)
                {
                    DestroyImmediate(contentPanel.transform.GetChild(0).gameObject);
                }
            }

            if (RecordingManager.Instance.performances == null)
            {
                emptyNotification.SetActive(true);
                return;
            }

            performancesList = RecordingManager.Instance.performances.ToList();

            if (performancesList.Count <= 0)
            {
                emptyNotification.SetActive(true);
                return;
            }
            else
            {
                emptyNotification.SetActive(false);
                GameObject newListItem;
                for (int i = 0; i < performancesList.Count; i++)
                {
                    newListItem = Instantiate(performanceTemplate, contentPanel.transform);
                    newListItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
                    newListItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = performancesList[i].PerformanceName;
                    newListItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = performancesList[i].CreatedDate.ToString("dd/MM/yyyy HH:mm:ss");

                    newListItem.GetComponent<Button>().AddEventListener(i, OnListItemClick);
                }
            }

        }

        private void OnListItemClick(int itemIndex)
        {
            OnPlayPerformance?.Invoke(this, new PlayPerformanceEventArgs { recordingFilePath = performancesList[itemIndex].VoiceRecordingLocation, videoId = performancesList[itemIndex].YouTubeVideoId });
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {

        }
    }
}
