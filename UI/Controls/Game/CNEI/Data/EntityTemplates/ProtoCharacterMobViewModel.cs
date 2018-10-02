namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Managers;
    using AtomicTorch.CBND.CoreMod.Characters;
    using AtomicTorch.CBND.GameApi.Resources;
    using JetBrains.Annotations;
    using System.Linq;

    public class ProtoCharacterMobViewModel : ProtoEntityViewModel
    {
        private static readonly ITextureResource defaultIcon =
            new TextureResource("Content/Textures/StaticObjects/ObjectUnknown.png");

        private IProtoCharacterMob creature;

        public ProtoCharacterMobViewModel([NotNull] IProtoCharacterMob creature) : base(creature, defaultIcon)
        {
            this.creature = creature;
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel{IProtoEntity}" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels{}" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (this.creature == null)
            {
                return;
            }

            if (this.creature.LootDroplist != null && this.creature.LootDroplist.EnumerateAllItems().Any())
            {
                EntityViewModelsManager.AddRecipe(new RecipeViewModel(this,
                    this.creature.LootDroplist.EnumerateAllItems()));
            }
        }

        // TODO: Prewiew from skeleton
        //public override TextureBrush Icon
    }
}