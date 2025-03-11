using System;
using System.Collections.Generic;
using System.Numerics;

namespace DigitalSurvivors;

public class Player : GameObject
{
    public static Player instance;
    public override void Awake()
    {
        instance = this;
        Program.OnKeyPress += MovePlayer;
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
                break;
            case ConsoleKey.S:
                position.Y++;
                break;
            case ConsoleKey.D:
                position.X++;
                break;
            case ConsoleKey.A:
                position.X--;
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
        
        Thread.Sleep(100);
    }

    void Attack()
    {
    }
}