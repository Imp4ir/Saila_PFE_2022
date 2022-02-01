using UnityEngine;

public sealed class GolemFightingState : GolemBaseState
{
    public GolemFightingState(GolemStateMachine currentContext, GolemStateFactory golemStateFactory)
        : base(currentContext, golemStateFactory)
    {}

    public override void EnterState()
    {
        //Play in combat animation
        Ctx.ChangeAnimationState(Ctx.fightingAnim);
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
        Ctx.RotationBodyTowardsPlayer();
    }
    public override void OnTriggerExit(Collider triggerCollider)
    {
        if (triggerCollider.gameObject.CompareTag("Player"))
        {
            Ctx.atFightingRange = false;
        }
    }
    private void CheckSwitchStates()
    {
        // ready to do an attack
        if (Ctx.canAttack)
        {
            // if a melee type golem
            if (Ctx.golemData.golemType == GolemData.GolemTypes.Melee)
            {
                SwitchState(Factory.M_Attack());
            }
            // if a distance type golem
            if (Ctx.golemData.golemType == GolemData.GolemTypes.Distance)
            {
                SwitchState(Factory.D_Attack());
            }
        }

        // not at attacking distance
        if (!Ctx.atFightingRange)
        {
            SwitchState(Factory.FollowPlayer());
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
