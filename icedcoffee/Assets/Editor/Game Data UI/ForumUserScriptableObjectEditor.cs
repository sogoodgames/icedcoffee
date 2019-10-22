#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ForumUserScriptableObject))]
public class ForumUserScriptableObjectEditor : Editor {
    // ------------------------------------------------------------------------
    // Variables 
    // ------------------------------------------------------------------------
    private SerializedProperty m_id;
    private SerializedProperty m_username;
    private SerializedProperty m_icon;

    private ValidationOutput validation;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void OnEnable () {
        m_id = serializedObject.FindProperty("UserID");
        m_username = serializedObject.FindProperty("Username");
        m_icon = serializedObject.FindProperty("Icon");
    }

    // ------------------------------------------------------------------------
    public override void OnInspectorGUI () {
        serializedObject.Update();

        EditorGUILayout.LabelField(
            "Forum Account: " + m_username.stringValue,
            EditorStyles.boldLabel
        );
        EditorGUILayout.PropertyField(m_id);
        EditorGUILayout.PropertyField(m_username);
        GameDataEditorUtils.DrawIconField(m_icon);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
        if(GUILayout.Button("Validate") || validation == null) {
            ForumUserScriptableObject obj = target as ForumUserScriptableObject;
            validation = DataValidator.ValidateForumUser(obj);
        }
        GameDataEditorUtils.DrawValidationOutput(validation);

        serializedObject.ApplyModifiedProperties();
    }
}

#endif