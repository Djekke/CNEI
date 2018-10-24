namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects.Buffs;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects.Debuffs;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects.Invisible;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects.Neutral;
    using AtomicTorch.CBND.CoreMod.Items.Medical;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System;

    public class ProtoItemMedicalViewModel : ProtoItemViewModel
    {
        public ProtoItemMedicalViewModel([NotNull] IProtoItemMedical medical) : base(medical)
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
                if (Math.Abs(medical.EnergyRestore) > 0)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Energy restore", medical.EnergyRestore));
                }
                if (medical.MedicalToxicity > 0)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectMedicineOveruse>(),
                        medical.MedicalToxicity));
                }
            }

            // Hardcoded information
            AddStatusEffectsInformation();
        }

        private void AddStatusEffectsInformation()
        {
            switch (ProtoEntity)
            {
                case ItemAntiNausea _:
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectNausea>(), 1.0));
                    break;
                case ItemAntiRadiation _:
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectRadiationPoisoning>(), 0.3));
                    break;
                case ItemAntiRadiationPreExposure _:
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectRadiationPoisoning>(), 0.1));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectProtectionRadiation>(), 0.5));
                    break;
                case ItemAntiToxin _:
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectToxins>(), 0.3));
                    break;
                case ItemAntiToxinPreExposure _:
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectToxins>(), 0.1));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectProtectionToxins>(), 0.5));
                    break;
                case ItemBandage _:
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectBleeding>(), 0.3));
                    break;
                case ItemCigarCheap _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectHigh>(), 0.3));
                    break;
                case ItemCigarNormal _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectHigh>(), 0.5));
                    break;
                case ItemCigarPremium _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectHigh>(), 0.7));
                    break;
                case ItemHemostatic _:
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectBleeding>(), 1.0));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectProtectionBleeding>(), 0.25));
                    break;
                case ItemHerbGreen _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectHealingSlow>(), 0.1));
                    break;
                case ItemHerbPurple _:
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectRadiationPoisoning>(), 0.05));
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectToxins>(), 0.05));
                    break;
                case ItemHerbRed _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectEnergyRush>(), 0.05));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectToxins>(), 0.05));
                    break;
                case ItemMedkit _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectHealingSlow>(), 0.75));
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectBleeding>(), 1.0));
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectNausea>(), 1.0));
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectBrokenLeg>(), 1.0));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectSplintedLeg>(), 1.0));
                    break;
                case ItemNeuralImplant _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectPain>(), 1.0));
                    EntityInformation.Add(new ViewModelEntityInformation("Add LP", 50));
                    break;
                case ItemPainkiller _:
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectPain>(), 1.0));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectProtectionPain>(), 0.5));
                    break;
                case ItemPeredozin _:
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectMedicineOveruse>(), 1.0));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectAnalBlockage>(), 1.0));
                    break;
                case ItemRemedyHerbal _:
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectNausea>(), 1.0));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectHealingSlow>(), 0.4));
                    break;
                case ItemSplint _:
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectBrokenLeg>(), 1.0));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectSplintedLeg>(), 1.0));
                    break;
                case ItemStimpack _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectHealingFast>(), 0.4));
                    break;
                case ItemStrengthBoostBig _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectStrength>(), 0.7));
                    break;
                case ItemStrengthBoostSmall _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectStrength>(), 0.3));
                    break;
            }
        }
    }
}