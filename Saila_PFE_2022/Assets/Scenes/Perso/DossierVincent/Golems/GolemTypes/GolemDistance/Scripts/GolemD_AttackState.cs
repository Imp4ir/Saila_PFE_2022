using System.Collections;
using UnityEngine;
public sealed class GolemDAttackState : GolemBaseState
{
    public GolemDAttackState(GolemStateMachine currentContext, GolemStateFactory golemStateFactory)
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

        if (Ctx.attackFollowPlayer)
        {
            Ctx.attackFeedbackPoint.position = Ctx.playerPos;
            Ctx.RotationBodyTowardsPlayer();
            Ctx.attackHitBox.transform.position = Ctx.playerPos;
            Ctx.laserWarmup.SetPosition(0, Ctx.headTf.position);
            Ctx.laserWarmup.SetPosition(1, Ctx.playerPos);
        }
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
        Ctx.attackFollowPlayer = true;
        Ctx.laserWarmupGO.SetActive(true);
        
        yield return new WaitForSeconds(Ctx.golemData.attackDuration - 1.5f);
        
        Ctx.attackFollowPlayer = false;
        var lastPlayerPos = Ctx.playerPos;

        yield return new WaitForSeconds(0.5f);
        
        AttackFeedback();
        Ctx.attackHitBox.SetActive(true);
        Ctx.laserWarmupGO.SetActive(false);
        Ctx.laserGO.SetActive(true);
        Ctx.laser.SetPosition(0, Ctx.headTf.position);
        Ctx.laser.SetPosition(1, lastPlayerPos);

        yield return new WaitForSeconds(1f);
        
        Ctx.attackHitBox.SetActive(false);
        Ctx.laserGO.SetActive(false);
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
        Object.Destroy(clone.gameObject, 0.5f);
    }
}
