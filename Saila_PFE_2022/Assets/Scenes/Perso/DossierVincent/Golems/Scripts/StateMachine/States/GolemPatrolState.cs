using UnityEngine;

public sealed class GolemPatrolState : GolemBaseState
{
    public GolemPatrolState(GolemStateMachine currentContext, GolemStateFactory golemStateFactory)
        : base(currentContext, golemStateFactory)
    {}
    public override void EnterState()
    {
        //Activate player detection collider
        Ctx.playerDetection.gameObject.SetActive(true);
        //NavMesh tweaks
        Ctx.navMeshEnemy.speed = Ctx.golemData.patrollingSpeed;
        Ctx.navMeshEnemy.stoppingDistance = Ctx.golemData.patrolPointStoppingDistance;
        Ctx.waitingTime = Random.Range(Ctx.golemData.minWaitingTime, Ctx.golemData.maxWaitingTime + 1);
        //Play anim
        Ctx.ChangeAnimationState(Ctx.patrolAnim);
        //Random first patrol point
        SetNewDestination();
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
        Patrolling();
    }
    protected override void ExitState()
    {
        //Desactivate player detection collider
        Ctx.playerDetection.gameObject.SetActive(false);
        // Deactivate all state bools
        Ctx.isTravelling = false;
        Ctx.isWaiting = false;
    }
    public override void OnTriggerEnter(Collider triggerCollider)
    {
        // Detection of player if he is in his detection range
        if (triggerCollider.gameObject.CompareTag("Player"))
        {
            Ctx.isPlayerDetected = true;
            Ctx.playerTarget = triggerCollider.gameObject;
        }
    }
    private void CheckSwitchStates()
    {
        // if player is detected
        if (Ctx.isPlayerDetected)
        {
            SwitchState(Factory.FollowPlayer());
        }
    }

    #region Patrolling Functions

    private void Patrolling()
    {
        //Check if we're close to the destination
        if (Ctx.isTravelling && Ctx.navMeshEnemy.remainingDistance <= Ctx.golemData.patrolPointStoppingDistance)
        {
            Ctx.isTravelling = false;
            Ctx.isWaiting = true;
            Ctx.waitTimer = 0f;
        }
        //Instead if we're waiting...
        if (Ctx.isWaiting)
        {
            // Augment timer
            Ctx.waitTimer += Time.deltaTime;
            // When timer finished
            if (Ctx.waitTimer >= Ctx.waitingTime)
            {
                // Move NavMesh
                Ctx.isWaiting = false;
                SetNewDestination();
            }
        }
        //if on a point, mark the navmesh has not going to it
        for (int i = 0; i < Ctx.golemOnPatroPointScript.Length; i++)
        {
            if (Ctx.golemOnPatroPointScript[i].golemOnPoint)
            {
                Ctx.golemOnPatroPointScript[i].golemOnItsWayToPoint = false;
            }
        }
    }
    private void SetNewDestination()
    {
        //set patrol point
        Ctx.randomPatrolIndex = Random.Range(0, Ctx.patrolPoints.Length);
        //Checks 1: if the rand patrol point is equal to the current one
        //2: if the navmesh is already on it
        //3: if there is another navmesh on it
        //if one of them is true, repeat the process
        while (Ctx.currentPatrolIndex == Ctx.randomPatrolIndex
            || Ctx.golemOnPatroPointScript[Ctx.randomPatrolIndex].golemOnPoint
            || Ctx.golemOnPatroPointScript[Ctx.randomPatrolIndex].golemOnItsWayToPoint)
        {
            Ctx.randomPatrolIndex = Random.Range(0, Ctx.patrolPoints.Length);
        }
        //if none is true, go set this point as the new point
        Ctx.currentPatrolIndex = Ctx.randomPatrolIndex;

        //set destination
        if (Ctx.patrolPoints != null)
        {
            Vector3 targetVector = Ctx.patrolPoints[Ctx.currentPatrolIndex].position;
            Ctx.navMeshEnemy.SetDestination(targetVector);

            Ctx.waitingTime = Random.Range(Ctx.golemData.minWaitingTime, Ctx.golemData.maxWaitingTime + 1);
            Ctx.isTravelling = true;

            Ctx.golemOnPatroPointScript[Ctx.currentPatrolIndex].golemOnItsWayToPoint = true;
        }
    }

    #endregion

}
