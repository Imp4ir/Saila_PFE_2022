using System.Collections;
using UnityEngine;


public sealed class GolemDeadState : GolemBaseState
{
    public GolemDeadState(GolemStateMachine currentContext, GolemStateFactory golemStateFactory)
        : base(currentContext, golemStateFactory)
    {}
    public override void EnterState()
    {
        Ctx.StartCoroutine(Death());
    }
    
    #region Dead Functions

    private IEnumerator Death()
    {
        if (Ctx.attackCoroutine != null)
        {
            Ctx.StopCoroutine(Ctx.attackCoroutine);
        }
        Ctx.navMeshEnemy.ResetPath();
        Ctx.ChangeAnimationState(Ctx.deadAnim);
        
        yield return new WaitForSeconds(Ctx.golemData.deadAnimDuration);

        Ctx.combatZone.ennemiesAlive -= 1;
        Object.Destroy(Ctx.navMeshEnemy.gameObject);
    }

    #endregion
}
