using Assets._Scripts.Enum;
using AYellowpaper.SerializedCollections;
using JetBrains.Annotations;
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
using Newtonsoft.Json;
using Assets._Scripts.Models;

public class SheetCreator : MonoBehaviour
{
    [SerializeField] List<Toggle> NoteButton = new();
    [SerializedDictionary("Piano Note", "Piano Clip")]

    public SerializedDictionary<Note, AudioClip> PianoNoteAudio = new();

    [SerializeField] Slider tempoSlider;
    [SerializeField] int defaultTempo = 1;
    [SerializeField] TMP_InputField chordIndex;
    [SerializeField] TMP_InputField playAtIndex;
    [SerializeField] TMP_InputField fromIndex;
    [SerializeField] TMP_InputField toIndex;
    [SerializeField] TextMeshProUGUI textTempo;
    [SerializeField] TextMeshProUGUI sheetScrollView;

    [SerializeField] AudioClip audioClipTest;
    [SerializeField] TMP_Dropdown instrumentSelection;

    int currentInstrument;

    private Song song = new();

    //private List<List<Note>> song.InstrumentSheet[0].Sheet = new();

    private int selectedChordIndex = 1;
    private int maxChordIndex = 1;
    private AudioSource audioSource;
    private Coroutine playCoroutine;
    private List<List<Note>> copyCache = new();
    private int fromIndexValue = 1;
    private int toIndexValue = 1;
    //private ExplorerUtils explorer = new();

    private void Start()
    {
        song.InstrumentSheet.Add(new());
        chordIndex.text = "" + selectedChordIndex;
        playAtIndex.text = chordIndex.text;
        tempoSlider.value = defaultTempo;
        fromIndex.text = chordIndex.text;
        toIndex.text = chordIndex.text;
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating(nameof(SaveCurrentChord), 0f, 0.1f);
    }
    public void OnTempoSliderValueChanged()
    {
        textTempo.text = "Tempo: " + tempoSlider.value;

    }

    public void OnChordIndexIncrease()
    {
        SaveCurrentChord();
        selectedChordIndex++;
        chordIndex.text = "" + selectedChordIndex;
        if (selectedChordIndex > maxChordIndex)
        {
            maxChordIndex = selectedChordIndex;
        }
        if (selectedChordIndex > song.InstrumentSheet[0].Sheet.Count)
        {
            song.InstrumentSheet[0].Sheet.Add(new List<Note>());
            foreach (var toggle in NoteButton)
            {
                toggle.isOn = false;
            }
        }
        else
        {
            foreach (var toggle in NoteButton)
            {
                toggle.isOn = false;
            }
            foreach (var toggle in NoteButton)
            {
                if (song.InstrumentSheet[0].Sheet[selectedChordIndex - 1].Contains(toggle.GetComponent<ToggleNote>().Note))
                {
                    toggle.isOn = true;
                }
            }
        }
        //PrintStatus()
    }
    public void OnChordIndexDecrease()
    {
        SaveCurrentChord();
        if (selectedChordIndex > 1)
        {
            selectedChordIndex--;
            JumpTo(selectedChordIndex);
        }

    }

    public void OnChordTextChange()
    {
        selectedChordIndex = Int32.Parse(chordIndex.text);
        JumpTo(selectedChordIndex);
    }

    public void OnPlayAtIndexTextChange()
    {
        int index = Int32.Parse(playAtIndex.text);
        if (index < 1) index = 1;
        if (index > song.InstrumentSheet[0].Sheet.Count) index = song.InstrumentSheet[0].Sheet.Count;
        playAtIndex.text = index.ToString();
    }

    public void JumpTo(int chordIndexToJump)
    {
        if (chordIndexToJump < 1) chordIndexToJump = 1;
        if (chordIndexToJump > song.InstrumentSheet[0].Sheet.Count)
        {
            chordIndexToJump = song.InstrumentSheet[0].Sheet.Count;
        }
        selectedChordIndex = chordIndexToJump;
        chordIndex.text = "" + selectedChordIndex;
        foreach (var toggle in NoteButton)
        {
            toggle.isOn = false;
        }
        foreach (var toggle in NoteButton)
        {
            if (song.InstrumentSheet[0].Sheet[selectedChordIndex - 1].Contains(toggle.GetComponent<ToggleNote>().Note))
            {
                toggle.isOn = true;
            }
        }
    }

    public void SaveCurrentChord()
    {
        if (selectedChordIndex < 1)
        {
            selectedChordIndex = 1;
            chordIndex.text = "" + selectedChordIndex;
            if (selectedChordIndex > maxChordIndex)
            {
                maxChordIndex = selectedChordIndex;
            }
            if (selectedChordIndex > song.InstrumentSheet[0].Sheet.Count)
            {
                song.InstrumentSheet[0].Sheet.Add(new List<Note>());
                foreach (var toggle in NoteButton)
                {
                    toggle.isOn = false;
                }
            }
            else
            {
                foreach (var toggle in NoteButton)
                {
                    toggle.isOn = false;
                }
                foreach (var toggle in NoteButton)
                {
                    if (song.InstrumentSheet[0].Sheet[selectedChordIndex - 1].Contains(toggle.GetComponent<ToggleNote>().Note))
                    {
                        toggle.isOn = true;
                    }
                }
            }
        }
        if (song.InstrumentSheet[0].Sheet.Count < 1) song.InstrumentSheet[0].Sheet.Add(new());
        song.InstrumentSheet[0].Sheet[selectedChordIndex - 1].Clear();
        foreach (var toggle in NoteButton)
        {
            if (toggle.isOn)
            {
                song.InstrumentSheet[0].Sheet[selectedChordIndex - 1].Add(toggle.GetComponent<ToggleNote>().Note);
            }
        }
        UpdateSheetScrollView();
    }

    public void UpdateSheetScrollView()
    {
        string text = "";
        for (int i = 0; i < song.InstrumentSheet[0].Sheet.Count; i++)
        {
            text += (i + 1) + " | ";
            for (int j = 0; j < song.InstrumentSheet[0].Sheet[i].Count; j++)
            {
                text += " " + song.InstrumentSheet[0].Sheet[i][j].ToString() + "+";
            }
            text = text.Remove(text.Length - 1);
            text += "\n";
        }
        sheetScrollView.text = text;
    }

    public void OnButtonPlayClick()
    {
        SaveCurrentChord();
        OnButtonStopClick();
        playCoroutine = StartCoroutine(PlayNote());

    }

    public void OnButtonPlayAtClick()
    {
        SaveCurrentChord();
        OnButtonStopClick();
        playCoroutine = StartCoroutine(PlayAt(Int32.Parse(playAtIndex.text)));

    }

    public void OnButtonStopClick()
    {
        if (playCoroutine != null) StopCoroutine(playCoroutine);
    }

    private IEnumerator PlayNote()
    {
        foreach (var chord in song.InstrumentSheet[0].Sheet)
        {
            foreach (var note in chord)
            {
                //Debug.Log(PianoNoteAudio[note]);
                audioSource.PlayOneShot(PianoNoteAudio[note]);
            }
            yield return new WaitForSeconds(60 / tempoSlider.value);
        }
    }

    private IEnumerator PlayAt(int index)
    {
        if (index < song.InstrumentSheet[0].Sheet.Count)
        {
            for (int i = index - 1; i < song.InstrumentSheet[0].Sheet.Count; i++)
            {
                foreach (var note in song.InstrumentSheet[0].Sheet[i])
                {
                    audioSource.PlayOneShot(PianoNoteAudio[note]);
                }
                yield return new WaitForSeconds(60 / tempoSlider.value);
            }

        }
    }

    public void OnButtonClearClick()
    {
        foreach (var toggle in NoteButton)
        {
            toggle.isOn = false;
        }
        SaveCurrentChord();
    }

    public void OnButtonInsertAboveClick()
    {
        song.InstrumentSheet[0].Sheet.Insert(selectedChordIndex - 1, new());
        JumpTo(selectedChordIndex);
    }

    public void OnButtonInsertBelowClick()
    {
        song.InstrumentSheet[0].Sheet.Insert(selectedChordIndex, new());
        selectedChordIndex++;
        JumpTo(selectedChordIndex);
    }

    public void OnButtonDeleteRowClick()
    {
        if (song.InstrumentSheet[0].Sheet.Count > 1)
        {
            song.InstrumentSheet[0].Sheet.RemoveAt(selectedChordIndex - 1);
            JumpTo(selectedChordIndex);
        }
    }

    public void OnSelectionIndexTextChange()
    {
        fromIndexValue = Int32.Parse(fromIndex.text);
        toIndexValue = Int32.Parse(toIndex.text);
        if (fromIndexValue < 1) fromIndexValue = 1;
        if (fromIndexValue > song.InstrumentSheet[0].Sheet.Count) fromIndexValue = song.InstrumentSheet[0].Sheet.Count;
        if (toIndexValue < fromIndexValue) toIndexValue = fromIndexValue;
        if (toIndexValue > song.InstrumentSheet[0].Sheet.Count) toIndexValue = song.InstrumentSheet[0].Sheet.Count;
        fromIndex.text = fromIndexValue.ToString();
        toIndex.text = toIndexValue.ToString();
    }
    public void OnButtonCopyClick()
    {
        copyCache = song.InstrumentSheet[0].Sheet.GetRange(fromIndexValue - 1, toIndexValue - fromIndexValue + 1);

        var testString = fromIndexValue + " => " + toIndexValue + "\n";
        foreach (var item in copyCache)
        {
            foreach (var chord in item)
            {
                testString += chord.ToString() + " - ";
            }
            testString += "  |  ";
        }
        Debug.Log(testString);
    }
    public void OnButtonPasteClick()
    {
        if (copyCache != null)
        {
            foreach (var chord in copyCache)
            {
                song.InstrumentSheet[0].Sheet.Insert(selectedChordIndex, chord);
                selectedChordIndex++;
            }
            selectedChordIndex = Int32.Parse(chordIndex.text);
        }
    }
    public void OnButtonDeleteClick()
    {
        if (song.InstrumentSheet[0].Sheet.Count <= 1) return;
        selectedChordIndex = fromIndexValue;
        song.InstrumentSheet[0].Sheet.RemoveRange(fromIndexValue - 1, toIndexValue - fromIndexValue + 1);
        selectedChordIndex--;
        if (selectedChordIndex < 1) selectedChordIndex = 1;
        if (song.InstrumentSheet[0].Sheet.Count < 1) song.InstrumentSheet[0].Sheet.Add(new());
        Debug.Log(selectedChordIndex + " | " + song.InstrumentSheet[0].Sheet.Count);
        OnSelectionIndexTextChange();
        JumpTo(selectedChordIndex);
    }

    public void OnButtonImportClick()
    {
        //song.InstrumentSheet[0].Sheet.Clear();
        //song.InstrumentSheet[0].Sheet = explorer.OpenFileBrowser<List<List<Note>>>();
        //Debug.Log(JsonConvert.SerializeObject(song.InstrumentSheet[0].Sheet[0]));
        //PrintStatus();
        //selectedChordIndex = 1;
        //JumpTo(selectedChordIndex);
    }
    
    public void OnButtonExportClick()
    {
        //SaveSheet saveSheet = new(song.InstrumentSheet[0].Sheet);
        //explorer.SaveFileBrowser(song.InstrumentSheet[0].Sheet.ToArray());
    }

    private void PrintStatus()
    {
        Debug.Log(JsonConvert.SerializeObject(song.InstrumentSheet[0].Sheet));
        //Debug.Log(selectedChordIndex - 1 + " | "
        //    + string.Join(" - ", song.InstrumentSheet[0].Sheet[selectedChordIndex - 1].ToArray())
        //    + " | " + song.InstrumentSheet[0].Sheet[selectedChordIndex - 1].Count);
    }

    public void OnButtonChangeInstrumentClick()
    {

    }

    public void OnButtonPlayWholeSongClick()
    {

    }

    public void OnButtonPlayWholeSongAtClick()
    {

    }

}

[Serializable]
public class SaveSheet
{
    //[SerializeField] List<List<Note>> song.InstrumentSheet[0].Sheet = new();

    //public SaveSheet(List<List<Note>> song.InstrumentSheet[0].Sheet)
    //{
    //    this.song.InstrumentSheet[0].Sheet = song.InstrumentSheet[0].Sheet;
    //}
}
