namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.GameApi.Data.World;
    using JetBrains.Annotations;

    public abstract class ProtoStaticWorldObjectViewModel : ProtoEntityWithRecipeBondsViewModel
    {
        protected ProtoStaticWorldObjectViewModel([NotNull] IProtoStaticWorldObject staticObject)
            : base(staticObject)
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

            if (ProtoEntity is IProtoStaticWorldObject staticObject)
            {
                EntityInformation.Add(new ViewModelEntityInformation("HP",
                    staticObject.StructurePointsMax));
                EntityInformation.Add(new ViewModelEntityInformation("Explosive defense coef",
                    staticObject.StructureExplosiveDefenseCoef));
                EntityInformation.Add(new ViewModelEntityInformation("Is interactable",
                    staticObject.IsInteractableObject ? "Yes" : "No"));
                EntityInformation.Add(new ViewModelEntityInformation("Static object kind",
                    staticObject.Kind.ToString()));
                EntityInformation.Add(new ViewModelEntityInformation("Layout size",
                    staticObject.Layout.Bounds.Size.ToString()));
            }
        }
    }
}