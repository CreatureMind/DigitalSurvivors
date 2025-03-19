namespace DigitalSurvivors;

// Manages all of the write and read logic of the high score
public static class HighScoreManager
{
    // Adds a new entry to the high score table by writing it to a file
    public static void AddToHighScoreTable(Dictionary<string, int> newHighScore)
    {
        if (newHighScore == null || newHighScore.Count == 0)
            return; // Exit if the provided dictionary is empty or null

        // Format it as "Name:Score" and write the first entry to the file
        FileHandler.WriteToFile($"{newHighScore.First().Key}:{newHighScore.First().Value}");
    }

    // Retrieves the high score table from the file
    public static List<KeyValuePair<string, int>> GetHighScoreTable()
    {
        // Check if the score file exists
        if (!FileUtilities.DoesFileExist("Score.txt"))
        {
            Debug.LogWarning("Score.txt not found. Returning an empty high score table.");
            return new List<KeyValuePair<string, int>>(); // Return an empty list if the file is missing
        }

        string content = FileHandler.ReadFromFile(); // Read the content of the score file
        List<KeyValuePair<string, int>> highScoreTable = new List<KeyValuePair<string, int>>();
        string[] lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries); // Split content by lines

        // Process each line in the score file
        foreach (string line in lines)
        {
            string[] parts = line.Split(':'); // Split by colon to separate name and score

            if (parts.Length != 2)
            {
                Debug.LogError($"Skipping invalid line: {line}");
                continue; // Skip invalid lines
            }

            string name = parts[0].Trim(); // Get the name and trim spaces
            if (!int.TryParse(parts[1].Trim(), out int score)) // Try parsing the score
            {
                Debug.LogError($"Skipping invalid score in line: {line}");
                continue; // Skip invalid scores
            }

            highScoreTable.Add(new KeyValuePair<string, int>(name, score)); // Add valid entries to the high score table
        }

        return highScoreTable; // Returns the high score table
    }
}