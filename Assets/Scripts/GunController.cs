using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] GameObject GunShootSFX;
    [SerializeField] GameObject GunHoldSFX;
    [SerializeField] GameObject GunReleaseSFX;
    [SerializeField] LineRenderer LaserRenderer;
    [SerializeField] Transform GoBackTarget;
    [SerializeField] Transform Barrel;
    [SerializeField] LayerMask ShootLayers;
    [SerializeField] float shootCooldown = 0;
    [SerializeField] float rayLength = 100;
    [SerializeField] float rayRadius = 1;

    //[SerializeField] float speed = 1;
    //[SerializeField] GameObject Projectile;

    Transform cameraTransform;
    Transform currentTarget;
    float nextShootTargetTime = -1;
    bool isHeld;

    void Start()
    {
        currentTarget = GoBackTarget;
        LaserRenderer.startWidth = rayRadius * 2;
        LaserRenderer.endWidth = rayRadius * 2;

        cameraTransform = Camera.main.transform;
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
            LaserRenderer.SetPosition(0, Barrel.position);
            LaserRenderer.SetPosition(1, Barrel.position + transform.forward * rayLength);
        }
    }

    public void GetHeld(Transform holder)
    {
        currentTarget = holder;
        isHeld = true;
        LaserRenderer.enabled = true;
        PlayAudioOneShot(GunHoldSFX);
    }
    public void GetUnheld(bool ResetPosition = true)
    {
        if (GoBackTarget != null)
        {
            currentTarget = GoBackTarget;
        }

        isHeld = false;
        LaserRenderer.enabled = false;
        PlayAudioOneShot(GunReleaseSFX);
    }
    public void Shoot()
    {
        if (nextShootTargetTime > Time.time) return;
        if (Barrel == null) return;

        Vector3 dir = transform.forward;

        RaycastHit hitInfo;
        if (Physics.SphereCast(Barrel.position, rayRadius, dir, out hitInfo, rayLength, ShootLayers))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out Shootable_Base shootable)) shootable.OnShot();
            LaserRenderer.SetPosition(1, hitInfo.transform.position);
        }

        PlayAudioOneShot(GunShootSFX);

        StartCoroutine(LaserBlink());

        /*
        Ray ray = new Ray(Barrel.position, dir);
        Physics.Raycast(ray, out hitInfo, rayLength, ShootLayers)

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

    void PlayAudioOneShot(GameObject audio)
    {
        audio = Instantiate(audio, Barrel.position, Quaternion.identity);
        if (cameraTransform != null) audio.transform.parent = cameraTransform;
    }

    IEnumerator LaserBlink()
    {
        LaserRenderer.enabled = false;

        yield return new WaitForSeconds(.05f);

        if (isHeld) LaserRenderer.enabled = true;
    }
}
