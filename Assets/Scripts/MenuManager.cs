using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public Animator menuAnimator;
    public GameObject skinList;
    public GameObject settings;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickMenu()
    {
        EffectManager.instance.effectSounds[6].source.Play();
        if (!menuAnimator.GetBool("appear"))
            menuAnimator.SetBool("appear", true);
        else
            menuAnimator.SetBool("appear", false);

        if (CubeController.instance.skinGuide.enabled)
            CubeController.instance.skinGuide.enabled = false;
    }
    public void OnClickElse()
    {
        //EffectManager.instance.effectSounds[6].source.Play();
        if (menuAnimator.GetBool("appear"))
            menuAnimator.SetBool("appear", false);
    }

    public void OnClickMenuSkin()
    {
        if (!skinList.activeSelf)
        {
            EffectManager.instance.effectSounds[6].source.Play();
            skinList.SetActive(true);
            settings.SetActive(false);
        }
    }

    public void OnClickMenuSettings()
    {
        if (!settings.activeSelf)
        {
            EffectManager.instance.effectSounds[6].source.Play();
            settings.SetActive(true);
            skinList.SetActive(false);
        }
    }

    public void OpenBgmInfo()
    {
        Application.OpenURL("https://youtube.com/playlist?list=PLgXZWHQUz-k3zxkmX_1MNmYcaZYILzrG3&si=I171AYX6b5x9TVVS");
    }
}
