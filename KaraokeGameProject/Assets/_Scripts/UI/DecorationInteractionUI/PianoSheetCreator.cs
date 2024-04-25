using Assets._Scripts.Enum;
using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class PianoSheetCreator : MonoBehaviour
{
    [SerializeField] List<Toggle> pianoNoteButton = new();
    [SerializedDictionary("Piano Note", "Piano Clip")]
    public SerializedDictionary<PianoNote, AudioClip> pianoNoteAudio = new();
    [SerializeField] Slider tempoSlider;
    [SerializeField] int defaultTempo = 1;
    [SerializeField] TextMeshProUGUI chordIndex;
    [SerializeField] TextMeshProUGUI textTempo;
    [SerializeField] TextMeshProUGUI sheetScrollView;

    [SerializeField] AudioClip audioClipTest;

    List<List<PianoNote>> pianoSheet = new();

    private int selectedBeatIndex = 1;
    private int maxBeatIndex = 1;
    private AudioSource audioSource;

    private Coroutine playCoroutine;

    private void Start()
    {
        pianoSheet.Add(new());
        chordIndex.text = "Chord " + selectedBeatIndex;
        tempoSlider.value = defaultTempo;
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("SaveCurrentBeat", 0f, 0.1f);
    }
    public void OnTempoSliderValueChanged()
    {
        textTempo.text = "Tempo: " + tempoSlider.value;

    }

    public void OnBeatIndexIncrease()
    {
        SaveCurrentBeat();
        selectedBeatIndex++;
        chordIndex.text = "Chord " + selectedBeatIndex;
        if (selectedBeatIndex > maxBeatIndex)
        {
            maxBeatIndex = selectedBeatIndex;
        }
        if (selectedBeatIndex > pianoSheet.Count)
        {
            pianoSheet.Add(new List<PianoNote>());
            foreach (var toggle in pianoNoteButton)
            {
                toggle.isOn = false;
            }
        }
        else
        {
            foreach (var toggle in pianoNoteButton)
            {
                toggle.isOn = false;
            }
            foreach (var toggle in pianoNoteButton)
            {
                if (pianoSheet[selectedBeatIndex - 1].Contains(toggle.GetComponent<ToggleNote>().Note))
                {
                    toggle.isOn = true;
                }
            }
        }
        //PrintStatus()
    }
    public void OnBeatIndexDecrease()
    {
        SaveCurrentBeat();
        if (selectedBeatIndex > 1)
        {
            selectedBeatIndex--;
            chordIndex.text = "Chord " + selectedBeatIndex;
        }

        foreach (var toggle in pianoNoteButton)
        {
            toggle.isOn = false;
        }
        foreach (var toggle in pianoNoteButton)
        {
            if (pianoSheet[selectedBeatIndex - 1].Contains(toggle.GetComponent<ToggleNote>().Note))
            {
                toggle.isOn = true;
            }
        }

        //PrintStatus()

    }

    public void SaveCurrentBeat()
    {
        if (selectedBeatIndex < 1)
        {
            selectedBeatIndex = 1;
            chordIndex.text = "Chord " + selectedBeatIndex;
            if (selectedBeatIndex > maxBeatIndex)
            {
                maxBeatIndex = selectedBeatIndex;
            }
            if (selectedBeatIndex > pianoSheet.Count)
            {
                pianoSheet.Add(new List<PianoNote>());
                foreach (var toggle in pianoNoteButton)
                {
                    toggle.isOn = false;
                }
            }
            else
            {
                foreach (var toggle in pianoNoteButton)
                {
                    toggle.isOn = false;
                }
                foreach (var toggle in pianoNoteButton)
                {
                    if (pianoSheet[selectedBeatIndex - 1].Contains(toggle.GetComponent<ToggleNote>().Note))
                    {
                        toggle.isOn = true;
                    }
                }
            }
        }
        if (pianoSheet.Count < 1) pianoSheet.Add(new());
        pianoSheet[selectedBeatIndex - 1].Clear();
        foreach (var toggle in pianoNoteButton)
        {
            if (toggle.isOn)
            {
                //Debug.Log(toggle.GetComponent<ToggleNote>().Note);
                pianoSheet[selectedBeatIndex - 1].Add(toggle.GetComponent<ToggleNote>().Note);
            }
        }
        UpdateSheetScrollView();
        //PrintStatus()
    }

    public void UpdateSheetScrollView()
    {
        string text = "";
        for (int i = 0; i < pianoSheet.Count; i++)
        {
            text += (i + 1) + " | ";
            for (int j = 0; j < pianoSheet[i].Count; j++)
            {
                text += " " + pianoSheet[i][j].ToString() + "+";
            }
            text = text.Remove(text.Length - 1);
            text += "\n";
        }
        sheetScrollView.text = text;
    }

    public void OnButtonPlayClick()
    {
        SaveCurrentBeat();
        playCoroutine = StartCoroutine(PlayNote());

    }
    public void OnButtonPlayAtCurrentClick()
    {
        SaveCurrentBeat();
        playCoroutine = StartCoroutine(PlayAtCurrent(selectedBeatIndex));

    }

    public void OnButtonStopClick()
    {
        StopCoroutine(playCoroutine);
    }

    private IEnumerator PlayNote()
    {
        foreach (var chord in pianoSheet)
        {
            foreach (var note in chord)
            {
                //Debug.Log(pianoNoteAudio[note]);
                audioSource.PlayOneShot(pianoNoteAudio[note]);
            }
            yield return new WaitForSeconds(60 / tempoSlider.value);
        }
    }

    private IEnumerator PlayAtCurrent(int current)
    {
        if (current < pianoSheet.Count)
        {
            for (int i = current - 1; i < pianoSheet.Count; i++)
            {
                foreach (var note in pianoSheet[i])
                {
                    audioSource.PlayOneShot(pianoNoteAudio[note]);
                }
                yield return new WaitForSeconds(60 / tempoSlider.value);
            }

        }
    }

    public void OnButtonClearClick()
    {
        foreach (var toggle in pianoNoteButton)
        {
            toggle.isOn = false;
        }
        SaveCurrentBeat();
    }

    public void OnButtonInsertClick()
    {
        pianoSheet.Insert(selectedBeatIndex - 1, new());
        OnButtonClearClick();
    }

    public void OnButtonDeleteClick()
    {
        if (pianoSheet.Count > 1)
        {
            pianoSheet.RemoveAt(selectedBeatIndex - 1);
            selectedBeatIndex--;
            chordIndex.text = "Chord " + selectedBeatIndex;

            foreach (var toggle in pianoNoteButton)
            {
                toggle.isOn = false;
            }
            foreach (var toggle in pianoNoteButton)
            {
                if (pianoSheet[selectedBeatIndex - 1].Contains(toggle.GetComponent<ToggleNote>().Note))
                {
                    toggle.isOn = true;
                }
            }
        }
    }

    private void PrintStatus()
    {
        Debug.Log(selectedBeatIndex - 1 + " | "
            + string.Join(" - ", pianoSheet[selectedBeatIndex - 1].ToArray())
            + " | " + pianoSheet[selectedBeatIndex - 1].Count);
    }



}
