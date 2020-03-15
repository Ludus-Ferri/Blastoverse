using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopHUDController : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Show()
    {
        anim.SetTrigger("ShowTop");
    }

    public void Hide()
    {
        anim.SetTrigger("HideTop");
    }
}
