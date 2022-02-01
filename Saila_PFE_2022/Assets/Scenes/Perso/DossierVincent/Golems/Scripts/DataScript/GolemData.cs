using System;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.AI;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GolemData", order = 1)]
[InlineEditor()]
public class GolemData : ScriptableObject
{
    [Title("General")]
    // enum
    public enum GolemTypes
    {
        Melee,
        Distance
    }
    public GolemTypes golemType;
    [MinValue(0)]
    public int maxhealth;
    [MinValue(0)]
    public float currentHealth;

    [Title("Patrolling")]
    [Range(0.1f, 100f)]
    public float patrollingSpeed;
    public float patrolPointStoppingDistance;
    [MinValue(1)]
    public int minWaitingTime;
    [MaxValue(10)]
    public int maxWaitingTime;

    [Title("MovingToPlayer")]
    [Range(0.1f, 100f)]
    public float followingPlayerSpeed;
    public float playerStoppingDistance;
    
    [Title("InCombat")]
    public float attackDamage;
    public float attackDuration;
    public float attackCooldown;

    [Title("Stunned")]
    public float stunnedDuration;
    public float stunImmunityDuration;

    [Title("Dead")]
    public float deadAnimDuration;

}
