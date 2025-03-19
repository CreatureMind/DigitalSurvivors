using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DigitalSurvivors;

// Responsible for rendering the game scenes and UI and logs
public class Renderer
{
    private static readonly Queue<String> _logMessages = new(); // Stores recent log messages for display
    private static int CurrentScore { get; set; } // Tracks the player's current score

    public Renderer()
    {
        Awake();
    }

    private void Awake()
    {
        Debug.OnDebug += AddLog; // Subscribes to debug log events
        Enemy.UpdateScore += UpdateScore; // Subscribes to score update events
        CurrentScore = 0; // Resets score at the start
    }

    // Renders the scene, filtering the scenes by their name and drawing UI and game objects
    public void Render(Scene scene)
    {
        int padding = 2; // Controls spacing in the console UI

        if (scene == null)
            return;

        Console.Clear();
        List<GameObject> currentGameObjects = scene.GetGameObjects().Where(obj => obj != null).ToList();
        currentGameObjects.Sort(CompareGameObjects); // Sort objects based on their layer

        if (scene.SceneName == "MainScene")
        {
            var rule = new Markup("--------[red]DIGITAL SURVIVORS[/]--------");

            // Creates the menu UI
            var table = new Table()
                .AddColumn("")
                .AddRow(rule.Centered())
                .AddEmptyRow()
                .AddRow(new Markup("[red]What is your name?[/]").Centered())
                .AddRow(new Markup(new string(Program.userInput.ToArray())).Centered())
                .AddEmptyRow()
                .AddRow(new Markup("[red]HOW TO PLAY:[/]").Centered())
                .AddEmptyRow()
                .AddRow(new Markup("[[WASD]] - to move").Centered())
                .AddRow(new Markup("[[Spacebar]] - to attack").Centered())
                .AddRow(new Markup("[[Esc]] - to stop the game entirely").Centered())
                .AddRow(new Markup("[[Shift + X]] - to kill the player").Centered())
                .Border(TableBorder.None);

            var menuPanel = new Panel(table.Alignment(Justify.Center));
            menuPanel.Header($"[red] Digital Survivors [/]", Justify.Center);
            menuPanel.Height = scene.Height;
            menuPanel.Width = scene.Width * padding - padding / 2;
            menuPanel.Border = BoxBorder.Double;
            menuPanel.BorderColor(Color.Blue1);

            AnsiConsole.Write(menuPanel);
            PrintLog();
        }

        if (scene.SceneName == "GameScene")
        {
            // Creates the game scene UI
            var gamePanel = new Panel("");
            gamePanel.Header($"[red] {Program.userName} [/]", Justify.Center);
            gamePanel.Height = scene.Height;
            gamePanel.Width = scene.Width * padding - padding / 2;
            gamePanel.Border = BoxBorder.Double;
            gamePanel.BorderColor(Color.Blue1);

            // Creates the HP and score UI panel
            var uiPanel = new Panel(new Rows(
                new Markup("[[" + PrintLife() + " ]]"),
                new Text(""),
                new Text(""),
                new Text($"Score: {CurrentScore:D4}")
            ));
            uiPanel.Header($"[yellow] Health Points and Score [/]", Justify.Center);
            uiPanel.Height = 6;
            uiPanel.Width = scene.Width * padding - padding / 2;
            uiPanel.Border = BoxBorder.Double;
            uiPanel.BorderColor(Color.Blue1);

            AnsiConsole.Write(gamePanel);
            AnsiConsole.Write(uiPanel);
            PrintLog();

            // Renders all game objects on the screen
            foreach (var gameObject in currentGameObjects)
            {
                if (gameObject != null || gameObject.isDestroyed)
                {
                    // Ensures objects are within scene boundaries
                    if (gameObject.position.X > scene.Width
                        || gameObject.position.Y > scene.Height
                        || gameObject.position.X < 0
                        || gameObject.position.Y < 0)
                        continue;

                    Console.SetCursorPosition((int)gameObject.position.X * padding, (int)gameObject.position.Y);
                    AnsiConsole.Markup(DrawSprite(gameObject));
                }
            }
        }

        if (scene.SceneName == "ScoreScene")
        {
            var rule = new Markup("-------- [red][bold]HIGH SCORE[/][/] --------");

            var table = new Table()
                .AddColumn("") // Single column, auto-adjusts width
                .AddRow(rule.Centered())
                .AddEmptyRow()
                .AddRow(PrintHighScore())
                .Border(TableBorder.None); // No table border

            var scorePanel = new Panel(table.Alignment(Justify.Center));
            scorePanel.Header($"[red] Score [/]", Justify.Center);
            scorePanel.Height = scene.Height;
            scorePanel.Width = scene.Width * padding - padding / 2;
            scorePanel.Border = BoxBorder.Double;
            scorePanel.BorderColor(Color.Blue1);

            AnsiConsole.Write(scorePanel);
            PrintLog();
        }
    }

    // Adds messages to the log queue
    private void AddLog(String message)
    {
        if (string.IsNullOrWhiteSpace(message)) return; // Ignore null/empty messages

        _logMessages.Enqueue(message);

        if (_logMessages.Count >= 6) // Keep only the last 5 messages
            _logMessages.Dequeue();
    }

    // Prints the stored log messages
    private void PrintLog()
    {
        Queue<String> logMessages = new(_logMessages);

        foreach (string logEntry in logMessages)
        {
            if (!string.IsNullOrEmpty(logEntry)) // Ensure the message isn't null or empty
            {
                AnsiConsole.WriteLine(logEntry);
            }
        }
    }

    public static void ClearLog()
    {
        _logMessages.Clear();
    }

    // Displays the high score table
    public static Grid PrintHighScore()
    {
        List<KeyValuePair<string, int>> highScores = HighScoreManager.GetHighScoreTable();

        var sortedScores = highScores.OrderByDescending(entry => entry.Value).ToList();

        var grid = new Grid();
        grid.AddColumn(); // Rank column
        grid.AddColumn(); // Name column
        grid.AddColumn(); // Score column

        grid.AddRow(
            new Markup("[bold]RANK[/]", new Style(Color.Red)).LeftJustified(),
            new Markup("[bold]NAME[/]", new Style(Color.Green)).Centered(),
            new Markup("[bold]SCORE[/]", new Style(Color.Blue)).RightJustified()
        );

        grid.AddEmptyRow();

        if (sortedScores.Count == 0)
        {
            grid.AddRow(
                new Text("").LeftJustified(),
                new Markup("[italic]No high scores yet.[/]").Centered(),
                new Text("").RightJustified()
            );
        }
        else
        {
            int i = 1; // Start ranking from 1
            foreach (var entry in sortedScores)
            {
                grid.AddRow(
                    new Markup($"[bold]{i}.[/]").LeftJustified(),
                    new Markup($"  {entry.Key}  ").Centered(),
                    new Markup($"[cyan]{entry.Value:D4}[/]").RightJustified()
                );
                i++;
            }
        }

        return grid;
    }

    // Displays the player's remaining life using bar symbols
    private string PrintLife()
    {
        int maxLife = 100, currentLife = 0, bars = 46;
        string fullBar = " \u25a0", emptyBar = " \u25a1";

        if (Player.Instance != null)
        {
            currentLife = Player.Instance.Life;

            float remainingLife = MathF.Abs((float)currentLife / maxLife * bars);
            int remainingBars = (int)remainingLife;
            int lostLife = bars - remainingBars;

            return
                $"[red]{string.Concat(Enumerable.Repeat(fullBar, remainingBars))}{string.Concat(Enumerable.Repeat(emptyBar, lostLife))}[/]";
        }

        return "";
    }

    private void UpdateScore(int score)
    {
        CurrentScore += score;
    }

    public static int GetCurrentScore()
    {
        return CurrentScore;
    }

    // Compares objects based on layer priority
    private int CompareGameObjects(GameObject obj1, GameObject obj2)
    {
        if (obj1 == null && obj2 == null) return 0;
        if (obj1 == null) return 1; // Null objects should go last
        if (obj2 == null) return -1;

        return obj1.layer.CompareTo(obj2.layer);
    }

    // Converts game object sprites into Emojis
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