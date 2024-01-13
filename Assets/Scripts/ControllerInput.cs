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
    [SerializeField] float triggerHoldTreshold = .8f;
    [SerializeField] float handTriggerSize = .05f;

    public InputDevice _rightController;
    public InputDevice _leftController;
    public InputDevice _HMD;

    Transform leftHoldPos = null;
    Transform rightHoldPos = null;

    Transform leftModel = null;
    Transform rightModel = null;

    Animator leftAnimator = null;
    Animator rightAnimator = null;

    Vector3 oldLocalPosLeft = Vector3.zero;
    Vector3 oldLocalPosRight = Vector3.zero;

    Vector3 LocalVelocityLeft;
    Vector3 LocalVelocityRight;

    bool leftWasGripped;
    bool rightWasGripped;

    bool rightTriggerWasHeld;
    bool leftTriggerWasHeld;

    GunController rightHeldGun = null;
    GunController leftHeldGun = null;

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

        HandAnim(GripLeft, GripRight);

        if (GripLeft)
        {
            if(leftHeldGun == null)
            {
                Collider[] cols = Physics.OverlapSphere(leftModel.position, handTriggerSize, InteractableLayer);
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
                        if (rightHeldGun != null)
                        {
                            rightHeldGun.GetUnheld(false);
                            rightHeldGun = null;
                        }

                        leftHeldGun = outGunController;
                        leftHeldGun.GetHeld(leftHoldPos);

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
            else if(TriggerValueLeft > triggerHoldTreshold && leftTriggerWasHeld == false)
            {
                leftHeldGun.Shoot();
            }
        }
        else
        {
            if (leftHeldGun != null) leftHeldGun.GetUnheld();
            leftHeldGun = null;
        }

        if (GripRight)
        {
            if (rightHeldGun == null)
            {

                Collider[] cols = Physics.OverlapSphere(rightModel.position, handTriggerSize, InteractableLayer);
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
                        if (leftHeldGun != null)
                        {
                            leftHeldGun.GetUnheld(false);
                            leftHeldGun = null;
                        }

                        rightHeldGun = outGunController;
                        rightHeldGun.GetHeld(rightHoldPos);
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
            else if (TriggerValueRight > triggerHoldTreshold && rightTriggerWasHeld == false)
            {
                rightHeldGun.Shoot();
            }
        }
        else
        {
            if (rightHeldGun != null) rightHeldGun.GetUnheld();
            rightHeldGun = null;
        }

        if (GripLeft == false) leftWasGripped = false;
        if (GripRight == false) rightWasGripped = false;
        leftTriggerWasHeld = TriggerValueLeft > triggerHoldTreshold;
        rightTriggerWasHeld = TriggerValueRight > triggerHoldTreshold;
    }

    public void GiveHapticFeedback(float amount, float duration)
    {
        _leftController.SendHapticImpulse(0, amount, duration);
        _rightController.SendHapticImpulse(0, amount, duration);
    }

    void HandAnim(bool GripLeft, bool GripRight)
    {
        if (leftAnimator != null) leftAnimator.SetBool("Close", GripLeft);
        if (rightAnimator != null) rightAnimator.SetBool("Close", GripRight);
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

        if (leftModel.TryGetComponent(out HandAnimatorReferencer reference))
        {
            leftAnimator = reference.AnimatorReference;
            leftHoldPos = reference.HoldPosition;
        }
        if (rightModel.TryGetComponent(out reference))
        {
            rightAnimator = reference.AnimatorReference;
            rightHoldPos = reference.HoldPosition;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(leftModel.transform.position, handTriggerSize);
    }
}