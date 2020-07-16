namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Drones;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoItemDroneControlViewModel : ProtoItemViewModel
    {
        public ProtoItemDroneControlViewModel([NotNull] IProtoItemDroneControl droneControl) : base(droneControl)
        {
        }

        /// <summary>
        /// Initialize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is IProtoItemDroneControl droneControl)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Max drones to control",
                    droneControl.MaxDronesToControl));
            }
        }
    }
}