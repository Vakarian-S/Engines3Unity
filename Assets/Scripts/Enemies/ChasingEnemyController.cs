using UnityEngine;

public class ChasingEnemyController : MonoBehaviour
{
    private static readonly int Run = Animator.StringToHash("Run");
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private Transform playerTransform;

    [Header("Patrol Controller")] [SerializeField]
    private PatrollingController patrollingController;


    private bool _isChasingPlayer;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        if (playerTransform != null) return;
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("SkeletonChaseController: No player with tag 'Player' found in the scene.");
        }

        // Auto-find a patrolling controller if not assigned
        if (patrollingController != null) return;
        Debug.Log("Havent found the controller");
        patrollingController = GetComponent<PatrollingController>();
    }

    private void Update()
    {
        if (!playerTransform)
        {
            return;
        }

        var distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        Debug.Log("Distance to player is " + distanceToPlayer);
        _isChasingPlayer = distanceToPlayer <= detectionRadius;

        if (_isChasingPlayer)
        {
            patrollingController?.StopPatrolling();
            ChasePlayer();
        }
        else
        {
            if (patrollingController)
            {
                patrollingController?.StartPatrolling();
            }
            else
            {
                StopChasing();
            }
        }
    }


    private void ChasePlayer()
    {
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        _rigidbody2D.linearVelocity = directionToPlayer * movementSpeed;

        // Optional: flip sprite depending on x-direction
        if (directionToPlayer.x != 0)
        {
            var localScale = transform.localScale;
            localScale.x = Mathf.Sign(directionToPlayer.x) * Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }

        if (!_animator) return;

        _animator.ResetTrigger("Run");
        _animator.SetTrigger(Run);
    }

    private void StopChasing()
    {
        if (_rigidbody2D)
        {
            _rigidbody2D.linearVelocity = Vector2.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize detection radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}