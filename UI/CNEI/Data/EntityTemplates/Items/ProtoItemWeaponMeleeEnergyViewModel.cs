namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Weapons;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoItemWeaponMeleeEnergyViewModel : ProtoItemWeaponViewModel
    {
        public ProtoItemWeaponMeleeEnergyViewModel([NotNull] IProtoItemWeaponEnergy itemWeapon) : base(itemWeapon)
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

            if (ProtoEntity is ProtoItemWeaponMeleeEnergy weaponMeleeEnergy)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Energy use per shot",
                    weaponMeleeEnergy.EnergyUsePerShot));
                EntityInformation.Add(new ViewModelEntityInformation("Energy use per hit",
                    weaponMeleeEnergy.EnergyUsePerHit));
            }
        }
    }
}