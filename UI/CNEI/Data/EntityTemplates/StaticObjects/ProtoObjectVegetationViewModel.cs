﻿namespace CryoFall.CNEI.UI.Data
{
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Vegetation;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoObjectVegetationViewModel : ProtoStaticWorldObjectViewModel
    {
        public override string ResourceDictionaryName => "ProtoObjectVegetationDataTemplate.xaml";

        public override string ResourceDictionaryFolderName => "StaticObjects/";

        public ProtoObjectVegetationViewModel([NotNull] IProtoObjectVegetation vegetation) : base(vegetation)
        {
        }

        /// <summary>
        /// Initialize entity relationships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (ProtoEntity is IProtoObjectVegetation vegetation &&
                vegetation.DroplistOnDestroy != null &&
                vegetation.DroplistOnDestroy.EnumerateAllItems().Any())
            {
                DroplistOnDestroy = new DroplistRecipeViewModel(this, vegetation.DroplistOnDestroy.EnumerateAllItems());
                EntityViewModelsManager.AddRecipe(DroplistOnDestroy);
            }
        }

        /// <summary>
        /// Initialize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is IProtoObjectVegetation vegetation)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Grow stage count",
                    vegetation.GrowthStagesCount));
            }
        }

        public RecipeViewModel DroplistOnDestroy { get; private set; }
    }
}