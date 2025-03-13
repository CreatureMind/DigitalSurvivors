using System.Numerics;

namespace DigitalSurvivors;

public class Slash : GameObject
{
    private int life = 1;

    public Slash(float x, float y, char sprite)
    {
        position.X = x;
        position.Y = y;
        this.sprite = sprite;
    }
    
    public override void Awake()
    {
        layer = 10;
        _ = Update();
    }

    async Task Update()
    {
        await Task.Delay(200);
        Destroy();
    }
}