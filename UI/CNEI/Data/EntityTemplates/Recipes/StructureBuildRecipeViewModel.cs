﻿namespace CryoFall.CNEI.UI.Data
{
    using System;
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.Items.Tools.Toolboxes;
    using AtomicTorch.CBND.CoreMod.Systems.Construction;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class StructureBuildRecipeViewModel: RecipeViewModel
    {
        public override string ResourceDictionaryName => "StructureBuildRecipeDataTemplate.xaml";

        public override string RecipeTypeName => "Structure Build";

        /// <summary>
        /// Constructor for build recipe for IProtoStructure.
        /// Must be used only in InitEntityRelationships phase.
        /// </summary>
        /// <param name="structureViewModel">View Model of IProtoStructure.</param>
        /// <param name="config">Building config.</param>
        public StructureBuildRecipeViewModel([NotNull] ProtoObjectStructureViewModel structureViewModel,
            [NotNull] IConstructionStageConfigReadOnly config)
            : base(structureViewModel.ProtoEntity)
        {
            if (!EntityViewModelsManager.EntityDictonaryCreated)
            {
                throw new Exception("CNEI: Build constructor used before all entity VMs sets.");
            }

            InputItemsVMList = config.StageRequiredItems
                .Select(item => new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(item.ProtoItem),
                    item.Count * config.StagesCount))
                .ToList().AsReadOnly();

            StructureVM = structureViewModel;

            StationsList = EntityViewModelsManager.GetAllEntityViewModelsByType<IProtoItemToolToolbox>().AsReadOnly();
            ListedInTechNodes = structureViewModel.ListedInTechNodes;
            IsAutoUnlocked = structureViewModel.IsAutoUnlocked;
        }

        public ProtoEntityViewModel StructureVM { get; }
    }
}