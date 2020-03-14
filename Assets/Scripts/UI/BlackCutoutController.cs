using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCutoutController : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    public void FadeToBlack()
    {
        anim.SetTrigger("ToBlack");
    }

    public void FadeToVisible()
    {
        anim.SetTrigger("ToVisible");
    }
}
