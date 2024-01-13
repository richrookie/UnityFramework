using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void Init()
    {
        Managers.Sound.Play(Managers.Resource.Load<AudioClip>("Bgm"), Define.eSound.Bgm);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Managers.Data.UseSound = !Managers.Data.UseSound;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Managers.Sound.Play("Click");
        }
    }
}
