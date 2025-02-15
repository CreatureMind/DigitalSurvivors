namespace DigitalSurvivors;

class Program
{
    static void Main(string[] args)
    {
        bool finished = false;
        Player player = new Player();
        //Canvas canvas = new Canvas();

        while (!finished)
        {
            Canvas.DrawCanvas();
            player.Input();
            player.DrawPlayer();
            player.MovePlayer();
        }
    }
}