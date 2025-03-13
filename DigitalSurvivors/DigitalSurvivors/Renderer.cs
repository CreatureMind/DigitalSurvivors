using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DigitalSurvivors;

public class Renderer
{
    public Renderer()
    {
        Awake();
    }
    
    public void Render(Scene scene)
    {
        int padding = 2;
        
        if (scene == null)
            return;
        
        Console.Clear();
        List<GameObject> currentGameObjects = scene.GameObjects.Where(obj => obj != null).ToList();
        currentGameObjects.Sort(CompareGameObjects); 
        
        var gamePanel = new Panel("");
        gamePanel.Header($"[red] Wave X [/]", Justify.Center);
        gamePanel.Height = scene.Height;
        gamePanel.Width = scene.Width * padding - padding / 2;
        gamePanel.Border = BoxBorder.Double;
        gamePanel.BorderColor(Color.Blue1);
        
        var uiPanel = new Panel(new Rows(
            new Text(""),
            new Text(""),
            new Text(""),
            new Text("")
        ));
        uiPanel.Header($"[yellow] Stats [/]", Justify.Center);
        uiPanel.Height = 6;
        uiPanel.Width = scene.Width * padding - padding / 2;
        uiPanel.Border = BoxBorder.Double;
        uiPanel.BorderColor(Color.Blue1);
        
        AnsiConsole.Write(gamePanel);
        AnsiConsole.Write(uiPanel);
        
        foreach (var gameObject in currentGameObjects)
        {
            if (gameObject != null)
            {
                if(gameObject.position.X > scene.Width 
                   || gameObject.position.Y > scene.Height 
                   || gameObject.position.X < 0 
                   || gameObject.position.Y < 0) 
                    continue;
            
                Console.SetCursorPosition((int)gameObject.position.X * padding, (int)gameObject.position.Y);
                AnsiConsole.Markup(DrawSprite(gameObject));
            }
        }
        
        Console.SetCursorPosition(0, 0); // Prevent input lag
    }

    public void Awake()
    {
        Debug.OnDebug += Render;
    }

    private void Render(String message)
    {
        AnsiConsole.Write("Log:");
        Console.SetCursorPosition(0, 60);
        AnsiConsole.WriteLine(message);
    }

    private int CompareGameObjects(GameObject obj1, GameObject obj2)
    {
        if (obj1 == null && obj2 == null) return 0;
        if (obj1 == null) return 1; // Null objects should go last
        if (obj2 == null) return -1;
        
        // Your comparison logic here (e.g., comparing positions or layers)
        return obj1.layer.CompareTo(obj2.layer); // Example: comparing layers
    }

    private string DrawSprite(GameObject gameObject)
    {
        if (gameObject == null) return "";

        switch (gameObject.sprite)
        {
            case '@':
                return ":bright_button:";
            case 'N':
                return ":alien:";
            case 'M':
                return ":zombie:";
            case 'H':
                return ":robot:";
            case 'E':
                return ":alien_monster:";
            default:
                return gameObject.sprite.ToString();
        }
    }
}