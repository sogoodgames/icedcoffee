using System;

public static class DialogueProcesser
{
    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public static string PreprocessDialogue (
        string text,
        string nameReplace,
        string subjReplace,
        string objReplace,
        string posReplace
    ) {
        string output = text.Replace("[name]", nameReplace);

        output = output.Replace("[they]", subjReplace);
        output = output.Replace("[them]", objReplace);
        output = output.Replace("[their]", posReplace);
        
        string subjUppercase = UppercaseFirstLetter(subjReplace);
        string objUppercase = UppercaseFirstLetter(objReplace);
        string posUppercase = UppercaseFirstLetter(posReplace);
        output = output.Replace("[they-c]", subjUppercase);
        output = output.Replace("[them-c]", objUppercase);
        output = output.Replace("[their-c]", posUppercase);
        
        return output;
    }

    // ------------------------------------------------------------------------
    public static string FormatTime (DateTime time) {
        return time.ToString("HH:mm tt");
    }

    // ------------------------------------------------------------------------
    public static string FormatDateTime (DateTime time) {
        string text = FormatTime(time); 
        // only add the date if this message was sent more than a day ago
        if(time.Day != DateTime.Now.Day) {
            text = time.ToString("d MMM ") + text;
        }
        return text;
    }

    // ------------------------------------------------------------------------
    private static string UppercaseFirstLetter (string t) {
        return char.ToUpper(t[0]) + t.Substring(1);
    }
}
