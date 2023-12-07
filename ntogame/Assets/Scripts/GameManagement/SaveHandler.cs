using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using GameData;

public class SaveHandler : MonoBehaviour
{
    [SerializeField] private GameObject FastSavePrefab;
    [SerializeField] private SaveData Main = null;
    [SerializeField] private SaveData Fast = null;

    private SaveData Current = null;

    private Rigidbody Player = null;
    private Level Level = null;
    private InputHandler InputHandler = null;

    public bool _HasContinue => Main.CurrentLevel != 0;

    private static SaveHandler Instance = null;
    public static SaveHandler _Instance => Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if(File.Exists(Path.Combine(Application.dataPath, "MainSave.txt")))
        {
            Main = JsonUtility.FromJson<SaveData>(File.ReadAllText(Path.Combine(Application.dataPath, "MainSave.txt")));
        }
        else
        {
            Main = new SaveData();
        }

        if (File.Exists(Path.Combine(Application.dataPath, "FastSave.txt")))
        {
            Fast = JsonUtility.FromJson<SaveData>(File.ReadAllText(Path.Combine(Application.dataPath, "FastSave.txt")));
        }
        else
        {
            Fast = new SaveData();
        }


        if (!FindObjectOfType<Level>())
        {
            return;
        }

        if(Main.CurrentLevel != SceneManager.GetActiveScene().buildIndex)
        {
            InputHandler = FindObjectOfType<InputHandler>();
            InputHandler.OnKeyDown += KeyDown;

            Player = FindObjectOfType<Movement>().GetComponent<Rigidbody>();
            Level = FindObjectOfType<Level>();

            MainSave();
        }
        else
        {
            Current = Main;
            StopAllCoroutines();
            Initialize();
        }
    }

    public void LevelLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Player = null;
        InputHandler = null;
        Level = null;

        SceneManager.sceneLoaded -= LevelLoaded;
        if (!FindObjectOfType<Level>())
        {
            return;
        }

        StopAllCoroutines();
        Initialize();
    }

    private void Initialize()
    {
        InputHandler = FindObjectOfType<InputHandler>();
        InputHandler.OnKeyDown += KeyDown;

        Player = FindObjectOfType<Movement>().GetComponent<Rigidbody>();
        Player.position = new Vector3(Current.XPosition, Current.YPosition, Current.ZPosition);
        FindObjectOfType<PlayerCamera>().SetRotation(Current.YRotation);

        Camera.main.transform.position = Player.position;

        GameManager.Health = Current.Health;

        Level = FindObjectOfType<Level>();
        Level.ReadLevelInfo(Current.LevelInfo);
    }

    public void KeyDown()
    {
        if (InputManager.GetButtonDown("FastSave"))
        {
            FastSave();
        }
        else if (InputManager.GetButtonDown("FastLoad"))
        {
            FastLoad();
        }
    }

    public static void DeleteMain()
    {
        string path = Path.Combine(Application.dataPath, "MainSave.txt");
        if (File.Exists(Path.Combine(Application.dataPath, "MainSave.txt")))
        {
            File.Delete(Path.Combine(Application.dataPath, "MainSave.txt"));
        }

        if(Instance != null)
        {
            Instance.Main = new SaveData();
        }
    }

    public static void DeleteInstance()
    {
        Destroy(Instance.gameObject);
        Instance = null;
    }

    public void LoadMain()
    {
        Current = Main;
        SceneManager.sceneLoaded += LevelLoaded;
        SceneTransitions.instance.CallSceneTrans(Current.CurrentLevel);
    }

    public void FastLoad()
    {
        if(Fast.CurrentLevel == 0)
        {
            if(Main.CurrentLevel != 0)
            {
                Fast = Main.GetClone();
            }
            else
            {
                return;
            }
        }

        Current = Fast;
        SceneManager.sceneLoaded += LevelLoaded;
        SceneTransitions.instance.CallSceneTrans(Current.CurrentLevel);
    }

    public void MainSave()
    {
        Main.CurrentLevel = SceneManager.GetActiveScene().buildIndex;

        Main.Health = GameManager.Health;

        Main.XPosition = Player.position.x;
        Main.YPosition = Player.position.y;
        Main.ZPosition = Player.position.z;

        Main.YRotation = Player.rotation.eulerAngles.y;

        Main.LevelInfo = Level.GetLevelInfo();

        File.WriteAllText(Path.Combine(Application.dataPath, "MainSave.txt"), JsonUtility.ToJson(Main));
    }

    public void FastSave()
    {
        Instantiate(FastSavePrefab, FindObjectOfType<Canvas>().transform);

        Fast.CurrentLevel = SceneManager.GetActiveScene().buildIndex;

        Fast.Health = GameManager.Health;

        Fast.XPosition = Player.position.x;
        Fast.YPosition = Player.position.y;
        Fast.ZPosition = Player.position.z;

        Fast.YRotation = Player.rotation.eulerAngles.y;

        Fast.LevelInfo = Level.GetLevelInfo();

        File.WriteAllText(Path.Combine(Application.dataPath, "FastSave.txt"), JsonUtility.ToJson(Fast));
    }
}