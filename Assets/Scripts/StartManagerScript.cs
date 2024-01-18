using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StartManagerScript : MonoBehaviour
{
    [SerializeField] AudioSource TvAudio;
    [SerializeField] Image BlackScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FadeIn(bool fadeToBlack = true, int fadeSpeed = 5) 
    {
        Color objectColor = BlackScreen.GetComponent<Image>().color;
        float fadeAmount;

        if (fadeToBlack)
        {
            while
        }


        yield return new WaitForEndOfFrame();
    }   
}
