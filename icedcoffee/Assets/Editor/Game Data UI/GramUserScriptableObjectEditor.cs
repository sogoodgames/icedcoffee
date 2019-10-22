#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GramUserScriptableObject))]
public class GramUserScriptableObjectEditor : Editor {
    // ------------------------------------------------------------------------
    // Variables 
    // ------------------------------------------------------------------------
    private SerializedProperty m_username;
    private SerializedProperty m_friend;
    private SerializedProperty m_icon;

    private ValidationOutput validation;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void OnEnable () {
        m_username = serializedObject.FindProperty("Username");
        m_friend = serializedObject.FindProperty("UserId");
        m_icon = serializedObject.FindProperty("Icon");
    }

    // ------------------------------------------------------------------------
    public override void OnInspectorGUI () {
        serializedObject.Update();

        EditorGUILayout.LabelField(
            "Gram Account: " + m_username.stringValue,
            EditorStyles.boldLabel
        );
        EditorGUILayout.PropertyField(m_username);
        EditorGUILayout.PropertyField(m_friend);
        GameDataEditorUtils.DrawIconField(m_icon);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
        if(GUILayout.Button("Validate") || validation == null) {
            GramUserScriptableObject obj = target as GramUserScriptableObject;
            validation = DataValidator.ValidateGramUser(obj);
        }
        GameDataEditorUtils.DrawValidationOutput(validation);

        serializedObject.ApplyModifiedProperties();
    }
}

#endif