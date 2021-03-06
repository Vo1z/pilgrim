using Ingame.Input;
using Ingame.Inventory;
using Ingame.Movement;
using Ingame.Player;
using Ingame.Utils;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.Player
{
    public sealed class PlayerInitSystem : IEcsInitSystem
    {
        private readonly EcsFilter<PlayerModel> _playerFilter;

        public void Init()
        {
            //todo move to settings
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            foreach (var i in _playerFilter)
            {
                ref var playerEntity = ref _playerFilter.GetEntity(i);
                ref var playerModel = ref _playerFilter.Get1(i);
                ref var playerGravityComponent = ref playerEntity.Get<GravityComponent>();
                ref var playerCharacterControllerModel = ref playerEntity.Get<CharacterControllerModel>();
                ref var playerFrictionComp = ref playerEntity.Get<FrictionComponent>();
                ref var playerTransformModel = ref playerEntity.Get<TransformModel>();
                playerEntity.Get<TimerComponent>();
                playerEntity.Get<VelocityComponent>();
                playerEntity.Get<InventoryComponent>();
                
                var playerData = playerModel.playerMovementData;

                playerGravityComponent.gravityAcceleration = playerData.GravityAcceleration;
                playerGravityComponent.maximalGravitationalForce = playerData.MaximumGravitationForce;
                playerCharacterControllerModel.slidingForceModifier = playerData.SlidingForceModifier;
                playerFrictionComp.frictionPower = playerData.MovementFriction;
                playerTransformModel.transform = playerCharacterControllerModel.characterController.transform;
                playerModel.currentSpeed = playerModel.isCrouching ? playerData.CrouchWalkSpeed : playerData.WalkSpeed;
                playerModel.currentLeanDirection = LeanDirection.None;
            }
        }
    }
}