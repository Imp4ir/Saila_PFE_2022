using UnityEngine;
public abstract class GolemBaseState
{
    private GolemStateMachine _ctx;
    private GolemStateFactory _factory;

    protected GolemStateMachine Ctx => _ctx;
    protected GolemStateFactory Factory => _factory;
    public GolemBaseState(GolemStateMachine currentContext, GolemStateFactory golemStateFactory)
    {
        _ctx = currentContext;
        _factory = golemStateFactory;
    }
    public virtual void EnterState()
    {}

    public virtual void UpdateState()
    {}

    protected virtual void ExitState()
    {}
    public virtual void OnTriggerEnter(Collider triggerCollider)
    {}
    public virtual void OnTriggerStay(Collider triggerCollider)
    {}
    public virtual void OnTriggerExit(Collider triggerCollider)
    {}

    protected void SwitchState(GolemBaseState newState)
    {
        // current state exit state
        ExitState();
        // new state enter state
        newState.EnterState();
        Debug.Log($"<color=green>Entering State "+ newState + "</color>");
        // switch current state of context
        _ctx.currentState = newState;
    }
}
