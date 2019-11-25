#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GramCommentScriptableObject))]
public class GramCommentScriptableObjectEditor : Editor {
    // ------------------------------------------------------------------------
    // Variables 
    // ------------------------------------------------------------------------
    private SerializedProperty m_id;
    private SerializedProperty m_friend;
    private SerializedProperty m_comment;

    private ValidationOutput validation;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void OnEnable () {
        m_id = serializedObject.FindProperty("m_id");
        m_friend = serializedObject.FindProperty("UserId");
        m_comment = serializedObject.FindProperty("Comment");
    }

    // ------------------------------------------------------------------------
    public override void OnInspectorGUI () {
        serializedObject.Update();

        string friend = ((Friend)m_friend.enumValueIndex).ToString();

        EditorGUILayout.LabelField(
            "Gram Comment: " + friend,
            EditorStyles.boldLabel
        );
        GameDataEditorUtils.DrawIdGenerator(m_id);
        EditorGUILayout.PropertyField(m_friend);
        EditorGUILayout.PropertyField(m_comment);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
        if(GUILayout.Button("Validate") || validation == null) {
            GramCommentScriptableObject obj = target as GramCommentScriptableObject;
            validation = DataValidator.ValidateGramComment(obj);
        }
        GameDataEditorUtils.DrawValidationOutput(validation);

        serializedObject.ApplyModifiedProperties();
    }
}

#endif