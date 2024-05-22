using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Models
{
    public class Song
    {
        [SerializeField] private List<Sheet> instrumentSheet = new();
        [SerializeField] private List<Lyric> lyric = new();

        public Song()
        {
            if (instrumentSheet.Count == 0) instrumentSheet.Add(new());
            if (lyric.Count == 0) lyric.Add(new());
        }

        public Song(List<Sheet> instrumentSheet, List<Lyric> lyric)
        {
            this.instrumentSheet = instrumentSheet;
            this.lyric = lyric;
            if (instrumentSheet.Count == 0) instrumentSheet.Add(new());
            if (lyric.Count == 0) lyric.Add(new());
        }

        public List<Sheet> InstrumentSheet { get => instrumentSheet; set => instrumentSheet = value; }
        public List<Lyric> Lyric { get => lyric; set => lyric = value; }
    }
}
