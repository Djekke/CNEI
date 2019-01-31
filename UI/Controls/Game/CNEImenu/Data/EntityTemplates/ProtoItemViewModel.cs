namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Items;
    using AtomicTorch.CBND.CoreMod.Items.Generic;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System.Windows;

    public class ProtoItemViewModel : ProtoEntityViewModel
    {
        public override string ResourceDictonaryName => "ProtoItemDataTemplate.xaml";

        public ProtoItemViewModel([NotNull] IProtoItem item) : base(item)
        {
            Description = item.Description;
        }

        /// <summary>
        /// Initilize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is IProtoItem item)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Stack size",
                    item.MaxItemsPerStack.ToString()));
            }
            if (ProtoEntity is IProtoItemWithDurablity itemWithDurablity)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Durability max",
                    itemWithDurablity.DurabilityMax));
            }
            if (ProtoEntity is IProtoItemOrganic itemOrganic)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Organic value",
                    itemOrganic.OrganicValue));
            }
            if (ProtoEntity is IProtoItemFertilizer itemFertilizer)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Plant grow speed increase",
                    (itemFertilizer.PlantGrowthSpeedMultiplier * 100) + "%"));
            }
            if (ProtoEntity is IProtoItemFuel itemFuel)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Fuel amount",
                    itemFuel.FuelAmount));
            }
            if (ProtoEntity is IProtoItemLiquidStorage liquidStorage)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Capacity",
                    liquidStorage.Capacity));
                EntityInformation.Add(new ViewModelEntityInformation("Liquid type",
                    liquidStorage.LiquidType.ToString()));
            }
        }

        /// <summary>
        /// Finalize Recipe Link creation and prepare recipe VM list to observation.
        /// </summary>
        public override void FinalizeRecipeLinking()
        {
            base.FinalizeRecipeLinking();
            if (RecipeVMList.EntityCount == 0)
            {
                RecipesVisibility = Visibility.Collapsed;
                IsRecipesExpanded = false;
            }
            if (UsageVMList.EntityCount == 0)
            {
                UsageVisibility = Visibility.Collapsed;
                IsUsageExpanded = false;
            }
            RecipePrevPage = new ActionCommand(() => RecipeVMList.PrevPage());
            RecipeNextPage = new ActionCommand(() => RecipeVMList.NextPage());
            UsagePrevPage = new ActionCommand(() => UsageVMList.PrevPage());
            UsageNextPage = new ActionCommand(() => UsageVMList.NextPage());
        }

        public BaseCommand RecipePrevPage { get; private set; }

        public BaseCommand RecipeNextPage { get; private set; }

        public BaseCommand UsagePrevPage { get; private set; }

        public BaseCommand UsageNextPage { get; private set; }

        public Visibility RecipesVisibility { get; private set; } = Visibility.Visible;

        public Visibility UsageVisibility { get; private set; } = Visibility.Visible;

        public bool IsInfoExpanded { get; set; } = true;

        public bool IsRecipesExpanded { get; set; } = true;

        public bool IsUsageExpanded { get; set; } = true;
    }
}