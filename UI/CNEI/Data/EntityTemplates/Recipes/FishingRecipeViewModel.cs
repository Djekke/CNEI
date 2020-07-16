namespace CryoFall.CNEI.UI.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.Items.Fishing.Base;
    using AtomicTorch.CBND.CoreMod.Items.Tools;
    using AtomicTorch.CBND.CoreMod.Systems.Crafting;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class FishingRecipeViewModel : RecipeViewModel
    {
        public override string ResourceDictionaryName => "FishingRecipeDataTemplate.xaml";

        public override string RecipeTypeName => "Fishing";

        /// <summary>
        /// Constructor for fishing recipe.
        /// Must be used only in InitEntityRelationships phase.
        /// </summary>
        /// <param name="entityViewModel">View Model of fish entity.</param>
        public FishingRecipeViewModel([NotNull] ProtoItemFishViewModel fishViewModel) : base(fishViewModel.ProtoEntity)
        {
            if (!EntityViewModelsManager.EntityDictionaryCreated)
            {
                throw new Exception("CNEI: Droplist constructor used before all entity VMs sets.");
            }

            OutputItemsList.Add(fishViewModel);

            FishVM = fishViewModel;

            var protoFish = fishViewModel.ProtoEntity as IProtoItemFish;

            InputItemsList = protoFish.BaitWeightList.Entries
                .Select(e => EntityViewModelsManager.GetEntityViewModel(e.Value))
                .ToList();

            BaitVMList = protoFish.BaitWeightList.Entries
                .Select(e => new ViewModelEntityWithCount(
                    EntityViewModelsManager.GetEntityViewModel(e.Value),
                    e.Weight + "%"))
                .ToList().AsReadOnly();

            IsInSaltWaterText = protoFish.IsSaltwaterFish ? "Yes" : "No";

            RequiredFishingKnowledgeLevel = protoFish.RequiredFishingKnowledgeLevel;

            icon = fishViewModel.Icon;

            FishingToolsVMList = EntityViewModelsManager.GetAllEntityViewModelsByType<IProtoItemToolFishing>().ToList();
        }

        public string IsInSaltWaterText { get; protected set; }

        public byte RequiredFishingKnowledgeLevel { get; protected set; }

        public ProtoEntityViewModel FishVM { get; protected set; }

        public IReadOnlyList<BaseViewModel> BaitVMList { get; protected set; }

        public IReadOnlyList<BaseViewModel> FishingToolsVMList { get; protected set; }
    }
}