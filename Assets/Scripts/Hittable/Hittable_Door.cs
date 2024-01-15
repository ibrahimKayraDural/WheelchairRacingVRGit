using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable_Door : Hittable_Base
{
    [SerializeField] Animator doorAnimator;
    [SerializeField] GameObject DoorOpenSFX;

    bool isHit;

    public override void OnHitted()
    {
        if (isHit) return;

        doorAnimator.SetBool("Open", true);

        GameObject audio = Instantiate(DoorOpenSFX, transform.position, Quaternion.identity);
        if (Camera.main != null) audio.transform.parent = Camera.main.transform;

        UE_OnHit?.Invoke();

        isHit = true;
    }
}
