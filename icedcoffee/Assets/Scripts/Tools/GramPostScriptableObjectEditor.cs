using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GramPostScriptableObject))]
public class GramPostScriptableObjectEditor : GameDataEditor {
    // ------------------------------------------------------------------------
    // Variables 
    // ------------------------------------------------------------------------
    private SerializedProperty m_id;
    private SerializedProperty m_clueGiven;
    private SerializedProperty m_clueNeeded;
    private SerializedProperty m_description;
    private SerializedProperty m_image;
    private SerializedProperty m_comments;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void OnEnable () {
        m_id = serializedObject.FindProperty("UserId");
        m_clueGiven = serializedObject.FindProperty("ClueGiven");
        m_clueNeeded = serializedObject.FindProperty("ClueNeeded");
        m_description = serializedObject.FindProperty("Description");
        m_image = serializedObject.FindProperty("PostImage");
        m_comments = serializedObject.FindProperty("Comments");
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
        EditorGUILayout.PropertyField(m_description);
        EditorGUILayout.PropertyField(m_image);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Clues", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_clueGiven);
        EditorGUILayout.PropertyField(m_clueNeeded);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Comments", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_comments, true);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
        if(GUILayout.Button("Validate") || validation == null) {
            GramPostScriptableObject obj = target as GramPostScriptableObject;
            validation = DataValidator.ValidateGramPost(obj);
        }
        DrawValidationOutput(validation);

        serializedObject.ApplyModifiedProperties();
    }
}