using System.Numerics;

namespace DigitalSurvivors;

public class Scene
{
    public int Width { get; }
    public int Height { get; }

    public Scene(int width, int height)
    {
        Width = width;
        Height = height;
        gameObjects = new List<GameObject>();
        Program.OnSceneLoaded += OnSceneLoaded;
    }
    
    public List<GameObject> gameObjects { get; }

    void OnSceneLoaded(Scene scene)
    {
        gameObjects.Add(new Player());
    }
}