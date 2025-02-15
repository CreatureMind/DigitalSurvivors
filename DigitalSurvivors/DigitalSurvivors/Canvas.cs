namespace DigitalSurvivors;

public static class Canvas
{
    public static int Width { get; set; }
    public static int Height { get; set; }

    static Canvas()
    {
        Width = 100;
        Height = 50;
        
        Console.SetWindowSize(Width + 1, Height + 1);
        Console.SetBufferSize(Width + 2, Height + 2);
        Console.CursorVisible = false;
    }

    public static void DrawCanvas()
    {
        Console.Clear();
        
        Console.SetCursorPosition(0, 0);
        Console.Write("\u2554");
        
        for (int x = 1; x <= Width - 1; x++)
        {
            Console.SetCursorPosition(x, 0);
            Console.Write("\u2550");
        }
        
        Console.SetCursorPosition(Width, 0);
        Console.Write("\u2557");
        
        for (int x = 1; x <= Width - 1; x++)
        {
            Console.SetCursorPosition(x, Height);
            Console.Write("\u2550");
        }
        
        Console.SetCursorPosition(0, Height);
        Console.Write("\u255a");
        
        for (int y = 1; y <= Height - 1; y++)
        {
            Console.SetCursorPosition(0, y);
            Console.Write("\u2551");
        }
        
        Console.SetCursorPosition(Width, Height);
        Console.Write("\u255d");

        for (int y = 1; y <= Height - 1; y++)
        {
            Console.SetCursorPosition(Width, y);
            Console.Write("\u2551");
        }
    }
}