using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneCameraManager : MonoBehaviour
{
    public static MainSceneCameraManager instance;

    [HideInInspector] public CinemachineVirtualCamera cam;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }
}
