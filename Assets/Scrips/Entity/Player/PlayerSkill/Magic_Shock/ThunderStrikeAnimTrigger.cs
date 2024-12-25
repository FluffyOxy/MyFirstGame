using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeAnimTrigger : MonoBehaviour
{
    ThunderStrikeController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInParent<ThunderStrikeController>();
    }

    public void AnimFinishTrigger()
    {
        controller.AnimFinishTrigger();
    }

    public void AnimHitTrigger()
    {
        controller.AnimHitTrigger();
    }
}
