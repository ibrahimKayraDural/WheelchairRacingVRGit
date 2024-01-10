using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class WheelController : MonoBehaviour
{
    [SerializeField] WheelchairController WCController;
    [SerializeField] bool isRightWheel;

    public void PushTheWheel(Vector3 velocity)
    {
        velocity.y = 0;
        float power = velocity.magnitude;

        Vector3 forwardVector = Vector3.forward;
        forwardVector.y = 0;

        float dot = Vector3.Dot(velocity.normalized, forwardVector);

        //Debug.Log(power * dot);

        WCController.PushChair(power * dot, isRightWheel);
    }
    public void PushBrake(float fractionPower)
    {
        WCController.HoldBrakes(fractionPower, isRightWheel);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(transform.forward.x, 0, transform.forward.z) * 1000);
    }
}