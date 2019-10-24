#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;

public static class GameDataEditorUtils { 
    private static int selection = 0;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public static void DrawValidationOutput (ValidationOutput validation) {
        EditorGUILayout.LabelField("Validation status:");
        
        if(validation.Successful) {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.green;
            EditorGUILayout.LabelField("PASS", style);
        } else {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;
            EditorGUILayout.LabelField("FAIL", style);
        }

        GUIStyle msgStyle = new GUIStyle();
        msgStyle.stretchWidth = true;
        msgStyle.stretchHeight = true;
        msgStyle.wordWrap = true;
        EditorGUILayout.LabelField("Message:");
        EditorGUILayout.LabelField(validation.Message.ToString(), msgStyle);
    }

    // ------------------------------------------------------------------------
    public static void DrawIconField (SerializedProperty iconProperty) {
        iconProperty.objectReferenceValue = EditorGUILayout.ObjectField(
            "Chat icon: ",
            iconProperty.objectReferenceValue,
            typeof(Sprite),
            false
        );
    }

    // ------------------------------------------------------------------------
    public static void DrawMessageSelection (
        SerializedProperty chatProperty,
        SerializedProperty branchArrayProperty
    ) {
        ChatScriptableObject chatObj =
            chatProperty.objectReferenceValue as ChatScriptableObject;
        Assert.IsNotNull(chatObj, "Can't find chat object on message editor.");

        // draw a foldout
        // iterate all elements of the array
        // draw a message selection dropdown
        EditorGUILayout.LabelField(branchArrayProperty.name);
        //SerializedProperty arrayCopy = branchArrayProperty.Copy();
        for (int i = 0; i < branchArrayProperty.arraySize; i++) {
            DrawMessageSelectionDropdown(
                branchArrayProperty,
                chatObj,
                i
            );
        }
    }

    // ------------------------------------------------------------------------
    // draws a dropdown of messages to select from
    // and sets the message node value
    // of the branch array at the index given
    public static void DrawMessageSelectionDropdown (
        SerializedProperty branchArrayProperty,
        ChatScriptableObject chatObj,
        int index
    ) {
        // find message node property
        // and corresponding message scriptable object
        SerializedProperty msgProperty =
            branchArrayProperty.GetArrayElementAtIndex(index);
        Assert.IsNotNull(
            msgProperty,
            "Can't find message property at index " + index
        );

        MessageScriptableObject messageObj = 
            chatObj.GetMessage(msgProperty.intValue);
        Assert.IsNotNull(
            messageObj,
            "Can't find message object at index " + index
        );

        // draw a popup with all messages in that chat's message list
        // that don't have a clue trigger
        // return the node of the message selected
        string[] messageNames = new string[chatObj.Messages.Length];
        for(int i = 0; i < chatObj.Messages.Length; i++) {
            messageNames[i] = chatObj.Messages[i].DebugName;
        }
        
        selection = EditorGUILayout.Popup(selection, messageNames);
        int node = chatObj.Messages[selection].Node;
        
        msgProperty.intValue = node;
    }
}

#endif