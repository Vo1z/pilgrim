﻿using Ingame.Guns;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame
{
    public sealed class GunRecoilSystem : IEcsRunSystem
    {
        private readonly EcsFilter<GunModel, TransformModel> _gunFilter;
        
        public void Run()
        {
            foreach (var i in _gunFilter)
            {
                ref var gunEntity = ref _gunFilter.GetEntity(i);
                ref var gunModel = ref _gunFilter.Get1(i);
                ref var gunTransformModel = ref _gunFilter.Get2(i);
                var gunData = gunModel.gunData;
                var gunTransform = gunTransformModel.transform;

                bool isShooting = gunEntity.Has<ShootComponent>();
                
                var targetLocalRotation = GetGunRotationDueToRecoil(gunData, gunTransformModel);
                var targetLocalPos = GetGunLocalPositionDueToRecoil(gunData, gunTransformModel, isShooting);
                var transitionSpeed = gunData.Instability * Time.deltaTime;

                if(isShooting)
                    gunTransform.localRotation = Quaternion.Lerp(gunTransform.localRotation, targetLocalRotation, transitionSpeed);
                
                gunTransform.localPosition = Vector3.Lerp(gunTransform.localPosition, targetLocalPos, transitionSpeed);
            }
        }

        private Quaternion GetGunRotationDueToRecoil(GunData gunData, TransformModel gunTransformModel)
        {

            var recoilAngleOffset = Quaternion.AngleAxis(-gunData.VerticalRecoilForce, Vector3.left);
            var gunRotationDueRecoil = gunTransformModel.initialLocalRotation * recoilAngleOffset;
            
            return gunRotationDueRecoil;
        }

        private Vector3 GetGunLocalPositionDueToRecoil(GunData gunData, TransformModel gunTransformModel, bool isShooting)
        {
            if (!isShooting)
                return gunTransformModel.initialLocalPos;
            
            var localPosOffset = Vector3.back * gunData.FrontRecoilForce;
            localPosOffset = Vector3.ClampMagnitude(localPosOffset, gunData.MinMaxRecoilPositionOffset);
            var gunLocalPosDueRecoil = gunTransformModel.initialLocalPos + localPosOffset;

            return gunLocalPosDueRecoil;
        }
    }
}