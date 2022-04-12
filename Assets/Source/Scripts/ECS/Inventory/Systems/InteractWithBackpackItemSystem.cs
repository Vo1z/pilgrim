﻿using Ingame.Health;
using Ingame.Interaction.Common;
using Ingame.Movement;
using Ingame.Player;
using Leopotam.Ecs;
using Support.Extensions;

namespace Ingame.Inventory
{
    public sealed class InteractWithBackpackItemSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly EcsFilter<TransformModel, BackpackItemTag, PerformInteractionTag> _backpackInteractItem;
        private readonly EcsFilter<PlayerModel, InventoryComponent, HealthComponent> _playerFilter;

        public void Run()
        {
            if(_playerFilter.IsEmpty())
                return;
            
            ref var playerHealthEntity = ref _playerFilter.GetEntity(0);
            ref var playerInventory = ref _playerFilter.Get2(0);

            _world.NewEntity().Get<UpdateBackpackAppearanceEvent>();

            foreach (var i in _backpackInteractItem)
            {
                ref var itemEntity = ref _backpackInteractItem.GetEntity(i);
                ref var itemTransformModel = ref _backpackInteractItem.Get1(i);
                
                itemEntity.Del<PerformInteractionTag>();
                itemTransformModel.transform.SetGameObjectInactive();

                if (itemEntity.Has<AdrenalineTag>())
                {
                    playerInventory.currentNumberOfAdrenaline--;
                }
                
                if (itemEntity.Has<BandageTag>())
                {
                    playerHealthEntity.Get<StopBleedingTag>();
                    playerInventory.currentNumberOfBandages--;
                }
                
                if (itemEntity.Has<CreamTag>())
                {
                    playerInventory.currentNumberOfCreamTubes--;
                }
                
                if (itemEntity.Has<EnergyDrinkTag>())
                {
                    playerInventory.currentNumberOfEnergyDrinks--;
                }
                
                if (itemEntity.Has<InhalatorTag>())
                {
                    playerInventory.currentNumberOfInhalators--;
                }
                
                if (itemEntity.Has<MorphineTag>())
                {
                    playerInventory.currentNumberOfMorphine--;
                }
            }
        }
    }
}