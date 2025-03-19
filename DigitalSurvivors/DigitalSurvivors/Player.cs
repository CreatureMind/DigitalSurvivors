using System;
using System.Collections.Generic;
using System.Numerics;

namespace DigitalSurvivors;

// Player class for all player logic, handling position, rendering, destruction, input, attack and damage
public class Player : GameObject
{
    // Event triggered when the player attacks
    public static event Action<GameObject> OnAttack;

    public static Player? Instance { get; private set; }    // Singleton instance of the Player class
    public int Life { get; set; } = 100;                    // Player's life points
    private char _dir;                                      // Direction of the player's attack
    private static float _delayTime = 0.5f;                 // Attack delay time
    private float _attackDelayTime = _delayTime;
    private bool _canAttack = true;                         // Flag indicating if the player can attack

    // Called when the Player object is initialized, sets his sprite, layer and position
    public override void Awake()
    {
        Instance = this;
        
        Program.OnKeyPress += MovePlayer;
        sprite = '@';
        layer = 1;
        position.X = Program.currentScene.Width / 2;
        position.Y = Program.currentScene.Height / 2;
    }

    public override void Update()
    {
        // Handle attack delay
        if (!_canAttack)
            _attackDelayTime -= Program.deltaTime;

        // If attack delay is over, reset the attack flag and delay time
        if (_attackDelayTime <= 0.0f)
        {
            _canAttack = !_canAttack;
            _attackDelayTime = _delayTime;
        }

        // Check if the player is dead, and handle the end of the game
        if (Life <= 0)
        {
            Destroy();
            Program.HighScoreTable.Add(Program.userName, Renderer.GetCurrentScore()); // Add score to the table
            HighScoreManager.AddToHighScoreTable(Program.HighScoreTable);
            Program.currentScene.ClearGameObjects();
            Program.SetScene(new Scene(50, 50, "ScoreScene")); // Switch to the score scene
        }
    }

    // Handles player movement based on key press
    void MovePlayer(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.W:
                position.Y--;
                _dir = 'u';
                break;
            case ConsoleKey.S:
                position.Y++;
                _dir = 'd';
                break;
            case ConsoleKey.D:
                position.X++;
                _dir = 'r';
                break;
            case ConsoleKey.A:
                position.X--;
                _dir = 'l';
                break;
            case ConsoleKey.Spacebar:
                // If the player can attack, trigger an attack in the current direction
                if (_canAttack)
                {
                    Attack(_dir);
                    _canAttack = !_canAttack; // Toggle the attack flag to prevent spamming
                }

                break;
        }

        // Wrap the player position around the screen if it goes out of bounds
        if (position.X > Program.currentScene.Width - 2)
            position.X = 1;
        if (position.Y > Program.currentScene.Height - 2)
            position.Y = 1;
        if (position.X < 1)
            position.X = Program.currentScene.Width - 2;
        if (position.Y < 1)
            position.Y = Program.currentScene.Height - 2;
    }

    // Handles the player's attack in the specified direction
    void Attack(char dir)
    {
        // Validate direction
        if (dir != 'u' && dir != 'd' && dir != 'l' && dir != 'r')
            return;

        // Loop to create a slash effect (attack range of 4)
        for (int i = 1; i < 5; i++)
        {
            float newX = position.X;
            float newY = position.Y;
            char slashSprite = dir is 'u' or 'd' ? '\u2502' : '\u2500'; // Choose the correct symbol for slash

            // Adjust position based on the attack direction
            switch (dir)
            {
                case 'u': newY -= i; break;
                case 'd': newY += i; break;
                case 'r': newX += i; break;
                case 'l': newX -= i; break;
            }

            Slash newSlash = new Slash(newX, newY, slashSprite);

            // Add slash to the scene
            Program.currentScene?.AddObject(newSlash);

            // Check for an enemy immediately at this position
            Enemy? enemy = Program.currentScene?.FindEnemyAt(newX, newY);
            enemy?.TakeDamage(newSlash.Damage);
        }
    }

    public void TakeDamage(int damage)
    {
        Console.Beep();
        Life -= damage;
    }
}