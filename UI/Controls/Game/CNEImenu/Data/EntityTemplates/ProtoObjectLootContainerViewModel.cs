namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Loot;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System.Linq;
    using System.Windows;

    public class ProtoObjectLootContainerViewModel : ProtoStaticWorldObjectViewModel
    {
        private readonly IProtoObjectLoot lootContainer;

        public override string ResourceDictonaryName => "ProtoObjectLootContainerDataTemplate.xaml";

        public ProtoObjectLootContainerViewModel([NotNull] IProtoObjectLoot lootContainer) : base(lootContainer)
        {
            this.lootContainer = lootContainer;
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (lootContainer == null)
            {
                return;
            }

            if (lootContainer.LootDroplist != null && lootContainer.LootDroplist.EnumerateAllItems().Any())
            {
                Droplist = new RecipeViewModel(this, lootContainer.LootDroplist.EnumerateAllItems());
                DroplistVisibility = Visibility.Visible;
                EntityViewModelsManager.AddRecipe(Droplist);
            }
        }

        public RecipeViewModel Droplist { get; private set; }

        public Visibility DroplistVisibility { get; private set; } = Visibility.Collapsed;

        public bool IsInfoExpanded { get; set; } = true;

        public bool IsRecipesExpanded { get; set; } = true;
    }
}