using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Transform _player;
    private Animator _animController;
    private NavMeshAgent _navMeshAgent;
    private SphereCollider _sphereTrigger;

    [Header("Statistics")]
    //[SerializeField] private HealthBar healthBar;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float timeStunned;
    [SerializeField] private float timeAnimDamage = 1.5f;
    [SerializeField] private float timeAnimDie;
    // [SerializeField] private GameObject healing;
    // [SerializeField] private Score score;
    // private int aumentoScore = 100;
    private int _currentHealth;
    [HideInInspector] public bool isStunning;

    [Header("Patrol")]
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float waitTimePatrol = 3f;
    [SerializeField] protected Transform[] patrolPoints;
    [SerializeField] private float marginDistancePatrolPoint;
    private int _currentPatrolIndex;
    private bool _isWaiting;

    [Header("Follow")]
    [SerializeField] private float followRange = 15f;
    [SerializeField] private float followSpeed = 7f;
    [SerializeField] private float incrementFollow = 0f;
    [SerializeField] private float rotationSpeed = 5f;
    private float _distanceToPlayer;
    private Quaternion _targetRotation;

    [Header("Attack")]
    public int attackDamage = 20;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 2.0f;
    [SerializeField] private BoxCollider handCollider;
    private bool _isActiveCollider;
    private bool _canAttack = true;
    private bool _isAttacking;

    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Attacking = Animator.StringToHash("Attacking");
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Stunning = Animator.StringToHash("Stunning");
    private static readonly int Dying = Animator.StringToHash("Dying");
    private static readonly int Receiving = Animator.StringToHash("Receiving");

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animController = GetComponent<Animator>();
        _sphereTrigger = GetComponent<SphereCollider>();
    }
    private void Start()
    {
        // score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>();
        _currentHealth = maxHealth;
        // healthBar.SetMaxHealth(maxHealth);
        _sphereTrigger.radius = followRange;
    }
    private void FixedUpdate()
    {
        if (isStunning) return;

        var position = _player.position;
        var position1 = transform.position;
        _distanceToPlayer = Vector3.Distance(position1, position);

        Vector3 directionToPlayer = position - position1;


        if (_distanceToPlayer > followRange)
        {
            StatePatrol();
        }
        else if (_distanceToPlayer <= attackRange)
        {
            RootToPlayer(directionToPlayer);
            StateAttack();
        }
        else
        {
            RootToPlayer(directionToPlayer);
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, Time.deltaTime * rotationSpeed);
            StateFollow();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == _player)
        {
            followRange += incrementFollow;
            _sphereTrigger.radius = followRange;

            _isWaiting = false;
            CancelInvoke(nameof(MoveToNextPatrolPoint));

            StateFollow();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform == _player)
        {
            followRange -= incrementFollow;
            _sphereTrigger.radius = followRange;

            StatePatrol();
        }
    }

    private void StatePatrol()
    {
        // _animController.SetBool(Running, false);
        // _animController.SetBool(Attacking, false);
        // _animController.SetBool(Stunning, false);
        // _animController.SetBool(Receiving, false);

        if (patrolPoints == null)
        {
            StateFollow();
            return;
        }
        if (!_isWaiting)
        {
            // _animController.SetBool(Walking, true);
        }
        else
        {
            return;
        }

        if (patrolPoints.Length <= 0) return;

        Vector3 currentPatrolPoint = patrolPoints[_currentPatrolIndex].position;

        _navMeshAgent.stoppingDistance = 1f;
        Move(patrolSpeed, currentPatrolPoint);

        if (!(Vector3.Distance(transform.position, currentPatrolPoint) <= marginDistancePatrolPoint)) return;

        // _animController.SetBool(Walking, false);
        Invoke(nameof(MoveToNextPatrolPoint), waitTimePatrol);
        _isWaiting = true;
    }
    private void StateFollow()
    {
        // _animController.SetBool(Walking, false);
        // _animController.SetBool(Attacking, false);
        // _animController.SetBool(Stunning, false);
        // _animController.SetBool(Receiving, false);

        if (!_isAttacking)
        {
            // _animController.SetBool(Running, true);
        }
        else
        {
            return;
        }

        if (_distanceToPlayer <= attackRange && _canAttack)
        {
            StateAttack();
        }
        else
        {
            _navMeshAgent.stoppingDistance = attackRange;
            Move(followSpeed, _player.position);
        }
    }
    private void StateAttack()
    {
        // _animController.SetBool(Running, false);
        // _animController.SetBool(Walking, false);
        // _animController.SetBool(Stunning, false);
        // _animController.SetBool(Receiving, false);

        if (!_canAttack)
        {
            return;
        }
        // _animController.SetBool(Attacking, true);

        _isAttacking = true;
        Move(-1f, _player.position);

        _canAttack = false;

        Invoke(nameof(ResetAttackCooldown), attackCooldown);
    }

    private void MoveToNextPatrolPoint()
    {
        _currentPatrolIndex++;

        if (_currentPatrolIndex >= patrolPoints.Length)
        {
            _currentPatrolIndex = 0;
        }

        _isWaiting = false;
    }

    private void Move(float speed, Vector3 destination)
    {
        _navMeshAgent.speed = speed;
        _navMeshAgent.SetDestination(destination);
    }

    private void RootToPlayer(Vector3 directionToPlayer)
    {
        _targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void ResetAttackCooldown()
    {
        _canAttack = true;
        _isAttacking = false;

        // _animController.SetBool(Attacking, false);
    }
    public void SwitchCollider()
    {
        _isActiveCollider = !_isActiveCollider;
        handCollider.enabled = _isActiveCollider;
    }

    public void TakeDamage(int damage)
    {
        StopAllCoroutines();

        _isAttacking = false;
        _canAttack = true;

        // _animController.SetBool(Receiving, false);
        // _animController.SetBool(Stunning, false);
        // _animController.SetBool(Walking, false);
        // _animController.SetBool(Attacking, false);
        // _animController.SetBool(Running, false);

        isStunning = true;

        _currentHealth -= damage;
        // healthBar.SetHealth(_currentHealth);
        if (_currentHealth <= 0)
        {
            // _animController.SetBool(Dying, true);
            // _animController.SetBool(Receiving, false);
            // _animController.SetBool(Stunning, false);
            // _animController.SetBool(Walking, false);
            // _animController.SetBool(Attacking, false);
            // _animController.SetBool(Running, false);

            // healthBar.SetHealth(0);
            Move(0, _player.position);
            Invoke(nameof(Die), timeAnimDie);
        }

        // _animController.SetBool(Receiving, true);
        StartCoroutine(ReceiveDamage());
    }

    private IEnumerator ReceiveDamage()
    {
        yield return new WaitForSeconds(timeAnimDamage);

        // _animController.SetBool(Receiving, false);
        // _animController.SetBool(Stunning, true);
        // _animController.SetBool(Walking, false);
        // _animController.SetBool(Attacking, false);
        // _animController.SetBool(Running, false);

        Move(0, _player.position);

        yield return new WaitForSeconds(timeStunned + timeAnimDamage);

        isStunning = false;
    }

    private void Die()
    {
        Vector3 newPosition = transform.position + new Vector3(0f, 3f, 0f);

        // Instantiate(healing, newPosition, healing.transform.rotation);
        // score.UpdateScore(aumentoScore);
        Destroy(gameObject);
    }
}