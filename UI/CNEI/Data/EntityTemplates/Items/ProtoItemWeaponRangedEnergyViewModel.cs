namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Weapons;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoItemWeaponRangedEnergyViewModel : ProtoItemWeaponViewModel
    {
        public ProtoItemWeaponRangedEnergyViewModel([NotNull] IProtoItemWeaponEnergy itemWeapon) : base(itemWeapon)
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

            if (ProtoEntity is ProtoItemWeaponRangedEnergy weaponRangedEnergy)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Energy use per shot",
                    weaponRangedEnergy.EnergyUsePerShot));
            }
        }
    }
}