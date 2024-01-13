using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shootable_Base : MonoBehaviour
{
    public UnityEvent UE_OnShot;
    public virtual void OnShot() { }
}
