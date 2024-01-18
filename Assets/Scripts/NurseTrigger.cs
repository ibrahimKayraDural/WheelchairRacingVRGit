using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NurseTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] NurseController NC;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        NC.GOOO();
        Destroy(gameObject);
    }
}
