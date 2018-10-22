namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Tools;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;

    public class ProtoItemToolPickaxeViewModel : ProtoItemWeaponViewModel
    {
        private readonly IProtoItemToolMining pickaxe;

        public ProtoItemToolPickaxeViewModel([NotNull] IProtoItemToolMining pickaxe) : base(pickaxe)
        {
            this.pickaxe = pickaxe;
        }

        /// <summary>
        /// Initilize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            EntityInformation.Add(new ViewModelEntityInformation("Damage to rocks", pickaxe.DamageToRocks));
        }
    }
}