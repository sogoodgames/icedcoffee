using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEditor;

public class ChatParserWindow : EditorWindow {
    string inputFolder = "";
    string outputFolder = "";

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    [MenuItem("Iced Coffee/Chat Parser")]
    static void Init() {
        ChatParserWindow window =
            (ChatParserWindow)EditorWindow.GetWindow(typeof(ChatParserWindow));
        window.Show();
    }

    // ------------------------------------------------------------------------
    void OnGUI() {
        EditorGUILayout.LabelField("Chat Parser", EditorStyles.largeLabel);
        EditorGUILayout.LabelField("Chat Folders");
        
        EditorGUILayout.LabelField(inputFolder);
        if(GUILayout.Button("Select Input Folder")) {
            inputFolder = EditorUtility.OpenFolderPanel("Chats", Application.dataPath, "");
        }

        EditorGUILayout.LabelField(outputFolder);
        if(GUILayout.Button("Select Output Folder")) {
            outputFolder = EditorUtility.OpenFolderPanel("Chats", Application.dataPath, "");
        }

        EditorGUI.BeginDisabledGroup(
            String.IsNullOrEmpty(inputFolder) ||
            String.IsNullOrEmpty(outputFolder)
        );
        if(GUILayout.Button("Import chats")) {
            int indexOfAssets = outputFolder.IndexOf("Assets");
            outputFolder = outputFolder.Substring(indexOfAssets);
            ChatParser.LoadChats(inputFolder, outputFolder);
        }
        EditorGUI.EndDisabledGroup();
    }
}