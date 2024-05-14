using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable_Destroy : Shootable_Base
{
    public override void OnShot()
    {
        base.OnShot();
        Destroy(gameObject);
    }
}
