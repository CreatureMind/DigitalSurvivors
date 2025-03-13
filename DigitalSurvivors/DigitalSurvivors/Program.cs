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
        
        Console.Clear();
        Console.WriteLine("Game Over!");
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