using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class ZombieMovement : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpSpeed = 5f;

    [Header("Range")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float chaseRange = 5f;

    [Header("Cooldown")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float freezeTime = 0.2f;


    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private float nextAttackTime;

    private AudioManager audioManager;

    private enum ZombieState
    {
        Idle,
        Attack,
        Run
    }

    private ZombieState currentState;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        currentState = ZombieState.Idle;
        nextAttackTime = Time.time;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        audioManager = FindObjectOfType<AudioManager>();
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case ZombieState.Idle:
                IdleState();
                break;
            case ZombieState.Attack:
                AttackState();
                break;
            case ZombieState.Run:
                RunState();
                break;
        }
    }

    private void IdleState()
    {
        animator.SetBool("Running", false);
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            currentState = ZombieState.Attack;
        }
        else if (Vector2.Distance(transform.position, player.position) < chaseRange)
        {
            currentState = ZombieState.Run;
        }
    }

    private void AttackState()
    {

        if (Time.time >= nextAttackTime)
        {
            StartCoroutine(AttackDelayCoroutine());


            nextAttackTime = Time.time + attackCooldown;
        }

        if (Vector2.Distance(transform.position, player.position) > attackRange)
        {
            currentState = ZombieState.Run;
        }
    }
    private IEnumerator AttackDelayCoroutine()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        animator.SetBool("Running", false);
        audioManager.Play("ZombieAttacking");

        yield return new WaitForSeconds(freezeTime);
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.AddForce(direction * jumpSpeed, ForceMode2D.Impulse);
        animator.SetTrigger("Attack");
    }

    private void RunState()
    {
        animator.SetBool("Running", true);

        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
        Vector2 lookDirecton = player.position - transform.position;
        rb.rotation = Mathf.Atan2(lookDirecton.y, lookDirecton.x) * Mathf.Rad2Deg;

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            currentState = ZombieState.Attack;
        }
        else if (Vector2.Distance(transform.position, player.position) > chaseRange)
        {
            currentState = ZombieState.Idle;
        }
    }
}
