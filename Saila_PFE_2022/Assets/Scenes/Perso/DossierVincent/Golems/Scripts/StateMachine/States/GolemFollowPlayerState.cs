using UnityEngine;

public sealed class GolemFollowPlayerState : GolemBaseState
{
    public GolemFollowPlayerState(GolemStateMachine currentContext, GolemStateFactory golemStateFactory)
        : base(currentContext, golemStateFactory)
    {}

    public override void EnterState()
    {
        // Activate melee range detection
        Ctx.attackRangeDetection.gameObject.SetActive(true);
        // NavMesh tweaks
        Ctx.navMeshEnemy.speed = Ctx.golemData.followingPlayerSpeed;
        Ctx.navMeshEnemy.stoppingDistance = Ctx.golemData.playerStoppingDistance;
        //Play anim
        Ctx.ChangeAnimationState(Ctx.followPlayerAnim);
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
        Ctx.navMeshEnemy.SetDestination(Ctx.playerPos);
        Ctx.RotationBodyTowardsPlayer();
    }
    public override void OnTriggerStay(Collider triggerCollider)
    {
        if (triggerCollider.gameObject.CompareTag("Player"))
        {
            Ctx.atFightingRange = true;
        }
    }
    private void CheckSwitchStates()
    {
        // at attacking distance
        if (Ctx.atFightingRange)
        {
            SwitchState(Factory.Fighting());
        }

        // stunned
        if (Ctx.isStunned)
        {
            SwitchState(Factory.Stunned());
        }

        // dead
        if (Ctx.golemData.currentHealth <= 0)
        {
            SwitchState(Factory.Dead());
        }
    }
}
