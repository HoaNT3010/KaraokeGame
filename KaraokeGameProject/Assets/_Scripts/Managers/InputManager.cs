using System;
using System.IO;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private const string CHARACTER_KEYBINDS_SAVE_PATH = "/CharacterInput.txt";
    public CharacterKeybinds CharacterKeybinds;

    public override void Awake()
    {
        base.Awake();
        LoadCharacterKeybinds();
    }

    [ContextMenu("Save Character Keybinds")]
    public void SaveCharacterKeybinds()
    {
        string saveData = JsonUtility.ToJson(CharacterKeybinds, true);
        string filePath = string.Concat(Application.persistentDataPath, CHARACTER_KEYBINDS_SAVE_PATH);
        try
        {
            File.WriteAllText(filePath, saveData);
            Debug.Log("Character keybinds saved successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save character keybinds: {e.Message}");
        }
    }

    [ContextMenu("Load Character Keybinds")]
    public void LoadCharacterKeybinds()
    {
        string filePath = string.Concat(Application.persistentDataPath, CHARACTER_KEYBINDS_SAVE_PATH);
        if (File.Exists(filePath))
        {
            try
            {
                string savedData = File.ReadAllText(filePath);
                CharacterKeybinds savedKeybinds = JsonUtility.FromJson<CharacterKeybinds>(savedData);
                if (savedKeybinds != null)
                {
                    CharacterKeybinds = savedKeybinds;
                    Debug.Log("Character keybinds loaded successfully.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load character keybinds: {e.Message}");
            }
        }
        else
        {
            CharacterKeybinds = new CharacterKeybinds();
            Debug.Log("Saved character keybinds not exists. Initialize new character keybinds.");
        }
    }
}
