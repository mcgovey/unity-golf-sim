using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager _instance;

    // Game state reference
    public GameState State { get; private set; }

    // Public property to access the singleton
    public static GameManager Instance
    {
        get
        {
            // If instance doesn't exist, try to find it in the scene
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                // If it still doesn't exist, create a new GameObject with GameManager
                if (_instance == null)
                {
                    GameObject managerObject = new GameObject("GameManager");
                    _instance = managerObject.AddComponent<GameManager>();
                }
            }

            return _instance;
        }
    }

    // Make sure we only have one instance
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize game state
        State = new GameState();
    }

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Game Started");
        Debug.Log(State.ToString());
    }

    // Update is called once per frame
    private void Update()
    {
        // Future game loop updates will go here
    }
}
