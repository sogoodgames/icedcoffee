#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MessageScriptableObject))]
public class MessageScriptableObjectEditor : Editor {
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    private SerializedProperty m_node;
    private SerializedProperty m_player;
    private SerializedProperty m_isClue;
    private SerializedProperty m_clueGiven;
    private SerializedProperty m_clueTrigger;
    private SerializedProperty m_messages;
    private SerializedProperty m_image;
    private SerializedProperty m_options;
    private SerializedProperty m_branch;

    private ValidationOutput validation;
    
    // ------------------------------------------------------------------------
    // Methods 
    // ------------------------------------------------------------------------
    private void OnEnable () {
        m_node = serializedObject.FindProperty("Node");
        m_player = serializedObject.FindProperty("Player");
        m_isClue = serializedObject.FindProperty("IsClueMessage");
        m_clueGiven = serializedObject.FindProperty("ClueGiven");
        m_clueTrigger = serializedObject.FindProperty("ClueTrigger");
        m_messages = serializedObject.FindProperty("Messages");
        m_image = serializedObject.FindProperty("Image");
        m_options = serializedObject.FindProperty("Options");
        m_branch = serializedObject.FindProperty("Branch");
    }

    // ------------------------------------------------------------------------
    public override void OnInspectorGUI () {
        serializedObject.Update();

        EditorGUILayout.LabelField("Message", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_node);
        EditorGUILayout.PropertyField(m_player);
        EditorGUILayout.PropertyField(m_image);
        EditorGUILayout.PropertyField(m_clueGiven);

        GUILayout.Space(20);

        if(m_player.boolValue) {
            EditorGUILayout.LabelField("Player Message Properties", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_isClue);
            if(!m_isClue.boolValue) {
                EditorGUILayout.PropertyField(m_options, true);
                EditorGUILayout.PropertyField(m_branch, true);
            } else {
                EditorGUILayout.PropertyField(m_messages, true);
            }
        } else {
            EditorGUILayout.LabelField("NPC Message Properties", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_clueTrigger);
            EditorGUILayout.PropertyField(m_messages, true);
            EditorGUILayout.PropertyField(m_branch, true);
        }

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
        if(GUILayout.Button("Validate") || validation == null) {
            MessageScriptableObject obj = target as MessageScriptableObject;
            validation = DataValidator.ValidateMessage(obj);
        }
        GameDataEditorUtils.DrawValidationOutput(validation);

        serializedObject.ApplyModifiedProperties();
    }
}

#endif