using UnityEngine;

public class Shootable_Lock : Shootable_Base
{
    [SerializeField] Animator doorAnimator;

    bool isShot;

    public override void OnShot()
    {
        if (isShot) return;

        doorAnimator.SetBool("Open", true);
        UE_OnShot?.Invoke();

        isShot = true;
    }
}
