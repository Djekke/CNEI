namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Generic;
    using AtomicTorch.CBND.CoreMod.Systems.Crafting;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System.Linq;

    public class RecipeMulchViewModel : RecipeViewModel
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

            if (ProtoEntity is Recipe recipe)
            {
                InputItemsVMList = EntityViewModelsManager.GetAllEntityViewModelsByType<IProtoItemOrganic>()
                    .Select(item => new ViewModelEntityWithCount(item))
                    .ToList().AsReadOnly();
            }
        }
    }
}