using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightSetter : MonoBehaviour
{
    [SerializeField] Transform OffsetTarget;

    float currentHeight;

    void Start()
    {
        if (GamePreferences.Instance != null)
        {
            currentHeight = GamePreferences.Instance.Height;
        }
    }

    public void SetHeight(float setTo)
    {
        currentHeight = setTo;
        if (GamePreferences.Instance != null) GamePreferences.Instance.Height = currentHeight;
        if (OffsetTarget != null) OffsetTarget.position = new Vector3(OffsetTarget.position.x, currentHeight, OffsetTarget.position.z);
    }
}
