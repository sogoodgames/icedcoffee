#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;

public static class GameDataEditorUtils { 
    private static bool open = false;

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
    // chatProperty: the parent chat that contains these messages
    // optionArrayProperty: the list of potential dialogue options
    // branchArrayProperty: the branches that the options lead to
    public static void DrawPlayerMessageOptions (
        SerializedProperty chatProperty,
        SerializedProperty optionArrayProperty,
        SerializedProperty branchArrayProperty
    ) {
        ChatScriptableObject chatObj =
            chatProperty.objectReferenceValue as ChatScriptableObject;
        Assert.IsNotNull(chatObj, "Can't find chat object on message editor.");

        open = EditorGUILayout.Foldout(open, "Dialogue Options");
        if(!open) {
            return;
        }

        // how many options & corresponding branches
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Number of options: ");
        optionArrayProperty.arraySize = EditorGUILayout.IntField(
            optionArrayProperty.arraySize
        );
        branchArrayProperty.arraySize = optionArrayProperty.arraySize;
        EditorGUILayout.EndHorizontal();

        // iterate all elements of the potential dialogue options
        for (int i = 0; i < optionArrayProperty.arraySize; i++) {
            EditorGUILayout.BeginVertical(EditorStyles.textArea);
            // dialogue field
            SerializedProperty dialogueOptionProperty =
                optionArrayProperty.GetArrayElementAtIndex(i);
            Assert.IsNotNull(
                dialogueOptionProperty,
                "Can't find option property at index " + i
            );

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Option: ");
            dialogueOptionProperty.stringValue = EditorGUILayout.TextField(
                dialogueOptionProperty.stringValue
            );
            EditorGUILayout.EndHorizontal();

            // branch selection
            DrawMessageSelectionDropdown(
                branchArrayProperty,
                chatObj,
                i
            );
            EditorGUILayout.EndVertical();
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
        int selection = 0;
        // if we can't find the message object, that means we just created
        // this entry in the array and haven't set its value yet
        // so leave it to the default value of 0
        if(messageObj != null) {
            // otherwise, find the index of the current selection
            // in the chat messages
            selection = messageObj.GetIndexInChat();
        }

        // draw a popup with all messages in that chat's message list
        // that don't have a clue trigger
        string[] messageNames = new string[chatObj.Messages.Length];
        for(int i = 0; i < chatObj.Messages.Length; i++) {
            messageNames[i] = chatObj.Messages[i].DebugName;
        }
        
        // draw a popup to select a new message
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Branch: ");
        selection = EditorGUILayout.Popup(selection, messageNames);
        EditorGUILayout.EndHorizontal();
        
        // the actual message property is the message's node,
        // not its index in the list of chats
        int node = chatObj.Messages[selection].Node;
        msgProperty.intValue = node;
    }
}

#endif