using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] Transform GoBackTarget;

    Transform currentTarget;

    void Start()
    {
        currentTarget = GoBackTarget;
    }
    public void GetHeld(Transform holder)
    {
        currentTarget = holder;
    }
    public void GetUnheld(bool ResetPosition = true)
    {
        if (GoBackTarget != null)
        {
            currentTarget = GoBackTarget;
        }
    }

    void Update()
    {
        if(currentTarget != null)
        {
            transform.position = currentTarget.position;
            transform.rotation = currentTarget.rotation;
        }
    }
}
