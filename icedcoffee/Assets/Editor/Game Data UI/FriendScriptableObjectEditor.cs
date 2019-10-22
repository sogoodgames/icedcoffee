#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FriendScriptableObject))]
public class FriendScriptableObjectEditor : Editor {
    // ------------------------------------------------------------------------
    // Variables 
    // ------------------------------------------------------------------------
    private SerializedProperty m_friend;
    private SerializedProperty m_name;
    private SerializedProperty m_clue;
    private SerializedProperty m_icon;

    private ValidationOutput validation;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void OnEnable () {
        m_friend = serializedObject.FindProperty("Friend");
        m_name = serializedObject.FindProperty("Name");
        m_clue = serializedObject.FindProperty("ContactClue");
        m_icon = serializedObject.FindProperty("Icon");
    }

    // ------------------------------------------------------------------------
    public override void OnInspectorGUI () {
        serializedObject.Update();

        string friend = ((Friend)m_friend.enumValueIndex).ToString();

        EditorGUILayout.LabelField("Friend: " + friend, EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_friend);
        EditorGUILayout.PropertyField(m_name);
        EditorGUILayout.PropertyField(m_clue);
        GameDataEditorUtils.DrawIconField(m_icon);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
        if(GUILayout.Button("Validate") || validation == null) {
            FriendScriptableObject obj = target as FriendScriptableObject;
            validation = DataValidator.ValidateFriend(obj);
        }
        GameDataEditorUtils.DrawValidationOutput(validation);

        serializedObject.ApplyModifiedProperties();
    }
}

#endif