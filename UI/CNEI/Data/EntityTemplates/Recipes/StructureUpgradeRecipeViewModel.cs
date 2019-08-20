namespace CryoFall.CNEI.UI.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using AtomicTorch.CBND.CoreMod.Systems.Construction;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class StructureUpgradeRecipeViewModel: RecipeViewModel
    {
        public override string RecipeTypeName => "Structure upgrade";

        /// <summary>
        /// Constructor for upgrade recipe for IProtoStructure.
        /// Must be used only in InitEntityRelationships phase.
        /// </summary>
        /// <param name="structureViewModel">View Model of IProtoStructure.</param>
        /// <param name="upgradeEntry">Entry of upgrade config.</param>
        public StructureUpgradeRecipeViewModel([NotNull] ProtoObjectStructureViewModel structureViewModel,
            [NotNull] IConstructionUpgradeEntryReadOnly upgradeEntry)
            : base(upgradeEntry.ProtoStructure)
        {
            if (!EntityViewModelsManager.EntityDictonaryCreated)
            {
                throw new Exception("CNEI: Upgrade constructor used before all entity VMs sets.");
            }

            var inputTempList = new List<BaseViewModel>() { new ViewModelEntityWithCount(structureViewModel) };
            inputTempList.AddRange(upgradeEntry.RequiredItems
                .Select(item => new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(item.ProtoItem),
                    item.Count)));
            InputItemsVMList = inputTempList.AsReadOnly();

            if (!(EntityViewModelsManager.GetEntityViewModel(upgradeEntry.ProtoStructure)
                is ProtoObjectStructureViewModel newStructureViewModel))
            {
                throw new Exception("CNEI: Can not find ProtoObjectStructureViewModel for " +
                                    upgradeEntry.ProtoStructure);
            }

            OutputItemsVMList = new List<BaseViewModel>()
            {
                new ViewModelEntityWithCount(newStructureViewModel)
            }.AsReadOnly();

            OriginText = "Upgrade from:";
            IsStationCraft = Visibility.Visible;
            StationsList = new List<ProtoEntityViewModel>() { structureViewModel }.AsReadOnly();
            // Can not simply get it from result entityVM because it can has not initilized Tech.
            //ListedInTechNodes = newStructureViewModel.ListedInTechNodes;
            ListedInTechNodes = upgradeEntry.ProtoStructure.ListedInTechNodes
                .Select(EntityViewModelsManager.GetEntityViewModel)
                .ToList().AsReadOnly();
            IsAutoUnlocked = structureViewModel.IsAutoUnlocked;
        }
    }
}