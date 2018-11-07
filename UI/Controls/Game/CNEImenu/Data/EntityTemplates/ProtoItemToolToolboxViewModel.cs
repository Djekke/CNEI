namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Tools.Toolboxes;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;

    public class ProtoItemToolToolboxViewModel : ProtoItemViewModel
    {
        public ProtoItemToolToolboxViewModel([NotNull] IProtoItemToolToolbox toolbox) : base(toolbox)
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

            if (ProtoEntity is IProtoItemToolToolbox toolbox)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Construction speed multiplier",
                toolbox.ConstructionSpeedMultiplier));
            }
        }
    }
}