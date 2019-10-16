using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

public class SaveDataLoader {
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public string DirectoryName = "saves";
    public string FileName = "save.binary";

    private PlayerSaveData _saveData;
    public PlayerSaveData SaveData {
        get {return _saveData;}
    }

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    // first time initialization of new save
    public void CreateNewSave () {
        _saveData = new PlayerSaveData();
        Save();
    }

    // ------------------------------------------------------------------------
    public void Save () {
        string filePath = GetFilePath();
        FileStream saveFile = File.Create(filePath);

        BinaryFormatter formatter = new BinaryFormatter();
        try {
            formatter.Serialize(saveFile, _saveData);
        }
        catch (SerializationException e) {
            Debug.LogError("File saving failed; reason: " + e.Message);
        }

        saveFile.Close();
    }

    // ------------------------------------------------------------------------
    public void Load () {
        string filePath = GetFilePath();

        if(!File.Exists(filePath)) {
            Debug.LogError("Save file does not exist and load attempted");
            return;
        }
        
        FileStream saveFile = File.Open(filePath, FileMode.Open);
        BinaryFormatter formatter = new BinaryFormatter();
        try {
            _saveData = (PlayerSaveData)formatter.Deserialize(saveFile);
        } 
        catch (SerializationException e) {
            Debug.LogError("File loading failed; reason: " + e.Message);
        }

        Debug.Log("Loaded save data with start time: " + _saveData.GameStartTime.ToString());
        Debug.Log("Found clues: " + _saveData.FoundClues.Count);

        saveFile.Close();
    }

    // ------------------------------------------------------------------------
    public void FoundClue (ClueID id) {
        _saveData.FoundClues.Add(id);
        Save();
    }

    // ------------------------------------------------------------------------
    private string GetFilePath () {
        string dir = Application.persistentDataPath + "/" + DirectoryName;
        if(!Directory.Exists(dir)) {
            Directory.CreateDirectory(dir);
        }
        dir += "/" + FileName;
        return dir;
    }
}