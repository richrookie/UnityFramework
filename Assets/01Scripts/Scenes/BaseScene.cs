using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.eScene SceneType { get; protected set; } = Define.eScene.UnKnown;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
    }
}
