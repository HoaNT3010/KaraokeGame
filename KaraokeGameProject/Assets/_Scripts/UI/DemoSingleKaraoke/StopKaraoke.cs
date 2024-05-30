using System;
using UnityEngine;
using UnityEngine.UI;

namespace KaraokeGame
{
    public class StopKaraoke : MonoBehaviour
    {
        public event EventHandler StopKaraokeEvent;

        private void Start()
        {
            gameObject.GetComponent<Button>().interactable = false;
        }

        public void ClickStopKaraoke()
        {
            StopKaraokeEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
