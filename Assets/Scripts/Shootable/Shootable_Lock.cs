using UnityEngine;

public class Shootable_Lock : Shootable_Base
{
    bool isShot;

    public override void OnShot()
    {
        if (isShot) return;



        isShot = true;
    }
}
