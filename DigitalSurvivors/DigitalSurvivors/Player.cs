using System;
using System.Collections.Generic;
using System.Numerics;

namespace DigitalSurvivors;

public class Player : GameObject
{
    public static event Action<GameObject> OnAttack;
    
    public static Player? Instance;
    public int Life { get; set; } = 100;
    private char _dir;
    private static float _delayTime = 0.5f;
    private float _attackDelayTime = _delayTime;
    private bool _canAttack = true;
    
    public override void Awake()
    {
        Instance = this;
        Program.OnKeyPress += MovePlayer;
        //Program.OnKeyPress += Attack;
        sprite = '@';
        layer = 1;
        position.X = Program.currentScene.Width / 2;
        position.Y = Program.currentScene.Height / 2;
    }

    public override void Update()
    {
        if(!_canAttack)
            _attackDelayTime -= Program.deltaTime;
        
        if (_attackDelayTime <= 0.0f)
        {
            _canAttack = !_canAttack;
            _attackDelayTime = _delayTime;
        }
        
        if (Life <= 0)
        {
            Destroy();
            Program.HighScoreTable.Add(Program.userName, Renderer.GetCurrentScore());
            HighScoreManager.AddToHighScoreTable(Program.HighScoreTable);
            Program.currentScene.ClearGameObjects();
            Program.SetScene(new Scene(50, 50, "ScoreScene"));
        }
    }
    
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
                if (_canAttack)
                {
                    Attack(_dir);
                    _canAttack = !_canAttack;
                }
                break;
        }
        
        if(position.X > Program.currentScene.Width - 2)
            position.X = 1;
        if(position.Y > Program.currentScene.Height - 2)
            position.Y = 1;
        if(position.X < 1)
            position.X = Program.currentScene.Width - 2;
        if(position.Y < 1)
            position.Y = Program.currentScene.Height - 2;
    }
    
    void Attack(char dir)
    {
        // Validate direction
        if (dir != 'u' && dir != 'd' && dir != 'l' && dir != 'r')
            return;
        
        for (int i = 1; i < 5; i++)
        {
            float newX = position.X;
            float newY = position.Y;
            char slashSprite = dir is 'u' or 'd' ? '\u2502' : '\u2500';

            switch (dir)
            {
                case 'u':
                    newY -= i;
                    break;
                case 'd':
                    newY += i;
                    break;
                case 'r':
                    newX += i;
                    break;
                case 'l':
                    newX -= i;
                    break;
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