namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Characters;
    using AtomicTorch.CBND.GameApi.Resources;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System.Linq;

    public class ProtoCharacterMobViewModel : ProtoEntityViewModel
    {
        private static readonly ITextureResource DefaultIcon =
            new TextureResource("Content/Textures/StaticObjects/ObjectUnknown.png");

        private readonly IProtoCharacterMob creature;

        public ProtoCharacterMobViewModel([NotNull] IProtoCharacterMob creature) : base(creature, DefaultIcon)
        {
            this.creature = creature;
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (creature == null)
            {
                return;
            }

            if (creature.LootDroplist != null && creature.LootDroplist.EnumerateAllItems().Any())
            {
                EntityViewModelsManager.AddRecipe(new RecipeViewModel(this,
                    creature.LootDroplist.EnumerateAllItems()));
            }
        }

        // TODO: Prewiew from skeleton
        //public override TextureBrush Icon
    }
}