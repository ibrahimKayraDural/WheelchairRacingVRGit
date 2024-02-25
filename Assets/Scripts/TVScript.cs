using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TVScript : MonoBehaviour
{
    [SerializeField] TextMeshPro TVText;
    [SerializeField] AudioSource TVAudio;
    public void WakeUp()
    {
        TVText.text = "Wake Up";
        TVAudio.Play();
    }
    public void Aproach()
    {
        TVText.text = "Aproach";
        TVAudio.Play();
    }
    public void PickUp()
    {
        TVText.text = "Pick Up";
        TVAudio.Play();
    }
    public void RunAway()
    {
        TVText.text = "Run Away";
        TVAudio.Play();
    }
}
