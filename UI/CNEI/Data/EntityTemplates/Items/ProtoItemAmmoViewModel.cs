namespace CryoFall.CNEI.UI.Data
{
    using System.Collections.Generic;
    using AtomicTorch.CBND.CoreMod.Items.Ammo;
    using AtomicTorch.CBND.GameApi.Data.Weapons;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoItemAmmoViewModel : ProtoItemViewModel
    {
        public ProtoItemAmmoViewModel([NotNull] IProtoItemAmmo ammo) : base(ammo)
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

            if (ProtoEntity is IProtoItemAmmo ammo && ammo.DamageDescription != null)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Range",
                    ammo.DamageDescription.RangeMax));
                EntityInformation.Add(new ViewModelEntityInformation("Damage value",
                    ammo.DamageDescription.DamageValue));
                EntityInformation.Add(new ViewModelEntityInformation("Armor piercing coefficient",
                    ammo.DamageDescription.ArmorPiercingCoef));
                EntityInformation.Add(new ViewModelEntityInformation("Final damage multiplier",
                    ammo.DamageDescription.FinalDamageMultiplier));

                if (ammo.DamageDescription?.DamageProportions.Count > 0)
                {
                    foreach (DamageProportion proportion in ammo.DamageDescription.DamageProportions)
                    {
                        EntityInformation.Add(new ViewModelEntityInformation("Damage by " + proportion.DamageType,
                            (proportion.Proportion * 100) + "%"));
                    }
                }
            }
        }

        public void AddCompatibleGun([NotNull] ProtoEntityViewModel gunViewModel)
        {
            if (CompatibleGuns.Count == 0)
            {
                AmmoUsage = new AmmoUsageViewModel(this, gunViewModel);
                EntityViewModelsManager.AddRecipe(AmmoUsage);
            }
            else
            {
                AmmoUsage.AddAmmoUsage(gunViewModel);
            }

            CompatibleGuns.Add(gunViewModel);
        }

        public List<ProtoEntityViewModel> CompatibleGuns { get; private set; }
            = new List<ProtoEntityViewModel>();

        public AmmoUsageViewModel AmmoUsage { get; private set; }
    }
}