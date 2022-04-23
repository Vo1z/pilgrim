﻿using Ingame.Hud;
using Ingame.Input;
using Leopotam.Ecs;

namespace Ingame.Gunplay
{
    public sealed class GunShootInputConverterSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        
        private readonly EcsFilter<GunModel, ShootTimerComponent, InHandsTag, HudIsVisibleTag>
                        .Exclude<AwaitingReloadTag, AwaitingShutterDistortionTag> _gunsFilter;
        
        private readonly EcsFilter<ShootInputEvent> _shootInputEvent;

        public void Run()
        {
            if (_shootInputEvent.IsEmpty())
                return;

            foreach (var i in _gunsFilter)
            {
                ref var gunEntity = ref _gunsFilter.GetEntity(i);
                ref var gunModel = ref _gunsFilter.Get1(i);
                ref var shootTimerComponent = ref _gunsFilter.Get2(i);
                var gunData = gunModel.gunData;

                bool shotCanBePerformed = shootTimerComponent.timePassedFromLastShot > gunData.PauseBetweenShots;
                shotCanBePerformed = shotCanBePerformed && gunEntity.Has<BulletIsInShutterTag>();

                if (shotCanBePerformed)
                {
                    shootTimerComponent.timePassedFromLastShot = 0;

                    gunEntity.Del<BulletIsInShutterTag>();
                    gunEntity.Get<AwaitingShotTag>();

                    TryMovingBulletFromMagazineToTheShutter(gunEntity);
                }
            }
        }

        private void TryMovingBulletFromMagazineToTheShutter(EcsEntity gunEntity)
        {
            if (gunEntity.Has<GunMagazineComponent>())
            {
                ref var magazineComp = ref gunEntity.Get<GunMagazineComponent>(); 
                
                if (magazineComp.amountOfBullets < 1)
                    return;
                
                magazineComp.amountOfBullets -= 1;
                gunEntity.Get<BulletIsInShutterTag>();
            }
        }
    }
}