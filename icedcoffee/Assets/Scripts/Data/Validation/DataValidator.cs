using System.Text;
using UnityEngine;

public class ValidationOutput {
    public bool Successful;
    public StringBuilder Message;

    public ValidationOutput (string objectName) {
        Successful = true;
        Message = new StringBuilder("Validating: ");
        Message.Append(objectName);
        Message.Append("\n");
    }

    public void AddError(string message) {
        Successful = false;
        Message.Append(message);
        Message.Append("\n");
    }
}

public static class DataValidator {
    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public static ValidationOutput ValidateChat (ChatScriptableObject chat) {
        ValidationOutput output = new ValidationOutput(chat.Friend.ToString());

        if(chat.Friend == Friend.NoFriend) {
            output.AddError("Must select valid friend (not NoFriend).");
        }

        if(chat.Icon == null) {
            output.AddError("Chat friend icon is missing.");
        }

        if(chat.Messages.Length == 0) {
            output.AddError("Chat messages empty.");
        }

        return output;
    }

    // ------------------------------------------------------------------------
    public static ValidationOutput ValidateMessage (
        MessageScriptableObject message
    ) {
        ValidationOutput output = new ValidationOutput(message.Node.ToString());

        // all messages should have:
        // a non-negative ID
        if(message.Node < 0) {
            output.AddError("Message has a negative ID. ID should be >= 0.");
        }

        if(message.Player) {
            // player messages should have:
            // if not a clue message
            //   no messages
            //   options
            //   the same number of branches as options
            // if a clue message
            //   messages
            //   no options or branches
            // no clue trigger (only npc messages are triggered by clues)
            if(!message.IsClueMessage) {
                if(message.Options.Length < 1) {
                    output.AddError("Message is marked Player (and not clue message) and has no options. Should have 0 messages and >0 options.");
                }
                if(message.Options.Length != message.Branch.Length) {
                    output.AddError("Message is marked Player (and not clue message) and the number of options does not equal the number of branches. There should be 1 branch for each option.");
                }
                if(message.Messages.Length > 0) {
                    output.AddError("Message is marked Player (and not clue message) and has messages. Should have 0 messages and >0 options.");
                }
            } else {
                if(message.Messages.Length < 1) {
                output.AddError("Message is marked Player (clue message) and has no messages. Should have >0 messages and 0 options.");
                }
                if(message.Options.Length > 0) {
                    output.AddError("Message is marked Player (clue message) and has options. Should have >0 messages and 0 options.");
                }
                if(message.Branch.Length > 0) {
                    output.AddError("Message is marked Player (clue message) and has a branch. Clue messages don't have branches; did you mean to make this NOT a clue message, or an NPC message?.");
                }
            }

            if(message.ClueTrigger != ClueID.NoClue) {
                output.AddError("Message is marked Player and has a clue trigger. Player messages are never triggered by clues; did you mean to make this an NPC message?");
            }
        } else {
            // npc messages should have:
            // messages
            // no options
            // exactly 1 branch
            // not be a presented clue (only player presents clues)
            if(message.Messages.Length < 1) {
                output.AddError("Message is marked NPC and has no messages. Should have >0 messages and 0 options.");
            }
            if(message.Options.Length > 0) {
                output.AddError("Message is marked NPC and has options. Should have >0 messages and 0 options.");
            }
            if(message.Branch.Length != 1) {
                output.AddError("Message is marked NPC and has fewer/more than 1 branch. Should have only one branch.");
            }
            if(message.IsClueMessage) {
                output.AddError("Message is marked NPC and as a clue message. NPCs don't present clues; did you mean to mark this as Player?");
            }
        }

        return output;
    }

    // ------------------------------------------------------------------------
    public static ValidationOutput ValidateFriend (
        FriendScriptableObject friend
    ) {
        ValidationOutput output = new ValidationOutput(friend.Name);

        if(friend.Friend == Friend.NoFriend) {
            output.AddError("Friend type is NoFriend. Please select a friend.");
        } else if(friend.Friend == Friend.You) {
            output.AddError("Friend type is You. Please select a friend.");
        }

        if(string.IsNullOrEmpty(friend.Name)) {
            output.AddError("Friend name is empty. Please give ur friend a name.");
        }

        if(friend.Icon == null) {
            output.AddError("Friend has no icon. Please set icon.");
        }

        return output;
    }

    // ------------------------------------------------------------------------
    public static ValidationOutput ValidateClue (ClueScriptableObject clue) {
        ValidationOutput output = new ValidationOutput(clue.ClueID.ToString());

        if(string.IsNullOrEmpty(clue.Note)) {
            output.AddError("Clue note is empty. Please add a note.");
        }

        if(clue.CanSend && clue.Message == null) {
            output.AddError("Clue is marked CanSend and doesn't have a message. Please add a message.");
        }

        return output;
    }
}