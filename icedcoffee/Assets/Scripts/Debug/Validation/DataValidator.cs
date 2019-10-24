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
        ValidationOutput output = new ValidationOutput(chat.DisplayName);

        foreach(Friend friend in chat.Friends) {
            if(friend == Friend.NoFriend) {
                output.AddError("Invalid friend listed (can't be NoFriend).");
            }
        }

        if(chat.Icon == null) {
            output.AddError("Chat friend icon is missing.");
        }

        if(chat.Messages == null || chat.Messages.Length == 0) {
            output.AddError("Chat messages empty.");
        }

        return output;
    }

    // ------------------------------------------------------------------------
    public static ValidationOutput ValidateMessage (
        MessageScriptableObject message
    ) {
        ValidationOutput output = new ValidationOutput(message.DebugName);

        // all messages should have:
        // an ID > 0
        if(message.Node < 1) {
            output.AddError("Message node should be >0.");
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
                if(message.Options == null || message.Options.Length < 1) {
                    output.AddError("Message is marked Player (and not clue message) and has no options. Should have 0 messages and >0 options.");
                }
                if(message.Options == null || message.Branch == null || message.Options.Length != message.Branch.Length) {
                    output.AddError("Message is marked Player (and not clue message) and the number of options does not equal the number of branches. There should be 1 branch for each option.");
                }
                if(message.Messages != null && message.Messages.Length > 0) {
                    output.AddError("Message is marked Player (and not clue message) and has messages. Should have 0 messages and >0 options.");
                }
            } else {
                if(message.Messages == null || message.Messages.Length < 1) {
                output.AddError("Message is marked Player (clue message) and has no messages. Should have >0 messages and 0 options.");
                }
                if(message.Options != null && message.Options.Length > 0) {
                    output.AddError("Message is marked Player (clue message) and has options. Should have >0 messages and 0 options.");
                }
                if(message.Branch != null && message.Branch.Length > 0) {
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
            // if not a leaf, 1 branch
            // not be a presented clue (only player presents clues)
            if(message.Messages == null || message.Messages.Length < 1) {
                output.AddError("Message is marked NPC and has no messages. Should have >0 messages and 0 options.");
            }
            if(message.Options != null && message.Options.Length > 0) {
                output.AddError("Message is marked NPC and has options. Should have >0 messages and 0 options.");
            }
            if(!message.IsLeafMessage && (message.Branch == null || message.Branch.Length != 1)) {
                output.AddError("Message is marked NPC (not leaf node) and doesn't have a single branch. Should have exactly 1.");
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

    // ------------------------------------------------------------------------
    public static ValidationOutput ValidatePhoto (PhotoScriptableObject photo) {
        ValidationOutput output = new ValidationOutput(photo.PhotoID.ToString());

        if(photo.Image == null) {
            output.AddError("No image given. Please add image.");
        }

        if(photo.Width < 0.01f) {
            output.AddError("Width is too small. Please input valid width.");
        }
        if(photo.Height < 0.01f) {
            output.AddError("Height is too small. Please input valid width.");
        }

        if(string.IsNullOrEmpty(photo.Description)) {
            output.AddError("Photo description is empty. Please add a description.");
        }

        return output;
    }

    // ------------------------------------------------------------------------
    public static ValidationOutput ValidateMusicUser (
        MusicUserScriptableObject user
    ) {
        ValidationOutput output = new ValidationOutput(user.Username);

        if(string.IsNullOrEmpty(user.Username)) {
            output.AddError("Username empty.");
        }

        if(user.FriendID == Friend.NoFriend) {
            output.AddError("Friend needs to be value other than NoFriend.");
        }

        if(string.IsNullOrEmpty(user.PlaylistName)) {
            output.AddError("Playlist name empty.");
        }

        if(user.Playlist == null || user.Playlist.Length < 1) {
            output.AddError("Playlist empty.");
        }

        return output;
    }

    // ------------------------------------------------------------------------
    public static ValidationOutput ValidateSong (SongScriptableObject song) {
        ValidationOutput output = new ValidationOutput(song.Title);

        if(string.IsNullOrEmpty(song.Title)) {
            output.AddError("Song title empty.");
        }

        if(string.IsNullOrEmpty(song.Artist)) {
            output.AddError("Song artist empty.");
        }

        if(string.IsNullOrEmpty(song.Album)) {
            output.AddError("Song album empty.");
        }

        return output;
    }

    // ------------------------------------------------------------------------
    public static ValidationOutput ValidateGramUser (GramUserScriptableObject user) {
        ValidationOutput output = new ValidationOutput(user.Username);

        if(string.IsNullOrEmpty(user.Username)) {
            output.AddError("Username empty.");
        }

        if(user.UserId == Friend.NoFriend) {
            output.AddError("Friend needs to be value other than NoFriend.");
        }

        if(user.Icon == null) {
            output.AddError("Icon empty.");
        }

        return output;
    }

    // ------------------------------------------------------------------------
    public static ValidationOutput ValidateGramPost (GramPostScriptableObject post) {
        ValidationOutput output = new ValidationOutput(post.DebugName);

        if(post.UserId == Friend.NoFriend) {
            output.AddError("Friend needs to be value other than NoFriend.");
        }

        if(string.IsNullOrEmpty(post.Description)) {
            output.AddError("Post has no description.");
        }

        if(post.PostImage == PhotoID.NoPhoto) {
            output.AddError("Post has no image.");
        }

        return output;
    }

    // ------------------------------------------------------------------------
    public static ValidationOutput ValidateGramComment (
        GramCommentScriptableObject comment
    ) {
        ValidationOutput output = new ValidationOutput(comment.DebugName);

        if(comment.UserId == Friend.NoFriend) {
            output.AddError("Friend needs to be value other than NoFriend.");
        }

        if(string.IsNullOrEmpty(comment.Comment)) {
            output.AddError("Comment is empty.");
        }

        return output;
    }

    // ------------------------------------------------------------------------
    public static ValidationOutput ValidateForumUser (ForumUserScriptableObject user) {
        ValidationOutput output = new ValidationOutput(user.UserID.ToString());

        if(string.IsNullOrEmpty(user.Username)) {
            output.AddError("Username empty.");
        }

        if(user.UserID == Friend.NoFriend) {
            output.AddError("Friend needs to be value other than NoFriend.");
        }

        if(user.Icon == null) {
            output.AddError("Icon empty.");
        }

        return output;
    }

    // ------------------------------------------------------------------------
    public static ValidationOutput ValidateForumPost (ForumPostScriptableObject post) {
        ValidationOutput output = new ValidationOutput(post.DebugName);

        if(post.UserID == Friend.NoFriend) {
            output.AddError("Friend needs to be value other than NoFriend.");
        }

        if(string.IsNullOrEmpty(post.Title)) {
            output.AddError("Title empty.");
        }

        if(string.IsNullOrEmpty(post.Body) && post.Photo == PhotoID.NoPhoto) {
            output.AddError("Both post body and image are empty- fill in at least one.");
        }

        if(post.Time < 0) {
            output.AddError("Time must be positive.");
        }

        if(post.NumComments < 0) {
            output.AddError("Number of comments must be positive.");
        }

        return output;
    }
}