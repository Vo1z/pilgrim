using Ingame.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Ingame.CameraWork
{
    public sealed class CameraLeanSystem : IEcsRunSystem
    {
        private readonly EcsFilter<CameraLeanCallback, TransformModel> _mainCameraFilter;

        public void Run()
        {
            foreach (var i in _mainCameraFilter)
            {
                ref var cameraEntity = ref _mainCameraFilter.GetEntity(i);
                ref var leanCallback = ref _mainCameraFilter.Get1(i);
                ref var cameraTransformModel = ref _mainCameraFilter.Get2(i);
                var cameraTransform = cameraTransformModel.transform;

                bool isAiming = cameraEntity.Has<CameraIsAimingTag>();
                var targetLocalPos = isAiming
                    ? cameraTransformModel.initialLocalPos
                    : cameraTransformModel.initialLocalPos + leanCallback.positionOffset;

                if (Vector3.Distance(cameraTransform.localPosition, targetLocalPos) < .001f)
                {
                    cameraTransform.localPosition = targetLocalPos;

                    return;
                }

                cameraTransform.localPosition = Vector3.Lerp
                (cameraTransform.localPosition,
                    targetLocalPos,
                    leanCallback.enterLeanSpeed * Time.fixedDeltaTime
                );
            }
        }
    }
}