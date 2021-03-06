using System;
using Ingame.Enemy.Data;
using UnityEngine;
using UnityEngine.AI;

namespace Ingame.Enemy
{
    [Serializable]
    public struct EnemyMovementComponent 
    {
        public EnemyMovementData EnemyMovementData;
        [HideInInspector]
        public Transform Waypoint;
        [HideInInspector]
        public NavMeshAgent NavMeshAgent;
    }
}
