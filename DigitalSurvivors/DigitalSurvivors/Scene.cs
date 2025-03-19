using System.Numerics;

namespace DigitalSurvivors;

// Manages game objects within a scene, handling their addition, removal, and updates.
public class Scene
{
    public int Width { get; } // Scene width in grid units
    public int Height { get; } // Scene height in grid units
    public string SceneName { get; } // Name of the current scene
    private List<GameObject> _gameObjects = new(); // Stores all game objects in the scene

    public Scene(int width, int height, string sceneName)
    {
        Width = width;
        Height = height;
        SceneName = sceneName;

        Program.OnSceneLoaded += OnSceneLoaded; // Subscribe to scene load event
        Player.OnAttack += AddObject; // Subscribe to player attack event
    }

    // Called when a new scene is loaded
    void OnSceneLoaded(Scene scene)
    {
        if (scene == null) return;

        // Clear objects when switching to the high score screen
        if (scene.SceneName == "ScoreScene")
        {
            _gameObjects.Clear();
            return;
        }

        // When starting a new game, initialize player and enemy spawner
        if (scene.SceneName == "GameScene" && _gameObjects.Count <= 0)
        {
            _gameObjects.Add(new Player());
            _gameObjects.Add(new EnemySpawner());
        }
    }

    // Adds a new game object to the scene
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

    // Removes a specific object from the scene
    public void RemoveObject(GameObject gameObject)
    {
        if (gameObject == null)
        {
            Debug.Log("Attempted to remove a null GameObject");
            return;
        }

        if (!_gameObjects.Contains(gameObject))
        {
            Debug.Log($"Attempted to remove {gameObject}, but it was not found in the list.");
            return;
        }

        if (gameObject.isDestroyed)
        {
            _gameObjects.Remove(gameObject);
        }
    }

    // Finds an enemy at a specific position in the scene
    public Enemy? FindEnemyAt(float x, float y)
    {
        return _gameObjects.OfType<Enemy>().FirstOrDefault(enemy => enemy.position.X == x && enemy.position.Y == y);
    }

    public Player? FindPlayerAt(float x, float y)
    {
        return _gameObjects.OfType<Player>().FirstOrDefault(player => player.position.X == x && player.position.Y == y);
    }
}