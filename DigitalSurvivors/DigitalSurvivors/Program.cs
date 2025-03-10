using System;
using System.Threading;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DigitalSurvivors;

class Program
{
    static int playerX = 10;
    static int playerY = 10;
    static bool isRunning = true;

    // List of enemies, each with position (x, y) and speed
    static List<Enemy> enemies = new List<Enemy>
    {
        new Enemy(5, 5, 0.1f),   // First enemy, moves at speed 0.1
        new Enemy(15, 3, 0.2f),  // Second enemy, moves at speed 0.2
        new Enemy(2, 10, 0.3f)   // Third enemy, moves at speed 0.3
    };

    static void Main()
    {
        Console.CursorVisible = false;
        Thread inputThread = new Thread(ReadInput);
        inputThread.Start();

        while (isRunning)
        {
            UpdateEnemies(); // Move enemies based on their speed
            Render();
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
                MovePlayer(key);
            }
        }
    }

    static void MovePlayer(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.W: if (playerY > 0) playerY--; break;
            case ConsoleKey.S: if (playerY < Console.WindowHeight - 1) playerY++; break;
            case ConsoleKey.A: if (playerX > 0) playerX--; break;
            case ConsoleKey.D: if (playerX < Console.WindowWidth - 1) playerX++; break;
            case ConsoleKey.Escape: isRunning = false; break;
        }
    }

    static void UpdateEnemies()
    {
        foreach (var enemy in enemies)
        {
            // Move each enemy based on its unique fractional speed
            float deltaX = 0;
            float deltaY = 0;

            // Calculate movement direction towards the player
            if (enemy.X < playerX) deltaX = 1f;
            else if (enemy.X > playerX) deltaX = -1f;

            if (enemy.Y < playerY) deltaY = 1f;
            else if (enemy.Y > playerY) deltaY = -1f;

            // Apply fractional speed
            enemy.X += deltaX * enemy.Speed;
            enemy.Y += deltaY * enemy.Speed;
        }
    }

    static void Render()
    {
        Console.Clear();

        // Draw Player (round to nearest int)
        Console.SetCursorPosition((int)Math.Round(playerX), (int)Math.Round(playerY));
        AnsiConsole.Markup("[red]@[/]"); // Red player icon

        // Draw Enemies (round to nearest int)
        foreach (var enemy in enemies)
        {
            Console.SetCursorPosition((int)Math.Round(enemy.X), (int)Math.Round(enemy.Y));
            AnsiConsole.Markup("[green]E[/]"); // Green enemy icon
        }

        Console.SetCursorPosition(0, 0); // Prevent input lag
    }
}

class Enemy
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Speed { get; set; } // Speed determines how many steps the enemy moves per update

    public Enemy(float x, float y, float speed)
    {
        X = x;
        Y = y;
        Speed = speed;
    }
}