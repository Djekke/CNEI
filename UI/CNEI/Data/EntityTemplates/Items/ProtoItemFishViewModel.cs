namespace CryoFall.CNEI.UI.Data
{
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.Items.Fishing.Base;
    using AtomicTorch.CBND.GameApi.Scripting;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoItemFishViewModel : ProtoItemViewModel
    {
        public ProtoItemFishViewModel([NotNull] IProtoItemFish fish) : base(fish)
        {
        }

        /// <summary>
        /// Initialize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is IProtoItemFish fish)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Max length",
                    fish.MaxLength));
                EntityInformation.Add(new ViewModelEntityInformation("Max weight",
                    fish.MaxWeight));
                EntityInformation.Add(new ViewModelEntityInformation("Required fishing knowledge level",
                    fish.RequiredFishingKnowledgeLevel));
                EntityInformation.Add(new ViewModelEntityInformation("Live in salt water",
                    fish.IsSaltwaterFish ? "Yes" : "No"));
            }
        }

        /// <summary>
        /// Initialize entity relationships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            base.InitAdditionalRecipes();

            if (!(ProtoEntity is IProtoItemFish fish))
            {
                return;
            }

            if(fish.DropItemsList != null &&
                fish.DropItemsList.EnumerateAllItems().Any())
            {
                Droplist = new DroplistRecipeViewModel(this, fish.DropItemsList.EnumerateAllItems());
                EntityViewModelsManager.AddRecipe(Droplist);
            }

            if(fish.BaitWeightList.Entries.Count > 0)
            {
                foreach(var baitWithWeight in fish.BaitWeightList.Entries)
                {
                    var baitVM = EntityViewModelsManager.GetEntityViewModel(baitWithWeight.Value);
                    if (baitVM is ProtoItemFishingBaitViewModel baitViewModel)
                    {
                        baitViewModel.AddRelatedFish(this, baitWithWeight.Weight);
                    }
                    else
                    {
                        Api.Logger.Error("CNEI: Wrong bait veiw model " + baitVM);
                    }
                }
            }
        }

        public RecipeViewModel Droplist { get; private set; }
    }
}