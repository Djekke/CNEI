namespace CryoFall.CNEI.UI.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.Items.Fishing.Base;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using CryoFall.CNEI.Managers;

    public class ViewModelFishingCalculator : BaseViewModel
    {
        private readonly List<ProtoEntityViewModel> fishViewModelList;

        private readonly List<ProtoEntityViewModel> baitViewModelList;

        public Dictionary<string, double> BaitWeightSumDictionary;

        public Dictionary<string, BaseCommand> BaitSortDictionary { get; set; }

        private bool isSaltWaterFish = false;

        private int knowledgeLevel = 0;

        public bool IsSaltWaterFish
        {
            get { return isSaltWaterFish; }
            set
            {
                if (value == isSaltWaterFish)
                {
                    return;
                }

                isSaltWaterFish = value;
                NotifyThisPropertyChanged();
                CalculateBaitWeightSum();
                FishDetailsList.Refresh();
            }
        }

        public int KnowledgeLevel
        {
            get { return knowledgeLevel; }
            set
            {
                if (value == knowledgeLevel)
                {
                    return;
                }

                knowledgeLevel = value;
                NotifyThisPropertyChanged();
                CalculateBaitWeightSum();
                FishDetailsList.Refresh();
            }
        }
        private bool FishingFilter(FishDetails fishDetails)
        {
            return fishDetails.IsSaltWaterFish == IsSaltWaterFish &&
                   fishDetails.RequiredFishingKnowledgeLevel <= KnowledgeLevel;
        }

        public BaseCommand SortByName { get; }

        public BaseCommand SortByKnowledge { get; }

        public void SortByPropertyName(string propertyName)
        {
            if(FishDetailsList.SortPropertyName == propertyName)
            {
                FishDetailsList.SortDirection = !FishDetailsList.SortDirection;
            }
            else
            {
                FishDetailsList.SortPropertyName = propertyName;
                FishDetailsList.SortDirection = true;
            }
            FishDetailsList.Refresh();
        }

        public FilteredObservableWithSorting<FishDetails> FishDetailsList { get; private set; }

        public ViewModelFishingCalculator()
        {
            fishViewModelList = EntityViewModelsManager.GetAllEntityViewModelsByType<ProtoItemFish>();
            baitViewModelList = EntityViewModelsManager.GetAllEntityViewModelsByType<ProtoItemFishingBait>();
            BaitWeightSumDictionary = baitViewModelList.ToDictionary(b => b.Type, b => 0d);
            BaitSortDictionary = baitViewModelList
                .ToDictionary(b => b.Type,
                              b => new ActionCommand(() =>
                                SortByPropertyName("BaitWeightDictionary[" + b.Type + "].Chance")) as BaseCommand);
            FishDetailsList = new FilteredObservableWithSorting<FishDetails>();
            foreach (var fishViewModel in fishViewModelList)
            {
                if(fishViewModel.ProtoEntity is IProtoItemFish fish)
                {

                    FishDetailsList.Add(new FishDetails()
                    {
                        FishViewModel = fishViewModel,
                        Name = fishViewModel.Title,
                        IsSaltWaterFish = fish.IsSaltwaterFish,
                        RequiredFishingKnowledgeLevel = fish.RequiredFishingKnowledgeLevel,
                        BaitWeightDictionary = fish.BaitWeightList.Entries
                                               .ToDictionary(e => e.Value.Id,
                                               e => new FishDetails.BaitWithWeight(this)
                                               {
                                                   Name = e.Value.Name,
                                                   Id = e.Value.Id,
                                                   Weight = e.Weight
                                               })
                    });
                }
            }
            FishDetailsList.RemoveAllFilters();
            FishDetailsList.AddFilter(FishingFilter);
            CalculateBaitWeightSum();
            SortByPropertyName("Name");

            SortByName = new ActionCommand(() => SortByPropertyName("Name"));
            SortByKnowledge = new ActionCommand(() => SortByPropertyName("RequiredFishingKnowledgeLevel"));
        }

        private void CalculateBaitWeightSum()
        {
            foreach (var baitVM in baitViewModelList)
            {
                BaitWeightSumDictionary[baitVM.Type] = 0d;
            }
            foreach (FishDetails fishDetails in FishDetailsList.Items.ToList())
            {
                foreach (var baitVM in baitViewModelList)
                {
                    BaitWeightSumDictionary[baitVM.Type] += fishDetails.BaitWeightDictionary[baitVM.Type].Weight;
                }
            }
        }

        public class FishDetails
        {
            public ProtoEntityViewModel FishViewModel { get; set; }

            public string Name { get; set; }

            public bool IsSaltWaterFish { get; set; }

            public int RequiredFishingKnowledgeLevel { get; set; }

            public Dictionary<string, BaitWithWeight> BaitWeightDictionary { get; set; }

            public class BaitWithWeight
            {
                private ViewModelFishingCalculator viewModelFishingCalculator;

                public string Name { get; set; }

                public string Id { get; set; }

                public double Weight { get; set; }

                public double Chance
                {
                    get
                    {
                        return (Weight /
                                viewModelFishingCalculator.BaitWeightSumDictionary[Id]);
                    }
                }

                public BaitWithWeight(ViewModelFishingCalculator viewModelFishingCalculator)
                {
                    this.viewModelFishingCalculator = viewModelFishingCalculator;
                }
            }
        }
    }
}