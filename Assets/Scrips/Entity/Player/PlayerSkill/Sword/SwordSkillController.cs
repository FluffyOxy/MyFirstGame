using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rg;
    private CircleCollider2D col;
    private Player player;

    private bool canRotate;
    private bool isReturning;
    private float returnSpeed;
    private float freezeTime;
    private float damageRate;

    [SerializeField] ParticleSystem swordHitFx;

    #region Bounce
    private bool isBouncing = false;
    private int bounceTime;
    private List<Enemy> bounceTarget = new List<Enemy>();
    private int targetIndex;
    private float bouncingRadius;
    private float bouncingSpeed;
    #endregion

    #region Pierce
    private bool isPierce = false;
    private int pierceTime;
    #endregion

    #region Spin
    private bool isSpin = false;
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private float spinDamageCooldown;
    private float spinDamageCoolDownTimer;
    [SerializeField] float swordDamageRadius;
    private float spinDir;
    private float spinMoveForwardSpeed;
    private float spinMoveForwardDistance;
    #endregion

    private float clearSwordDistance;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rg = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();

        canRotate = true;
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.right = rg.velocity;
        }

        if (isReturning)
        {
            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
            if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 ||viewportPosition.y > 1)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);
                float greatSpeed = 10;
                while(distance > greatSpeed * 10)
                {
                    greatSpeed *= 10;
                }

                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * greatSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            }

            if (Vector2.Distance(transform.position, player.transform.position) < clearSwordDistance)
            {
                player.ClearTheThrownSword();
            }
        }
        else if (isBouncing && bounceTarget.Count > 0)
        {
            if (bounceTime > 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, bounceTarget[targetIndex].transform.position, bouncingSpeed * Time.deltaTime);
                float swordHitDistance = 0.1f;
                if (Vector2.Distance(transform.position, bounceTarget[targetIndex].transform.position) < swordHitDistance)
                {
                    SwordSkillDamage(bounceTarget[targetIndex]);
                    ++targetIndex;
                    --bounceTime;
                    if (targetIndex >= bounceTarget.Count)
                    {
                        targetIndex = 0;
                    }
                }
            }
            else
            {
                isBouncing = false;
                isReturning = true;
            }
        }
        else if(isSpin)
        {
            if(Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                SpinBegin();
            }
            if(wasStopped)
            {
                spinTimer -= Time.deltaTime;
                spinDamageCoolDownTimer -= Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinMoveForwardDistance, transform.position.y), spinMoveForwardSpeed * Time.deltaTime);
                if (spinTimer < 0)
                {
                    isReturning = true;
                }
                if (spinDamageCoolDownTimer < 0)
                {
                    spinDamageCoolDownTimer = spinDamageCooldown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, swordDamageRadius);
                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                        }
                    }
                }
            }
        }
    }

    public void SetUp(Vector2 _launchDir, float _gravity, float _returnSpeed, float _clearDistance, float _freezeTime, float _damageRate)
    {
        rg.velocity = _launchDir;
        rg.gravityScale = _gravity;
        player = PlayerManager.instance.player;
        returnSpeed = _returnSpeed;
        clearSwordDistance = _clearDistance;
        freezeTime = _freezeTime;
        damageRate = _damageRate;
        if (!isPierce)
        {
            anim.SetBool("Rotation", true);
        }

        spinDir = Mathf.Clamp(_launchDir.x, -1, 1);
    }

    public void SetUpBounce(int _bounceTime, float _bouncingRadius, float _bouncingSpeed)
    {
        isBouncing = true;
        bounceTime = _bounceTime;
        bouncingRadius = _bouncingRadius;
        bouncingSpeed = _bouncingSpeed;
    }

    public void SetUpPierce(int _pierceTime)
    {
        isPierce = true;
        pierceTime = _pierceTime;
    }

    public void SetUpSpin(float _maxTravelDistance, float _spinDuration, float _spinDamageRate, float _spinMoveForwardSpeed, float _spinMoveForwardDistance)
    {
        isSpin = true;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        spinDamageCooldown = _spinDamageRate;
        spinMoveForwardSpeed = _spinMoveForwardSpeed;
        spinMoveForwardDistance = _spinMoveForwardDistance;
    }

    public void ReturnSword()
    {
        rg.constraints = RigidbodyConstraints2D.FreezePosition;
        rg.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isSpin)
        {
            SpinBegin();
        }
        if (collision.GetComponent<Enemy>() != null)
        {
            SwordSkillDamage(collision.GetComponent<Enemy>());
            if (isBouncing && bounceTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bouncingRadius);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        bounceTarget.Add(hit.GetComponent<Enemy>());
                    }
                }
            }
        }
        else
        {
            isBouncing = false;
        }

        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy _enemy)
    {
        _enemy.StartCoroutine("FreezeTimerFor", freezeTime);
        player.DoDamageTo_Sword(_enemy, damageRate, transform);
    }

    private void SpinBegin()
    {
        wasStopped = true;
        rg.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void StuckInto(Collider2D collision)
    {
        if(pierceTime > 0 && collision.GetComponent<Enemy>() != null)
        {
            --pierceTime;
            return;
        }

        if(isSpin)
        {
            return;
        }

        canRotate = false;
        col.enabled = false;
        rg.isKinematic = true;
        rg.constraints = RigidbodyConstraints2D.FreezeAll;
        swordHitFx.Play();

        if (!isBouncing)
        {
            anim.SetBool("Rotation", false);
            transform.parent = collision.transform;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, bouncingRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, swordDamageRadius);
    }
}
