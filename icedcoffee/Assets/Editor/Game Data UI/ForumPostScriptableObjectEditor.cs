#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ForumPostScriptableObject))]
public class ForumPostScriptableObjectEditor : Editor {
    // ------------------------------------------------------------------------
    // Variables 
    // ------------------------------------------------------------------------
    private SerializedProperty m_id;
    private SerializedProperty m_title;
    private SerializedProperty m_clueGiven;
    private SerializedProperty m_clueNeeded;
    private SerializedProperty m_body;
    private SerializedProperty m_numComments;
    private SerializedProperty m_time;
    private SerializedProperty m_photo;

    private ValidationOutput validation;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void OnEnable () {
        m_id = serializedObject.FindProperty("UserID");
        m_title = serializedObject.FindProperty("Title");
        m_clueGiven = serializedObject.FindProperty("ClueGiven");
        m_clueNeeded = serializedObject.FindProperty("ClueNeeded");
        m_body = serializedObject.FindProperty("Body");
        m_numComments = serializedObject.FindProperty("NumComments");
        m_time = serializedObject.FindProperty("Time");
        m_photo = serializedObject.FindProperty("Photo");
    }

    // ------------------------------------------------------------------------
    public override void OnInspectorGUI () {
        serializedObject.Update();

        string friend = ((Friend)m_id.enumValueIndex).ToString();

        EditorGUILayout.LabelField(
            "Forum Post: " + friend,
            EditorStyles.boldLabel
        );
        EditorGUILayout.PropertyField(m_id);
        EditorGUILayout.PropertyField(m_title);
        EditorGUILayout.PropertyField(m_numComments);
        EditorGUILayout.PropertyField(m_time);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Content", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_body);
        EditorGUILayout.PropertyField(m_photo);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Clues", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_clueGiven);
        EditorGUILayout.PropertyField(m_clueNeeded);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
        if(GUILayout.Button("Validate") || validation == null) {
            ForumPostScriptableObject obj = target as ForumPostScriptableObject;
            validation = DataValidator.ValidateForumPost(obj);
        }
        GameDataEditorUtils.DrawValidationOutput(validation);

        serializedObject.ApplyModifiedProperties();
    }
}

#endif