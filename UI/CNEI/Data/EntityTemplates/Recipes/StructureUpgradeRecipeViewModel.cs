namespace CryoFall.CNEI.UI.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.Systems.Construction;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class StructureUpgradeRecipeViewModel: RecipeViewModel
    {
        public override string ResourceDictionaryName => "StructureUpgradeRecipeDataTemplate.xaml";

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

            StructureVM = structureViewModel;

            InputItemsList = upgradeEntry.RequiredItems
                .Select(item => EntityViewModelsManager.GetEntityViewModel(item.ProtoItem))
                .ToList();
            InputItemsList.Add(structureViewModel);

            InputItemsVMList = upgradeEntry.RequiredItems
                .Select(item => new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(item.ProtoItem),
                    item.Count))
                .ToList().AsReadOnly();

            if (!(EntityViewModelsManager.GetEntityViewModel(upgradeEntry.ProtoStructure)
                is ProtoObjectStructureViewModel newStructureViewModel))
            {
                throw new Exception("CNEI: Can not find ProtoObjectStructureViewModel for " +
                                    upgradeEntry.ProtoStructure);
            }

            OutputItemsList.Add(newStructureViewModel);

            UpgradedStructureVM = newStructureViewModel;

            // Can not simply get it from result entityVM because it can has not initilized Tech.
            //ListedInTechNodes = newStructureViewModel.ListedInTechNodes;
            ListedInTechNodes = upgradeEntry.ProtoStructure.ListedInTechNodes
                .Select(EntityViewModelsManager.GetEntityViewModel)
                .ToList().AsReadOnly();
            IsAutoUnlocked = structureViewModel.IsAutoUnlocked;
        }

        public ProtoEntityViewModel StructureVM { get; }

        public ProtoEntityViewModel UpgradedStructureVM { get; }

        public IReadOnlyList<BaseViewModel> InputItemsVMList { get; protected set; }

        public bool IsAutoUnlocked { get; protected set; }
    }
}