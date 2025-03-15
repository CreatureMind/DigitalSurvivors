using System.Numerics;

namespace DigitalSurvivors;

public class Scene
{
    //public static Scene instance;
    
    public int Width { get; }
    public int Height { get; }

    public Scene(int width, int height)
    {
        //instance = this;
        Width = width;
        Height = height;
        GameObjects = new List<GameObject>();
        
        Program.OnSceneLoaded += OnSceneLoaded;
        Player.OnAttack += AddObject;
        //GameObject.OnDestroy += RemoveObject;
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
        if (gameObject == null || gameObject.isDestroyed)
            return;
        
        if (!GameObjects.Contains(gameObject))
        {
            GameObjects.Add(gameObject);
        }
        else
        {
            Debug.Log("Attempted to add existing object");
        }
    }

    public void RemoveObject(GameObject gameObject)
    {
        if (gameObject == null)
        {
            Debug.Log("Attempted to remove a null GameObject");
            return;
        }

        // lock (GameObjects)
        // {
        //     lock (gameObject)
        //     {
                if (!GameObjects.Contains(gameObject))
                {
                    Debug.Log($"Attempted to remove {gameObject}, but it was not found in the list.");
                    return;
                }
    
                if (gameObject.isDestroyed)
                {
                    GameObjects.Remove(gameObject);
                }
            //}
            
        //}
        
        //Debug.Log($"Successfully removed {gameObject}");
    }
    
    public Enemy? FindEnemyAt(float x, float y)
    {
        return GameObjects.OfType<Enemy>().FirstOrDefault(enemy => enemy.position.X == x && enemy.position.Y == y);
    }
    
    public Player? FindPlayerAt(float x, float y)
    {
        return GameObjects.OfType<Player>().FirstOrDefault(player => player.position.X == x && player.position.Y == y);
    }
}