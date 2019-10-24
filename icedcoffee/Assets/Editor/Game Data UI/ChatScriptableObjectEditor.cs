#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

[CustomEditor(typeof(ChatScriptableObject))]
public class ChatScriptableObjectEditor : Editor {
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    private SerializedProperty m_friend;
    private SerializedProperty m_icon;
    private SerializedProperty m_clueNeeded;
    private SerializedProperty m_messages;

    private ValidationOutput validation;
    
    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void OnEnable () {
        m_friend = serializedObject.FindProperty("Friend");
        m_icon = serializedObject.FindProperty("Icon");
        m_clueNeeded = serializedObject.FindProperty("ClueNeeded");
        m_messages = serializedObject.FindProperty("Messages");
    }

    // ------------------------------------------------------------------------
    public override void OnInspectorGUI () {
        serializedObject.Update();

        string friend = ((Friend)m_friend.enumValueIndex).ToString();

        EditorGUILayout.LabelField("Chat: " + friend, EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_friend);
        EditorGUILayout.PropertyField(m_clueNeeded);
        GameDataEditorUtils.DrawIconField(m_icon);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Messages", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_messages, true);
        bool changed = EditorGUI.EndChangeCheck();
        if(changed) {
            SetMessageChat(target);
        }

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
        if(GUILayout.Button("Validate") || validation == null) {
            ChatScriptableObject obj = target as ChatScriptableObject;
            validation = DataValidator.ValidateChat(obj);
        }
        GameDataEditorUtils.DrawValidationOutput(validation);

        serializedObject.ApplyModifiedProperties();
    }

    // ------------------------------------------------------------------------
    private void SetMessageChat (Object target) {
        ChatScriptableObject chat = target as ChatScriptableObject;
        Assert.IsNotNull(chat, "Can't find chat object on chat editor.");

        for(int i = 0; i < m_messages.arraySize; i++) {
            SerializedProperty msgProperty = 
                m_messages.GetArrayElementAtIndex(i);
            Assert.IsNotNull(
                msgProperty,
                "Can't find message property at index " + i
            );

            MessageScriptableObject msgObject = 
                msgProperty.objectReferenceValue as MessageScriptableObject;
            Assert.IsNotNull(
                msgObject,
                "Can't find message object at index " + i
            );

            msgObject.Chat = chat;
            Debug.Log("set message " + msgObject.Node + " to chat " + msgObject.Chat.Friend);
        }
    }
}

#endif