namespace CryoFall.CNEI.UI.Data
{
    using System;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects.Buffs;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects.Debuffs;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects.Neutral;
    using AtomicTorch.CBND.CoreMod.Helpers.Client;
    using AtomicTorch.CBND.CoreMod.Items.Food;
    using AtomicTorch.CBND.CoreMod.Items.Implants;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoItemFoodViewModel : ProtoItemViewModel
    {
        public ProtoItemFoodViewModel([NotNull] IProtoItemFood food) : base(food)
        {
        }

        /// <summary>
        /// Initialize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is IProtoItemFood food)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Stay fresh",
                    food.FreshnessDuration == TimeSpan.Zero
                        ? "forever"
                        : ClientTimeFormatHelper.FormatTimeDuration(food.FreshnessDuration)));
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
                if (Math.Abs(food.StaminaRestore) > 0)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Stamina restore", food.StaminaRestore));
                }
            }

            // TODO: Rework this.
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
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectNausea>(), 0.5));
                    break;
                case ItemBottleWaterStale _:
                    EntityInformation.Add(new ViewModelEntityInformation("Effect depends on",
                        EntityViewModelsManager.GetEntityViewModelByType<ItemImplantArtificialStomach>()));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectNausea>(), 0.5));
                    break;
                case ItemCactusDrink _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectHealthyFood>(), 0.1));
                    break;
                case ItemCarbonara _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectHeartyFood>(), 0.5));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectSavoryFood>(), 0.1));
                    break;
                case ItemCarrotGrilled _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectSavoryFood>(), 0.15));
                    break;
                case ItemCheese _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectSavoryFood>(), 0.1));
                    break;
                case ItemChiliBeans _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectHeartyFood>(), 0.3));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectSavoryFood>(), 0.1));
                    break;
                case ItemChiliPepper _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectPain>(), 0.3));
                    break;
                case ItemCoffeeBeans _:
                    EntityInformation.Add(new ViewModelEntityInformation("Effect depends on",
                        EntityViewModelsManager.GetEntityViewModelByType<ItemImplantArtificialStomach>()));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectNausea>(), 0.2));
                    break;
                case ItemCoffeeCup _:
                    EntityInformation.Add(new ViewModelEntityInformation("Effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectEnergyRush>(), 0.2));
                    break;
                case ItemDrinkEnergy _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectEnergyRush>(), 0.3));
                    break;
                case ItemDrinkHerbal _:
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectToxins>(), 0.05));
                    break;
                case ItemEggsRaw _:
                    EntityInformation.Add(new ViewModelEntityInformation("Effect depends on",
                        EntityViewModelsManager.GetEntityViewModelByType<ItemImplantArtificialStomach>()));
                    EntityInformation.Add(new ViewModelEntityInformation("Effect chance", "33%"));
                    EntityInformation.Add(new ViewModelEntityInformation("Effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectNausea>(), 0.5));
                    break;
                case ItemInsectMeatFried _:
                    EntityInformation.Add(new ViewModelEntityInformation("Effect depends on",
                        EntityViewModelsManager.GetEntityViewModelByType<ItemImplantArtificialStomach>()));
                    EntityInformation.Add(new ViewModelEntityInformation("Effect chance", "25%"));
                    EntityInformation.Add(new ViewModelEntityInformation("Effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectNausea>(), 0.25));
                    break;
                case ItemInsectMeatRaw _:
                    EntityInformation.Add(new ViewModelEntityInformation("Effect depends on",
                        EntityViewModelsManager.GetEntityViewModelByType<ItemImplantArtificialStomach>()));
                    EntityInformation.Add(new ViewModelEntityInformation("Effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectNausea>(), 0.75));
                    break;
                case ItemLiquor _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectDrunk>(), 0.35));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectHealingSlow>(), 0.1));
                    break;
                case ItemMeatRaw _:
                    EntityInformation.Add(new ViewModelEntityInformation("Effect depends on",
                        EntityViewModelsManager.GetEntityViewModelByType<ItemImplantArtificialStomach>()));
                    EntityInformation.Add(new ViewModelEntityInformation("Effect chance", "50%"));
                    EntityInformation.Add(new ViewModelEntityInformation("Effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectNausea>(), 0.75));
                    break;
                case ItemMilk _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectHealthyFood>(), 0.05));
                    break;
                case ItemMushroomPink _:
                    EntityInformation.Add(new ViewModelEntityInformation("Effect depends on",
                        EntityViewModelsManager.GetEntityViewModelByType<ItemImplantArtificialStomach>()));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectNausea>(), 1.0));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectToxins>(), 1.0));
                    break;
                case ItemMushroomRust _:
                    EntityInformation.Add(new ViewModelEntityInformation("Effect depends on",
                        EntityViewModelsManager.GetEntityViewModelByType<ItemImplantArtificialStomach>()));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectNausea>(), 0.5));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectToxins>(), 0.2));
                    break;
                case ItemOnigiri _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectSavoryFood>(), 0.15));
                    break;
                case ItemPieBerry _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectHeartyFood>(), 0.4));
                    break;
                case ItemSaladFruit _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectHealthyFood>(), 0.25));
                    break;
                case ItemSaladVegetable _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectHealthyFood>(), 0.3));
                    break;
                case ItemSandwich _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectSavoryFood>(), 0.2));
                    break;
                case ItemStew _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectHeartyFood>(), 0.6));
                    break;
                case ItemTequila _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectDrunk>(), 0.4));
                    break;
                case ItemTincture _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectDrunk>(), 0.2));
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
                case ItemYuccaFried _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectSavoryFood>(), 0.1));
                    break;
            }
        }
    }
}