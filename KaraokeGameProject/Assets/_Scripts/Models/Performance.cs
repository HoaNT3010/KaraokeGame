using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Models
{
    public class Performance
    {
        [SerializeField] private List<Song> song = new();
        [SerializeField] private List<VoiceRecord> voiceRecord = new();

        public Performance()
        {
            if (song.Count == 0) song.Add(new());
            if (voiceRecord.Count == 0) voiceRecord.Add(new());
        }

        public Performance(List<Song> song, List<VoiceRecord> voiceRecord)
        {
            this.Song = song;
            this.VoiceRecord = voiceRecord;
            if (song.Count == 0) song.Add(new());
            if (voiceRecord.Count == 0) voiceRecord.Add(new());
        }

        public List<Song> Song { get => song; set => song = value; }
        public List<VoiceRecord> VoiceRecord { get => voiceRecord; set => voiceRecord = value; }
    }
}
