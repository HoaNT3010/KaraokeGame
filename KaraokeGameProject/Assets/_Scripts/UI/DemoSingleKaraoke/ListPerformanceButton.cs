using UnityEngine;

namespace KaraokeGame
{
    public class ListPerformanceButton : MonoBehaviour
    {
        public GameObject performanceListPanel;

        private void Start()
        {
            performanceListPanel.SetActive(false);
        }

        public void ShowPerformanceListPanel()
        {
            performanceListPanel.SetActive(!performanceListPanel.activeSelf);
        }
    }
}
