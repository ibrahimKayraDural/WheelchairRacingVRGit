using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class ControllerInput : MonoBehaviour
{
    [SerializeField] XRBaseController leftXRController;
    [SerializeField] XRBaseController rightXRController;
    [SerializeField] LayerMask InteractableLayer;

    public InputDevice _rightController;
    public InputDevice _leftController;
    public InputDevice _HMD;

    Transform leftModel = null;
    Transform rightModel = null;

    MeshRenderer leftRenderer = null;
    MeshRenderer rightRenderer = null;

    Vector3 oldLocalPosLeft = Vector3.zero;
    Vector3 oldLocalPosRight = Vector3.zero;

    Vector3 LocalVelocityLeft;
    Vector3 LocalVelocityRight;

    bool leftWasGripped;
    bool rightWasGripped;

    bool rightTriggerWasHeld;
    bool leftTriggerWasHeld;

    GunController rightHoldsGun = null;
    GunController leftHoldsGun = null;

    void Update()
    {
        if (!_rightController.isValid || !_leftController.isValid || !_HMD.isValid)
            InitializeInputDevices();
        if (leftModel == null || rightModel == null)
            InitializeModels();

        bool GripLeft = _leftController.TryGetFeatureValue(CommonUsages.gripButton, out bool boolValue) ? boolValue : false;
        bool GripRight = _rightController.TryGetFeatureValue(CommonUsages.gripButton, out boolValue) ? boolValue : false;
        float TriggerValueLeft = _leftController.TryGetFeatureValue(CommonUsages.trigger, out float floatValue) ? floatValue : 0;
        float TriggerValueRight = _rightController.TryGetFeatureValue(CommonUsages.trigger, out floatValue) ? floatValue : 0;

        LocalVelocityLeft = (leftXRController.transform.localPosition - oldLocalPosLeft) * Time.deltaTime;
        LocalVelocityRight = (rightXRController.transform.localPosition - oldLocalPosRight) * Time.deltaTime;

        oldLocalPosLeft = leftXRController.transform.localPosition;
        oldLocalPosRight = rightXRController.transform.localPosition;

        PlaceholderAnim(GripLeft, GripRight, TriggerValueLeft, TriggerValueRight);

        if (GripLeft)
        {
            if(leftHoldsGun == null)
            {
                Collider[] cols = Physics.OverlapSphere(leftModel.position, leftModel.localScale.x, InteractableLayer);
                WheelController WController = null;

                foreach (Collider col in cols)
                {
                    if (col.gameObject.TryGetComponent(out WheelController outWheelController))
                    {
                        WController = outWheelController;
                        break;
                    }
                    else if (col.gameObject.TryGetComponent(out GunController outGunController))
                    {
                        if (rightHoldsGun != null)
                        {
                            rightHoldsGun.GetUnheld(false);
                            rightHoldsGun = null;
                        }

                        leftHoldsGun = outGunController;
                        leftHoldsGun.GetHeld(leftModel);

                        break;
                    }
                }

                if (WController != null)
                {
                    if (leftWasGripped == false)
                    {
                        _leftController.SendHapticImpulse(0, .1f, .1f);
                        leftWasGripped = true;
                    }

                    WController.PushTheWheel(LocalVelocityLeft);

                    if (TriggerValueLeft > 0)
                    {
                        WController.PushBrake(TriggerValueLeft * Time.deltaTime);
                    }
                }
            }
        }
        else
        {
            if (leftHoldsGun != null) leftHoldsGun.GetUnheld();
            leftHoldsGun = null;
        }

        if (GripRight)
        {
            if (rightHoldsGun == null)
            {

                Collider[] cols = Physics.OverlapSphere(rightModel.position, rightModel.localScale.x, InteractableLayer);
                WheelController WController = null;

                foreach (Collider col in cols)
                {
                    if (col.gameObject.TryGetComponent(out WheelController outController))
                    {
                        WController = outController;
                        break;
                    }
                    else if (col.gameObject.TryGetComponent(out GunController outGunController))
                    {
                        if (leftHoldsGun != null)
                        {
                            leftHoldsGun.GetUnheld(false);
                            leftHoldsGun = null;
                        }

                        rightHoldsGun = outGunController;
                        rightHoldsGun.GetHeld(rightModel);
                        break;
                    }
                }

                if (WController != null)
                {
                    if (rightWasGripped == false)
                    {
                        _rightController.SendHapticImpulse(0, .1f, .1f);
                        rightWasGripped = true;
                    }

                    WController.PushTheWheel(LocalVelocityRight);

                    if (TriggerValueRight > 0)
                    {
                        WController.PushBrake(TriggerValueRight * Time.deltaTime);
                    }
                }
            }
        }
        else
        {
            if (rightHoldsGun != null) rightHoldsGun.GetUnheld();
            rightHoldsGun = null;
        }

        if (GripLeft == false) leftWasGripped = false;
        if (GripRight == false) rightWasGripped = false;
        //if (TriggerValueLeft < .8f) leftTriggerWasHeld = false;
        //if (TriggerValueRight < .8f) rightTriggerWasHeld = false;
    }

    public void GiveHapticFeedback(float amount, float duration)
    {
        _leftController.SendHapticImpulse(0, amount, duration);
        _rightController.SendHapticImpulse(0, amount, duration);
    }

    void PlaceholderAnim(bool GripLeft, bool GripRight, float TriggerValueLeft, float TriggerValueRight)
    {
        leftModel.localScale = Vector3.one * (1 - TriggerValueLeft + .1f) / 10;
        rightModel.localScale = Vector3.one * (1 - TriggerValueRight + .1f) / 10;

        if (leftRenderer != null) leftRenderer.material.color = GripLeft ? Color.red : Color.yellow;
        if (rightRenderer != null) rightRenderer.material.color = GripRight ? Color.red : Color.yellow;
    }

    void InitializeInputDevices()
    {
        if (!_rightController.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref _rightController);
        if (!_leftController.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref _leftController);
        if (!_HMD.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.HeadMounted, ref _HMD);
    }
    void InitializeInputDevice(InputDeviceCharacteristics inputCharacteristics, ref InputDevice inputDevice)
    {
        List<InputDevice> devices = new List<InputDevice>();
        //Call InputDevices to see if it can find any devices with the characteristics we're looking for
        InputDevices.GetDevicesWithCharacteristics(inputCharacteristics, devices);

        //Our hands might not be active and so they will not be generated from the search.
        //We check if any devices are found here to avoid errors.
        if (devices.Count > 0)
        {
            inputDevice = devices[0];
        }
    }
    void InitializeModels()
    {
        leftModel = leftXRController.model;
        rightModel = rightXRController.model;

        if (leftModel.TryGetComponent(out MeshRenderer MR))
        {
            leftRenderer = MR;
            leftRenderer.material.color = Color.yellow;
        }
        if (rightModel.TryGetComponent(out MR))
        {
            rightRenderer = MR;
            rightRenderer.material.color = Color.yellow;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 pos = rightXRController.transform.position;
        Gizmos.DrawLine(pos, pos + LocalVelocityRight * 1000000);
    }
}