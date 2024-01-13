using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum eJoystickType
{
    NotUse,
    Fixed,
    Follow,
    RunningGame
}

public class JoystickController : MonoBehaviour
{
    [Header("조이스틱 타입")]
    public eJoystickType _joystickType = eJoystickType.NotUse;
    [Header("조이스틱 반경")]
    public float _joystickBound = 50.0f;
    [Header("움직일 물체 - Rigidbody 필요")]
    public Rigidbody MoveObjectRigid = null;

    public float Threshold = 10.0f;

    public float _speed = 5.0f;
    private float _curSpeed = 0.0f;
    public float xBound = 0.0f;
    public bool AutoRun = false;
    public float xSensitivity = 0.0f;
    public float xAcceletor = 0.0f;

    public bool UseAccelerate = false;
    public float Accelerate = 100.0f;

    private RectTransform _canvasRect;
    private RectTransform _joystick;
    private RectTransform _joystickHandle;
    [SerializeField]
    private Image _joystickImage;
    private Image _joystickHandleImage;

    private Vector3 _oriPos;
    private Vector3 newDir = Vector3.zero;
    private float _oriXPos;
    private int _walkableMask;

    [HideInInspector]
    public bool CanMove = false;
    private bool _isButtonClick = false;
    private bool _isMouseDown = false;

    public System.Action DownAction = null;
    public System.Action<Vector2> JoystickMoveAction = null;
    public System.Action UpAction = null;

    private void Awake()
    {
        _curSpeed = 0.0f;
        _walkableMask = LayerMask.NameToLayer("Walkable");
        _canvasRect = this.GetComponent<RectTransform>();
        _joystick = _joystickImage.GetComponent<RectTransform>();
        _joystickHandle = _joystick.GetChild(0).GetComponent<RectTransform>();
        _joystickHandleImage = _joystickHandle.GetComponent<Image>();
        _joystickImage.enabled = false;
        _joystickHandleImage.enabled = false;

        AddDownEvent(() => _isMouseDown = true);
        AddUpEvent(() => _isMouseDown = false);

        switch (_joystickType)
        {
            case eJoystickType.NotUse:
                _joystick.gameObject.SetActive(false);
                break;

            case eJoystickType.Fixed:
                break;

            case eJoystickType.Follow:
                break;
        }

        CanMove = true;
        _isButtonClick = false;

        Managers.Game.JoystickController = this;
    }

    private void Update()
    {
        if (!CanMove)
            return;

        if (UseAccelerate)
        {
            if (!_isMouseDown)
            {
                _curSpeed = Mathf.Max(0, _curSpeed - Accelerate * Time.deltaTime);

                CheckMoveDir();
            }

            MoveObjectRigid.position += newDir * Time.deltaTime * _curSpeed;
        }


        switch (_joystickType)
        {
            case eJoystickType.NotUse:
                break;

            case eJoystickType.Fixed:
                if (Input.GetMouseButtonDown(0))
                {
                    _joystickImage.enabled = true;
                    _joystickHandleImage.enabled = true;

                    _joystick.anchoredPosition = Input.mousePosition * 2688f / Screen.height;
                    _joystickHandle.anchoredPosition = Vector2.zero;
                    _oriPos = _joystick.anchoredPosition;

                    DownAction?.Invoke();
                }
                else if (Input.GetMouseButton(0) && !_isButtonClick)
                {
                    _joystickHandle.anchoredPosition = Input.mousePosition * 2688f / Screen.height - _oriPos;
                    if (_joystickHandle.anchoredPosition.magnitude > _joystickBound)
                        _joystickHandle.anchoredPosition = _joystickHandle.anchoredPosition.normalized * _joystickBound;

                    if (_joystickHandle.anchoredPosition.magnitude < Threshold)
                        return;

                    JoystickMoveAction?.Invoke(_joystickHandle.anchoredPosition);

                    Vector3 dir = new Vector3(_joystickHandle.anchoredPosition.x, 0, _joystickHandle.anchoredPosition.y);
                    if (MoveObjectRigid != null)
                        Move(dir);
                }
                else if (Input.GetMouseButtonUp(0) && !_isButtonClick)
                {
                    _joystickImage.enabled = false;
                    _joystickHandleImage.enabled = false;
                    UpAction?.Invoke();
                }
                break;

            case eJoystickType.Follow:
                if (Input.GetMouseButtonDown(0))
                {
                    _joystickImage.enabled = true;
                    _joystickHandleImage.enabled = true;

                    _joystick.anchoredPosition = Input.mousePosition * 2688f / Screen.height;
                    _joystickHandle.anchoredPosition = Vector2.zero;
                    _oriPos = _joystick.anchoredPosition;

                    DownAction?.Invoke();
                }
                else if (Input.GetMouseButton(0) && !_isButtonClick)
                {
                    _joystickHandle.anchoredPosition = Input.mousePosition * 2688f / Screen.height - _oriPos;
                    if (_joystickHandle.anchoredPosition.magnitude > _joystickBound)
                    {
                        _joystick.anchoredPosition = (Vector2)_oriPos + _joystickHandle.anchoredPosition - _joystickBound * _joystickHandle.anchoredPosition.normalized;
                        _joystickHandle.anchoredPosition = _joystickHandle.anchoredPosition.normalized * _joystickBound;
                        _oriPos = _joystick.anchoredPosition;
                    }

                    if (_joystickHandle.anchoredPosition.magnitude < Threshold)
                        return;

                    JoystickMoveAction?.Invoke(_joystickHandle.anchoredPosition);

                    Vector3 dir = new Vector3(_joystickHandle.anchoredPosition.x, 0, _joystickHandle.anchoredPosition.y);
                    if (MoveObjectRigid != null)
                        Move(dir);
                }
                else if (Input.GetMouseButtonUp(0) && !_isButtonClick)
                {
                    _joystickImage.enabled = false;
                    _joystickHandleImage.enabled = false;
                    UpAction?.Invoke();
                }
                break;
        }
    }

    public void AddDownEvent(System.Action action)
    {
        DownAction -= action;
        DownAction += action;
    }

    public void AddMoveEvent(System.Action<Vector2> action)
    {
        JoystickMoveAction -= action;
        JoystickMoveAction += action;
    }

    public void AddUpEvent(System.Action action)
    {
        UpAction -= action;
        UpAction += action;
    }

    private void Move(Vector3 dir)
    {
        newDir = dir.normalized;

        MoveObjectRigid.rotation = Quaternion.LookRotation(newDir);

        CheckMoveDir();

        if (UseAccelerate)
            _curSpeed = Mathf.Min(_speed, _curSpeed + Time.deltaTime * Accelerate);
        else
            MoveObjectRigid.position += newDir * Time.deltaTime * _speed;
    }

    private void CheckMoveDir()
    {
        bool moveForward = Physics.Raycast(MoveObjectRigid.position + Vector3.forward * Define.BOUND + Vector3.up * 50, Vector3.down, 100, 1 << _walkableMask);
        bool moveBack = Physics.Raycast(MoveObjectRigid.position + Vector3.back * Define.BOUND + Vector3.up * 50, Vector3.down, 100, 1 << _walkableMask);
        bool moveRight = Physics.Raycast(MoveObjectRigid.position + Vector3.right * Define.BOUND + Vector3.up * 50, Vector3.down, 100, 1 << _walkableMask);
        bool moveLeft = Physics.Raycast(MoveObjectRigid.position + Vector3.left * Define.BOUND + Vector3.up * 50, Vector3.down, 100, 1 << _walkableMask);

        if (!moveForward && newDir.z >= 0) newDir.z = 0;
        if (!moveBack && newDir.z < 0) newDir.z = 0;
        if (!moveRight && newDir.x >= 0) newDir.x = 0;
        if (!moveLeft && newDir.x < 0) newDir.x = 0;
    }
}
