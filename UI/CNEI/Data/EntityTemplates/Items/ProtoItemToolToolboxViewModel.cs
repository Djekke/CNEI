namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Tools;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoItemToolToolboxViewModel : ProtoItemViewModel
    {
        public ProtoItemToolToolboxViewModel([NotNull] IProtoItemToolToolbox toolbox) : base(toolbox)
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

            if (ProtoEntity is IProtoItemToolToolbox toolbox)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Construction speed multiplier",
                toolbox.ConstructionSpeedMultiplier));
            }
        }
    }
}