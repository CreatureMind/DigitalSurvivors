using System.Numerics;

namespace DigitalSurvivors;

public class Scene
{
    //public static Scene instance;
    
    public int Width { get; }
    public int Height { get; }
    public string SceneName { get; }
    private List<GameObject> _gameObjects = new();

    public Scene(int width, int height, string sceneName)
    {
        //instance = this;
        Width = width;
        Height = height;
        SceneName = sceneName;
        
        Program.OnSceneLoaded += OnSceneLoaded;
        Player.OnAttack += AddObject;
        //GameObject.OnDestroy += RemoveObject;
    }
    
    void OnSceneLoaded(Scene scene)
    {
        if(scene == null) return;
        
        if (scene.SceneName == "ScoreScene")
        {
            _gameObjects.Clear();
            return;
        }

        if (scene.SceneName == "GameScene" && _gameObjects.Count <= 0)
        {
            _gameObjects.Add(new Player());
            _gameObjects.Add(EnemySpawner.SpawnEnemy(EnemyType.Normal));
            _gameObjects.Add(EnemySpawner.SpawnEnemy(EnemyType.Medium));
            _gameObjects.Add(EnemySpawner.SpawnEnemy(EnemyType.Heavy));
        }
    }

    public void AddObject(GameObject gameObject)
    {
        if (gameObject == null || gameObject.isDestroyed)
            return;
        
        if (!_gameObjects.Contains(gameObject))
        {
            _gameObjects.Add(gameObject);
        }
        else
        {
            Debug.Log("Attempted to add existing object");
        }
    }
    
    public GameObject[] GetGameObjects()
    {
        return _gameObjects.ToArray();
    }

    public void ClearGameObjects()
    {
        _gameObjects.Clear();
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
                if (!_gameObjects.Contains(gameObject))
                {
                    Debug.Log($"Attempted to remove {gameObject}, but it was not found in the list.");
                    return;
                }
    
                if (gameObject.isDestroyed)
                {
                    _gameObjects.Remove(gameObject);
                }
            //}
            
        //}
        
        //Debug.Log($"Successfully removed {gameObject}");
    }
    
    public Enemy? FindEnemyAt(float x, float y)
    {
        return _gameObjects.OfType<Enemy>().FirstOrDefault(enemy => enemy.position.X == x && enemy.position.Y == y);
    }
    
    public Player? FindPlayerAt(float x, float y)
    {
        return _gameObjects.OfType<Player>().FirstOrDefault(player => player.position.X == x && player.position.Y == y);
    }
}