using Assets._Scripts.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Models
{
    public class Sheet
    {
        private int instrumentType = 0;
        private List<List<PianoNote>> pianoSheet = new();
    }
}
