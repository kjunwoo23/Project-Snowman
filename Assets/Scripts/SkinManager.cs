using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Skin
{
    public bool unLocked;
    public string skinName;
    public int price;
    public string description;
    public string option;
    public float snowBonus;
    public bool centerBonus;
    public Material body;
    public Material head;
    public TextMeshProUGUI priceText;
    public AudioClip music;
}

public class SkinManager : MonoBehaviour
{
    public static SkinManager instance;
    public Skin[] skins;
    public Material lit;

    public MeshRenderer[] cubeMeshes;
    int curSkin;

    int choosedBuySkin;

    public Animator skinChangeAnimator;
    public TextMeshProUGUI skinChangeText;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < skins.Length; i++)
        {
            if (PlayerPrefs.HasKey("Skin" + i.ToString() + "Locked"))
                if (PlayerPrefs.GetInt("Skin" + i.ToString() + "Locked") == 1)
                    skins[i].unLocked = true;
            if (skins[i].price < 1000 * 10)
                skins[i].priceText.text = skins[i].price.ToString();
            else if (skins[i].price < 1000 * 1000 * 10)
                skins[i].priceText.text = (skins[i].price / 1000).ToString() + "K";
            else
                skins[i].priceText.text = (skins[i].price / 1000 / 1000).ToString() + "M";
        }
        if (PlayerPrefs.HasKey("Skin"))
            curSkin = PlayerPrefs.GetInt("Skin");
        ChangeSkin(curSkin);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeSkin(int i)
    {
        curSkin = i;
        PlayerPrefs.SetInt("Skin", curSkin);
        cubeMeshes[0].materials = new Material[2] { lit, skins[i].body };
        cubeMeshes[1].materials = new Material[2] { lit, skins[i].head };
        SoundManager.instance.bgmPlayer.clip = skins[i].music;
        SoundManager.instance.bgmPlayer.Play();
    }
    public void TryBuySkin(int i)
    {
        if (skins[i].unLocked)
        {
            EffectManager.instance.effectSounds[7].source.Play();
            skinChangeText.text = "<" + skins[i].skinName + ">\n<size=50>" + skins[i].description + "</size>\n<size=30>" + skins[i].option + skins[i].snowBonus.ToString() + "</size>";
            skinChangeAnimator.SetTrigger("changed");
            ChangeSkin(i);
            return;
        }
        EffectManager.instance.effectSounds[6].source.Play();
        SnowManager.instance.tryBuyText.text = "<size=7>해당 스킨을 구매하시겠습니까? (" + skins[i].price + " 스노우)</size>\n스킨명: " + skins[i].skinName +
            "\n<size=9>특이사항: " + skins[i].description + "</size>\n<size=7>" + skins[i].option + skins[i].snowBonus.ToString() + "</size>";

        SnowManager.instance.tryBuyUIAnimator.SetBool("tryBuy", true);
        choosedBuySkin = i;
    }
    public void CancelTryBuySkin()
    {
        EffectManager.instance.effectSounds[6].source.Play();
        SnowManager.instance.tryBuyUIAnimator.SetBool("tryBuy", false);
    }
    public void UnlockSkin(int i)
    {
        EffectManager.instance.effectSounds[8].source.Play();
        SnowManager.instance.curSnow -= skins[i].price;
        PlayerPrefs.SetInt("Skin" + i.ToString() + "Locked", 1);
        skins[i].unLocked = true;
        RecalculateSnowBonus();
        SnowManager.instance.tryBuyUIAnimator.SetBool("tryBuy", false);
        skinChangeText.text = "<" + skins[i].skinName + ">\n<size=50>" + skins[i].description + "</size>\n<size=30>" + skins[i].option + skins[i].snowBonus.ToString() + "</size>";
        skinChangeAnimator.SetTrigger("changed");
        ChangeSkin(i);
    }
    public void OnClickBuyYes()
    {
        if (SnowManager.instance.curSnow < skins[choosedBuySkin].price)
        {
            EffectManager.instance.effectSounds[9].source.Play();
            OnClickBuyNo();
        }
        else
            UnlockSkin(choosedBuySkin);
    }
    public void OnClickBuyNo()
    {
        EffectManager.instance.effectSounds[6].source.Play();
        SnowManager.instance.tryBuyUIAnimator.SetBool("tryBuy", false);
    }

    public void RecalculateSnowBonus()
    {
        float tmp = 1, tmp2 = 1;
        for (int i = 0; i < skins.Length; i++)
            if (skins[i].unLocked)
                if (!skins[i].centerBonus)
                    tmp *= skins[i].snowBonus;
                else
                    tmp2 *= skins[i].snowBonus;
        SnowManager.instance.skinTotalSnowBonus = tmp;
        SnowManager.instance.centerBonus = tmp2;
        SnowManager.instance.ShowTotalSnowBonus();
    }
}
