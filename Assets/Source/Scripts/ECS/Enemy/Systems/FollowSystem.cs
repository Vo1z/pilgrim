using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Enemy.ECS{
    sealed class FollowSystem : IEcsRunSystem {
        private readonly EcsFilter<EnemyMovementComponent,LocateTargetComponent,FollowStateTag> _enemyFilter;
        
        void IEcsRunSystem.Run () {
            foreach (var i in _enemyFilter)
            {
                ref var entity = ref _enemyFilter.GetEntity(i);
                ref var movement = ref _enemyFilter.Get1(i);
                ref var target = ref _enemyFilter.Get2(i);

                movement.Waypoint = target.Target;
                movement.NavMeshAgent.speed = movement.EnemyMovementData.SpeedForward;
                movement.NavMeshAgent.destination = movement.Waypoint.position;
                movement.NavMeshAgent.isStopped = false;
                
            }
        }
    }
}