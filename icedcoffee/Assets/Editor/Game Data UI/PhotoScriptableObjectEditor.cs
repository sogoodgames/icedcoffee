#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PhotoScriptableObject))]
public class PhotoScriptableObjectEditor : Editor {
    // ------------------------------------------------------------------------
    // Variables 
    // ------------------------------------------------------------------------
    private SerializedProperty m_photo;
    private SerializedProperty m_clue;
    private SerializedProperty m_image;
    private SerializedProperty m_width;
    private SerializedProperty m_height;
    private SerializedProperty m_description;

    private ValidationOutput validation;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void OnEnable () {
        m_photo = serializedObject.FindProperty("PhotoID");
        m_clue = serializedObject.FindProperty("ClueID");
        m_image = serializedObject.FindProperty("Image");
        m_width = serializedObject.FindProperty("Width");
        m_height = serializedObject.FindProperty("Height");
        m_description = serializedObject.FindProperty("Description");
    }

    // ------------------------------------------------------------------------
    public override void OnInspectorGUI () {
        serializedObject.Update();

        string photo = ((PhotoID)m_photo.enumValueIndex).ToString();

        EditorGUILayout.LabelField("Photo: " + photo, EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_photo);
        EditorGUILayout.PropertyField(m_clue);
        EditorGUILayout.PropertyField(m_description);
        EditorGUILayout.PropertyField(m_image);
        EditorGUILayout.PropertyField(m_width);
        EditorGUILayout.PropertyField(m_height);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
        if(GUILayout.Button("Validate") || validation == null) {
            PhotoScriptableObject obj = target as PhotoScriptableObject;
            validation = DataValidator.ValidatePhoto(obj);
        }
        GameDataEditorUtils.DrawValidationOutput(validation);

        serializedObject.ApplyModifiedProperties();
    }
}

#endif