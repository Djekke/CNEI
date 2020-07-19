namespace CryoFall.CNEI.UI.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.Items.Fishing.Base;
    using AtomicTorch.CBND.CoreMod.Items.Tools;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.GameEngine.Common.Extensions;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class FishingViewModel : RecipeViewModel
    {
        public override string ResourceDictionaryName => "FishingDataTemplate.xaml";

        public override string RecipeTypeName => "Fishing";

        /// <summary>
        /// Constructor for fishing recipe.
        /// Must be used only in InitEntityRelationships phase.
        /// </summary>
        /// <param name="baitViewModel">View Model of fishing bait entity.</param>
        /// <param name="fishViewModel">View Model of fish entity.</param>
        /// <param name="chance">Chance for catching this fish with this bait.</param>
        public FishingViewModel([NotNull] ProtoEntityViewModel baitViewModel,
            [NotNull] ProtoEntityViewModel fishViewModel,
            double weight)
            : base(baitViewModel.ProtoEntity)
        {
            if (!EntityViewModelsManager.EntityDictionaryCreated)
            {
                throw new Exception("CNEI: Droplist constructor used before all entity VMs sets.");
            }

            BaitEntity = baitViewModel;

            InputItemsList.Add(baitViewModel);

            AddBaitUsage(fishViewModel, weight);

            StationsList = EntityViewModelsManager.GetAllEntityViewModelsByType<IProtoItemToolFishing>()
                .ToList().AsReadOnly();
        }

        public void AddBaitUsage([NotNull] ProtoEntityViewModel fishViewModel, double weight)
        {
            if(fishViewModel.ProtoEntity is ProtoItemFish fish)
            {
                FishDetailsList.Add(new FishDetails()
                {
                    FishViewModel = fishViewModel,
                    Name = fishViewModel.Title,
                    RequiredFishingKnowledgeLevel = fish.RequiredFishingKnowledgeLevel,
                    IsSaltwaterFish = fish.IsSaltwaterFish ? "Yes" : "No",
                    Weight = weight
                });
                FishDetailsList.SortByDesc(e => e.Weight);
            }

            OutputItemsList.Add(fishViewModel);
        }

        public ProtoEntityViewModel BaitEntity { get; protected set; }

        public List<FishDetails> FishDetailsList { get; protected set; }
            = new List<FishDetails>();

        public class FishDetails
        {
            public BaseViewModel FishViewModel { get; set; }

            public string Name { get; set; }

            public string IsSaltwaterFish { get; set; }

            public int RequiredFishingKnowledgeLevel { get; set; }

            public double Weight { get; set; }
        }
    }
}