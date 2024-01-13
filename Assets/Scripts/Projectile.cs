using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float Lifetime = 5;
    [SerializeField] DestroyVFX destroyEffect;

    GameObject owner;
    Rigidbody rb;
    Collider coll;
    bool isInitialized;

    public void Initialize(Vector3 direction, float speed, GameObject owner)
    {
        if (isInitialized) return;

        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();

        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.drag = 0;
        rb.angularDrag = 0;
        rb.velocity = direction * speed;

        coll.isTrigger = true;

         this.owner = owner;

        Invoke(nameof(DestroyProjectile), Lifetime);

        isInitialized = true;
    }

    public void DestroyProjectile()
    {
        if(destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (TryGetComponent(out Shootable_Base shootable)) shootable.OnShot();

        if (other.gameObject == owner) return;

        DestroyProjectile();
    }
}
