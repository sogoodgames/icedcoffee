using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ClueScriptableObject))]
public class ClueScriptableObjectEditor : GameDataEditor {
    // ------------------------------------------------------------------------
    // Variables 
    // ------------------------------------------------------------------------
    private SerializedProperty m_clue;
    private SerializedProperty m_lockState;
    private SerializedProperty m_canSend;
    private SerializedProperty m_phoneNumber;
    private SerializedProperty m_note;
    private SerializedProperty m_message;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void OnEnable () {
        m_clue = serializedObject.FindProperty("ClueID");
        m_lockState = serializedObject.FindProperty("InitialLockState");
        m_canSend = serializedObject.FindProperty("CanSend");
        m_phoneNumber = serializedObject.FindProperty("PhoneNumberGiven");
        m_note = serializedObject.FindProperty("Note");
        m_message = serializedObject.FindProperty("Message");
    }

    // ------------------------------------------------------------------------
    public override void OnInspectorGUI () {
        serializedObject.Update();

        string clue = ((ClueID)m_clue.enumValueIndex).ToString();

        EditorGUILayout.LabelField("Clue: " + clue, EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_clue);
        EditorGUILayout.PropertyField(m_lockState);
        EditorGUILayout.PropertyField(m_phoneNumber);
        EditorGUILayout.PropertyField(m_note);
        EditorGUILayout.PropertyField(m_canSend);
        if(m_canSend.boolValue) {
            EditorGUILayout.PropertyField(m_message);
        }

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
        if(GUILayout.Button("Validate") || validation == null) {
            ClueScriptableObject obj = target as ClueScriptableObject;
            validation = DataValidator.ValidateClue(obj);
        }
        DrawValidationOutput(validation);

        serializedObject.ApplyModifiedProperties();
    }
}