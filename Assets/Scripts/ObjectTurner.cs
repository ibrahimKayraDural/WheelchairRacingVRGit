using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTurner : MonoBehaviour
{
    [SerializeField] Transform _Object;
    [SerializeField] Vector3 _RotationVector = new Vector3(0, 10, 0);
    [SerializeField] bool _StartEnabled = true;

    bool _turnIsEnabled;

    void Start()
    {
        _turnIsEnabled = _StartEnabled;
    }
    void Update()
    {
        if(_turnIsEnabled)
        {
            _Object.Rotate(_RotationVector * Time.deltaTime * 10);
        }
    }

    public void SetIsEnabled(bool setTo) => _turnIsEnabled = setTo;
}
