namespace CryoFall.CNEI.UI.Data
{
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.Technologies;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class TechGroupViewModel : ProtoEntityViewModel
    {
        public TechGroupViewModel([NotNull] TechGroup techGroup) : base(techGroup)
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

            if (ProtoEntity is TechGroup techGroup)
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
                if (techGroup.Nodes?.Count > 0)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Contains nodes",
                        techGroup.Nodes.Select(EntityViewModelsManager.GetEntityViewModel)));
                }
            }
        }
    }
}