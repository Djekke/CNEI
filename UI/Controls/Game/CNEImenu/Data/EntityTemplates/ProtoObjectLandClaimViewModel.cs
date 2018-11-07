namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures.LandClaim;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;

    public class ProtoObjectLandClaimViewModel : ProtoObjectStructureViewModel
    {
        public ProtoObjectLandClaimViewModel([NotNull] IProtoObjectLandClaim landClaim) : base(landClaim)
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

            if (ProtoEntity is IProtoObjectLandClaim landClaim)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Land claim size",
                    landClaim.LandClaimSize));
                EntityInformation.Add(new ViewModelEntityInformation("Destruction timeout",
                    landClaim.DestructionTimeout));
            }
        }
    }
}