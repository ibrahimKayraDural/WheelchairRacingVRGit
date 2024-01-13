using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable_Door : Hittable_Base
{
    [SerializeField] Animator doorAnimator;

    bool isHit;

    public override void OnHitted()
    {
        if (isHit) return;

        doorAnimator.SetBool("Open", true);
        UE_OnHit?.Invoke();

        isHit = true;
    }
}
