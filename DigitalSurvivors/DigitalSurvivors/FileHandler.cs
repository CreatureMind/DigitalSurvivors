namespace DigitalSurvivors;

static class FileHandler
{
    private static string filePath = "Score.txt"; // The default file path.
    private static FileStream fs; // FileStream object for handling file operations.

    // Method to write content to the file.
    public static void WriteToFile(string content)
    {
        try
        {
            // Converts the string content to a byte array and appends it to the file.
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(content + "\n");
            fs = new FileStream(filePath, FileMode.Append, FileAccess.Write);
            fs.Write(buffer, 0, buffer.Length);
        }
        // Handles case where the directory does not exist.
        catch (DirectoryNotFoundException dirEx)
        {
            Console.WriteLine("Error: Directory not found: " + dirEx.Message);
        }
        // Handles case where the file does not exist.
        catch (FileNotFoundException fileEx)
        {
            Console.WriteLine("Error: File not found: " + fileEx.Message);
        }
        // Handles insufficient permission issues.
        catch (UnauthorizedAccessException unEx)
        {
            Console.WriteLine("Error: Access denied: " + unEx.Message);
        }
        // Handles input/output exceptions.
        catch (IOException ioEx)
        {
            Console.WriteLine("Error: In/Output: " + ioEx.Message);
        }
        // Catches any other unexpected exceptions.
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            // Ensures the file stream is closed properly to prevent resource leaks.
            if(fs != null)
                fs.Close();
        }
    }

    // Method to read content from the file.
    public static string ReadFromFile()
    {
        try
        {
            // Opens the file for reading.
            fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);

            // Converts byte array to string and returns it
            return System.Text.Encoding.UTF8.GetString(buffer);
        }
        // Handles case where the directory does not exist.
        catch (DirectoryNotFoundException dirEx)
        {
            Console.WriteLine("Error: Directory not found: " + dirEx.Message);
        }
        // Handles case where the file does not exist.
        catch (FileNotFoundException fileEx)
        {
            Console.WriteLine("Error: File not found: " + fileEx.Message);
        }
        // Handles insufficient permission issues.
        catch (UnauthorizedAccessException unEx)
        {
            Console.WriteLine("Error: Access denied: " + unEx.Message);
        }
        // Handles input/output exceptions.
        catch (IOException ioEx)
        {
            Console.WriteLine("Error: In/Output: " + ioEx.Message);
        }
        // Catches any other unexpected exceptions.
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            // Ensures the file stream is closed properly to prevent resource leaks.
            if(fs != null)
                fs.Close();
        }

        return null; // Returns null if an error occurs.
    }
}

// Utility class for additional file-related operations.
static class FileUtilities
{
    // Checks if a file exists at the specified path.
    public static bool DoesFileExist(string filePath)
    {
        return File.Exists(filePath);
    }

    // Retrieves the size of the file in bytes.
    public static int GetFileSize(string filePath)
    {
        if (File.Exists(filePath))
        {
            long fileSize = new FileInfo(filePath).Length;
            return (int)fileSize;
        }
        return -1; // Returns -1 if the file does not exist.
    }
}