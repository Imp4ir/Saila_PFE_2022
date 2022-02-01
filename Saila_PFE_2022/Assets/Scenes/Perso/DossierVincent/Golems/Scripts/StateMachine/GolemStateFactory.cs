public class GolemStateFactory
{
    private readonly GolemStateMachine _context;

    public GolemStateFactory(GolemStateMachine currentContext)
    {
        _context = currentContext;
    }

    public GolemBaseState Patrol()
    {
        return new GolemPatrolState(_context, this);
    }
    public GolemBaseState FollowPlayer()
    {
        return new GolemFollowPlayerState(_context, this);
    }
    public GolemBaseState Fighting()
    {
        return new GolemFightingState(_context, this);
    }
    public GolemBaseState Stunned()
    {
        return new GolemStunnedState(_context, this);
    }
    public GolemBaseState Dead()
    {
        return new GolemDeadState(_context, this);
    }
    
    public GolemBaseState M_Attack()
    {
        return new GolemMAttackState(_context, this);
    }
    public GolemBaseState D_Attack()
    {
        return new GolemDAttackState(_context, this);
    }
}
