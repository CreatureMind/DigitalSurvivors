using System;
using System.Text;
using System.Threading;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DigitalSurvivors;

class Program
{
    public static Scene currentScene;
    static bool isRunning = true;

    public static event Action<ConsoleKey> OnKeyPress;
    public static event Action<Scene> OnSceneLoaded;

    // List of enemies, each with position (x, y) and speed
    static List<Enemy> enemies = new List<Enemy>
    {
        new Enemy(5, 5, 0.1f),   // First enemy, moves at speed 0.1
        new Enemy(15, 3, 0.2f),  // Second enemy, moves at speed 0.2
        new Enemy(2, 10, 0.3f)   // Third enemy, moves at speed 0.3
    };

    public static void SetScene(Scene scene)
    {
        currentScene = scene;
        OnSceneLoaded?.Invoke(scene);
    }

    static void Main()
    {
        Renderer renderer = new Renderer();
        SetScene( new Scene(50, 50));
        Console.CursorVisible = false;
        Thread inputThread = new Thread(ReadInput);
        inputThread.Start();

        while (isRunning)
        {
            renderer.Render(currentScene);
            Thread.Sleep(200); // Controls game speed
        }
    }

    static void ReadInput()
    {
        while (isRunning)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                OnKeyPress?.Invoke(key);
            }
        }
    }
}