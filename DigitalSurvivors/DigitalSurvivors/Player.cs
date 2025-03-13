using System;
using System.Collections.Generic;
using System.Numerics;

namespace DigitalSurvivors;

public class Player : GameObject
{
    public static event Action<GameObject> OnAttack;
    
    public static Player instance;
    public char dir;
    
    public override void Awake()
    {
        instance = this;
        Program.OnKeyPress += MovePlayer;
        Program.OnKeyPress += Attack;
        sprite = '@';
        layer = 1;
        position.X = Program.currentScene.Width / 2;
        position.Y = Program.currentScene.Height / 2;
    }
    
    void MovePlayer(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.W:
                position.Y--;
                dir = 'u';
                break;
            case ConsoleKey.S:
                position.Y++;
                dir = 'd';
                break;
            case ConsoleKey.D:
                position.X++;
                dir = 'r';
                break;
            case ConsoleKey.A:
                position.X--;
                dir = 'l';
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
        
        Console.Write(new Vector2(position.X, position.Y));
        
        Thread.Sleep(100);
    }

    void Attack(ConsoleKey key)
    {
        if (key == ConsoleKey.Spacebar)
        {
            if (dir == 'u')
            {
                for (int i = 1; i < 5; i++)
                {
                    OnAttack?.Invoke(new Slash(position.X, position.Y - i, '\u2502'));
                }
            }
            else if (dir == 'd')
            {
                for (int i = 1; i < 5; i++)
                {
                    OnAttack?.Invoke(new Slash(position.X, position.Y + i, '\u2502'));
                }
            }
            else if (dir == 'r')
            {
                for (int i = 1; i < 5; i++)
                {
                    OnAttack?.Invoke(new Slash(position.X + i, position.Y, '\u2500'));
                }
            }
            else if (dir == 'l')
            {
                for (int i = 1; i < 5; i++)
                {
                    OnAttack?.Invoke(new Slash(position.X - i, position.Y, '\u2500'));
                }
            }
        }
    }
}