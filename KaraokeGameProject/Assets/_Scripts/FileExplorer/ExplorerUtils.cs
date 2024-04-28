
using AnotherFileBrowser.Windows;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace MyUtils
{
    public class ExplorerUtils
    {
        //public RawImage rawImage;

        public T OpenFileBrowser<T>()
        {
            var bp = new BrowserProperties();
            bp.filter = "JSON files (*.json, *.txt) | *.json; *.txt";
            bp.filterIndex = 0;
            T deserializeObject = default(T);
            new FileBrowser().OpenFileBrowser(bp, path =>
            {
                if (File.Exists(path))
                {
                    string loadData = File.ReadAllText(path);
                    deserializeObject = JsonConvert.DeserializeObject<T>(loadData);
                    
                }

                Debug.Log(path);
            });
            if (deserializeObject != null) { return deserializeObject; }
            return deserializeObject;
        }
        public void SaveFileBrowser<T>(T[] saveObject)
        {
            var bp = new BrowserProperties();
            bp.filter = "JSON files (*.json, *.txt) | *.json; *.txt";
            bp.filterIndex = 0;
            
            new FileBrowser().SaveFileBrowser(bp, path =>
            {
                if (!path.Contains(".json")) path += ".json";
                string jsonString = JsonConvert.SerializeObject(saveObject);
                Debug.Log(jsonString);
                File.WriteAllText(path, jsonString);

                
                Debug.Log(path);
            });

        }

    }
}
