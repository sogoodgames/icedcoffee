#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;

[CustomEditor(typeof(MessageScriptableObject))]
public class MessageScriptableObjectEditor : Editor {
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    private SerializedProperty m_node;
    private SerializedProperty m_sender;
    private SerializedProperty m_isClue;
    private SerializedProperty m_isLeaf;
    private SerializedProperty m_clueGiven;
    private SerializedProperty m_clueTrigger;
    private SerializedProperty m_messages;
    private SerializedProperty m_image;
    private SerializedProperty m_branch;
    private SerializedProperty m_chat;

    private ValidationOutput validation;
    private int selection = 0;
    
    // ------------------------------------------------------------------------
    // Methods 
    // ------------------------------------------------------------------------
    private void OnEnable () {
        m_node = serializedObject.FindProperty("m_node");
        m_sender = serializedObject.FindProperty("Sender");
        m_isClue = serializedObject.FindProperty("IsClueMessage");
        m_isLeaf = serializedObject.FindProperty("IsLeafMessage");
        m_clueGiven = serializedObject.FindProperty("ClueGiven");
        m_clueTrigger = serializedObject.FindProperty("ClueTrigger");
        m_messages = serializedObject.FindProperty("Messages");
        m_image = serializedObject.FindProperty("Image");
        m_branch = serializedObject.FindProperty("Branch");
        m_chat = serializedObject.FindProperty("Chat");
    }

    // ------------------------------------------------------------------------
    public override void OnInspectorGUI () {
        serializedObject.Update();

        MessageScriptableObject messageObj = target as MessageScriptableObject;
        Assert.IsNotNull(
            messageObj,
            "Can't find message object on message editor "
        );

        EditorGUILayout.LabelField("Message", EditorStyles.boldLabel);
        
        GameDataEditorUtils.DrawIdGenerator(m_node);
        
        EditorGUILayout.PropertyField(m_sender);
        EditorGUILayout.PropertyField(m_image);
        EditorGUILayout.PropertyField(m_clueGiven);

        GUILayout.Space(20);

        if(messageObj.Player) {
            EditorGUILayout.LabelField("Player Message Properties", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_isClue);

            if(!m_isClue.boolValue) {
                GameDataEditorUtils.DrawPlayerMessageOptions(
                    m_chat,
                    m_messages,
                    m_branch
                );
            } else {
                EditorGUILayout.PropertyField(m_messages, true);
            }
        } else {
            EditorGUILayout.LabelField("NPC Message Properties", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_clueTrigger);
            EditorGUILayout.PropertyField(m_isLeaf);
            EditorGUILayout.PropertyField(m_messages, true);
    
            if(!m_isLeaf.boolValue) {
                // draw branch as a single option (bc there should only be 1)
                m_branch.arraySize = 1;
                ChatScriptableObject chatObj =
                    m_chat.objectReferenceValue as ChatScriptableObject;

                if(chatObj != null) {
                    GameDataEditorUtils.DrawMessageSelectionDropdown (
                        m_branch,
                        chatObj,
                        0
                    );
                } else {
                    EditorGUILayout.LabelField(
                        "Need to attach message to a chat before you can set" +
                        " what message comes next."
                    );
                }
            } else {
                // if this is a leaf node, clear branches
                m_branch.ClearArray();
            }
        }

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Validation", EditorStyles.boldLabel);
        if(GUILayout.Button("Validate") || validation == null) {
            
            validation = DataValidator.ValidateMessage(messageObj);
        }
        GameDataEditorUtils.DrawValidationOutput(validation);

        serializedObject.ApplyModifiedProperties();
    }
}

#endif