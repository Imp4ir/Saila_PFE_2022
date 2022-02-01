using Sirenix.OdinInspector;
using UnityEngine;

public class CombatZone : MonoBehaviour
{
    [MinValue(10)]
    public float combatZoneRadius;
    private float _combatZoneOffsetRadius;
    [ShowInInspector, ReadOnly]
    private float _playerSlashMidPointDist;
    [ShowInInspector, ReadOnly]
    private bool _playerDetected;
    [Required("Middle point of enemy's patrol zone missing !")]
    public Transform combatZoneMidPoint;
    private Vector3 _midPointPos;
    public Color combatZoneBoundsColor;

    private GolemStateMachine[] _golems;
    [ReadOnly]
    public int ennemiesAlive;
    private GameObject _player;
    private Vector3 _playerPos;

    private void Start()
    {
        // Gets and counts all golem ennemies
        _golems = GetComponentsInChildren<GolemStateMachine>(true);
        foreach (var unused in _golems)
        {
            ennemiesAlive += 1;
        }

        _midPointPos = combatZoneMidPoint.position;
    }

    private void Update()
    {
        if (_playerDetected)
        {
            // If player is out of bounds from combat zone, make him stay at the limit
            _playerPos = _player.transform.position;
            _combatZoneOffsetRadius = combatZoneRadius - 3;
            _playerSlashMidPointDist = Vector3.Distance(_playerPos, _midPointPos);
            
            if (_playerSlashMidPointDist >= _combatZoneOffsetRadius)
            {
                var fromOriginToObject = _playerPos - _midPointPos;
                fromOriginToObject *= _combatZoneOffsetRadius / _playerSlashMidPointDist;
                _player.transform.position = _midPointPos + fromOriginToObject;
            }

            // If Player killed all ennemies, deactivate walls
            if (ennemiesAlive <= 0)
            {
                _playerDetected = false;
            }
        }
        else
        {
            // If at least one enemy is alive
            if (ennemiesAlive > 0)
            {
                foreach (var golem in _golems)
                {
                    // If player is detected, set variables
                    if (golem != null && golem.isPlayerDetected)
                    {
                        _player = golem.playerTarget;
                        _playerDetected = true;
                    }
                }
            }
        }
    }

    private void OnValidate()
    {
        _midPointPos = combatZoneMidPoint.position;
    }

    public void OnDrawGizmos()
    {
        // Sphere Gizmo of Combat Zone bounds
        Gizmos.color = combatZoneBoundsColor;
        Gizmos.DrawWireSphere(_midPointPos, combatZoneRadius);
    }
}
