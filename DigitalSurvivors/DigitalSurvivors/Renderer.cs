using System;
using System.Text;
using System.Threading;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DigitalSurvivors;

public class Renderer
{
    public void Render(Scene scene)
    {
        int padding = 2;
        
        if (scene == null)
            return;
        
        Console.Clear();
        List<GameObject> curentGameObjects = new List<GameObject>(scene.gameObjects);
        curentGameObjects.Sort((obj1, obj2) => obj1.layer > obj2.layer ? 1 : 0); 

        var panel = new Panel("");
        panel.Header($"[red] Wave X [/]", Justify.Center);
        panel.Height = scene.Height;
        panel.Width = scene.Width * padding - padding / 2;
        panel.Border = BoxBorder.Double;
        panel.BorderColor(Color.Blue1);
        
        AnsiConsole.Render(panel);
        
        foreach (var gameObject in curentGameObjects)
        {
            if(gameObject.position.X > scene.Width || gameObject.position.Y > scene.Height || gameObject.position.X < 0 || gameObject.position.Y < 0) continue;
            
            Console.SetCursorPosition((int)gameObject.position.X * padding, (int)gameObject.position.Y);
            //AnsiConsole.Markup($"{gameObject.sprite}");
            AnsiConsole.Markup(DrawSprite(gameObject));
        }
        
        Console.SetCursorPosition(0, 0); // Prevent input lag
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