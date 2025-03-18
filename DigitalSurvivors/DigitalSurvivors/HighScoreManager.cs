namespace DigitalSurvivors;

public static class HighScoreManager
{
    public static void AddToHighScoreTable(Dictionary<string,int> newHighScore)
    {
        if (newHighScore == null || newHighScore.Count == 0)
            return;
    
        // Format it as "Name:Score"
        // Write the first entry to the file
        FileHandler.WriteToFile($"{newHighScore.First().Key}:{newHighScore.First().Value}");
    }

    public static List<KeyValuePair<string, int>> GetHighScoreTable()
    {
        if (!FileUtilities.DoesFileExist("Score.txt"))
        {
            Debug.LogWarning("Score.txt not found. Returning an empty high score table.");
            return new List<KeyValuePair<string, int>>();  // Return an empty list if the file is missing
        }
        
        string content = FileHandler.ReadFromFile();
        List<KeyValuePair<string, int>> highScoreTable = new List<KeyValuePair<string, int>>();
        string[] lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        foreach (string line in lines)
        {
            string[] parts = line.Split(':');

            if (parts.Length != 2) 
            {
                Debug.LogError($"Skipping invalid line: {line}");
                continue;
            }

            string name = parts[0].Trim();
            if (!int.TryParse(parts[1].Trim(), out int score))
            {
                Debug.LogError($"Skipping invalid score in line: {line}");
                continue;
            }

            highScoreTable.Add(new KeyValuePair<string, int>(name, score));
        }
        
        return highScoreTable;
    }
}