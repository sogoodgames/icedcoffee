using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChatScriptableObject))]
public class ChatScriptableObjectEditor : Editor {
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    private SerializedProperty m_friend;
    private SerializedProperty m_icon;
    private SerializedProperty m_clueNeeded;
    private SerializedProperty m_messages;

    ValidationOutput validation;
    private bool showValidation;
    
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

        EditorGUILayout.PropertyField(m_friend);
        EditorGUILayout.PropertyField(m_icon);
        EditorGUILayout.PropertyField(m_clueNeeded);
        EditorGUILayout.PropertyField(m_messages, true);

        showValidation = EditorGUILayout.Foldout(showValidation, "Validate");
        if(showValidation) {
            if(GUILayout.Button("Validate") || validation == null) {
                ChatScriptableObject obj = target as ChatScriptableObject;
                validation = DataValidator.ValidateChat(obj);
            }
            GameDataEditorUtils.DrawValidationOutput(validation);
        }

        serializedObject.ApplyModifiedProperties();
    }
}