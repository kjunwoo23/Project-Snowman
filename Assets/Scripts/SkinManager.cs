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

    public void ChangeSkin(int i)
    {
        cubeMeshes[0].materials = new Material[2] { lit, skins[i].body };
        cubeMeshes[1].materials = new Material[2] { lit, skins[i].head };
    }
}
