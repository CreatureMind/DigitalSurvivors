using System;
using System.Collections.Generic;

namespace DigitalSurvivors;

public class Player
{
    ConsoleKeyInfo _keyInfo = new ConsoleKeyInfo();
    private char key;
    
    private readonly List<Position> _playerPosition = new List<Position>();

    private int X { get; set; } = Canvas.Width / 2;
    private int Y { get; set; } = Canvas.Height / 2;

    public void DrawPlayer()
    {
        _playerPosition.Clear();
        _playerPosition.Add(new Position(X, Y));

        foreach (Position pos in _playerPosition)
        {
            Console.SetCursorPosition(X, Y);
            Console.Write("\u2588");
        }
    }

    public void Input()
    {
        if (Console.KeyAvailable)
        {
            _keyInfo = Console.ReadKey(true);
            key = _keyInfo.KeyChar;
        }
    }
    
    public void MovePlayer()
    {
        /*if (_keyInfo.Key == ConsoleKey.W)
        {
            _playerPosition.Add(new Position(X, Y+1));
            //Y--;
        }*/
        switch (key)
        {
            case 'w':
                Y--;
                break;
            case 's':
                Y++;
                break;
            case 'd':
                X++;
                break;
            case 'a':
                X--;
                break;
        }
        _playerPosition.Add(new Position(X, Y));
        _playerPosition.RemoveAt(0);
        Thread.Sleep(100);
    }
}