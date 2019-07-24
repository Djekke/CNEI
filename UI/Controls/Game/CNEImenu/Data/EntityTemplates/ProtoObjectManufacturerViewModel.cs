namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures.Manufacturers;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;

    public class ProtoObjectManufacturerViewModel : ProtoObjectStructureViewModel
    {
        public ProtoObjectManufacturerViewModel([NotNull] IProtoObjectManufacturer manufacturer) : base(manufacturer)
        {
        }

        /// <summary>
        /// Initilize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is IProtoObjectManufacturer protoManufacturer)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Input slots count",
                    protoManufacturer.ContainerInputSlotsCount));
                EntityInformation.Add(new ViewModelEntityInformation("Output slots count",
                    protoManufacturer.ContainerOutputSlotsCount));
                if (protoManufacturer.ContainerFuelSlotsCount > 0)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Fuel slots count",
                        protoManufacturer.ContainerFuelSlotsCount));
                    EntityInformation.Add(new ViewModelEntityInformation("Fuel produce byproducts",
                        protoManufacturer.IsFuelProduceByproducts ? "Yes" : "No"));
                }
            }
        }
    }
}