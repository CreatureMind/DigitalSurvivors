namespace DigitalSurvivors;

public class Enemy : GameObject
{
    public float Speed { get; set; } // Speed determines how many steps the enemy moves per update

    public override void Awake()
    {
        sprite = 'E';
    }

    public Enemy(float x, float y, float speed)
    {
        Speed = speed;
    }

    public void TakeDamage(int damage)
    {
        
    }
    
    static void UpdateEnemies()
    {
        // foreach (var enemy in enemies)
        // {
        //     // Move each enemy based on its unique fractional speed
        //     float deltaX = 0;
        //     float deltaY = 0;
        //
        //     // Calculate movement direction towards the player
        //     if (enemy.X < playerX) deltaX = 1f;
        //     else if (enemy.X > playerX) deltaX = -1f;
        //
        //     if (enemy.Y < playerY) deltaY = 1f;
        //     else if (enemy.Y > playerY) deltaY = -1f;
        //
        //     // Apply fractional speed
        //     enemy.X += deltaX * enemy.Speed;
        //     enemy.Y += deltaY * enemy.Speed;
        // }
    }
}