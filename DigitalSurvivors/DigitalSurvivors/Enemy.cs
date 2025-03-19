namespace DigitalSurvivors;

// Represents an enemy in the game that moves toward the player and deals damage
public class Enemy : GameObject
{
    // Event triggered when the enemy is defeated to update the player's score
    public static event Action<int> UpdateScore;

    private int _life; // Enemy's health points
    private int _damage; // Damage dealt to the player on contact
    private readonly float _speed; // Speed determines how many steps the enemy moves per update
    private int _score; // The score you get when enemy killed
    private EnemyType _enemyType; // Type of enemy (Normal, Medium, Heavy)

    private float _moveProgress; // Tracks movement progress
    private bool _moveHorizontally = true; // Flag for choosing between horizontal and vertical movement
    private float _movementTime = 0.0f; // Controls movement timing

    // Constructor
    public Enemy(float x, float y, int life, int damage, float speed, int score, EnemyType enemyType)
    {
        position.X = x;
        position.Y = y;
        _life = life;
        _damage = damage;
        _speed = speed;
        _enemyType = enemyType;
        _score = score;
        
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
        } // Assigns a character sprite based on enemy type
    }

    public override void Awake()
    {
        layer = 0;
    }

    public override void Update()
    {
        // Controls movement timing to move in intervals
        if (_movementTime < 0.25f)
        {
            _movementTime += Program.deltaTime;
        }
        else
        {
            _movementTime = 0.0f;
            MoveEnemy();
        }

        // If the enemy's health reaches zero update the score and remove it from the scene
        if (_life <= 0)
        {
            UpdateScore?.Invoke(_score);
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
            // Moves toward the player's position choosing between X and Y directions moves like zigzag 
            if (_moveHorizontally)
            {
                if (position.X != Player.Instance.position.X)
                    moveX = (position.X < Player.Instance.position.X) ? 1 : -1;
            }
            else
            {
                if (position.Y != Player.Instance.position.Y)
                    moveY = (position.Y < Player.Instance.position.Y) ? 1 : -1;
            }
        }

        position.X += moveX;
        position.Y += moveY;

        _moveHorizontally = !_moveHorizontally; // Switch movement direction

        // Checks if the enemy collides with the player and deal damage
        Player? player = Program.currentScene?.FindPlayerAt(position.X, position.Y);
        player?.TakeDamage(_damage);
    }

    public void TakeDamage(int damage)
    {
        Console.Beep();
        _life -= damage;
    }
}