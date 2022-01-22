﻿using Leopotam.Ecs;
using UnityEngine;

namespace Ingame
{
    public sealed class GunShootSystem : IEcsRunSystem
    {
        private readonly EcsFilter<GunModel, ShootEvent> _shootingGunFilter;

        public void Run()
        {
            foreach (var i in _shootingGunFilter)
            {
                ref var gunModel = ref _shootingGunFilter.Get1(i);

                var hitObject = GetHitObjectWithRayCast(gunModel.barrelTransform);

                if(hitObject == null)
                    return;
                
                if(hitObject.name.Equals("Target"))
                    Object.Destroy(hitObject);
            }
        }

        private GameObject GetHitObjectWithRayCast(Transform barrelTransform)
        {
            var ray = new Ray(barrelTransform.position, barrelTransform.forward);
            var layerMask = ~LayerMask.GetMask("Ignore Raycast", "HUD", "PlayerStatic", "Weapon");
            
            if (Physics.Raycast(ray, out RaycastHit hit, 200f, layerMask, QueryTriggerInteraction.Ignore))
                return hit.transform.gameObject;
            
            return null;
        }
    }
}