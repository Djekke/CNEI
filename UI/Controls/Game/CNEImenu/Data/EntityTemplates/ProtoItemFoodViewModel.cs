namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects.Buffs;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects.Debuffs;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects.Neutral;
    using AtomicTorch.CBND.CoreMod.Items.Food;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System;

    public class ProtoItemFoodViewModel : ProtoItemViewModel
    {
        public ProtoItemFoodViewModel([NotNull] IProtoItemFood food) : base(food)
        {
        }

        /// <summary>
        /// Initilize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is IProtoItemFood food)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Stay fresh",
                    food.FreshnessDuration == TimeSpan.Zero ? "forever" : food.FreshnessDuration.ToString("g")));
                if (Math.Abs(food.FoodRestore) > 0)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Food restore", food.FoodRestore));
                }
                if (Math.Abs(food.WaterRestore) > 0)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Water restore", food.WaterRestore));
                }
                if (Math.Abs(food.HealthRestore) > 0)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Health restore", food.HealthRestore));
                }
                if (Math.Abs(food.EnergyRestore) > 0)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Energy restore", food.EnergyRestore));
                }
            }

            // Hardcoded information
            AddStatusEffectsInformation();
        }

        private void AddStatusEffectsInformation()
        {
            switch (ProtoEntity)
            {
                case ItemBeer _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectDrunk>(), 0.2));
                    break;
                case ItemBottleWaterSalty _:
                case ItemBottleWaterStale _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectNausea>(), 0.5));
                    break;
                case ItemChiliPepper _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectPain>(), 0.3));
                    break;
                case ItemCoffeeBeans _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectNausea>(), 0.2));
                    break;
                case ItemCoffeeCup _:
                    EntityInformation.Add(new ViewModelEntityInformation("Effect depends on", "freshnes"));
                    EntityInformation.Add(new ViewModelEntityInformation("Effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectEnergyRush>(), 0.2));
                    break;
                case ItemDrinkEnergy _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectEnergyRush>(), 0.3));
                    break;
                case ItemDrinkHerbal _:
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectToxins>(), 0.025));
                    break;
                case ItemEggsRaw _:
                    EntityInformation.Add(new ViewModelEntityInformation("Effect chance", "33%"));
                    EntityInformation.Add(new ViewModelEntityInformation("Effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectNausea>(), 0.5));
                    break;
                case ItemLiquor _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectDrunk>(), 0.35));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectHealingSlow>(), 0.1));
                    break;
                case ItemMeatRaw _:
                    EntityInformation.Add(new ViewModelEntityInformation("Effect chance", "50%"));
                    EntityInformation.Add(new ViewModelEntityInformation("Effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectNausea>(), 0.75));
                    break;
                case ItemMushroomPink _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectNausea>(), 1.0));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectToxins>(), 1.0));
                    break;
                case ItemMushroomRust _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectNausea>(), 1.0));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectToxins>(), 0.1));
                    break;
                case ItemTincture _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectHealingSlow>(), 0.25));
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectToxins>(), 0.1));
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectRadiationPoisoning>(), 0.1));
                    break;
                case ItemVodka _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectDrunk>(), 0.45));
                    break;
                case ItemWine _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectDrunk>(), 0.25));
                    break;
            }
        }
    }
}