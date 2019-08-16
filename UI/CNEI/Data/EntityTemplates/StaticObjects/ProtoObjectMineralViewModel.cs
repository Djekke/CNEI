namespace CryoFall.CNEI.UI.Data
{
    using System.Collections.Generic;
    using System.Windows;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Minerals;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.GameEngine.Common.Extensions;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoObjectMineralViewModel : ProtoStaticWorldObjectViewModel
    {
        public override string ResourceDictionaryName => "ProtoObjectMineralDataTemplate.xaml";

        public override string ResourceDictionaryFolderName => "StaticObjects/";

        public ProtoObjectMineralViewModel([NotNull] IProtoObjectMineral mineral) : base(mineral)
        {
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (ProtoEntity is IProtoObjectMineral mineral)
            {
                if (mineral.DropItemsConfig == null)
                {
                    return;
                }

                HashSet<IProtoItem> droplist = new HashSet<IProtoItem>();
                droplist.AddRange(mineral.DropItemsConfig.Stage1.EnumerateAllItems());
                droplist.AddRange(mineral.DropItemsConfig.Stage2.EnumerateAllItems());
                droplist.AddRange(mineral.DropItemsConfig.Stage3.EnumerateAllItems());
                droplist.AddRange(mineral.DropItemsConfig.Stage4.EnumerateAllItems());
                if (droplist.Count > 0)
                {
                    Droplist = new RecipeViewModel(this, droplist);
                    DroplistVisibility = Visibility.Visible;
                    EntityViewModelsManager.AddRecipe(Droplist);
                }
            }
        }

        public RecipeViewModel Droplist { get; private set; }

        public Visibility DroplistVisibility { get; private set; } = Visibility.Collapsed;

        public bool IsInfoExpanded { get; set; } = true;

        public bool IsRecipesExpanded { get; set; } = true;
    }
}