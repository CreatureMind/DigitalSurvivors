using System.Numerics;

namespace DigitalSurvivors;

// Spawns enemies at random positions with a delay
public class EnemySpawner : GameObject
{
    private float _delayTime = 1f; // Delay before spawning a new enemy
    
    static Random randomX = new();
    static Random randomY = new();
    private Random _roll = new(); // Randomizer for enemy type selection

    private EnemyType randonEnemy;

    public override void Update()
    {
        // Countdown before spawning a new enemy
        if (_delayTime > 0)
        {
            _delayTime -= Program.deltaTime;
        }
        else
        {
            _delayTime = 4f; // Reset delay timer
            
            int enemyChance = _roll.Next(1, 101); // Get a random number from 1 to 100
            
            // Chose enemy type based on chance
            if (enemyChance <= 60)
                randonEnemy = EnemyType.Normal; // 60% chance
            else if (enemyChance <= 90)
                randonEnemy = EnemyType.Medium; // 30% chance
            else
                randonEnemy = EnemyType.Heavy; // 10% chance (rest)
            
            // Spawn and add the enemy to the scen
            Program.currentScene?.AddObject(SpawnEnemy(randonEnemy));
        }
    }
    
    public static Enemy SpawnEnemy(EnemyType enemyType)
    {
        int posX, posY;

        // Randomly select an edge: 0 = top, 1 = bottom, 2 = left, 3 = right
        int edge = randomX.Next(0, 4);

        switch (edge)
        {
            case 0: // Top edge
                posX = randomX.Next(1, Program.currentScene.Width - 1);
                posY = 1;
                break;
            case 1: // Bottom edge
                posX = randomX.Next(1, Program.currentScene.Width - 1);
                posY = Program.currentScene.Height - 2;
                break;
            case 2: // Left edge
                posX = 1;
                posY = randomY.Next(1, Program.currentScene.Height - 1);
                break;
            case 3: // Right edge
                posX = Program.currentScene.Width - 2;
                posY = randomY.Next(1, Program.currentScene.Height - 1);
                break;
            default:
                posX = 1;
                posY = 1;
                break;
        }

        // Create an enemy with stats based on its type
        switch (enemyType)
        {
            case EnemyType.Normal:
                return new Enemy(posX, posY, 5, 2, 1f,10, EnemyType.Normal);
            case EnemyType.Medium:
                return new Enemy(posX, posY, 10, 4, 0.5f,20, EnemyType.Medium);
            case EnemyType.Heavy:
                return new Enemy(posX, posY, 20, 6, 0.2f,40, EnemyType.Heavy);
            default:
                return new Enemy(posX, posY, 5, 2, 1f,10, EnemyType.Normal);
        }
    }
}