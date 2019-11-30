#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GramPostScriptableObject))]
public class GramPostScriptableObjectEditor : Editor {
    // ------------------------------------------------------------------------
    // Variables 
    // ------------------------------------------------------------------------
    private SerializedProperty m_id;
    private SerializedProperty m_friend;
    private SerializedProperty m_clueGiven;
    private SerializedProperty m_clueNeeded;
    private SerializedProperty m_description;
    private SerializedProperty m_image;
    private SerializedProperty m_startComments;
    private SerializedProperty m_allComments;
    private SerializedProperty m_likes;
    private SerializedProperty m_postType;
    private SerializedProperty m_days;
    private SerializedProperty m_hour;
    private SerializedProperty m_minute;

    private ValidationOutput validation;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void OnEnable () {
        m_id = serializedObject.FindProperty("m_id");
        m_friend = serializedObject.FindProperty("UserId");
        m_clueGiven = serializedObject.FindProperty("ClueGiven");
        m_clueNeeded = serializedObject.FindProperty("ClueNeeded");
        m_description = serializedObject.FindProperty("Description");
        m_image = serializedObject.FindProperty("PostImage");
        m_startComments = serializedObject.FindProperty("StartComments");
        m_allComments = serializedObject.FindProperty("AllComments");
        m_likes = serializedObject.FindProperty("StartLikes");
        m_postType = serializedObject.FindProperty("PostType");
        m_days = serializedObject.FindProperty("PostTimeDays");
        m_hour = serializedObject.FindProperty("PostTimeHour");
        m_minute = serializedObject.FindProperty("PostTimeMinute");
    }

    // ------------------------------------------------------------------------
    public override void OnInspectorGUI () {
        serializedObject.Update();

        string friend = ((Friend)m_friend.enumValueIndex).ToString();

        EditorGUILayout.LabelField(
            "Forum Post: " + friend,
            EditorStyles.boldLabel
        );
        GameDataEditorUtils.DrawIdGenerator(m_id);

        EditorGUILayout.PropertyField(m_postType);
        bool canEditFriend = true;
        if((GramPostType)m_postType.enumValueIndex == GramPostType.PlayerPost) {
            m_friend.enumValueIndex = (int)Friend.You;
            canEditFriend = false;
        }
        EditorGUI.BeginDisabledGroup(!canEditFriend);
        EditorGUILayout.PropertyField(m_friend);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(m_likes);
        GameDataEditorUtils.DrawDateTimeField(m_days, m_hour, m_minute);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Content", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_description);
        EditorGUILayout.PropertyField(m_image);
        
        GUILayout.Space(20);

        EditorGUILayout.LabelField("Clues", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_clueGiven);
        EditorGUILayout.PropertyField(m_clueNeeded);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Comments", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_allComments, true);
        EditorGUILayout.PropertyField(m_startComments, true);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
        if(GUILayout.Button("Validate") || validation == null) {
            GramPostScriptableObject obj = target as GramPostScriptableObject;
            validation = DataValidator.ValidateGramPost(obj);
        }
        GameDataEditorUtils.DrawValidationOutput(validation);

        serializedObject.ApplyModifiedProperties();
    }
}

#endif