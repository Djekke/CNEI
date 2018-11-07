namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Equipment;
    using JetBrains.Annotations;

    public class ProtoItemEquipmentImplantViewModel : ProtoItemEquipmentViewModel
    {
        public ProtoItemEquipmentImplantViewModel([NotNull] IProtoItemEquipmentImplant implant)
            : base(implant)
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

            if (ProtoEntity is IProtoItemEquipmentImplant implant)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Biomaterial to install",
                    implant.BiomaterialAmountRequiredToInstall));
                EntityInformation.Add(new ViewModelEntityInformation("Biomaterial to uninstall",
                    implant.BiomaterialAmountRequiredToUninstall));
            }
        }
    }
}