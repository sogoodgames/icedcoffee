#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

[CustomEditor(typeof(ChatScriptableObject))]
public class ChatScriptableObjectEditor : Editor {
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    private SerializedProperty m_id;
    private SerializedProperty m_clueNeeded;
    private SerializedProperty m_messages;

    private ValidationOutput validation;
    
    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void OnEnable () {
        m_id = serializedObject.FindProperty("m_id");
        m_clueNeeded = serializedObject.FindProperty("ClueNeeded");
        m_messages = serializedObject.FindProperty("Messages");
    }

    // ------------------------------------------------------------------------
    public override void OnInspectorGUI () {
        serializedObject.Update();

        ChatScriptableObject chatObj = target as ChatScriptableObject;
        Assert.IsNotNull(chatObj, "Can't find chat object on chat editor.");

        EditorGUILayout.LabelField(
            "Chat: " + chatObj.DisplayName,
            EditorStyles.boldLabel
        );
        GameDataEditorUtils.DrawIdGenerator(m_id);
        EditorGUILayout.PropertyField(m_clueNeeded);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Messages", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_messages, true);
        bool changed = EditorGUI.EndChangeCheck();
        if(changed) {
            SetMessageChat(chatObj);
        }

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
        if(GUILayout.Button("Validate") || validation == null) {
            validation = DataValidator.ValidateChat(chatObj);
        }
        GameDataEditorUtils.DrawValidationOutput(validation);

        serializedObject.ApplyModifiedProperties();
    }

    // ------------------------------------------------------------------------
    private void SetMessageChat (ChatScriptableObject chat) {
        for(int i = 0; i < m_messages.arraySize; i++) {
            SerializedProperty msgProperty = 
                m_messages.GetArrayElementAtIndex(i);
            Assert.IsNotNull(
                msgProperty,
                "Can't find message property at index " + i
            );

            MessageScriptableObject msgObject = 
                msgProperty.objectReferenceValue as MessageScriptableObject;
            
            // message object might be null if this slot is new
            if(msgObject != null) {
                msgObject.Chat = chat;
                //Debug.Log("set message " + msgObject.Node + " to chat " + msgObject.Chat.ID);
            }
        }
    }
}

#endif