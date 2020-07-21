namespace CryoFall.CNEI.UI.Data
{
    using System;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects;
    using AtomicTorch.CBND.CoreMod.Helpers.Client;
    using AtomicTorch.CBND.CoreMod.Items.Food;
    using AtomicTorch.CBND.GameApi.Scripting;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public partial class ProtoItemFoodViewModel : ProtoItemViewModel
    {
        public ProtoItemFoodViewModel([NotNull] IProtoItemFood food) : base(food)
        {
        }

        /// <summary>
        /// Initialize entity relationships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            base.InitAdditionalRecipes();

            if (!(ProtoEntity is IProtoItemFood food))
            {
                return;
            }

            foreach (EffectAction effect in food.Effects)
            {
                var statusEffectVM = EntityViewModelsManager.GetEntityViewModel(effect.ProtoStatusEffect);
                if (statusEffectVM is ProtoStatusEffectViewModel statusEffectViewModel)
                {
                    statusEffectViewModel.AddConsumable(this, effect.Intensity);
                }
                else
                {
                    Api.Logger.Error("CNEI: It's not a status effect " + statusEffectVM + " in " + this);
                }
            }
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
                foreach(EffectAction effect in food.Effects)
                {
                    EntityInformation.Add(new ViewModelEntityInformation(effect.Intensity > 0 ? "Add effect" : "Remove effect",
                        EntityViewModelsManager.GetEntityViewModel(effect.ProtoStatusEffect), effect.Intensity));
                }
            }
        }
    }
}