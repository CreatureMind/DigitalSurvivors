using System.Numerics;

namespace DigitalSurvivors;

public static class EnemySpawner
{
    static Random randomX = new Random();
    static Random randomY = new Random();
    
    public static Enemy SpawnEnemy(EnemyType enemyType)
    {
        int posX, posY;

        posX = randomX.Next(1, Program.currentScene.Width - 1);
        
        if (posX == 1 || posX == Program.currentScene.Width - 2)
        {
            posY = randomY.Next(1, Program.currentScene.Height);
        }
        else
        {
            posY = (randomY.Next(0, 2) == 0) ? 1 : Program.currentScene.Height - 2;
        }

        switch (enemyType)
        {
            case EnemyType.Normal:
                return new Enemy(posX, posY, 5, 2, 0.03f, 3, EnemyType.Normal);
            case EnemyType.Medium:
                return new Enemy(posX, posY, 10, 4, 0.02f, 6, EnemyType.Medium);
            case EnemyType.Heavy:
                return new Enemy(posX, posY, 20, 6, 0.01f, 10, EnemyType.Heavy);
            default:
                return new Enemy(posX, posY, 5, 2, 0.03f, 3, EnemyType.Normal);
        }
    }
}