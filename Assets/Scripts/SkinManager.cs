using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skin
{
    public Material body;
    public Material head;
}

public class SkinManager : MonoBehaviour
{
    public static SkinManager instance;
    public Skin[] skins;
    public Material lit;

    public MeshRenderer[] cubeMeshes;
    int curSkin;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
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
    }
}
