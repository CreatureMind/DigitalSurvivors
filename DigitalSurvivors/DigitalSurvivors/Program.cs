using System;
using System.Data;
using System.Text;
using System.Threading;
using Spectre.Console;

namespace DigitalSurvivors;

class Program
{
    // Event triggered when a key is pressed
    public static event Action<ConsoleKey> OnKeyPress;
    
    // Event triggered when a new scene is loaded
    public static event Action<Scene> OnSceneLoaded;

    public static Scene currentScene;                               // Stores the current active scene
    public static Dictionary<string, int> HighScoreTable = new();   // Dictionary to store player high score
    public static bool isRunning = true;                            // Flag to control the main game loop
    public static float deltaTime;                                  // Tracks the time difference between frames for update loops

    public static List<char> userInput = new();                     // Stores user input for name entry in the main menu
    public static string userName = string.Empty;                   // Stores the final username after confirmation

    private static DateTime previousFrameTime = DateTime.MinValue;  // Used to calculate time difference between frames

    // Sets the current scene and triggers the OnSceneLoaded event
    public static void SetScene(Scene scene)
    {
        currentScene = scene;
        OnSceneLoaded?.Invoke(scene);
    }

    static void Main(string[] args)
    {
        // Instansiate the renderer
        Renderer renderer = new Renderer();
        
        // Initialize the game with the main scene
        SetScene(new Scene(50, 50, "MainScene"));
        
        Console.CursorVisible = false;
        
        // Start a separate thread for handling player input
        Thread inputThread = new Thread(ReadInput);
        inputThread.Start();
        
        // Main game loop
        while (isRunning)
        {
            if (currentScene != null)
            {
                renderer.Render(currentScene); // Render the current scene

                // Calculate time elapsed since last frame
                if (previousFrameTime != DateTime.MinValue)
                    deltaTime = (float)(DateTime.Now - previousFrameTime).TotalSeconds;

                previousFrameTime = DateTime.Now;

                // If it's the "GameScene" loop through all game objects update functions
                if (currentScene.SceneName == "GameScene")
                {
                    if (currentScene.GetGameObjects() != null)
                    {
                        List<GameObject> tempGameObjects = new List<GameObject>(currentScene.GetGameObjects());
                        tempGameObjects.ForEach(gameObject => gameObject.Update());
                    }
                    else
                    {
                        Debug.LogError("There are no objects in the scene.");
                    }
                }

                // Small delay to control frame rate
                Thread.Sleep(100);
            }
        }

        // Cleanup when the game loop ends
        Console.Clear();
        Console.WriteLine("Thank you for playing Digital Survivors!");
    }

    // Reads player input in a separate thread
    static void ReadInput()
    {
        while (isRunning)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);

                OnKeyPress?.Invoke(key.Key);

                // Handle input in the main menu scene
                if (currentScene.SceneName == "MainScene")
                {
                    if (key.Key == ConsoleKey.Enter)
                    {
                        if (userInput.Count > 0)
                        {
                            userName = new string(userInput.ToArray());
                            userInput.Clear();
                        }
                        else
                        {
                            // Default name if no input is provided
                            userName = "Player";
                            userInput.Clear();
                        }

                        // Clear console logs and switch to the game scene
                        Renderer.ClearLog();
                        SetScene(new Scene(50, 50, "GameScene"));
                    }
                    else if (key.Key == ConsoleKey.Backspace && userInput.Count > 0)
                    {
                        // Remove the last character from username input
                        userInput.RemoveAt(userInput.Count - 1);
                    }
                    else if (userInput.Count > 10)
                    {
                        // Prevent username from exceeding 10 characters
                        Debug.LogWarning("User name can't be more than 10 characters");
                    }
                    else if (key.KeyChar is >= 'A' and <= 'Z' or >= 'a' and <= 'z' or >= '0' and <= '9')
                    {
                        // Allow only alphanumeric characters
                        userInput.Add(key.KeyChar);
                    }
                    else
                    {
                        Debug.LogWarning("User name must contain only English alphanumeric characters (a-Z & 0-9)");
                    }
                }

                // Allow player to exit the game with the Escape key
                if (key.Key == ConsoleKey.Escape)
                    isRunning = false;

                // Secret debug command: Press Shift + X to set player life to 0 to skip to high score
                if (key.Modifiers.HasFlag(ConsoleModifiers.Shift) && key.Key == ConsoleKey.X)
                {
                    Player.Instance.Life = 0;
                }
            }
        }
    }
}