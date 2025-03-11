namespace DigitalSurvivors;

public class Enemy : GameObject
{
    public int Life; // Life determines how much health point the enemy has
    public int Damage; // Damage determines how much damage the enemy deals to the player
    public float Speed; // Speed determines how many steps the enemy moves per update
    public int Exp; // Exp determines how many experience the player gets for killing it
    public EnemyType EnemyType;

    public Enemy(float x, float y, int life, int damage, float speed, int exp, EnemyType enemyType)
    {
        position.X = x;
        position.Y = y;
        Life = life;
        Damage = damage;
        Speed = speed;
        Exp = exp;
        
        switch (enemyType)
        {
            case EnemyType.Normal:
                sprite = 'N';
                break;
            case EnemyType.Medium:
                sprite = 'M';
                break;
            case EnemyType.Heavy:
                sprite = 'H';
                break;
            default:
                sprite = 'E';
                break;
        } // Sets the sprite
    }
    
    public override void Awake()
    {
        layer = 0;
        _ = Update();
    }

    async Task Update()
    {
        while (true)
        {
            MoveEnemy();
            await Task.Delay(25);
        }
    }

    void MoveEnemy()
    {
        // Move enemy based on its unique fractional speed
        // Calculate movement direction towards the player
        float deltaX = Player.instance.position.X > position.X ? 1 : (Player.instance.position.X < position.X ? -1 : 0);
        float deltaY = Player.instance.position.Y > position.Y ? 1 : (Player.instance.position.Y < position.Y ? -1 : 0);
    
        // Apply fractional speed
        position.X += deltaX * Speed;
        position.Y += deltaY * Speed;
    }

    public void TakeDamage(int damage)
    {
        
    }
}