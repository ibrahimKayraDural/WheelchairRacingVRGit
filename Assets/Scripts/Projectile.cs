using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [SerializeField] string HitTag = "Shootable";

    Rigidbody rb;
    Collider coll;
    bool isInitialized;

    public void Initialize(Vector3 direction, float speed)
    {
        if (isInitialized) return;

        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();

        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.drag = 0;
        rb.angularDrag = 0;
        rb.velocity = direction;

        coll.isTrigger = true;

        isInitialized = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != HitTag) return;
        if (TryGetComponent(out Shootable_Base shootable) == false) return;

        shootable.OnShot();

        Destroy(gameObject);
    }
}
