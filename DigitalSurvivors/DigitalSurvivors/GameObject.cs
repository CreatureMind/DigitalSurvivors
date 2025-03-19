using System.Numerics;

namespace DigitalSurvivors;

// Base class for all game objects, handling position, rendering, and destruction
public class GameObject
{
    public Vector2 position; // Stores the object's position
    public int layer; // Determines the rendering order
    public char sprite; // The character that represents the object
    public bool isDestroyed { get; private set; } = false; // Tracks if the object has been destroyed

    // Default constructor, calls Awake method for initialization
    public GameObject()
    {
        Awake();
    }

    // Constructor that initializes position, layer, and sprite
    public GameObject(Vector2 position, int layer, char sprite)
    {
        this.position = position;
        this.layer = layer;
        this.sprite = sprite;
        Awake();
    }

    public virtual void Awake()
    {
    }

    // Method to be called every frame
    public virtual void Update()
    {
    }
    
    // Destroys the object, marking it as destroyed and removing it from the scene
    public void Destroy()
    {
        if (!isDestroyed)
        {
            isDestroyed = true;
            Program.currentScene.RemoveObject(this);
        }
    }
}