using System.Collections;
using UnityEngine;


public sealed class GolemStunnedState : GolemBaseState
{
    public GolemStunnedState(GolemStateMachine currentContext, GolemStateFactory golemStateFactory)
        : base(currentContext, golemStateFactory)
    {}
    public override void EnterState()
    {
        Ctx.StartCoroutine(Stunned());
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
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
        // if stunned duration finished
        if (!Ctx.isStunned)
        {
            SwitchState(Factory.FollowPlayer());
        }

        // dead
        if (Ctx.golemData.currentHealth <= 0)
        {
            SwitchState(Factory.Dead());
        }
    }
    
    #region Stunned Functions

    private IEnumerator Stunned()
    {
        if (Ctx.attackCoroutine != null)
        {
            Ctx.StopCoroutine(Ctx.attackCoroutine);
        }
        Ctx.navMeshEnemy.isStopped = true;
        Ctx.ChangeAnimationState(Ctx.stunnedAnim);
        
        yield return new WaitForSeconds(Ctx.golemData.stunnedDuration);
        
        Ctx.StartCoroutine(Ctx.StunImmunityPhase());
        Ctx.navMeshEnemy.isStopped = false;
        Ctx.isStunned = false;
    }
    
    #endregion
}
