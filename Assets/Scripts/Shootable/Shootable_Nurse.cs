using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable_Nurse : Shootable_Base
{
    [SerializeField] GameObject OneShot_Rico;

    public override void OnShot()
    {
        base.OnShot();

        Instantiate(OneShot_Rico).transform.position = transform.position;
    }
}
