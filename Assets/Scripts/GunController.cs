using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] Transform GoBackTarget;
    [SerializeField] Transform Barrel;
    [SerializeField] GameObject Projectile;
    [SerializeField] float shootCooldown = 0;
    [SerializeField] float speed = 1;

    Transform currentTarget;

    float nextShootTargetTime = -1;

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
    public void Shoot()
    {
        if (nextShootTargetTime > Time.time) return;
        if (Barrel == null || Projectile == null) return;

        Vector3 dir = transform.forward;
        Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
        GameObject instantiatedBullet = Instantiate(Projectile, Barrel.position, rotation);

        if(instantiatedBullet.TryGetComponent(out Projectile projectile))
        {
            projectile.Initialize(dir, speed, gameObject);
        }
        else
        {
            Destroy(instantiatedBullet);
        }

        nextShootTargetTime = Time.time + shootCooldown;
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
