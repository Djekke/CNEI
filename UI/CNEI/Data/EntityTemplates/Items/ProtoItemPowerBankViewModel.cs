namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Devices;
    using JetBrains.Annotations;

    public class ProtoItemPowerBankViewModel : ProtoItemEquipmentViewModel
    {
        public ProtoItemPowerBankViewModel([NotNull] IProtoItemPowerBank powerBank) : base(powerBank)
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

            if (ProtoEntity is IProtoItemPowerBank powerBank)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Energy capacity",
                    powerBank.EnergyCapacity));
            }
        }
    }
}