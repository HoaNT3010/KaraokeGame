using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject keyRebindingCanvas;

    private void Start()
    {
        keyRebindingCanvas.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            keyRebindingCanvas.gameObject.SetActive(!keyRebindingCanvas.gameObject.activeInHierarchy);
        }
    }
}
