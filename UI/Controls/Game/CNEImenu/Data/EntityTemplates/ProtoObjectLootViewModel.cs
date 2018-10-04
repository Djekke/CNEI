namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Loot;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System.Linq;

    public class ProtoObjectLootViewModel : ProtoStaticWorldObjectViewModel
    {
        private readonly IProtoObjectLoot loot;

        public ProtoObjectLootViewModel([NotNull] IProtoObjectLoot loot) : base(loot)
        {
            this.loot = loot;
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (loot == null)
            {
                return;
            }

            if (loot.LootDroplist != null && loot.LootDroplist.EnumerateAllItems().Any())
            {
                EntityViewModelsManager.AddRecipe(new RecipeViewModel(this, loot.LootDroplist.EnumerateAllItems()));
            }
        }
    }
}