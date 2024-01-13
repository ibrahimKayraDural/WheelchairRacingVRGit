using System.Collections;
using System.Collections.Generic;
using UnityEngine.Splines;
using UnityEngine;

public class NurseController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SplineAnimate SplineAnimRef;
    [SerializeField] float speed = 1;
    float currentTime;

    [SerializeField] float DefaultSpeed = 1;
    [SerializeField] float slowTime = 3;
    float slowTargetTime;
    bool isSlowed = false;
    [SerializeField] float slowAmount = 0.5f;

    bool slowToggle = true;
    void Start()
    {
        SplineAnimRef = gameObject.GetComponent<SplineAnimate>();
    }

    // Update is called once per frame
    void Update()
    {


        currentTime += Time.deltaTime * speed;
        SplineAnimRef.ElapsedTime = currentTime;


        if (isSlowed)
        {
            if (slowTargetTime > Time.time && slowToggle)
            {
                speed *= slowAmount;
                
                slowToggle = false;
            }
            else if (slowTargetTime < Time.time)
            {
                speed = DefaultSpeed;
                isSlowed = false;
                slowToggle = true;
                Debug.Log(speed);
            }
        }
    }

    public void slowNurse()
    {
        isSlowed = true;

        slowTargetTime = Time.time + slowTime;
        Debug.Log(Time.time);
        Debug.Log(slowTargetTime);
    }

    IEnumerator slowTimer()
    {
        while (!isSlowed)
        {


            yield return new WaitForSeconds(slowTime);
        }
    }
}