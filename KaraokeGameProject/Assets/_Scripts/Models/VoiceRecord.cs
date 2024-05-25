using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Models
{
    public class VoiceRecord
    {
        [SerializeField] private string recordUrl = "";

        public VoiceRecord()
        {
        }

        public VoiceRecord(string recordUrl)
        {
            this.recordUrl = recordUrl;
        }

        public string RecordUrl { get => recordUrl; set => recordUrl = value; }
    }
}
