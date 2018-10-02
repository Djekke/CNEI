namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Managers;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Minerals;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.GameEngine.Common.Extensions;
    using JetBrains.Annotations;
    using System.Collections.Generic;

    public class ProtoObjectMineralViewModel : ProtoStaticWorldObjectViewModel
    {
        private IProtoObjectMineral mineral;

        public ProtoObjectMineralViewModel([NotNull] IProtoObjectMineral mineral) : base(mineral)
        {
            this.mineral = mineral;
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel{IProtoEntity}" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels{}" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (this.mineral.DropItemsConfig == null)
            {
                return;
            }
            HashSet<IProtoItem> droplist = new HashSet<IProtoItem>();
            droplist.AddRange(this.mineral.DropItemsConfig.Stage1.EnumerateAllItems());
            droplist.AddRange(this.mineral.DropItemsConfig.Stage2.EnumerateAllItems());
            droplist.AddRange(this.mineral.DropItemsConfig.Stage3.EnumerateAllItems());
            droplist.AddRange(this.mineral.DropItemsConfig.Stage4.EnumerateAllItems());
            if (droplist.Count > 0)
            {
                EntityViewModelsManager.AddRecipe(new RecipeViewModel(this, droplist));
            }
        }
    }
}