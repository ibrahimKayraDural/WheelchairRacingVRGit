using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    [SerializeField] AudioSource _AudioSource;
    [SerializeField, Range(0, 1)] float _MaxVolume = 1;
    [SerializeField] bool _StartAtZero;

    void Start()
    {
        if (_StartAtZero) SetVolume(0);
        else if (_AudioSource.volume > _MaxVolume) SetVolume(_MaxVolume);
    }

    public void SetVolume(float setTo)
    {
        setTo = Mathf.Clamp(setTo, 0, 1);
        setTo = Mathf.Lerp(0, _MaxVolume, setTo);
        _AudioSource.volume = setTo;
    }
}
