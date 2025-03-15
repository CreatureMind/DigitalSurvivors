using System.Numerics;

namespace DigitalSurvivors;

public class Slash : GameObject
{
    private int life = 1;
    private float destructionTime = 0.1f;
    public int Damage = 1;

    public Slash(float x, float y, char sprite)
    {
        position.X = x;
        position.Y = y;
        this.sprite = sprite;
    }
    
    public override void Awake()
    {
        layer = 10;
    }

    public override void Update()
    {
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