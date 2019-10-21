using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GramUserScriptableObject))]
public class GramUserScriptableObjectEditor : GameDataEditor {
    // ------------------------------------------------------------------------
    // Variables 
    // ------------------------------------------------------------------------
    private SerializedProperty m_username;
    private SerializedProperty m_friend;
    private SerializedProperty m_icon;

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
        DrawIconField(m_icon);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
        if(GUILayout.Button("Validate") || validation == null) {
            GramUserScriptableObject obj = target as GramUserScriptableObject;
            validation = DataValidator.ValidateGramUser(obj);
        }
        DrawValidationOutput(validation);

        serializedObject.ApplyModifiedProperties();
    }
}