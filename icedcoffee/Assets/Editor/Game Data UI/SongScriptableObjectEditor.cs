#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SongScriptableObject))]
public class SongScriptableObjectEditor : Editor {
    // ------------------------------------------------------------------------
    // Variables 
    // ------------------------------------------------------------------------
    private SerializedProperty m_title;
    private SerializedProperty m_artist;
    private SerializedProperty m_album;

    private ValidationOutput validation;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void OnEnable () {
        m_title = serializedObject.FindProperty("Title");
        m_artist = serializedObject.FindProperty("Artist");
        m_album = serializedObject.FindProperty("Album");
    }

    // ------------------------------------------------------------------------
    public override void OnInspectorGUI () {
        serializedObject.Update();

        EditorGUILayout.LabelField(
            "Song: " + m_title.stringValue,
            EditorStyles.boldLabel
        );
        EditorGUILayout.PropertyField(m_title);
        EditorGUILayout.PropertyField(m_artist);
        EditorGUILayout.PropertyField(m_album);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
        if(GUILayout.Button("Validate") || validation == null) {
            SongScriptableObject obj = target as SongScriptableObject;
            validation = DataValidator.ValidateSong(obj);
        }
        GameDataEditorUtils.DrawValidationOutput(validation);

        serializedObject.ApplyModifiedProperties();
    }
}

#endif