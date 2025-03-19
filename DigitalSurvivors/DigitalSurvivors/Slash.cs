using System.Numerics;

namespace DigitalSurvivors;

// Represents the slash game object that the player creates when player attacks
public class Slash : GameObject
{
    private float destructionTime = 0.1f;   // The time that it takes to self-destruct
    public int Damage = 2;                  // The damage the slash deals to the enemy

    // Constructor
    public Slash(float x, float y, char sprite)
    {
        position.X = x;
        position.Y = y;
        this.sprite = sprite;
    }

    public override void Awake()
    {
        layer = 10; // Sets the slash's layer to be above all enemies
    }

    public override void Update()
    {
        // After the creations starts the timer to destroy
        if (destructionTime > 0)
        {
            destructionTime -= Program.deltaTime;
        }
        else
        {
            Destroy();
        }
    }
}