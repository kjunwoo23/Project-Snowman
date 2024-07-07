using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    public Animator tutorialAnimator;
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

    public void OnClickMainTutorial()
    {
        MenuManager.instance.OnClickElse();
        if (!tutorialAnimator.GetBool("mainAppear"))
            tutorialAnimator.SetBool("mainAppear", true);
    }

    public void OnClickPushTutorial()
    {
        MenuManager.instance.OnClickElse();
        if (!tutorialAnimator.GetBool("pushAppear"))
            tutorialAnimator.SetBool("pushAppear", true);
    }

    public void OnClickTutorialQuit()
    {
        if (tutorialAnimator.GetBool("mainAppear"))
            tutorialAnimator.SetBool("mainAppear", false);
        if (tutorialAnimator.GetBool("pushAppear"))
            tutorialAnimator.SetBool("pushAppear", false);
    }
}
