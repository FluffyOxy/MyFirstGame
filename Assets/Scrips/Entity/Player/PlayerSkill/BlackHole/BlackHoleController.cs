using System.Collections.Generic;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
    private bool canGrow;
    private float growSpeed;
    private float shrinkSpeed;
    private float maxSize;

    private List<Enemy> enemyTargeted = new List<Enemy>();

    private GameObject hotKeyPrefab;
    private List<KeyCode> hotKeyCodeList = new List<KeyCode>();
    private List<GameObject> usedHotKeyList = new List<GameObject>();
    private float hotKeyLabelYOffset;

    private bool isBeginAttack;
    private float attackCooldown;
    private float attackTimer;
    private int attackAmount;
    private float cloneCreateOffset;

    private float smallDelayAfterAbilityFinish;

    public bool canPlayerExitState;

    private float targetTimer;
    private bool isTargetTimerValid;

    public void SetUp(float _growSpeed, float _maxSize, float _attackCooldown, int _attackAmount, float _cloneCreateOffset, float _shrinkSpeed, float _smallDelayAfterAbilityFinish, float _targetWindow)
    {
        canGrow = true;
        isBeginAttack = false;
        canPlayerExitState = false;
        growSpeed = _growSpeed;
        maxSize = _maxSize;
        attackCooldown = _attackCooldown;
        attackAmount = _attackAmount;
        cloneCreateOffset = _cloneCreateOffset;
        shrinkSpeed = _shrinkSpeed;
        smallDelayAfterAbilityFinish = _smallDelayAfterAbilityFinish;
        targetTimer = _targetWindow;
        isTargetTimerValid = true;
        SceneAudioManager.instance.playerSFX.blackHoleLoop.Play(null);
    }

    public void SetUpHotKey(GameObject _hotKeyPrefab, List<KeyCode> _hotKeyCodeList, float _hotKeyLabelYOffset)
    {
        hotKeyPrefab = _hotKeyPrefab;
        foreach(var key in _hotKeyCodeList)
        {
            hotKeyCodeList.Add(key);
        }
        hotKeyLabelYOffset = _hotKeyLabelYOffset;
    }

    private void Update()
    {
        targetTimer -= Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Space) || targetTimer < 0 && isTargetTimerValid)
        {
            isTargetTimerValid = false;
            isBeginAttack = true;
            DestroyHotKeys();
        }

        if (isBeginAttack)
        {
            List<Enemy> DeadEnemies = new List<Enemy>();
            foreach(var enemy in enemyTargeted)
            {
                if(enemy == null || enemy.isDead)
                {
                    DeadEnemies.Add(enemy);
                }
            }
            foreach (var enemy in DeadEnemies)
            {
                enemyTargeted.Remove(enemy);
            }

            attackTimer -= Time.deltaTime;
            if (attackTimer < 0)
            {
                attackTimer = attackCooldown;
                if(attackAmount <= 0)
                {
                    Invoke("FinishBlackHoleAbility", smallDelayAfterAbilityFinish);
                }
                else if (enemyTargeted.Count <= 0)
                {
                    Invoke("FinishBlackHoleAbility", 0);
                }
                else
                {
                    if(!SkillManager.intance.clone.isUsingCrystalInsteadOfClone())
                    {
                        PlayerManager.instance.player.makeTransprent(true);
                    }
                    int randomIndex = Random.Range(0, enemyTargeted.Count);
                    float xOffset;
                    if(Random.Range(0, 100) > 50)
                    {
                        xOffset = cloneCreateOffset;
                    }
                    else
                    {
                        xOffset = -cloneCreateOffset;
                    }
                    SkillManager.intance.clone.CreateClone(enemyTargeted[randomIndex].transform, new Vector3(xOffset, 0), enemyTargeted[randomIndex]);
                    --attackAmount;
                }
            }
        }

        if(!isBeginAttack && canGrow)
        {
            transform.localScale = 
                Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
            SceneAudioManager.instance.playerSFX.blackHoleLoop.SetVolume(transform.localScale.x / maxSize);
        }

        if(!isBeginAttack && !canGrow)
        {
            transform.localScale = 
                Vector2.Lerp(transform.localScale, new Vector2(0, 0), shrinkSpeed * Time.deltaTime);
            SceneAudioManager.instance.playerSFX.blackHoleLoop.SetVolume(transform.localScale.x / maxSize);
            if (transform.localScale.x < 0.1)
            {
                SceneAudioManager.instance.playerSFX.blackHoleLoop.Stop();
                Destroy(gameObject);
            }
        }
    }

    private void FinishBlackHoleAbility()
    {
        canPlayerExitState = true;
        PlayerManager.instance.player.makeTransprent(false);
        isBeginAttack = false;
        canGrow = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null && !isBeginAttack && canGrow)
        {
            enemy.FreezeTime(true);
            enemy.SetCanBeDamage_Temp(true);
            CreateHotKeyLabel(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null && !isBeginAttack && !canGrow)
        {
            enemy.FreezeTime(false);
            enemy.ResetCanBeDamage_Temp();
        }
    }

    private void CreateHotKeyLabel(Enemy enemy)
    {
        if(hotKeyCodeList.Count <= 0)
        {
            Debug.Log("not enough keyCode to assign");
            return;
        }

        GameObject newHotKey = Instantiate(hotKeyPrefab, enemy.transform.position + new Vector3(0, hotKeyLabelYOffset), Quaternion.identity);
        KeyCode chosenKey = hotKeyCodeList[Random.Range(0, hotKeyCodeList.Count)];
        hotKeyCodeList.Remove(chosenKey);
        usedHotKeyList.Add(newHotKey);
        newHotKey.GetComponent<BlackHole_HotKeyController>().SetupHotKey(chosenKey, this, enemy);
    }

    public void TargetEnemy(Enemy _enemy)
    {
        enemyTargeted.Add(_enemy);
    }

    public void DestroyHotKeys()
    {
        for(int i = 0; i < usedHotKeyList.Count; i++)
        {
            Destroy(usedHotKeyList[i]);
        }
        usedHotKeyList.Clear();
    }
}
