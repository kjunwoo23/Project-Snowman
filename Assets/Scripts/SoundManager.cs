using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip clip;
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public Slider slider;
    public AudioSource bgmPlayer;
    public Sound[] bgmSounds;

    float defaultVol;
    Coroutine fadeInOutBgm;
    Coroutine fadeBgm;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        if (PlayerPrefs.HasKey("SoundVolume"))
        {
            bgmPlayer.volume = PlayerPrefs.GetFloat("SoundVolume");
            slider.value = bgmPlayer.volume;
        }
        else
        {
            bgmPlayer.volume = 0.5f;
            PlayerPrefs.SetFloat("SoundVolume", 0.5f);
        }
        //bgmPlayer.clip = bgmSounds[1].clip;
        //bgmPlayer.Play();
        //text.text = bgmPlayer.volume.ToString();
    }


    // Update is called once per frame
    void Update()
    {

    }
    public void SetBgmVolume(Slider slider)
    {
        float value = Mathf.Round(slider.value * 100) * 0.01f;
        bgmPlayer.volume = value;
        PlayerPrefs.SetFloat("SoundVolume", value);
        //text.text = value.ToString();
        //if (bgmPlayer.clip == bgmSounds[3].clip)
        //bgmPlayer.volume = slider.value * 0.5f;
        defaultVol = value;
    }
    public void ChangeBGM(int i)
    {
        bgmPlayer.clip = bgmSounds[i].clip;
        bgmPlayer.time = 0;
        bgmPlayer.Play();
    }
    public void ChangeBGM(int i, float t)
    {
        bgmPlayer.clip = bgmSounds[i].clip;
        bgmPlayer.time = t;
        bgmPlayer.Play();
    }

    public void StartFadeOutBGM(float t)
    {
        if (fadeInOutBgm != null)
            StopCoroutine(fadeInOutBgm);
        fadeInOutBgm = StartCoroutine(FadeOutBGM(t));
    }
    public IEnumerator FadeOutBGM(float t)
    {
        //defaultVol = slider.value;

        while (bgmPlayer.volume > 0)
        {
            bgmPlayer.volume -= Time.deltaTime / t * slider.value;
            yield return null;
        }

        fadeInOutBgm = null;
    }

    public void StartFadeInBGM(float t)
    {
        if (fadeInOutBgm != null)
            StopCoroutine(fadeInOutBgm);
        fadeInOutBgm = StartCoroutine(FadeInBGM(t));
    }
    public IEnumerator FadeInBGM(float t)
    {
        while (bgmPlayer.volume < slider.value)
        {
            bgmPlayer.volume += Time.deltaTime / t * slider.value;
            yield return null;
        }

        fadeInOutBgm = null;
    }

    public void StartChangeBgmSmoothly(int idx, float o, float i)
    {
        if (fadeBgm != null)
            StopCoroutine(fadeBgm);
        if (fadeInOutBgm != null)
            StopCoroutine(fadeInOutBgm);

        fadeBgm = StartCoroutine(ChangeBgmSmoothly(idx, 0, o, i));
    }
    public void StartChangeBgmSmoothly(int idx, float t, float o, float i)
    {
        if (fadeBgm != null)
            StopCoroutine(fadeBgm);
        if (fadeInOutBgm != null)
            StopCoroutine(fadeInOutBgm);

        fadeBgm = StartCoroutine(ChangeBgmSmoothly(idx, t, o, i));
    }

    public IEnumerator ChangeBgmSmoothly(int idx, float t, float o, float i)
    {
        StartFadeOutBGM(o);
        //yield return null;
        yield return fadeInOutBgm;

        ChangeBGM(idx, t);

        StartFadeInBGM(i);
        //yield return null;
        yield return fadeInOutBgm;
        fadeBgm = null;
    }
}
