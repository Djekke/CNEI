namespace CryoFall.CNEI.UI.Data
{
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.Systems.Crafting;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class HandCraftingRecipeViewModel : BasicRecipeViewModel
    {
        public override string RecipeTypeName => "Hand craft";

        public HandCraftingRecipeViewModel([NotNull] Recipe recipe) : base(recipe)
        {
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (!(ProtoEntity is Recipe recipe))
            {
                return;
            }

            InputItemsList = recipe.InputItems
                .Select(i => EntityViewModelsManager.GetEntityViewModel(i.ProtoItem))
                .ToList();

            InputItemsVMList = recipe.InputItems
                .Select(i =>
                    new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(i.ProtoItem), i.Count))
                .ToList().AsReadOnly();

            OutputItemsList = recipe.OutputItems.Items
                .Select(i => EntityViewModelsManager.GetEntityViewModel(i.ProtoItem))
                .ToList();

            OutputItemsVMList = recipe.OutputItems.Items
                .Select(i => new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(i.ProtoItem),
                    i.Count, i.CountRandom, i.Probability))
                .ToList().AsReadOnly();

            ListedInTechNodes = recipe.ListedInTechNodes
                .Select(EntityViewModelsManager.GetEntityViewModel)
                .ToList().AsReadOnly();
        }
    }
}