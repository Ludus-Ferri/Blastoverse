using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCutoutController : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField]
    private Animator anim;
#pragma warning restore CS0649

    public void FadeToBlack()
    {
        anim.SetTrigger("ToBlack");
    }

    public void FadeToVisible()
    {
        anim.SetTrigger("ToVisible");
    }
}
