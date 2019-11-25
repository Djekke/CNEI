namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.Items;
    using AtomicTorch.CBND.CoreMod.Items.Generic;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoItemViewModel: ProtoEntityWithRecipeBondsViewModel
    {
        public override string ResourceDictionaryName => "ProtoItemDataTemplate.xaml";

        public override string ResourceDictionaryFolderName => "Items/";

        public ProtoItemViewModel([NotNull] IProtoItem item) : base(item)
        {
            Description = item.Description;
        }

        /// <summary>
        /// Initialize information about entity - invoked after all entity view Models created,
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
            if (ProtoEntity is IProtoItemWithDurablity itemWithDurability)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Durability max",
                    itemWithDurability.DurabilityMax));
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
    }
}