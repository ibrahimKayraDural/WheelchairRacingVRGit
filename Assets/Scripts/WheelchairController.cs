using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class WheelchairController : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;

    [SerializeField] float MaxVelocity = 10f;
    [SerializeField] float BrakePower = 50f;
    [SerializeField] float PushPower = 50f;
    [SerializeField] float TurnPower = 50f;

    public Vector3 StartPos { get; private set; }
    public Quaternion StartRot { get; private set; }

    void Start()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody>();

        StartPos = transform.position;
        StartRot = transform.rotation;
    }

    void Update()
    {
        Vector3 targetVelocity = _rb.velocity;
        targetVelocity.y = 0;
        targetVelocity = targetVelocity.magnitude * transform.forward;
        targetVelocity.y = _rb.velocity.y;
        if (targetVelocity.magnitude > MaxVelocity) targetVelocity = targetVelocity.normalized * MaxVelocity;

        _rb.velocity = targetVelocity;


        if (Input.GetKeyDown(KeyCode.A)) PushChair(100, false);
        if (Input.GetKeyDown(KeyCode.D)) PushChair(100, true);
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = StartPos;
            transform.rotation = StartRot;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }

        //Debug.Log(_rb.angularVelocity);
        
    }

    public void PushChair(float powerFraction, bool fromRight)
    {
        _rb.velocity += transform.forward / 100 * powerFraction * PushPower;

        int direcitonInt = fromRight ? -1 : 1;

        _rb.AddTorque(Vector3.up * direcitonInt * TurnPower * powerFraction);
    }

    public void HoldBrakes(float powerFraction, bool fromLeft)
    {
        Vector3 brakeVelocity = -transform.forward / 100 * powerFraction * BrakePower;
        _rb.velocity += brakeVelocity;
    }
}
