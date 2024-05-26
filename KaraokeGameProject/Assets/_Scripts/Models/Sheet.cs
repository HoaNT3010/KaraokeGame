using Assets._Scripts.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Models
{
    public class InstrumentSheet
    {
        [SerializeField] private int instrumentType = 0;
        [SerializeField] private List<List<Note>> sheet = new();

        public InstrumentSheet()
        {
            if (sheet.Count == 0) sheet.Add(new());
        }

        public InstrumentSheet(int instrumentType, List<List<Note>> sheet)
        {
            this.InstrumentType = instrumentType;
            this.Sheet = sheet;
            if (sheet.Count == 0) sheet.Add(new());
        }

        public int InstrumentType { get => instrumentType; set => instrumentType = value; }
        public List<List<Note>> Sheet { get => sheet; set => sheet = value; }
    }
}
