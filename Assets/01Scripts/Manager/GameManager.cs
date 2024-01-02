using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void Init()
    {
        Managers.Sound.Play(Managers.Resource.Load<AudioClip>("Bgm"), Define.eSound.Bgm);
    }
}
