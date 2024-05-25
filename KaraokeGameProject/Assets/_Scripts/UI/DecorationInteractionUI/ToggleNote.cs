using Assets._Scripts.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleNote : MonoBehaviour
{
    [SerializeField] Note note;

    public Note Note { get => note; }
}
