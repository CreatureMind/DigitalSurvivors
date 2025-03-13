using System.Numerics;

namespace DigitalSurvivors;

public class GameObject
{
    public static event Action<GameObject> OnDestroy;
    
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
    
    public void Destroy()
    {
        // Invoke the static destroy event
        OnDestroy?.Invoke(this);
    }
}