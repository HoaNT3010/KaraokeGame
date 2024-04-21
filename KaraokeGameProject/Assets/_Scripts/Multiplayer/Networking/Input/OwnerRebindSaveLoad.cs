using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class OwnerRebindSaveLoad : NetworkBehaviour
{
    public InputActionAsset actions;

    public void OnEnable()
    {
        if (IsOwner)
        {
            string rebinds = PlayerPrefs.GetString("rebinds");
            if (!string.IsNullOrEmpty(rebinds))
                actions.LoadBindingOverridesFromJson(rebinds);
        } 
    }

    public void OnDisable()
    {
        if(IsOwner)
        {
            string rebinds = actions.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString("rebinds", rebinds);
        }
    }
}
