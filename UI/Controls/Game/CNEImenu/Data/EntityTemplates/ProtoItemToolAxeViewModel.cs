namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Tools;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;

    public class ProtoItemToolAxeViewModel : ProtoItemWeaponViewModel
    {
        private readonly IProtoItemToolWoodcutting axe;

        public ProtoItemToolAxeViewModel([NotNull] IProtoItemToolWoodcutting axe) : base(axe)
        {
            this.axe = axe;
        }

        /// <summary>
        /// Initilize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            EntityInformation.Add(new ViewModelEntityInformation("Damage to tree", axe.DamageToTree));
        }
    }
}