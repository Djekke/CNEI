﻿namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects.Buffs;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects.Debuffs;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects.Invisible;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects.Neutral;
    using AtomicTorch.CBND.CoreMod.Items.Medical;
    using CryoFall.CNEI.Managers;

    public partial class ProtoItemMedicalViewModel : ProtoItemViewModel
    {
        private void AddStatusEffectsInformation()
        {
            switch (ProtoEntity)
            {
                case ItemAntiMutation _:
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectMutation>(), 1.0));
                    break;
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
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectToxins>(), 0.4));
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
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectHealingSlow>(), 0.1));
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
                case ItemHeatPreExposure _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectProtectionHeat>(), 0.5));
                    break;
                case ItemHemostatic _:
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectBleeding>(), 1.0));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectProtectionBleeding>(), 0.5));
                    break;
                case ItemHerbBlue _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectToxins>(), 0.1));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectNausea>(), 0.05));
                    break;
                case ItemHerbGreen _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectHealingSlow>(), 0.07));
                    break;
                case ItemHerbPurple _:
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectRadiationPoisoning>(), 0.05));
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectToxins>(), 0.05));
                    break;
                case ItemHerbRed _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectEnergyRush>(), 0.1));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectToxins>(), 0.1));
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
                case ItemNeuralEnhancer _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectPain>(), 1.0));
                    EntityInformation.Add(new ViewModelEntityInformation("Add LP",
                        ItemNeuralEnhancer.UsageGivesLearningPointsAmount));
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
                case ItemPsiPreExposure _:
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectProtectionPsi>(), 0.5));
                    break;
                case ItemRemedyHerbal _:
                    EntityInformation.Add(new ViewModelEntityInformation("Remove effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectNausea>(), 1.0));
                    EntityInformation.Add(new ViewModelEntityInformation("Add effect",
                        EntityViewModelsManager.GetEntityViewModelByType<StatusEffectHealingSlow>(), 0.3));
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