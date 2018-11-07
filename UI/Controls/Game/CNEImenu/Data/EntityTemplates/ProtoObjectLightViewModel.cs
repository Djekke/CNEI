namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures.Lights;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;

    public class ProtoObjectLightViewModel : ProtoObjectStructureViewModel
    {
        public ProtoObjectLightViewModel([NotNull] IProtoObjectLight light) : base(light)
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

            if (ProtoEntity is IProtoObjectLight light)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Fuel capacity",
                    light.FuelCapacity));
                EntityInformation.Add(new ViewModelEntityInformation("Compatible fuel",
                    EntityViewModelsManager.GetEntityViewModelByInterface(light.FuelItemsContainerPrototype.FuelType)));
            }
        }
    }
}