using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IStageEntity
{
    public void StageChangeFinish();
}

public class StageController : MonoBehaviour
{
    [SerializeField] public Transform bossInitTransform;
    [SerializeField] public Entity bossEntity;
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private float previewBossDuration;
    [SerializeField] private float orthoSize;
    private float defaultOrthoSize;

    [SerializeField] private List<BoxCollider2D> airWalls;
    [SerializeField] private BoxCollider2D airWall_Player;

    [SerializeField] private bool isShowBoss;

    [SerializeField] private Slider bossHealth;
    void Start()
    {
        DisableAirWalls();
        airWall_Player.enabled = false;
        bossHealth.gameObject.SetActive(false);
    }

    private void Update()
    {
        bossHealth.value = bossEntity.cs.getCurrentHealthValue() / bossEntity.cs.GetStatByType(StatType.MaxHealth);
    }

    public void ActivateAirWalls()
    {
        foreach(var wall in airWalls)
        {
            wall.enabled = true;
        }
    }
    public void DisableAirWalls()
    {
        foreach (var wall in airWalls)
        {
            wall.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            (bossEntity as IStageEntity).StageChangeFinish();
            bossHealth.GetComponentInChildren<TextMeshProUGUI>().text = bossEntity.entityName;

            defaultOrthoSize = MainSceneCameraManager.instance.cam.m_Lens.OrthographicSize;
            GetComponent<BoxCollider2D>().enabled = false;

            if(isShowBoss)
            {
                MainSceneCameraManager.instance.cam.Follow = bossInitTransform;
                Invoke("LookAtBattleField", previewBossDuration);
                airWall_Player.enabled = true;
            }
            else
            {
                MainSceneCameraManager.instance.cam.m_Lens.OrthographicSize = orthoSize;
                MainSceneCameraManager.instance.cam.Follow = cameraPosition;
                airWall_Player.enabled = false;
                bossHealth.gameObject.SetActive(true);
            }

            ActivateAirWalls();
        }
    }
    private void LookAtBattleField()
    {
        MainSceneCameraManager.instance.cam.Follow = cameraPosition;
        MainSceneCameraManager.instance.cam.m_Lens.OrthographicSize = orthoSize;
        airWall_Player.enabled = false;
        bossHealth.gameObject.SetActive(true);
    }

    public void StageExit()
    {
        MainSceneCameraManager.instance.cam.m_Lens.OrthographicSize = defaultOrthoSize;
        MainSceneCameraManager.instance.cam.Follow = PlayerManager.instance.player.transform;
        DisableAirWalls();
        bossHealth.gameObject.SetActive(false);
    }
}
