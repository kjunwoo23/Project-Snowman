using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SnowManager : MonoBehaviour
{
    public static SnowManager instance;

    public TextMeshProUGUI snowText;
    public Animator tryBuyUIAnimator;
    public TextMeshProUGUI tryBuyText;

    public float curSnow;
    public float addSnowAmountPerSec;
    public float addSnowAmountPerClear;

    public TextMeshProUGUI totalSnowBonusText;
    public float skinTotalSnowBonus;
    public float centerBonus;

    float sec;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Snow"))
            curSnow = PlayerPrefs.GetFloat("Snow");
        if (curSnow < 1000 * 10)
            snowText.text = Mathf.Round(curSnow).ToString();
        else if (curSnow < 1000 * 1000 * 10)
            snowText.text = Mathf.Floor(curSnow / 1000).ToString() + "K";
        else
            snowText.text = Mathf.Floor(curSnow / 1000 / 1000).ToString() + "M";
        PlayerPrefs.SetFloat("Snow", curSnow);
        SkinManager.instance.RecalculateSnowBonus();
        //curSnow = 0;
    }

    // Update is called once per frame
    void Update()
    {
        sec += Time.deltaTime;
        if (sec >= 1)
        {
            GetSnow(addSnowAmountPerSec * skinTotalSnowBonus);
            sec = 0;
        }

        
    }
    public void GetSnow(float snow)
    {
        curSnow += snow;
        if (curSnow < 1000 * 10)
            snowText.text = Mathf.Round(curSnow).ToString();
        else if (curSnow < 1000 * 1000 * 10)
            snowText.text = Mathf.Floor(curSnow / 1000).ToString() + "K";
        else
            snowText.text = Mathf.Floor(curSnow / 1000 / 1000).ToString() + "M";
        PlayerPrefs.SetFloat("Snow", curSnow);
        PlayerPrefs.SetFloat("Snow", curSnow);
    }
    public void UseSnow(float snow)
    {
        if (curSnow < snow) return;
        curSnow -= snow;
        if (curSnow < 1000 * 10)
            snowText.text = Mathf.Round(curSnow).ToString();
        else if (curSnow < 1000 * 1000 * 10)
            snowText.text = Mathf.Floor(curSnow / 1000).ToString() + "K";
        else
            snowText.text = Mathf.Floor(curSnow / 1000 / 1000).ToString() + "M";
        PlayerPrefs.SetFloat("Snow", curSnow);
    }
    public void ShowTotalSnowBonus()
    {
        if (skinTotalSnowBonus < 1000 * 10)
            totalSnowBonusText.text = "x" + Mathf.Round(skinTotalSnowBonus).ToString() + "ก่";
        else
            totalSnowBonusText.text = "x" + Mathf.Floor(skinTotalSnowBonus / 1000).ToString() + "K" + "ก่";
    }
}
