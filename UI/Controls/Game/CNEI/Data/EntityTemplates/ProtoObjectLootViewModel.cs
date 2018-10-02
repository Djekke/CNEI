namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Managers;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Loot;
    using JetBrains.Annotations;
    using System.Linq;

    public class ProtoObjectLootViewModel : ProtoStaticWorldObjectViewModel
    {
        private IProtoObjectLoot loot;

        public ProtoObjectLootViewModel([NotNull] IProtoObjectLoot loot) : base(loot)
        {
            this.loot = loot;
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel{IProtoEntity}" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels{}" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (this.loot == null)
            {
                return;
            }
            if (this.loot.LootDroplist != null && this.loot.LootDroplist.EnumerateAllItems().Any())
            {
                EntityViewModelsManager.AddRecipe(new RecipeViewModel(this,
                    this.loot.LootDroplist.EnumerateAllItems()));
            }
        }
    }
}