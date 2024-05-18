using System.Collections;
using System.Collections.Generic;
using UnityEngine.Splines;
using UnityEngine;

public class KullanatTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    bool tisSlowingTime;
    float currentTime;
    Collider wc;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //currentTime += Time.deltaTime;
        //wc.gameObject.GetComponent<SplineAnimate>().ElapsedTime = currentTime;
        //if (tisSlowingTime && wc != null)
        //{
        //    for (int i = 0; i < 100; i++)
        //    {
        //        wc.gameObject.GetComponent<SplineAnimate>().MaxSpeed -= 0.003f;
        //        Debug.Log(wc.gameObject.GetComponent<SplineAnimate>().MaxSpeed);
        //    }
        //    tisSlowingTime = false;
        //}
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<SplineAnimate>() != null)
        {
            collision.gameObject.GetComponent<NurseController>().slowNurse();

            
            //this.gameObject.GetComponent<BoxCollider>().enabled = false;

            //Debug.Log(collision.gameObject.GetComponent<NurseController>().name);

        }
    }
}
