using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NurseTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] NurseController NC;
    [SerializeField] GameObject Song;
    [SerializeField] UnityEvent OnTriggered;

    private void OnTriggerEnter(Collider other)
    {
        NC.GOOO();
        Instantiate(Song, transform.position, Quaternion.identity);
        OnTriggered?.Invoke();

        Destroy(gameObject);
    }
}
