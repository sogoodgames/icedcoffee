using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GramCommentScriptableObject))]
public class GramCommentScriptableObjectEditor : GameDataEditor {
    // ------------------------------------------------------------------------
    // Variables 
    // ------------------------------------------------------------------------
    private SerializedProperty m_id;
    private SerializedProperty m_comment;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void OnEnable () {
        m_id = serializedObject.FindProperty("UserId");
        m_comment = serializedObject.FindProperty("Comment");
    }

    // ------------------------------------------------------------------------
    public override void OnInspectorGUI () {
        serializedObject.Update();

        string friend = ((Friend)m_id.enumValueIndex).ToString();

        EditorGUILayout.LabelField(
            "Gram Comment: " + friend,
            EditorStyles.boldLabel
        );
        EditorGUILayout.PropertyField(m_id);
        EditorGUILayout.PropertyField(m_comment);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
        if(GUILayout.Button("Validate") || validation == null) {
            GramCommentScriptableObject obj = target as GramCommentScriptableObject;
            validation = DataValidator.ValidateGramComment(obj);
        }
        DrawValidationOutput(validation);

        serializedObject.ApplyModifiedProperties();
    }
}