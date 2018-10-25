namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Technologies;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System.Linq;

    public class ProtoTechGroupViewModel : ProtoEntityViewModel
    {
        public ProtoTechGroupViewModel([NotNull] ProtoTechGroup techGroup) : base(techGroup, techGroup.Icon)
        {
            Description = techGroup.Description;
        }

        /// <summary>
        /// Initilize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is ProtoTechGroup techGroup)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Tier", techGroup.Tier.ToString()));
                EntityInformation.Add(new ViewModelEntityInformation("LP to unlock", techGroup.LearningPointsPrice));
                if (techGroup.GroupRequirements?.Count > 0)
                {
                    foreach (BaseTechGroupRequirement groupRequirement in techGroup.GroupRequirements)
                    {
                        switch (groupRequirement)
                        {
                            case BaseTechGroupRequirementGroupUnlocked requirementGroup:
                                EntityInformation.Add(new ViewModelEntityInformation(
                                    "Require " + (requirementGroup.GroupNodesUnlockedPercent * 100) + "%",
                                    EntityViewModelsManager.GetEntityViewModel(requirementGroup.Group)));
                                break;
                        }
                    }
                }
                if (techGroup.AllNodes?.Count > 0)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Contains nodes",
                        techGroup.AllNodes.Select(EntityViewModelsManager.GetEntityViewModel)));
                }
            }
        }
    }
}