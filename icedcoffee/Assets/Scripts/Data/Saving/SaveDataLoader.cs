using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
using UnityEngine.Assertions;

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
    // Properties
    // ------------------------------------------------------------------------
    private string FilePath {
        get {
            string dir = Application.persistentDataPath + "/" + DirectoryName;
            if(!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            dir += "/" + FileName;
            return dir;
        }
    }

    // tells you whether or not a save file exists to load
    public bool CanLoad {
        get { return File.Exists(FilePath); }
    }

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    // first time initialization of new save
    public void CreateNewSave (
        List<ChatProgressionData> defaultChats,
        List<ClueID> defaultClues
    ) {
        Debug.Log("attempting create new save");

        DateTime startTime = DateTime.Now;
        _saveData = new PlayerSaveData(defaultChats, defaultClues, startTime);
        Save();
    }

    // ------------------------------------------------------------------------
    public void Save () {
        Debug.Log("attempting save");

        string filePath = FilePath;
        FileStream saveFile = File.Create(filePath);
        Assert.IsTrue(
            File.Exists(filePath),
            "Failed to create save file at path " + filePath
        );

        BinaryFormatter formatter = new BinaryFormatter();
        try {
            formatter.Serialize(saveFile, _saveData);
        }
        catch (SerializationException e) {
            Debug.LogError("File saving failed; reason: " + e.Message);
        }

        Debug.Log("SAVED.");
        LogSaveData();

        saveFile.Close();
    }

    // ------------------------------------------------------------------------
    public void Load () {
        string filePath = FilePath;
        Assert.IsTrue(
            File.Exists(filePath),
            "Save file does not exist at path " + filePath
        );
        
        FileStream saveFile = File.Open(filePath, FileMode.Open);
        Assert.IsNotNull(
            saveFile,
            "Tried to load save file, but file not found. Path: " + filePath
        );

        BinaryFormatter formatter = new BinaryFormatter();
        try {
            _saveData = (PlayerSaveData)formatter.Deserialize(saveFile);
        } 
        catch (SerializationException e) {
            Debug.LogError("File loading failed; reason: " + e.Message);
        }

        Debug.Log("LOADED.");
        LogSaveData();

        saveFile.Close();
    }

    // ------------------------------------------------------------------------
    public void FoundClue (ClueID id) {
        _saveData.FoundClues.Add(id);
        Save();
    }

    // ------------------------------------------------------------------------
    public void FoundChat (ChatProgressionData chatProgression) {
        _saveData.ChatProgressionData.Add(chatProgression);
        Save();
    }

    // ------------------------------------------------------------------------
    private void LogSaveData () {
        Debug.Log("- Start time: " + _saveData.GameStartTime.ToString());
        Debug.Log("- Found clues: " + _saveData.FoundClues.Count);
        Debug.Log("- Chats opened: ");
        foreach(ChatProgressionData chat in _saveData.ChatProgressionData) {
            chat.LogData();
        }
    }
}