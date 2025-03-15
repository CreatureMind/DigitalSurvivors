namespace DigitalSurvivors;

public class Enemy : GameObject
{
    private int _life; // Life determines how much health point the enemy has
    private int _damage; // Damage determines how much damage the enemy deals to the player
    private readonly float _speed; // Speed determines how many steps the enemy moves per update
    private int _exp; // Exp determines how many experience the player gets for killing it
    private EnemyType _enemyType;
    
    private float _moveProgress;
    private bool _moveHorizontally = true;
    private float _movementTime = 0.0f;

    public Enemy(float x, float y, int life, int damage, float speed, int exp, EnemyType enemyType)
    {
        position.X = x;
        position.Y = y;
        _life = life;
        _damage = damage;
        _speed = speed;
        _exp = exp;
        _enemyType = enemyType;

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
    }

    public override void Update()
    {
        if (_movementTime < 0.25f)
        {
            _movementTime += Program.deltaTime;
        }
        else
        {
            _movementTime = 0.0f;
            MoveEnemy();
        }

        if (_life <= 0)
        {
            Destroy();
        }
    }
    
    void MoveEnemy()
    {
        _moveProgress += _speed;
        if (_moveProgress < 1) return; // Move only when progress reaches 1
        _moveProgress = 0;
        
        int moveX = 0, moveY = 0;

        if (Player.Instance != null)
        {
            if (_moveHorizontally)
            {
                if (position.X != Player.Instance.position.X) moveX = (position.X < Player.Instance.position.X) ? 1 : -1;
            }
            else
            {
                if (position.Y != Player.Instance.position.Y) moveY = (position.Y < Player.Instance.position.Y) ? 1 : -1;
            }
        }

        position.X += moveX;
        position.Y += moveY;

        _moveHorizontally = !_moveHorizontally; // Switch movement mode
        
        Player? player = Program.currentScene?.FindPlayerAt(position.X, position.Y);

        player?.TakeDamage(_damage);
    }

    public void TakeDamage(int damage)
    {
        Console.Beep();
        _life -= damage;
    }
}