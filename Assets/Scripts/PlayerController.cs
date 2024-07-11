using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private AudioSource audioSource;

    private PlayerMotor motor;

    [Header("Animation")]
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private const string IDLE = "Idle";
    [SerializeField]
    private const string WALK = "Walk";
    [SerializeField]
    private const string ATTACK1 = "Attack 1";
    [SerializeField]
    private const string ATTACK2 = "Attack 2";

    private string currentAnimationState;

    [Header("Attacking")]
    [SerializeField]
    private float attackDistance = 3f;
    [SerializeField]
    private float attackDelay = 0.4f;
    [SerializeField]
    private float attackSpeed = 1f;
    [SerializeField]
    private int attackDamage = 1;
    [SerializeField]
    private LayerMask attackLayer;

    [SerializeField]
    private GameObject hitEffect;
    [SerializeField]
    private AudioClip swordSwing;
    [SerializeField]
    private AudioClip hitSound;

    private bool attacking = false;
    private bool readyToAttack = true;
    private int attackCount;

    private void Awake()
    {
        motor = GetComponent<PlayerMotor>();
    }
    private void Update()
    {
        SetAnimations();
    }
    public void Attack()
    {
        if (!readyToAttack || attacking)
            return;

        readyToAttack = false;
        attacking = true;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);

        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(swordSwing);

        if (attackCount == 0)
        {
            ChangeAnimationState(ATTACK1);
            attackCount++;
        }
        else 
        {
            ChangeAnimationState(ATTACK2);
            attackCount = 0;
        }
    }

    private void ResetAttack() 
    {
        attacking = false;
        readyToAttack = true;
    }
    private void AttackRaycast() 
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance, attackLayer))
        {
            HitTarget(hit.point);

            if (hit.collider.TryGetComponent<Enemy>(out Enemy T))
            {
                T.TakeDamage(attackDamage);
            }
        }
    }

    private void HitTarget(Vector3 hitpointPos) 
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(hitSound);

        GameObject GO = Instantiate(hitEffect, hitpointPos, Quaternion.identity);
        Destroy(GO, 20);
    }

    public void ChangeAnimationState(string newState) 
    {
        //stop the same animation from interupting itself
        if (currentAnimationState == newState)
            return;

        //play animation
        currentAnimationState = newState;
        animator.CrossFadeInFixedTime(currentAnimationState, 0.2f);
    }

    private void SetAnimations()
    {
        if (!attacking) 
        {
            if (motor.GetPlayerVelocity().x == 0 && motor.GetPlayerVelocity().z == 0)
            {
                ChangeAnimationState(IDLE);
            }
            else 
            {
                ChangeAnimationState(WALK);
            }
        }
    }


}
