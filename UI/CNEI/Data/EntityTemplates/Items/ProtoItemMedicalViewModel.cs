namespace CryoFall.CNEI.UI.Data
{
    using System;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects.Neutral;
    using AtomicTorch.CBND.CoreMod.Items.Medical;
    using AtomicTorch.CBND.GameApi.Scripting;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public partial class ProtoItemMedicalViewModel : ProtoItemViewModel
    {
        public ProtoItemMedicalViewModel([NotNull] IProtoItemMedical medical) : base(medical)
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

            if (!(ProtoEntity is ProtoItemMedical medical))
            {
                return;
            }

            foreach (EffectAction effect in medical.Effects)
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

            if (medical.MedicalToxicity > 0)
            {
                var medicineOveruseVM =
                    EntityViewModelsManager.GetEntityViewModelByType<StatusEffectMedicineOveruse>();
                if (medicineOveruseVM is ProtoStatusEffectViewModel medicineOveruseViewModel)
                {
                    medicineOveruseViewModel.AddConsumable(this, medical.MedicalToxicity);
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

            if (ProtoEntity is ProtoItemMedical medical)
            {
                //EntityInformation.Add(new ViewModelEntityInformation("Medical toxicity", medical.MedicalToxicity));
                if (Math.Abs(medical.FoodRestore) > 0)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Food restore", medical.FoodRestore));
                }
                if (Math.Abs(medical.WaterRestore) > 0)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Water restore", medical.WaterRestore));
                }
                if (Math.Abs(medical.HealthRestore) > 0)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Health restore", medical.HealthRestore));
                }
                if (Math.Abs(medical.StaminaRestore) > 0)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Stamina restore", medical.StaminaRestore));
                }
                if (medical.MedicalToxicity > 0)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectMedicineOveruse>(),
                        medical.MedicalToxicity));
                }
                foreach (EffectAction effect in medical.Effects)
                {
                    EntityInformation.Add(new ViewModelEntityInformation(effect.Intensity > 0 ? "Add effect" : "Remove effect",
                        EntityViewModelsManager.GetEntityViewModel(effect.ProtoStatusEffect), effect.Intensity));
                }
            }
        }
    }
}