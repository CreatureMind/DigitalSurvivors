using System.Numerics;

namespace DigitalSurvivors;

public class GameObject
{
    //public static event Action<GameObject>? OnDestroy;
    
    public Vector2 position;
    public int layer;
    public char sprite;
    public bool isDestroyed { get; private set; } = false;

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
    
    public virtual void Update(){}

    public static Type GetClassType()
    {
        return typeof(GameObject);
    }

    
    public void Destroy()
    {
        if (!isDestroyed)
        {
            isDestroyed = true;
            Program.currentScene.RemoveObject(this);
        }

        //isDestroyed = true;

        //Scene.instance?.RemoveObject(this);
        //Program.currentScene.RemoveObject(this);
        // Invoke the static destroy event
        //OnDestroy?.Invoke(this);
    }
}