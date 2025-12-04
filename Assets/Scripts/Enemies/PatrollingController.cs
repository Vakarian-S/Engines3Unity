using System;
using UnityEngine;

public class PatrollingController : MonoBehaviour
{
    [Header("Patrol Path")] [SerializeField]
    private Transform[] patrolPoints;

    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float pointReachedDistance = 0.1f;

    [Header("Optional Chasing Behaviour")] [SerializeField]
    private ChasingEnemyController enemyChasingController;

    private static readonly int Run = Animator.StringToHash("Run");
    private Rigidbody2D _rigidbody2D;
    private int currentPatrolPointIndex = 0;
    private bool isPatrolling = true;
    private Animator _animator;


    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        // Auto-find a chasing controller if not assigned
        if (enemyChasingController == null)
        {
            enemyChasingController = GetComponent<ChasingEnemyController>();
        }
    }

    private void OnEnable()
    {
        StartPatrolling();
    }

    void Update()
    {
        if (!isPatrolling)
        {
            Debug.Log("Patrolling is disabled");
            return;
        }

        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.Log("I Have no points");
            return;
        }

        MoveTowardsCurrentPatrolPoint();
    }

    private void MoveTowardsCurrentPatrolPoint()
    {
        var targetPatrolPoint = patrolPoints[currentPatrolPointIndex];
        var currentPosition = _rigidbody2D.position;
        Vector2 targetPosition = targetPatrolPoint.position;

        Vector2 directionToPoint = (targetPatrolPoint.position - transform.position).normalized;
        _rigidbody2D.linearVelocity = directionToPoint * movementSpeed;

        if (directionToPoint.x != 0)
        {
            var localScale = transform.localScale;
            localScale.x = Mathf.Sign(directionToPoint.x) * Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }

        var distanceToTarget = Vector2.Distance(currentPosition, targetPosition);
        if (distanceToTarget <= pointReachedDistance)
        {
            SelectNextPatrolPoint();
        }

        if (!_animator) return;

        _animator.ResetTrigger("Run");
        _animator.SetTrigger(Run);
    }

    private void SelectNextPatrolPoint()
    {
        currentPatrolPointIndex++;

        if (currentPatrolPointIndex >= patrolPoints.Length)
        {
            currentPatrolPointIndex = 0; // loop
        }
    }

    // ---- PUBLIC API (for other scripts, like detection / chasing) ----

    public void StartPatrolling()
    {
        isPatrolling = true;
    }

    public void StopPatrolling()
    {
        isPatrolling = false;
    }
}