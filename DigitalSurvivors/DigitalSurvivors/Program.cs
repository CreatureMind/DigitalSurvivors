using System;
using System.Data;
using System.Text;
using System.Threading;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DigitalSurvivors;

class Program
{
    public static Scene currentScene;
    public static bool isRunning = true;
    public static float deltaTime;
    
    private static DateTime previousFrameTime = DateTime.MinValue;

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
            
            if(previousFrameTime != DateTime.MinValue)
                deltaTime = (float)(DateTime.Now - previousFrameTime).TotalSeconds;
            
            previousFrameTime = DateTime.Now;
            
            List<GameObject> tempGameObjects = new List<GameObject>(currentScene.GameObjects);
            tempGameObjects.ForEach(gameObject => gameObject.Update());

            Thread.Sleep(100);
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
                
                if(key == ConsoleKey.Escape)
                    isRunning = false;
            }
        }
    }
}