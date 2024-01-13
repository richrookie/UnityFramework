using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public JoystickController JoystickController = null;
    public void SetDownAction(System.Action action)
    {
        JoystickController?.AddDownEvent(action);
    }
    public void SetUpAction(System.Action action)
    {
        JoystickController?.AddUpEvent(action);
    }
    public void SetMoveAction(System.Action<Vector2> action)
    {
        JoystickController?.AddMoveEvent(action);
    }

    public void Init()
    {
        Managers.Sound.Play(Managers.Resource.Load<AudioClip>("Bgm"), Define.eSound.Bgm);
    }

    public void Clear()
    {
        if (JoystickController != null)
        {
            JoystickController.DownAction = null;
            JoystickController.UpAction = null;
            JoystickController.JoystickMoveAction = null;
        }
    }
}
