namespace CryoFall.CNEI.UI.Data
{
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.Items.Generic;
    using AtomicTorch.CBND.CoreMod.Systems.Crafting;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class RecipeMulchViewModel : ManufacturingRecipeViewModel
    {

        public RecipeMulchViewModel([NotNull] Recipe recipe) : base(recipe)
        {
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            base.InitAdditionalRecipes();

            InputItemsList = EntityViewModelsManager.GetAllEntityViewModelsByType<IProtoItemOrganic>();

            InputItemsVMList = EntityViewModelsManager.GetAllEntityViewModelsByType<IProtoItemOrganic>()
                .Select(item => new ViewModelEntityWithCount(item))
                .ToList().AsReadOnly();
        }
    }
}