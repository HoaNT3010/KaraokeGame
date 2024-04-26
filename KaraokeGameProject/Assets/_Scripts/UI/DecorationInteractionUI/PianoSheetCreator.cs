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
using MyUtils;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;
using Newtonsoft.Json;

public class PianoSheetCreator : MonoBehaviour
{
    [SerializeField] List<Toggle> pianoNoteButton = new();
    [SerializedDictionary("Piano Note", "Piano Clip")]
    public SerializedDictionary<PianoNote, AudioClip> pianoNoteAudio = new();
    [SerializeField] Slider tempoSlider;
    [SerializeField] int defaultTempo = 1;
    [SerializeField] TMP_InputField chordIndex;
    [SerializeField] TMP_InputField playAtIndex;
    [SerializeField] TMP_InputField fromIndex;
    [SerializeField] TMP_InputField toIndex;
    [SerializeField] TextMeshProUGUI textTempo;
    [SerializeField] TextMeshProUGUI sheetScrollView;

    [SerializeField] AudioClip audioClipTest;

    private List<List<PianoNote>> pianoSheet = new();

    private int selectedChordIndex = 1;
    private int maxChordIndex = 1;
    private AudioSource audioSource;
    private Coroutine playCoroutine;
    private List<List<PianoNote>> copyCache = new();
    private int fromIndexValue = 1;
    private int toIndexValue = 1;
    private ExplorerUtils explorer = new();

    private void Start()
    {
        pianoSheet.Add(new());
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
        if (selectedChordIndex > pianoSheet.Count)
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
                if (pianoSheet[selectedChordIndex - 1].Contains(toggle.GetComponent<ToggleNote>().Note))
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
        if (index > pianoSheet.Count) index = pianoSheet.Count;
        playAtIndex.text = index.ToString();
    }

    public void JumpTo(int chordIndexToJump)
    {
        if (chordIndexToJump < 1) chordIndexToJump = 1;
        if (chordIndexToJump > pianoSheet.Count)
        {
            chordIndexToJump = pianoSheet.Count;
        }
        selectedChordIndex = chordIndexToJump;
        chordIndex.text = "" + selectedChordIndex;
        foreach (var toggle in pianoNoteButton)
        {
            toggle.isOn = false;
        }
        foreach (var toggle in pianoNoteButton)
        {
            if (pianoSheet[selectedChordIndex - 1].Contains(toggle.GetComponent<ToggleNote>().Note))
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
            if (selectedChordIndex > pianoSheet.Count)
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
                    if (pianoSheet[selectedChordIndex - 1].Contains(toggle.GetComponent<ToggleNote>().Note))
                    {
                        toggle.isOn = true;
                    }
                }
            }
        }
        if (pianoSheet.Count < 1) pianoSheet.Add(new());
        pianoSheet[selectedChordIndex - 1].Clear();
        foreach (var toggle in pianoNoteButton)
        {
            if (toggle.isOn)
            {
                pianoSheet[selectedChordIndex - 1].Add(toggle.GetComponent<ToggleNote>().Note);
            }
        }
        UpdateSheetScrollView();
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

    private IEnumerator PlayAt(int index)
    {
        if (index < pianoSheet.Count)
        {
            for (int i = index - 1; i < pianoSheet.Count; i++)
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
        SaveCurrentChord();
    }

    public void OnButtonInsertAboveClick()
    {
        pianoSheet.Insert(selectedChordIndex - 1, new());
        JumpTo(selectedChordIndex);
    }

    public void OnButtonInsertBelowClick()
    {
        pianoSheet.Insert(selectedChordIndex, new());
        selectedChordIndex++;
        JumpTo(selectedChordIndex);
    }

    public void OnButtonDeleteRowClick()
    {
        if (pianoSheet.Count > 1)
        {
            pianoSheet.RemoveAt(selectedChordIndex - 1);
            JumpTo(selectedChordIndex);
        }
    }

    public void OnSelectionIndexTextChange()
    {
        fromIndexValue = Int32.Parse(fromIndex.text);
        toIndexValue = Int32.Parse(toIndex.text);
        if (fromIndexValue < 1) fromIndexValue = 1;
        if (fromIndexValue > pianoSheet.Count) fromIndexValue = pianoSheet.Count;
        if (toIndexValue < fromIndexValue) toIndexValue = fromIndexValue;
        if (toIndexValue > pianoSheet.Count) toIndexValue = pianoSheet.Count;
        fromIndex.text = fromIndexValue.ToString();
        toIndex.text = toIndexValue.ToString();
    }
    public void OnButtonCopyClick()
    {
        copyCache = pianoSheet.GetRange(fromIndexValue - 1, toIndexValue - fromIndexValue + 1);

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
                pianoSheet.Insert(selectedChordIndex, chord);
                selectedChordIndex++;
            }
            selectedChordIndex = Int32.Parse(chordIndex.text);
        }
    }
    public void OnButtonDeleteClick()
    {
        if (pianoSheet.Count <= 1) return;
        selectedChordIndex = fromIndexValue;
        pianoSheet.RemoveRange(fromIndexValue - 1, toIndexValue - fromIndexValue + 1);
        selectedChordIndex--;
        if (selectedChordIndex < 1) selectedChordIndex = 1;
        if (pianoSheet.Count < 1) pianoSheet.Add(new());
        Debug.Log(selectedChordIndex + " | " + pianoSheet.Count);
        OnSelectionIndexTextChange();
        JumpTo(selectedChordIndex);
    }

    public void OnButtonImportClick()
    {
        pianoSheet.Clear();
        pianoSheet = explorer.OpenFileBrowser<List<List<PianoNote>>>();
        Debug.Log(JsonConvert.SerializeObject(pianoSheet[0]));
        PrintStatus();
        selectedChordIndex = 1;
        JumpTo(selectedChordIndex);
    }
    
    public void OnButtonExportClick()
    {
        SaveSheet saveSheet = new(pianoSheet);
        explorer.SaveFileBrowser(pianoSheet.ToArray());
    }

    private void PrintStatus()
    {
        Debug.Log(JsonConvert.SerializeObject(pianoSheet));
        //Debug.Log(selectedChordIndex - 1 + " | "
        //    + string.Join(" - ", pianoSheet[selectedChordIndex - 1].ToArray())
        //    + " | " + pianoSheet[selectedChordIndex - 1].Count);
    }



}

[Serializable]
public class SaveSheet
{
    [SerializeField] List<List<PianoNote>> pianoSheet = new();

    public SaveSheet(List<List<PianoNote>> pianoSheet)
    {
        this.pianoSheet = pianoSheet;
    }
}
