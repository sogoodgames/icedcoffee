using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
using UnityEngine.Assertions;

// "player save data" = progression, unlocks, etc
// "settings" = app/game settings like pronouns, name, volume, etc
public class SaveDataLoader {
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    public string DirectoryName = "saves";
    public string SaveDataFileName = "save.binary";
    public string SettingsFileName = "settings.binary";

    // do NOT fuck with this on accident
    private PlayerSaveData _saveData;
    public PlayerSaveData SaveData {
        get {return _saveData;}
    }

    // public for now because i'm not worried (yet) about code irresponsibly 
    // changing these settings
    public GameSettings Settings;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    private string SaveDataFilePath {
        get {return FilePath(SaveDataFileName);}
    }

    private string SettingsFilePath {
        get {return FilePath(SettingsFileName);}
    }

    // tells you whether or not a save file exists to load
    public bool CanLoadSaveData {
        get { return File.Exists(SaveDataFilePath); }
    }

    public bool CanLoadSettings {
        get { return File.Exists(SettingsFilePath); }
    }

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private string FilePath (string fileName) {
        string dir = Application.persistentDataPath + "/" + DirectoryName;
        if(!Directory.Exists(dir)) {
            Directory.CreateDirectory(dir);
        }
        dir += "/" + fileName;
        return dir;
    }

    // ------------------------------------------------------------------------
    // first time initialization of new save file
    public void CreateNewSave (
        List<ChatProgressionData> defaultChats,
        List<ClueID> defaultClues,
        List<GramPostProgressionData> defaultGrams
    ) {
        DateTime startTime = DateTime.Now;
        _saveData = new PlayerSaveData(defaultChats, defaultClues, defaultGrams, startTime);
        SavePlayerData();
    }

    // ------------------------------------------------------------------------
    // first time initialization of new settings save file
    public void CreateSettings (
        string proSubj,
        string proObj,
        string proPos,
        string name,
        bool save
    ) {
        Settings = new GameSettings (proSubj, proObj, proPos, name);
        if(save) {
            SaveSettings();
        }
    }

    // ------------------------------------------------------------------------
    public void SavePlayerData () {
        string filePath = SaveDataFilePath;
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

        //Debug.Log("SAVED PROGRESSION.");
        //LogSaveData();

        saveFile.Close();
    }

    // ------------------------------------------------------------------------
    public void SaveSettings () {
        string filePath = SettingsFilePath;
        FileStream saveFile = File.Create(filePath);
        Assert.IsTrue(
            File.Exists(filePath),
            "Failed to create settings file at path " + filePath
        );

        BinaryFormatter formatter = new BinaryFormatter();
        try {
            formatter.Serialize(saveFile, Settings);
        }
        catch (SerializationException e) {
            Debug.LogError("File saving failed; reason: " + e.Message);
        }

        Debug.Log("SAVED SETTINGS.");
        saveFile.Close();
    }

    // ------------------------------------------------------------------------
    public void LoadSaveData () {
        string filePath = SaveDataFilePath;
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

        Debug.Log("LOADED PROGRESSION.");
        //LogSaveData();
        saveFile.Close();
    }

    // ------------------------------------------------------------------------
    public void LoadSettings () {
        string filePath = SettingsFilePath;
        if(!File.Exists(filePath)) {
            return;
        }
        
        FileStream saveFile = File.Open(filePath, FileMode.Open);
        BinaryFormatter formatter = new BinaryFormatter();
        try {
            Settings = (GameSettings)formatter.Deserialize(saveFile);
        } 
        catch (SerializationException e) {
            Debug.LogError("File loading failed; reason: " + e.Message);
        }

        Debug.Log("LOADED SETTINGS.");
        saveFile.Close();
    }

    // ------------------------------------------------------------------------
    public void FoundClue (ClueID id) {
        _saveData.FoundClues.Add(id);
        SavePlayerData();
    }

    // ------------------------------------------------------------------------
    public void FoundChat (ChatProgressionData chatProgression) {
        _saveData.ChatProgressionData.Add(chatProgression);
        SavePlayerData();
    }

    // ------------------------------------------------------------------------
    public void FoundGram (GramPostProgressionData gramProgression) {
        _saveData.GramPostProgressionData.Add(gramProgression);
        SavePlayerData();
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