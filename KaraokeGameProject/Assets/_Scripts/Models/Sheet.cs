using Assets._Scripts.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Models
{
    public class Sheet
    {
        [SerializeField] private int instrumentType = 0;
        [SerializeField] private List<List<Note>> pianoSheet = new();

        public Sheet()
        {
            if (pianoSheet.Count == 0) pianoSheet.Add(new());
        }

        public Sheet(int instrumentType, List<List<Note>> pianoSheet)
        {
            this.InstrumentType = instrumentType;
            this.PianoSheet = pianoSheet;
            if (pianoSheet.Count == 0) pianoSheet.Add(new());
        }

        public int InstrumentType { get => instrumentType; set => instrumentType = value; }
        public List<List<Note>> PianoSheet { get => pianoSheet; set => pianoSheet = value; }
    }
}
