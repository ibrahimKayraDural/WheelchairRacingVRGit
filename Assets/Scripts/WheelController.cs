using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public interface I_Interactable
{
    public void OnInteracted(InputDevice controllerDevice);
}

public class WheelController : MonoBehaviour, I_Interactable
{
    [SerializeField] WheelchairController WCController;
    [SerializeField] bool isRightWheel;

    Vector3 temp;
    public void OnInteracted(InputDevice controllerDevice)
    {
        if (controllerDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 outVelocity) == false) return;

        outVelocity.y = 0;
        temp = outVelocity;
        float power = outVelocity.magnitude;
        float dot = Vector3.Dot(outVelocity, new Vector3(transform.forward.x, 0, transform.forward.z));

        Debug.Log(dot);
        //Debug.Log(power);

        WCController.PushChair(power, isRightWheel);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(transform.forward.x, 0, transform.forward.z) * 1000);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position +temp * 1000);
    }
}