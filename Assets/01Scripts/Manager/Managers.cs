using Unity.VisualScripting;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers instance = null;
    public static Managers Instance { get { Init(); return instance; } }


    private GameManager _game = null;

    public static GameManager Game { get { return Instance._game; } }
    public static InputManager InputMgr { get { return Instance._input; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static UIManager UI { get { return Instance._ui; } }

    private InputManager _input = new InputManager();
    private ResourceManager _resource = new ResourceManager();
    private UIManager _ui = new UIManager();


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
        if (instance == null)
        {
            // === Default Setting === //
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            Input.multiTouchEnabled = false;
            // === Default Setting === //

            GameObject managers = new GameObject { name = "@Managers" };
            managers.AddComponent<Managers>();
            instance = managers.GetComponent<Managers>();

            instance._game = managers.GetOrAddComponent<GameManager>();

            DontDestroyOnLoad(managers);
        }
    }
}
