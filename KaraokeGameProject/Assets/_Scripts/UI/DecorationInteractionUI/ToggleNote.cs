using Assets._Scripts.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleNote : MonoBehaviour
{
    [SerializeField] PianoNote note;

    public PianoNote Note { get => note; }
}
