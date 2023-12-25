using Unity.VisualScripting;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers instance = null;
    public static Managers Instance { get { Init(); return instance; } }


    private GameManager _game = null;
    public static GameManager Game { get { return Instance._game; } }
    private InputManager _input = new InputManager();
    public static InputManager InputMgr { get { return Instance._input; } }
    private ResourceManager _resource = new ResourceManager();
    public static ResourceManager Resource { get { return Instance._resource; } }


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
