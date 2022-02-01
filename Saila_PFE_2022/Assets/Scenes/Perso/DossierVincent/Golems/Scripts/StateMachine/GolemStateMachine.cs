using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GolemStateMachine : MonoBehaviour
{
    // state variables
    [HideInInspector]
    public GolemBaseState currentState;
    GolemStateFactory _states;
    
    // data of golem
    [Space(10)]
    public GolemData golemData;

    #region Bools Variables
    
    [Title("Bools", "Globals")]

    [Title("Bools", "Patrol")]
    public bool isTravelling;
    public bool isWaiting;
    public bool isPlayerDetected;

    [Title("Bools", "FollowPlayer")]
    public bool atFightingRange;

    [Title("Bools", "Attack")]
    public bool canAttack;
    public bool hasAttacked;
    public bool hasHitPlayer;
    // Range Attack
    [HideInInspector]
    public bool attackFollowPlayer;

    [Title("Bools", "Stunned")]
    public bool isStunned;
    public bool canBeStunned;

    #endregion
    
    #region References Variables

    [Title("References", "General")]
    [Required("Main body of enemy missing !")]
    [ChildGameObjectsOnly]
    public Transform mainBody;
    [HideInInspector]
    public Vector3 mainBodyPos;
    [HideInInspector]
    public Vector3 playerPos;
    [HideInInspector]
    public NavMeshAgent navMeshEnemy;
    [HideInInspector]
    public GameObject playerTarget;
    [Required("combat Zone script missing !")]
    public CombatZone combatZone;
    public Animator animator;

    #endregion
    
    #region Animations Variables

    [Title("References", "Animations")]
    [Required("Need Patrolling Animation Clip from enemy's animator !")]
    public AnimationClip patrolAnim;
    [Required("Need MovingToPlayer Animation Clip from enemy's animator !")]
    public AnimationClip followPlayerAnim;
    [Required("Need InCombat Animation Clip from enemy's animator !")]
    public AnimationClip fightingAnim;
    [Required("Need Attack Animation Clip from enemy's animator !")]
    public AnimationClip attackAnim;
    [Required("Need Stunned Animation Clip from enemy's animator !")]
    public AnimationClip stunnedAnim;
    [Required("Need Dead Animation Clip from enemy's animator !")]
    public AnimationClip deadAnim;
    [Space(10)]
    [ReadOnly]
    public AnimationClip currentAnimation;

    #endregion
    
    #region Global Variables

    [Title("Patrol")]
    public Transform[] patrolPoints;
    [HideInInspector]
    public GolemOnPatrolPointDetection[] golemOnPatroPointScript;
    [Required("Player detection of enemy missing !")]
    public SphereCollider playerDetection;
    public int playerDetectionRadius;
    [ReadOnly]
    public int currentPatrolIndex;
    [HideInInspector]
    public int randomPatrolIndex;
    [ReadOnly]
    public float waitingTime;
    [HideInInspector]
    public float waitTimer;

    [Title("Fighting")]
    [Required("range detection of enemy missing !")]
    public SphereCollider attackRangeDetection;
    public int attackRangeDetectionRadius;

    [Title("Attack")]
    public Coroutine attackCoroutine;
    [ReadOnly]
    public float currentAttackCooldown;
    [Required("Attack hitbox of enemy missing !")]
    public GameObject attackHitBox;
    [Required("hitDetection script of enemy missing !")]
    public GolemsHitDetection hitDetectionScript;
    
    [Title("Only for Ranged Golem")]
    public GameObject laserGO;
    public GameObject laserWarmupGO;
    [HideInInspector]
    public LineRenderer laser;
    [HideInInspector]
    public LineRenderer laserWarmup;
    public Transform headTf;

    //Feedbacks à changer/supprimer pour plus tard
    [Title("Feedbacks")]
    public Slider healthSliderFeedback;
    public ParticleSystem attackFeedback;
    public Transform attackFeedbackPoint;

    #endregion

    private void Awake()
    {
        _states = new GolemStateFactory(this);

        #region Component Initializes

        // nav Mesh initializing
        navMeshEnemy = gameObject.GetComponent<NavMeshAgent>();
        if (navMeshEnemy == null)
        {
            Debug.LogError($"the NavMeshAgent Component is not attached to the gameobject {gameObject.name} !");
        }
        else
        {
            if (patrolPoints != null && patrolPoints.Length < 2)
            {
                Debug.LogError("Not enough patrol points for patrolling behaviour. Try to add 2 or more patrol points.");
            }
        }

        //Initialization of length of array
        if (patrolPoints != null)
        {
            golemOnPatroPointScript = new GolemOnPatrolPointDetection[patrolPoints.Length];
            //Initialization of array
            for (var i = 0; i < patrolPoints.Length; i++)
            {
                golemOnPatroPointScript[i] = patrolPoints[i].GetComponent<GolemOnPatrolPointDetection>();
            }
        }

        if (golemData.golemType == GolemData.GolemTypes.Distance)
        {
            laser = laserGO.GetComponent<LineRenderer>();
            laserWarmup = laserWarmupGO.GetComponent<LineRenderer>();
        }

        #endregion
        
        // first state initializing
        currentState = _states.Patrol();
        currentState.EnterState();
    }

    private void Start()
    {
        #region Variables Initializing
        
        // detection radius
        playerDetection.radius = playerDetectionRadius;
        attackRangeDetection.radius = attackRangeDetectionRadius;
        
        // detection disabled at start
        attackRangeDetection.gameObject.SetActive(false);
        attackHitBox.SetActive(false);
        
        // bools
        canAttack = true;
        canBeStunned = true;

        // other
        golemData.currentHealth = golemData.maxhealth;
        currentAttackCooldown = golemData.attackCooldown;
        healthSliderFeedback.maxValue = golemData.maxhealth;

        #endregion
    }

    private void Update()
    {
        // Variables constantly getting updated
        mainBodyPos = mainBody.position;
        // Assigns player position if the player has been detected
        if (playerTarget != null)
        {
            playerPos = playerTarget.transform.position;
        }
        // Feedbacks
        healthSliderFeedback.value = golemData.currentHealth;

        // Always active functions
        AttackCooldown();
        
        currentState.UpdateState();
    }
    
    private void OnTriggerEnter(Collider triggerCollider)
    {
        currentState.OnTriggerEnter(triggerCollider);
    }

    private void OnTriggerStay(Collider triggerCollider)
    {
        currentState.OnTriggerStay(triggerCollider);
    }

    private void OnTriggerExit(Collider triggerCollider)
    {
        currentState.OnTriggerExit(triggerCollider);
    }

    public void OnValidate()
    {
        playerDetection.radius = playerDetectionRadius;

        attackRangeDetection.radius = attackRangeDetectionRadius;
    }
    
    #region Global Functions
    
    /// <summary>
    /// Does damage to the enemy's health
    /// </summary>
    /// <param name="damage">Value of the damage taken</param>
    [PropertyOrder(-1)]
    [Button("Take DMG")]
    public void TakeDamage(float damage)
    {
        golemData.currentHealth -= damage;
        if (canBeStunned)
        {
            canBeStunned = false;
            isStunned = true;
        }
        Debug.Log($"Enemy took <color=red>{damage} damage</color> and has <color=green>{golemData.currentHealth} health</color> remaining");
    }

    /// <summary>
    /// Does damage to the player's health
    /// </summary>
    /// <param name="playerhealth">Value of the player's health</param>
    public void DoDamage(float playerhealth)
    {
        playerhealth -= golemData.attackDamage;
        Debug.Log($"Enemy did <color=red>{golemData.attackDamage}</color> damage to the player who has now <color=green>{playerhealth}</color> health left");
    }

    /// <summary>
    /// Changes current animation playing to a new one
    /// </summary>
    /// <param name="newAnim">New animation</param>
    public void ChangeAnimationState(AnimationClip newAnim)
    {
        // checks if the animation is not the same
        if (currentAnimation == newAnim) return;
        // play animation
        animator.Play(newAnim.name);
        // reassign the current state as new state
        currentAnimation = newAnim;
    }

    /// <summary>
    /// Cooldown of the attack working despite the state
    /// </summary>
    private void AttackCooldown()
    {
        if (hasAttacked && currentAttackCooldown > 0)
        {
            currentAttackCooldown -= Time.deltaTime;
        }
        else if (currentAttackCooldown <= 0)
        {
            currentAttackCooldown = golemData.attackCooldown;
            hasAttacked = false;
            canAttack = true;
        }
    }

    /// <summary>
    /// Rotates body towards the player (but not when attacking or when dying)
    /// </summary>
    public void RotationBodyTowardsPlayer()
    {
        var playerLookAtPos = new Vector3(playerPos.x, mainBodyPos.y, playerPos.z);
        mainBody.LookAt(playerLookAtPos);
    }
    public IEnumerator StunImmunityPhase()
    {
        yield return new WaitForSeconds(golemData.stunImmunityDuration);
        canBeStunned = true;
    }

    #endregion
}
