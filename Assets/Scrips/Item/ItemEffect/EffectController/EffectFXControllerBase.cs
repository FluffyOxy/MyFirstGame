using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFXControllerBase : MonoBehaviour
{
    public virtual void SelfDestory()
    {
        Destroy(gameObject);
    }

    public virtual void PlayFX(EffectExcuteData _targetData)
    {

    }
}
