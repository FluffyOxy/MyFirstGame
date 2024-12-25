using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DarkScreen : MonoBehaviour
{
    private Animator anim;
    [SerializeField] public float fadeDuration;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void FadeOut()
    {
        anim.SetTrigger("FadeOut");
    }

    public void FadeIn()
    {
        anim.SetTrigger("FadeIn");
    }
}
