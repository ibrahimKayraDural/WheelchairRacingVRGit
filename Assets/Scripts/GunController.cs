using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] LineRenderer laserRenderer;
    [SerializeField] Transform GoBackTarget;
    [SerializeField] Transform Barrel;
    [SerializeField] LayerMask ShootLayers;
    [SerializeField] float shootCooldown = 0;
    [SerializeField] float rayLength = 100;
    [SerializeField] float rayRadius = 1;

    //[SerializeField] float speed = 1;
    //[SerializeField] GameObject Projectile;

    Transform currentTarget;
    float nextShootTargetTime = -1;
    bool isHeld;

    void Start()
    {
        currentTarget = GoBackTarget;
        laserRenderer.startWidth = rayRadius * 2;
        laserRenderer.endWidth = rayRadius * 2;
    }
    void Update()
    {
        if (currentTarget != null)
        {
            transform.position = currentTarget.position;
            transform.rotation = currentTarget.rotation;
        }

        if(isHeld)
        {
            laserRenderer.SetPosition(0, Barrel.position);
            laserRenderer.SetPosition(1, Barrel.position + transform.forward * rayLength);
        }
    }

    public void GetHeld(Transform holder)
    {
        currentTarget = holder;
        isHeld = true;
        laserRenderer.enabled = true;
    }
    public void GetUnheld(bool ResetPosition = true)
    {
        if (GoBackTarget != null)
        {
            currentTarget = GoBackTarget;
        }

        isHeld = false;
        laserRenderer.enabled = false;
    }
    public void Shoot()
    {
        if (nextShootTargetTime > Time.time) return;
        if (Barrel == null) return;

        Vector3 dir = transform.forward;

        RaycastHit hitInfo;

        //Ray ray = new Ray(Barrel.position, dir);
        //Physics.Raycast(ray, out hitInfo, rayLength, ShootLayers)
        if (Physics.SphereCast(Barrel.position, rayRadius, dir, out hitInfo, rayLength, ShootLayers))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out Shootable_Base shootable)) shootable.OnShot();
            laserRenderer.SetPosition(1, hitInfo.transform.position);
        }


        StartCoroutine(LaserBlink());
        /*
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
        */

        nextShootTargetTime = Time.time + shootCooldown;
    }

    IEnumerator LaserBlink()
    {
        laserRenderer.enabled = false;

        yield return new WaitForSeconds(.1f);

        if (isHeld) laserRenderer.enabled = true;
    }
}
