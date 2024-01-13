using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers _instance = null;
    public static Managers Instance { get { Init(); return _instance; } }


    [HideInInspector]
    public GameManager _game = null;

    private InputManager _input = new InputManager();
    private ResourceManager _resource = new ResourceManager();
    private UIManager _ui = new UIManager();
    private SceneManagerEx _scene = new SceneManagerEx();
    private SoundManager _sound = new SoundManager();
    private PoolManager _pool = new PoolManager();
    private DataManager _data = new DataManager();

    public static GameManager Game { get { return Instance._game; } }
    public static InputManager InputMgr { get { return Instance._input; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static DataManager Data { get { return Instance._data; } }



    private void Start()
    {
        // Init();
    }

    private void Update()
    {
        _input?.OnUpdate();
    }


    private static void Init()
    {
        if (_instance == null)
        {
            // === Default Setting === //
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            Input.multiTouchEnabled = false;
            // === Default Setting === //

            GameObject managers = new GameObject { name = "@Managers" };
            managers.AddComponent<Managers>();
            DontDestroyOnLoad(managers);
            _instance = managers.GetComponent<Managers>();

            GameObject SceneTransition = Managers.Resource.Instantiate("SceneTransition");
            SceneTransition.transform.SetParent(_instance.transform);
            Scene._sceneTransitionAnimCtrl = SceneTransition.GetComponent<Animator>();

            _instance._sound.Init();
            _instance._scene.Init();
            _instance._pool.Init();
            _instance._data.Init();

            _instance._game = managers.GetOrAddComponent<GameManager>();

            DontDestroyOnLoad(managers);
        }
    }

    public static void GameInit()
    {
        _instance._game.Init();
    }

    public static void Clear()
    {
        InputMgr.Clear();
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
    }
}
