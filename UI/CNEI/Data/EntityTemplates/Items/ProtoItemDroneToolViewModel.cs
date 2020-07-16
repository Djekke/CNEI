namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Drones;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoItemDroneToolViewModel : ProtoItemWeaponViewModel
    {
        public ProtoItemDroneToolViewModel([NotNull] ProtoItemDroneTool droneTool) : base(droneTool)
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

            if (ProtoEntity is ProtoItemDroneTool droneTool)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Damage to tree",
                    droneTool.DamageToTree));
                EntityInformation.Add(new ViewModelEntityInformation("Damage to minerals",
                    droneTool.DamageToMinerals));
            }
        }
    }
}