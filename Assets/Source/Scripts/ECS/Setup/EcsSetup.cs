using Ingame.CameraWork;
using Ingame.Debuging;
using Ingame.Enemy.System;
using Ingame.Gunplay;
using Ingame.Health;
using Ingame.Hud;
using Ingame.Input;
using Ingame.Interaction.Common;
using Ingame.Interaction.Doors;
using Ingame.Inventory;
using Ingame.Movement;
using Ingame.Player;
using Ingame.Utils;
using LeoEcsPhysics;
using Leopotam.Ecs;
using Support;
using UnityEngine;
using Voody.UniLeo;
using Zenject;

namespace Ingame
{
    public sealed class EcsSetup : MonoBehaviour
    {
        [Inject] private StationaryInput _stationaryInput;
        [Inject] private EcsWorld _world;
        [Inject(Id = "UpdateSystems")] private EcsSystems _updateSystems;
        [Inject(Id = "FixedUpdateSystems")] private EcsSystems _fixedUpdateSystem;
#if UNITY_EDITOR
        private EcsProfiler _ecsProfiler;
#endif
        private void Awake()
        {
            Application.targetFrameRate = 240;
            
#if UNITY_EDITOR
            _ecsProfiler = new EcsProfiler(_world, new EcsWorldDebugListener(), _updateSystems, _fixedUpdateSystem);
#endif
            
            EcsPhysicsEvents.ecsWorld = _world;
            
            _updateSystems.ConvertScene();

            AddInjections();
            AddOneFrames();
            AddSystems();
            
            _updateSystems.Init();
            _fixedUpdateSystem.Init();
        }

        private void Update()
        {
            _updateSystems.Run();
        }

        private void FixedUpdate()
        {
            _fixedUpdateSystem.Run();
        }

        private void OnDestroy()
        {
#if UNITY_EDITOR
            _ecsProfiler.Dispose();
            _ecsProfiler = null;
#endif
            EcsPhysicsEvents.ecsWorld = null;
            
            _updateSystems.Destroy();
            _updateSystems = null;
            
            _fixedUpdateSystem.Destroy();
            _fixedUpdateSystem = null;
            
            _world.Destroy();
            _world = null;
        }

        private void AddInjections()
        {
            _updateSystems
                .Inject(_stationaryInput);
        }

        private void AddOneFrames()
        {
            _updateSystems
                .OneFrame<DebugRequest>()
                .OneFrame<JumpInputEvent>()
                .OneFrame<CrouchInputEvent>()
                .OneFrame<LeanInputRequest>()
                .OneFrame<MoveInputRequest>()
                .OneFrame<RotateInputRequest>()
                .OneFrame<ShootInputEvent>()
                .OneFrame<AimInputEvent>()
                .OneFrame<ReloadInputEvent>()
                .OneFrame<DistortTheShutterInputEvent>()
                .OneFrame<InteractInputEvent>()
                .OneFrame<HudReloadAnimationTriggerEvent>()
                .OneFrame<HudDistortTheShutterAnimationTriggerEvent>();
        }

        private void AddSystems()
        {
            //Init
            _updateSystems
                .Add(new CharacterControllerInitSystem())
                .Add(new TransformModelInitSystem())
                .Add(new PlayerInitSystem())
                .Add(new PlayerHudInitSystem())
                .Add(new GunInitSystem())
                .Add(new DeltaMovementInitializeSystem())
                .Add(new CameraInitializeSystem());

            //Update
            _updateSystems
                //Input
                .Add(new StationaryInputSystem())
                .Add(new PlayerInputToRotationConverterSystem())
                .Add(new PlayerHudInputToRotationConverterSystem())
                .Add(new PlayerInputToCrouchConverterSystem())
                .Add(new PlayerInputToLeanConverterSystem())
                .Add(new PlayerSpeedChangerSystem())
                //HUD
                .Add(new CameraInputToStatesConverterSystem())
                .Add(new HudInputToStatesConverterSystem())
                .Add(new HudItemRotatorDueDeltaRotationSystem())
                .Add(new HudItemRotatorDueVelocitySystem())
                .Add(new HudItemMoverDueSurfaceDetectionSystem())
                .Add(new HeadBobbingSystem())
                //Gun play
                .Add(new GunDistortTheShutterInputConverterSystem())
                .Add(new GunReloadInputConverterSystem())
                .Add(new GunShootInputConverterSystem())
                .Add(new GunRecoilSystem())
                .Add(new GunShootSystem())
                .Add(new GunDistortTheShutterCallbackReceiverSystem())
                .Add(new GunReloadCallbackReceiverSystem())
                .Add(new HudGunAnimationSystem())
                //AI
                .Add(new InitializeEntityReferenceSystem())
                .Add(new DetectSystem())
                .Add(new PatrolSystem())
                .Add(new FollowSystem())
                .Add(new AttackSystem())
                .Add(new FleeSystem())
                .Add(new HideSystem())
                .Add(new EnemySoldierBehaviourSystem())
                //Health
                .Add(new DamageSystem())
                .Add(new BleedingSystem())
                .Add(new DeathSystem())
                .Add(new DestroyDeadActorsSystem())
                //Interaction
                .Add(new InteractionSystem())
                .Add(new DoorRotationSystem())
                //Inventory
                .Add(new ItemPickupSystem())
                //Utils
                .Add(new TimeSystem())
                .Add(new DebugSystem())
                .Add(new ExternalEventsRemoverSystem());


            //FixedUpdate
            _fixedUpdateSystem
                 //Input   
                .Add(new PlayerInputToMovementConvertSystem())
                 //Utils
                 .Add(new DeltaMovementCalculationSystem())
                 //Movement
                .Add(new FrictionSystem())
                .Add(new SlidingSystem())
                .Add(new GravitationSystem())
                .Add(new PlayerInputToJumpConverterSystem())
                .Add(new CharacterControllerHeightChangingSystem())
                .Add(new LeanSystem())
                .Add(new CameraLeanSystem())
                .Add(new MovementSystem());
        }
    }
}