using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable_Quit : Shootable_Base
{
    [SerializeField] float waitDuration = 1.5f;

    bool isShot;

    public override void OnShot()
    {
        if (isShot) return;

        UE_OnShot?.Invoke();

        isShot = true;

        Invoke(nameof(QuitGame), waitDuration);
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
