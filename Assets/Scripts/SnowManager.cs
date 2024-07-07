using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SnowManager : MonoBehaviour
{
    public static SnowManager instance;

    public TextMeshProUGUI snowText;

    public float curSnow;
    public float addSnowAmountPerSec;
    public float addSnowAmountPerClear;

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
        snowText.text = Mathf.Round(curSnow).ToString(); 
    }

    // Update is called once per frame
    void Update()
    {
        sec += Time.deltaTime;
        if (sec >= 1)
        {
            GetSnow(addSnowAmountPerSec);
            sec = 0;
        }

        
    }
    public void GetSnow(float snow)
    {
        curSnow += snow;
        snowText.text = Mathf.Round(curSnow).ToString();
        PlayerPrefs.SetFloat("Snow", curSnow);
    }
    public void UseSnow(float snow)
    {
        if (curSnow < snow) return;
        curSnow -= snow;
        snowText.text = Mathf.Round(curSnow).ToString();
        PlayerPrefs.SetFloat("Snow", curSnow);
    }
}
