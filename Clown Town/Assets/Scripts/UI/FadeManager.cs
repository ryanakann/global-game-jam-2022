using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : Singleton<FadeManager>
{

    public GameEvent FadeComplete, UnfadeComplete;

    [HideInInspector]
    public bool fading { get { return !(anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0)); } }

    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Fade(bool fade)
    {
        anim.SetBool("Fade", fade);
    }
}
