using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]

public class WheelchairController : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;

    [Header("EditorValueMultiplier will work in editor")]
    [SerializeField] float EditorValueMultiplier = 200f;
    [Header("vvv Values That will be affected by Editor Value Multiplier STARTED vvv")]
    [SerializeField] float BrakePower = 50f;
    [SerializeField] float PushPower = 50f;
    [SerializeField] float TurnPower = 50f;
    [Header("^^^ Values That will be affected by Editor Value Multiplier ENDED ^^^")]
    [SerializeField] float MaxVelocity = 10f;
    [SerializeField] float TurnBrakePower = 0.001f;
    [SerializeField] [Range(0, 1)] float SpeedTurnRedPercent = .8f;
    [SerializeField] AnimationCurve BrakeTurnCurve;
    [SerializeField] AnimationCurve CollisionHapticCurve;
    [SerializeField] TextMeshProUGUI SpeedDisplay;
    [SerializeField] TextMeshProUGUI DebugScreen1;
    [SerializeField] TextMeshProUGUI DebugScreen2;
    [SerializeField] TextMeshProUGUI DebugScreen3;
    [SerializeField] ControllerInput controllerInput;

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

        int direction = Vector3.Dot(transform.forward, targetVelocity) > 0 ? 1 : -1;

        targetVelocity = targetVelocity.magnitude * transform.forward * direction;
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
        DisplaySpeed();
    }

    private void DisplaySpeed()
    {
        int speed = (int)_rb.velocity.magnitude;
        SpeedDisplay.text = speed.ToString();
        Color color = speed > MaxVelocity * SpeedTurnRedPercent ? Color.red : Color.white;
        SpeedDisplay.color = color;
    }

    public void PushChair(float powerFraction, bool fromRight)
    {
        _rb.velocity += transform.forward * powerFraction * PushPower * EditorValueMultiplier;

        int direcitonInt = fromRight ? -1 : 1;

        _rb.AddTorque(Vector3.up * direcitonInt * TurnPower * powerFraction * EditorValueMultiplier);
    }

    public void HoldBrakes(float powerFraction, bool fromRight)
    {
        Vector3 direciton = _rb.velocity.normalized;
        int direcitonInt = fromRight ? 1 : -1;

        Vector3 brakeVelocity = -direciton * powerFraction * BrakePower * EditorValueMultiplier;
        float turnMultiplier = _rb.velocity.magnitude / MaxVelocity;
        turnMultiplier = BrakeTurnCurve.Evaluate(turnMultiplier);

        if (_rb.velocity.magnitude >= brakeVelocity.magnitude)
        {
            _rb.velocity += brakeVelocity;

            _rb.AddTorque(Vector3.up * direcitonInt * TurnPower * powerFraction * EditorValueMultiplier * turnMultiplier * TurnBrakePower);
        }
        else
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);

            //float powerLeft = _rb.velocity.magnitude / brakeVelocity.magnitude;

            //_rb.AddTorque(Vector3.up * direcitonInt * TurnPower * powerFraction * EditorValueMultiplier * powerLeft);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        float speed = collision.relativeVelocity.magnitude;

        Debug(collision.gameObject.name + " " + speed);

        controllerInput.GiveHapticFeedback(CollisionHapticCurve.Evaluate(speed/MaxVelocity), .1f);

        
    }

    int debugIndex = 0;
    void Debug(string msg)
    {
        TextMeshProUGUI mesh = DebugScreen1;

        float alpha = .1f;

        Color temp = DebugScreen1.color;
        temp.a = alpha;
        DebugScreen1.color = temp;
        temp = DebugScreen2.color;
        temp.a = alpha;
        DebugScreen2.color = temp;
        temp = DebugScreen3.color;
        temp.a = alpha;
        DebugScreen3.color = temp;

        switch (debugIndex)
        {
            case 0:
                mesh =DebugScreen1;

                temp = DebugScreen1.color;
                temp.a = 1;
                DebugScreen1.color = temp;

                break;
            case 1:
                mesh= DebugScreen2;

                temp = DebugScreen2.color;
                temp.a = 1;
                DebugScreen2.color = temp;

                break;
            case 2:
                mesh=DebugScreen3;

                temp = DebugScreen3.color;
                temp.a = 1;
                DebugScreen3.color = temp;

                break;
        }

        mesh.text = msg;

        debugIndex = debugIndex + 1 >= 3 ? 0 : debugIndex + 1;
    }
}