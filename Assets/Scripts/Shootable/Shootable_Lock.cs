using UnityEngine;

public class Shootable_Lock : Shootable_Base
{
    [SerializeField] Animator doorAnimator;
    [SerializeField] GameObject DoorOpenSFX;

    bool isShot;

    public override void OnShot()
    {
        if (isShot) return;

        doorAnimator.SetBool("Open", true);

        GameObject audio = Instantiate(DoorOpenSFX, transform.position, Quaternion.identity);
        if (Camera.main != null) audio.transform.parent = Camera.main.transform;

        UE_OnShot?.Invoke();

        isShot = true;
    }
}
