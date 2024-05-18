using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable_Start : Shootable_Base
{
    [SerializeField] int levelToOpenIndex;

    [SerializeField] GameObject DoorOpenSFX;
    [SerializeField] LevelManager Lmanager;

    [SerializeField] float waitDuration = 1.5f;

    bool isShot;

    public override void OnShot()
    {
        if (isShot) return;

        GameObject audio = Instantiate(DoorOpenSFX, transform.position, Quaternion.identity);
        if (Camera.main != null) audio.transform.parent = Camera.main.transform;

        UE_OnShot?.Invoke();

        isShot = true;

        Invoke(nameof(GoToLevel), waitDuration);
    }

    void GoToLevel()
    {
        Lmanager.GoToLevel(levelToOpenIndex);
    }
}
