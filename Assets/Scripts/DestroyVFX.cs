using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyVFX : MonoBehaviour
{
    [SerializeField] float lifetime = 1;

    void Start()
    {
        Invoke(nameof(DestroyFX), lifetime);
    }

    void DestroyFX()
    {
        Destroy(gameObject);
    }
}
