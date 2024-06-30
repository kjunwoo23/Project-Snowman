using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public Animator menuAnimator;

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
        if (!menuAnimator.GetBool("appear"))
            menuAnimator.SetBool("appear", true);
        else
            menuAnimator.SetBool("appear", false);
    }
    public void OnClickElse()
    {
        menuAnimator.SetBool("appear", false);
    }
}
