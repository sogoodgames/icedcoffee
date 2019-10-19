using UnityEngine;
using UnityEditor;

public class GameDataEditor : Editor {
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    protected ValidationOutput validation;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public static void DrawValidationOutput (ValidationOutput validation) {
        EditorGUILayout.LabelField("Validation status:");
        
        if(validation.Successful) {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.green;
            EditorGUILayout.LabelField("PASS", style);
        } else {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;
            EditorGUILayout.LabelField("FAIL", style);
        }

        GUIStyle msgStyle = new GUIStyle();
        msgStyle.stretchWidth = true;
        msgStyle.stretchHeight = true;
        msgStyle.wordWrap = true;
        EditorGUILayout.LabelField("Message:");
        EditorGUILayout.LabelField(validation.Message.ToString(), msgStyle);
    }

    // ------------------------------------------------------------------------
    public static void DrawIconField (SerializedProperty iconProperty) {
        iconProperty.objectReferenceValue = EditorGUILayout.ObjectField(
            "Chat icon: ",
            iconProperty.objectReferenceValue,
            typeof(Sprite),
            false
        );
    }
}