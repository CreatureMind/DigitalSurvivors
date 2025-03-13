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
        GameObjects = new List<GameObject>();
        
        Program.OnSceneLoaded += OnSceneLoaded;
        Player.OnAttack += AddObject;
        GameObject.OnDestroy += RemoveObject;
    }
    
    public List<GameObject> GameObjects { get; set; }

    void OnSceneLoaded(Scene scene)
    {
        GameObjects.Add(new Player());
        GameObjects.Add(EnemySpawner.SpawnEnemy(EnemyType.Normal));
        GameObjects.Add(EnemySpawner.SpawnEnemy(EnemyType.Medium));
        GameObjects.Add(EnemySpawner.SpawnEnemy(EnemyType.Heavy));
    }

    public void AddObject(GameObject gameObject)
    {
        if (gameObject != null)
        {
            GameObjects.Add(gameObject);
        }
    }

    public void RemoveObject(GameObject gameObject)
    {
        if (gameObject != null && GameObjects.Contains(gameObject))
        {
            Debug.Log($"Slash count before removal: {GameObjects.Count(obj => obj is Slash)}");
            GameObjects.Remove(gameObject);
            Debug.Log($"Slash count after removal: {GameObjects.Count(obj => obj is Slash)}");
        }
        else
        {
            Debug.Log("Attempted to remove a null GameObject");
        }
    }
}