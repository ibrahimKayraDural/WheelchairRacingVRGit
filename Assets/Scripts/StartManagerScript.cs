using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StartManagerScript : MonoBehaviour
{
    [SerializeField] AudioSource TvAudio;
    [SerializeField] Image BlackScreen;
    [SerializeField] AnimationCurve soundCurve;

    // Start is called before the first frame update
    void Start()
    {
        TvAudio.volume = 0;
        StartCoroutine(SoundIn());
        Invoke("fadeInStarter", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void fadeInStarter()
    {

        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn(bool fadeToBlack = true, int fadeSpeed = 2) 
    {
        Color objectColor = BlackScreen.GetComponent<Image>().color;
        float fadeAmount;

        if (fadeToBlack)
        {
            while (BlackScreen.GetComponent<Image>().color.a>0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                BlackScreen.GetComponent<Image>().color = objectColor;

                yield return new WaitForEndOfFrame();
            }
        }
    }   

    public void TvSoundStop()
    {
        TvAudio.Stop();
    }

    public IEnumerator SoundIn(bool FadeInSound = true, float fadeSpeed = 0.6f)
    {
        float soundVolume = TvAudio.volume;
        float fadeAmount;

        if (FadeInSound)
        {
            while (TvAudio.volume < 0.5f/*BlackScreen.GetComponent<Image>().color.a > 0*/)
            {
                fadeAmount = soundVolume + (fadeSpeed * Time.deltaTime);

                soundVolume = fadeAmount;
                TvAudio.volume = soundVolume;

                yield return new WaitForEndOfFrame();
            }
            Invoke("TvSoundStop", 0.2f);
        }
    }

}
