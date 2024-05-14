using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]

public class WheelchairController : MonoBehaviour
{
    [SerializeField] GunController gc;
    [SerializeField] ControllerInput controllerInput;
    [SerializeField] Transform gunoffsetter;
    [SerializeField] Rigidbody _rb;
    [SerializeField] TextMeshProUGUI SpeedDisplay;
    [SerializeField] TextMeshProUGUI DebugScreen1;
    [SerializeField] TextMeshProUGUI DebugScreen2;
    [SerializeField] TextMeshProUGUI DebugScreen3;
    [SerializeField] Transform RightWheelMesh;
    [SerializeField] Transform LeftWheelMesh;
    [SerializeField] GameObject HitImpactSFX;

    float EditorValueMultiplier = 25f;
    [SerializeField] float BrakePower = 50f;
    [SerializeField] float PushPower = 50f;
    [SerializeField] float TurnPower = 50f;
    [SerializeField] float MaxVelocity = 10f;
    [SerializeField] float TurnBrakePower = 0.001f;
    [SerializeField] [Range(0, 1)] float HittableSpeedPercent = .8f;
    [SerializeField] float wheelMaxTurnSpeed = 1;
    [SerializeField] float impactSFXCooldown = .2f;
    //[SerializeField] AnimationCurve TorqueSpeedCurve;
    [SerializeField] AnimationCurve CollisionHapticCurve;
    [SerializeField] AnimationCurve BackwardsVelocityCurve;

    public Vector3 StartPos { get; private set; }
    public Quaternion StartRot { get; private set; }

    float velocityNormalized => _rb.velocity.magnitude / MaxVelocity;
    bool CanHitInteract => (int)_rb.velocity.magnitude > MaxVelocity * HittableSpeedPercent;

    Transform cameraTransform;
    float HitSFXTargetTime = -1;

    int debugDirection = 1;

    /*
        Hýz kaybetmeyi azalt veya sýfýrla    [OK]
        Tek tekerle itersen sadece dönsün    [OK]
        Yavaþlamak için tekeri geri çeksin   [OK]
     */
    void Start()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody>();

        StartPos = transform.position;
        StartRot = transform.rotation;

        cameraTransform = Camera.main.transform;
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

        if (RightWheelMesh != null && LeftWheelMesh != null)
        {
            Vector3 targetRotation = Vector3.zero;
            targetRotation.y = velocityNormalized * wheelMaxTurnSpeed * Time.deltaTime * -direction;

            RightWheelMesh.Rotate(targetRotation);
            LeftWheelMesh.Rotate(targetRotation);
        }
        //DEGUB START
        if (Input.GetKeyDown(KeyCode.A)) PushChair(0.001f * debugDirection, false);
        if (Input.GetKeyDown(KeyCode.D)) PushChair(0.001f * debugDirection, true);
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = StartPos;
            transform.rotation = StartRot;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            gc.Shoot();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            gc.GetHeld(gunoffsetter);
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            gc.GetUnheld();
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            debugDirection *= -1;
        }
        //DEBUG END

        DisplaySpeed();
    }

    private void DisplaySpeed()
    {
        int speed = (int)_rb.velocity.magnitude;
        SpeedDisplay.text = speed.ToString();
        Color color = CanHitInteract ? Color.red : Color.white;
        SpeedDisplay.color = color;
    }

    public void PushChair(float powerFraction, bool fromRight)
    {
        float backwardsPower = 1;
        if (powerFraction < 0) backwardsPower = BackwardsVelocityCurve.Evaluate(velocityNormalized);

        if (controllerInput.DoesBothHandsHoldWheel)
            _rb.velocity += transform.forward * powerFraction * PushPower * EditorValueMultiplier * backwardsPower;

        CustomDebug((controllerInput.DoesBothHandsHoldWheel ? "T":"F") + ", BP:" + backwardsPower);

        int direcitonInt = fromRight ? -1 : 1;

        _rb.AddTorque(Vector3.up * direcitonInt * TurnPower * powerFraction * EditorValueMultiplier);
    }

    public void HoldBrakes(float powerFraction, bool fromRight)
    {
        Vector3 direciton = _rb.velocity.normalized;
        int direcitonInt = fromRight ? 1 : -1;

        Vector3 brakeVelocity = -direciton * powerFraction * BrakePower * EditorValueMultiplier;

        //float turnMultiplier = _rb.velocity.magnitude / MaxVelocity;
        //turnMultiplier = TorqueSpeedCurve.Evaluate(turnMultiplier);

        if (_rb.velocity.magnitude >= brakeVelocity.magnitude)
        {
            _rb.velocity += brakeVelocity;

            _rb.AddTorque(Vector3.up * direcitonInt * TurnPower * powerFraction * EditorValueMultiplier * TurnBrakePower);
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

        //CustomDebug(collision.gameObject.name + " " + speed);

        controllerInput.GiveHapticFeedback(CollisionHapticCurve.Evaluate(speed/MaxVelocity), .1f);

        if((int)speed > MaxVelocity * HittableSpeedPercent)
        {
            //CustomDebug(collision.gameObject.name);

            if(HitSFXTargetTime < Time.time)
            {
                PlayAudioOneShot(HitImpactSFX);

                HitSFXTargetTime = Time.time + impactSFXCooldown;
            }

            if (collision.gameObject.TryGetComponent(out Hittable_Base hittable))
            {
                hittable.OnHitted();
            }
            else
            {
                Vector3 targetVel = _rb.velocity;
                targetVel /= 2;
                targetVel.y = _rb.velocity.y;
                _rb.velocity = targetVel;
            }
        }
    }

    void PlayAudioOneShot(GameObject audio)
    {
        audio = Instantiate(audio, transform.position, Quaternion.identity);
        if (cameraTransform != null) audio.transform.parent = cameraTransform;
    }

    int debugIndex = 0;
    void CustomDebug(string msg)
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