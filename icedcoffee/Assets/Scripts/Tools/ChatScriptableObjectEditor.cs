using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChatScriptableObject))]
public class ChatScriptableObjectEditor : GameDataEditor {
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    private SerializedProperty m_friend;
    private SerializedProperty m_icon;
    private SerializedProperty m_clueNeeded;
    private SerializedProperty m_messages;
    
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
        EditorGUILayout.PropertyField(m_messages, true);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
        if(GUILayout.Button("Validate") || validation == null) {
            ChatScriptableObject obj = target as ChatScriptableObject;
            validation = DataValidator.ValidateChat(obj);
        }
        GameDataEditorUtils.DrawValidationOutput(validation);

        serializedObject.ApplyModifiedProperties();
    }
}