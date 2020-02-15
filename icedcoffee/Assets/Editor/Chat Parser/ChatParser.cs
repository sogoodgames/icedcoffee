using System;
using System.Collections.Generic;
using System.IO;

using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

#if UNITY_EDITOR

// Given list of writer-made script documents,
// outputs chat scriptable objects
public static class ChatParser {
    // ------------------------------------------------------------------------
    // Constants
    // ------------------------------------------------------------------------
    // chat metadata labels
    private static readonly string c_NameLabel = "Name:";
    private static readonly string c_ClueLabel = "Clue:";

    // message data labels
    private static readonly string c_SenderLabel = "Sender:";
    private static readonly string c_IdLabel = "id:";
    private static readonly string c_MessagesLabel = "Messages:";
    private static readonly string c_BranchesLabel = "Branches:";
    private static readonly string c_IsPresentingClueLabel = "[Presenting Clue]";
    private static readonly string c_ClueGivenLabel = "Clue Given:";
    private static readonly string c_ClueTriggerLabel = "Clue Trigger:";
    private static readonly string c_ImageLabel = "Image:";
    
    // utils
    private static readonly char c_escapeChar = '\n';

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public static void LoadChats(
        string inputPath,
        string outputPath
    ) {
        string[] files = Directory.GetFiles(inputPath);

        foreach(string file in files) {
            if(!file.EndsWith(".txt")) continue;

            // load text file
            string text = File.ReadAllText(file);
            Assert.IsFalse(
                String.IsNullOrEmpty(text),
                file + " empty"
            );

            // create chat scriptable object
            ChatScriptableObject chat =
                ScriptableObject.CreateInstance<ChatScriptableObject>();

            // extract & init chat metadata
            text = ParseChatMetadata(text, chat);

            // parse remaining text as messages
            List<MessageScriptableObject> messages =
                new List<MessageScriptableObject>();

            // create folder for messages
            string dir = outputPath + "/" + chat.ID;
            System.IO.Directory.CreateDirectory(dir);
            
            foreach(MessageParseData data in ParseChat(text)) {
                // create message scriptable object
                MessageScriptableObject messageObj =
                    ScriptableObject.CreateInstance<MessageScriptableObject>();
            
                messageObj.InitFromData(data);
                messages.Add(messageObj);

                // save message as asset
                AssetDatabase.CreateAsset(
                    messageObj,
                    dir + "/message_" + chat.ID + "_" + messageObj.Node + ".asset"
                );
            }

            // init chat messages
            chat.InitFromData(messages.ToArray());

            // save chat as asset
            AssetDatabase.CreateAsset(
                chat, dir + "/chat_" + chat.ID + ".asset"
            );
        }

        // save all asset creations/modifications
        AssetDatabase.SaveAssets();
    }

    // ------------------------------------------------------------------------
    private static List<MessageParseData> ParseChat (string text) {        
        List<MessageParseData> data = new List<MessageParseData>();

        // remove line escapes/ tabs
        text = text.Trim('\r', c_escapeChar, '\t');

        // split chat into individual messages
        string[] seperators = new string[1] {"++"};
        string[] messageLines = text.Split(
            seperators,
            StringSplitOptions.RemoveEmptyEntries
        );

        // parse line into message data
        foreach(string line in messageLines) {
            data.Add(ParseMessageBlock(line));
        }

        return data;
    }

    // ------------------------------------------------------------------------
    // in: a block of text with a single message in it
    // out: initializes the messagescriptableobj
    private static MessageParseData ParseMessageBlock(string text) {
        // default data 
        int id = -1;
        Friend sender = Friend.NoFriend;
        string[] messages = new string[0];
        bool isLeaf = true;
        bool isClue = false;
        ClueID clueGiven = ClueID.NoClue;
        ClueID clueTrigger = ClueID.NoClue;
        int[] branches = new int[0];
        PhotoID image = PhotoID.NoPhoto;

        // find message data by searching for lines with certain labels
        char[] seperators = new char[1]{c_escapeChar};
        string[] lines = text.Split(
            seperators,
            StringSplitOptions.RemoveEmptyEntries
        );

        foreach(string line in lines) {
            if(line.StartsWith(c_IdLabel)) {
                string idString = line.Substring(c_IdLabel.Length).Trim();
                Int32.TryParse(idString, out id);
            } else if (line.StartsWith(c_SenderLabel)) {
                string enumString = line.Substring(c_SenderLabel.Length).Trim();
                Enum.TryParse(enumString, out sender);
            } else if (line.StartsWith(c_IsPresentingClueLabel)) {
                isClue = true;
            } else if (line.StartsWith(c_ClueGivenLabel)) {
                string enumString = line.Substring(c_ClueGivenLabel.Length).Trim();
                Enum.TryParse(enumString, out clueGiven);
            } else if (line.StartsWith(c_ClueTriggerLabel)) {
                string enumString = line.Substring(c_ClueTriggerLabel.Length).Trim();
                Enum.TryParse(enumString, out clueTrigger);
            } else if (line.StartsWith(c_ImageLabel)) {
                string enumString = line.Substring(c_ImageLabel.Length).Trim();
                Enum.TryParse(enumString, out image);
            } else if (line.StartsWith(c_BranchesLabel)) {
                branches = ParseBranches(line);
                isLeaf = false;
            } else if (line.StartsWith(c_MessagesLabel)) {
                messages = ParseMessages(text);
            }
        }

        // assert required data is filled correctly 
        Assert.IsFalse(id == -1);
        Assert.IsFalse(sender == Friend.NoFriend);
        Assert.IsFalse(messages.Length == 0);

        return new MessageParseData (
            id,
            sender,
            messages,
            isLeaf,
            isClue,
            clueGiven,
            clueTrigger,
            branches,
            image
        );
    }

    // ------------------------------------------------------------------------
    // input: the single line that contains the branches
    // output: an array of the branches
    private static int[] ParseBranches (string text) {
        text = text.Substring(c_BranchesLabel.Length);
        text = text.Replace("\r", "");

        string[] ids = text.Split(',');
        int[] branches = new int[ids.Length];
        
        for (int i = 0; i < branches.Length; i++) {
            Int32.TryParse(ids[i].Trim(), out branches[i]);
        }
        return branches;
    }

    // ------------------------------------------------------------------------
    // input: the entire text block for the message
    //        the messages must be the very ending of the message block
    // output: an array of messages, line-by-line
    private static string[] ParseMessages (string text) {
        text = text.Replace("\r", "");
        text = text.Substring(text.IndexOf(c_MessagesLabel) + c_MessagesLabel.Length + 1);

        char[] seperators = new char[1]{c_escapeChar};
        string[] messages = text.Split(
            seperators,
            StringSplitOptions.RemoveEmptyEntries
        );
        
        return messages;
    }

    // ------------------------------------------------------------------------
    // in: the entire text file
    // out: modifies ChatScriptableObject to be initialized with metadata
    //      returns text without metadata
    // this is a weird way to do this idc
    private static string ParseChatMetadata (
        string text,
        ChatScriptableObject chat
    ) {
        // find first close bracket
        int index = text.IndexOf('}');
        string metadata = text.Substring(0, index);

        // find name and clue
        int nameIndex = text.IndexOf(c_NameLabel);
        int clueIndex = text.IndexOf(c_ClueLabel);

        Assert.IsTrue(nameIndex > 0 && nameIndex < text.Length);
        Assert.IsTrue(clueIndex > 0 && clueIndex < text.Length);

        string name = text.Substring(
            nameIndex + c_NameLabel.Length,
            clueIndex - nameIndex - c_NameLabel.Length
        ).Trim();

        string clue = text.Substring(
            clueIndex + c_ClueLabel.Length,
            index - clueIndex - c_ClueLabel.Length
        ).Trim();

        // convert clue to clue enum
        ClueID clueID = ClueID.NoClue;
        Enum.TryParse(clue, out clueID);

        // initialize chat scriptable object
        chat.InitMetadata(
            (int)UnityEngine.Random.Range(0, 1000),
            name,
            clueID
        );

        // remove chat metadata
        return text.Substring(index+1);
    }
}

#endif