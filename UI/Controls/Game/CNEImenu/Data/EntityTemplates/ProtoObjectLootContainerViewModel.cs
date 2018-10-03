﻿namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Loot;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System.Linq;

    public class ProtoObjectLootContainerViewModel : ProtoStaticWorldObjectViewModel
    {
        private IProtoObjectLoot lootContainer;

        public ProtoObjectLootContainerViewModel([NotNull] IProtoObjectLoot lootContainer) : base(lootContainer)
        {
            this.lootContainer = lootContainer;
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel{IProtoEntity}" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels{}" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (this.lootContainer == null)
            {
                return;
            }

            if (this.lootContainer.LootDroplist != null && this.lootContainer.LootDroplist.EnumerateAllItems().Any())
            {
                EntityViewModelsManager.AddRecipe(new RecipeViewModel(this,
                    this.lootContainer.LootDroplist.EnumerateAllItems()));
            }
        }
    }
}