using Assets._Scripts.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Models
{
    public class Lyric
    {
        [SerializeField] private List<List<string>> lyricContent = new();

        public Lyric()
        {
            if (lyricContent.Count == 0) lyricContent.Add(new List<string>());
            
        }

        public Lyric(List<List<string>> lyricContent)
        {
            this.LyricContent = lyricContent;
            if (lyricContent.Count == 0) lyricContent.Add(new List<string>());
        }

        public List<List<string>> LyricContent { get => lyricContent; set => lyricContent = value; }
    }
}
