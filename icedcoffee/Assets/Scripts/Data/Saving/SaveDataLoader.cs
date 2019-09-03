using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

public class SaveDataLoader {
    public PlayerSaveData SaveData;
    public string DirectoryName = "saves";
    public string FileName = "save.binary";

    public SaveDataLoader () {
        Load();
    }

    public void Save () {
        string filePath = GetFilePath();
        FileStream saveFile = File.Create(filePath);

        BinaryFormatter formatter = new BinaryFormatter();
        try {
            formatter.Serialize(saveFile, SaveData);
        }
        catch (SerializationException e) {
            Debug.LogError("File saving failed; reason: " + e.Message);
        }

        saveFile.Close();
    }

    private void Load () {
        string filePath = GetFilePath();

        if(!File.Exists(filePath)) {
            // if save file doesn't exist yet,
            // create it and create an empty player save data
            SaveData = new PlayerSaveData();
            Save();
            return;
        }
        
        FileStream saveFile = File.Open(filePath, FileMode.Open);
        BinaryFormatter formatter = new BinaryFormatter();
        try {
            SaveData = (PlayerSaveData)formatter.Deserialize(saveFile);
        } 
        catch (SerializationException e) {
            Debug.LogError("File loading failed; reason: " + e.Message);
        }

        saveFile.Close();
    }

    private string GetFilePath () {
        string dir = Application.persistentDataPath + "/" + DirectoryName;
        if(!Directory.Exists(dir)) {
            Directory.CreateDirectory(dir);
        }
        dir += "/" + FileName;
        return dir;
    }
}