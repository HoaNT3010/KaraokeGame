using UnityEngine;

/// <summary>
/// Generic singleton class. Turn any component into a singleton component simply by inheriting this class.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance { get => instance; set => instance = value; }

    //public static T Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            instance = (T)FindObjectOfType(typeof(T));
    //            if (instance == null)
    //            {
    //                SetupInstance();
    //            }
    //        }
    //        return instance;
    //    }
    //}

    public virtual void Awake()
    {
        RemoveDuplicates();
    }

    private void RemoveDuplicates()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private static void SetupInstance()
    {
        Instance = (T)FindObjectOfType(typeof(T));
        if (Instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = typeof(T).Name;
            Instance = gameObj.AddComponent<T>();
            DontDestroyOnLoad(gameObj);
        }
    }

    public virtual void OnDestroy()
    {
        if (Instance != null)
        {
            Instance = null;
            Destroy(gameObject);
        }
    }
}
