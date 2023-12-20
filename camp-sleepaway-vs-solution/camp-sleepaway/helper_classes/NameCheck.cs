using System.Text.RegularExpressions;

internal static class NameCheck
{
    public static bool IsLettersOnly(string input)
    {
        // Check if a string contains only letters
        // returns true if the input string contains only english and swedish letters, false otherwise          
        return !string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, "^[a-zA-ZåäöÅÄÖ]+$");
    }
}
