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
    private static string UppercaseFirstLetter (string t) {
        return char.ToUpper(t[0]) + t.Substring(1);
    }
}
