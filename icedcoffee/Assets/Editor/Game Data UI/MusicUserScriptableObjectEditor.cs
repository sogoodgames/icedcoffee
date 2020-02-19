#if UNITY_EDITOR 

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MusicUserScriptableObject))]
public class MusicUserScriptableObjectEditor : Editor { 
    // ------------------------------------------------------------------------
    // Variables 
    // ------------------------------------------------------------------------
    private SerializedProperty m_username;
    private SerializedProperty m_playlistName;
    private SerializedProperty m_numFollowers;
    private SerializedProperty m_friendID;
    private SerializedProperty m_clueNeeded;
    private SerializedProperty m_clueGiven;
    private SerializedProperty m_playlist; 

    private ValidationOutput validation;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void OnEnable () {
        m_username = serializedObject.FindProperty("Username");
        m_playlistName = serializedObject.FindProperty("PlaylistName");
        m_numFollowers = serializedObject.FindProperty("NumFollowers");
        m_friendID = serializedObject.FindProperty("FriendID");
        m_clueNeeded = serializedObject.FindProperty("ClueNeededSO");
        m_clueGiven = serializedObject.FindProperty("ClueGivenSO");
        m_playlist = serializedObject.FindProperty("Playlist");
    }

    // ------------------------------------------------------------------------
    public override void OnInspectorGUI () {
        serializedObject.Update();

        EditorGUILayout.LabelField(
            "Music Account: " + m_username.stringValue,
            EditorStyles.boldLabel
        );
        EditorGUILayout.PropertyField(m_username);
        EditorGUILayout.PropertyField(m_friendID);
        EditorGUILayout.PropertyField(m_numFollowers);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Clues", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_clueGiven);
        EditorGUILayout.PropertyField(m_clueNeeded);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Playlist", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_playlistName);
        EditorGUILayout.PropertyField(m_playlist, true);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
        if(GUILayout.Button("Validate") || validation == null) {
            MusicUserScriptableObject obj = target as MusicUserScriptableObject;
            validation = DataValidator.ValidateMusicUser(obj);
        }
        GameDataEditorUtils.DrawValidationOutput(validation);

        serializedObject.ApplyModifiedProperties();
    }
}

#endif