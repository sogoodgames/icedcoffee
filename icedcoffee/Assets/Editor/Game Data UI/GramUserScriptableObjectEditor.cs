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
    private SerializedProperty m_followers;
    private SerializedProperty m_following;
    private SerializedProperty m_description;

    private ValidationOutput validation;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void OnEnable () {
        m_username = serializedObject.FindProperty("Username");
        m_friend = serializedObject.FindProperty("UserId");
        m_icon = serializedObject.FindProperty("Icon");
        m_followers = serializedObject.FindProperty("NumFollowers");
        m_following = serializedObject.FindProperty("NumFollowing");
        m_description = serializedObject.FindProperty("Description");
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
        EditorGUILayout.PropertyField(m_followers);
        EditorGUILayout.PropertyField(m_following);
        EditorGUILayout.PropertyField(m_description);
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