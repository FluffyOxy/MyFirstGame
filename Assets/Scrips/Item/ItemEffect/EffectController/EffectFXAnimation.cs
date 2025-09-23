using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFXAnimation: MonoBehaviour
{
    public void OnEffectFXFinish()
    {
        GetComponentInParent<EffectFXControllerBase>().SelfDestory();
    }
}
