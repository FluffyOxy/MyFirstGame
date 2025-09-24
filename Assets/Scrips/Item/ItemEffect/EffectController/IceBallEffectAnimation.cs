using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBallEffectAnimation : MonoBehaviour
{
    public void OnBurstFinish()
    {
        GetComponentInParent<IceBallEffectController>().SelfDestory();
    }
}
