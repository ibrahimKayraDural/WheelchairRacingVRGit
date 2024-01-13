using UnityEngine;
using UnityEngine.Events;

public class Hittable_Base : MonoBehaviour
{
    public UnityEvent UE_OnHit;
    public virtual void OnHitted() { }
}
