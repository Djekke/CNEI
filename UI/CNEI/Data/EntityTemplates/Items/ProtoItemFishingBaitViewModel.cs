namespace CryoFall.CNEI.UI.Data
{
    using System.Collections.Generic;
    using AtomicTorch.CBND.CoreMod.Items.Fishing.Base;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoItemFishingBaitViewModel : ProtoItemViewModel
    {
        public ProtoItemFishingBaitViewModel([NotNull] IProtoItemFishingBait bait) : base(bait)
        {
        }

        public void AddRelatedFish([NotNull] ProtoEntityViewModel fishViewModel, double chance)
        {
            if (RelatedFish.Count == 0)
            {
                BaitUsage = new FishingViewModel(this, fishViewModel, chance);
                EntityViewModelsManager.AddRecipe(BaitUsage);
            }
            else
            {
                BaitUsage.AddBaitUsage(fishViewModel, chance);
            }

            RelatedFish.Add(fishViewModel);
        }

        public List<ProtoEntityViewModel> RelatedFish { get; private set; }
            = new List<ProtoEntityViewModel>();

        public FishingViewModel BaitUsage { get; private set; }
    }
}