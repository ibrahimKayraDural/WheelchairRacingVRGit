using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NurseTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] NurseController NC;
    [SerializeField] GameObject Song;

    private void OnTriggerEnter(Collider other)
    {
        NC.GOOO();

        Instantiate(Song, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
