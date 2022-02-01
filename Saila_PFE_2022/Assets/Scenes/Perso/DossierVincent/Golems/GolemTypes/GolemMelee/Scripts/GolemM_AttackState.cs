using System.Collections;
using UnityEngine;
public sealed class GolemMAttackState : GolemBaseState
{
    public GolemMAttackState(GolemStateMachine currentContext, GolemStateFactory golemStateFactory)
        : base(currentContext, golemStateFactory)
    {}
    public override void EnterState()
    {
        Ctx.attackCoroutine = Ctx.StartCoroutine(AttackPhase());
    }

    protected override void ExitState()
    {
        Ctx.canBeStunned = true;
        Ctx.isStunned = false;
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
        HitDetection();
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
        // attack done
        if (Ctx.hasAttacked)
        {
            SwitchState(Factory.Fighting());
        }

        // dead
        if (Ctx.golemData.currentHealth <= 0)
        {
            SwitchState(Factory.Dead());
        }
    }
    
    private IEnumerator AttackPhase()
    {
        //Play attack animation
        Ctx.ChangeAnimationState(Ctx.attackAnim);

        yield return new WaitForSeconds(Ctx.golemData.attackDuration - 1);

        Ctx.attackHitBox.SetActive(true);
        AttackFeedback();

        yield return new WaitForSeconds(1);

        Ctx.attackHitBox.SetActive(false);
        Ctx.hitDetectionScript.playerHit = false;
        Ctx.hasHitPlayer = false;
        Ctx.hasAttacked = true;
        Ctx.canAttack = false;
    }
    
    private void HitDetection()
    {
        if (Ctx.hitDetectionScript.playerHit && !Ctx.hasHitPlayer)
        {
            Ctx.hasHitPlayer = true;
            //!!!! Need to change the 100 float with the real player's health after
            Ctx.DoDamage(100);
            Ctx.hitDetectionScript.playerHit = false;
        }
    }

    private void AttackFeedback()
    {
        var clone = Object.Instantiate(Ctx.attackFeedback, Ctx.attackFeedbackPoint.position, Quaternion.identity);
        Object.Destroy(clone.gameObject, 3f);
    }
}