using System;
using System.Data;
using System.Text;
using System.Threading;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DigitalSurvivors;

class Program
{
    public static event Action<ConsoleKey> OnKeyPress;
    public static event Action<Scene> OnSceneLoaded;

    public static Scene currentScene;
    public static Dictionary<string, int> HighScoreTable = new();
    public static bool isRunning = true;
    public static float deltaTime;

    public static List<char> userInput = new();
    public static string userName = string.Empty;

    private static DateTime previousFrameTime = DateTime.MinValue;

    public static void SetScene(Scene scene)
    {
        currentScene = scene;
        OnSceneLoaded?.Invoke(scene);
    }

    static void Main(string[] args)
    {
        Renderer renderer = new Renderer();
        SetScene(new Scene(50, 50, "MainScene"));
        Console.CursorVisible = false;
        Thread inputThread = new Thread(ReadInput);
        inputThread.Start();

        while (isRunning)
        {
            if (currentScene != null)
            {
                renderer.Render(currentScene);

                if (previousFrameTime != DateTime.MinValue)
                    deltaTime = (float)(DateTime.Now - previousFrameTime).TotalSeconds;

                previousFrameTime = DateTime.Now;

                if (currentScene.SceneName == "GameScene")
                {
                    List<GameObject> tempGameObjects = new List<GameObject>(currentScene.GetGameObjects());
                    tempGameObjects.ForEach(gameObject => gameObject.Update());
                }

                Thread.Sleep(100);
            }
        }

        Console.Clear();
        Console.WriteLine("Thank you for playing Digital Survivors!");
    }

    static void ReadInput()
    {
        while (isRunning)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);

                OnKeyPress?.Invoke(key.Key);

                if (currentScene.SceneName == "MainScene")
                {
                    if (key.Key == ConsoleKey.Enter)
                    {
                        if (userInput.Count > 0)
                        {
                            userName = new string(userInput.ToArray());
                            userInput.Clear();
                            //Debug.Log($"User name: {userName}");
                        }
                        else
                        {
                            userName = "Player";
                            userInput.Clear();
                        }

                        Renderer.ClearLog();
                        SetScene(new Scene(50, 50, "GameScene"));
                    }
                    else if (key.Key == ConsoleKey.Backspace && userInput.Count > 0)
                    {
                        userInput.RemoveAt(userInput.Count - 1);
                    }
                    else if (userInput.Count > 10)
                    {
                        Debug.LogWarning("User name can't be more than 10 characters");
                    }
                    else if (key.KeyChar is >= 'A' and <= 'Z' or >= 'a' and <= 'z' or >= '0' and <= '9')
                    {
                        userInput.Add(key.KeyChar);
                    }
                    else
                    {
                        Debug.LogWarning("User name must contain only alphanumeric characters (a-Z & 0-9)");
                    }
                }

                if (key.Key == ConsoleKey.Escape)
                    isRunning = false;

                if (key.Modifiers.HasFlag(ConsoleModifiers.Shift) && key.Key == ConsoleKey.X)
                {
                    Player.Instance.Life = 0;
                }
            }
        }
    }
}