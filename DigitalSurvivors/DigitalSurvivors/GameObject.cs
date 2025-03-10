using System.Numerics;

namespace DigitalSurvivors;

public class GameObject
{
    public Vector2 position;
    public int layer;
    public char sprite;

    public GameObject()
    {
        Awake();
    }

    public GameObject(Vector2 position, int layer, char sprite)
    {
        this.position = position;
        this.layer = layer;
        this.sprite = sprite;
        Awake();
    }
    
    public virtual void Awake(){}
}