namespace CryoFall.CNEI.UI.Data
{
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.Characters;
    using AtomicTorch.CBND.CoreMod.Skills;
    using AtomicTorch.CBND.CoreMod.Technologies;
    using AtomicTorch.CBND.GameApi.Scripting;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoCharacterMobViewModel : ProtoEntityWithRecipeBondsViewModel
    {
        public override string ResourceDictionaryName => "ProtoCharacterMobDataTemplate.xaml";

        public ProtoCharacterMobViewModel([NotNull] IProtoCharacterMob creature) : base(creature)
        {
        }

        /// <summary>
        /// Initialize entity relationships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (ProtoEntity is IProtoCharacterMob creature &&
                creature.LootDroplist != null &&
                creature.LootDroplist.EnumerateAllItems().Any())
            {
                Droplist = new DroplistRecipeViewModel(this, creature.LootDroplist.EnumerateAllItems());
                EntityViewModelsManager.AddRecipe(Droplist);
            }
        }

        /// <summary>
        /// Initialize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is ProtoCharacterMob characterMob)
            {
                foreach (var effect in characterMob.ProtoCharacterDefaultEffects.Values)
                {
                    EntityInformation.Add(new ViewModelEntityInformation(effect.Key.ToString(), effect.Value));
                }

                EntityInformation.Add(new ViewModelEntityInformation("On a server with default x1 rates", ""));
                // \Scripts\Systems\Weapons\WeaponSystem.cs:475
                var experienceForKill = characterMob.MobKillExperienceMultiplier * SkillHunting.ExperienceForKill;
                EntityInformation.Add(new ViewModelEntityInformation("- Experience for kill",
                    experienceForKill));
                // \Scripts\Skills\Base\ProtoSkill.cs:163
                var skillHunting = Api.FindProtoEntities<SkillHunting>().FirstOrDefault();
                var multiplier = skillHunting.ExperienceToLearningPointsConversionMultiplier * 0.01;
                // Using default server rates \Scripts\Technologies\Base\TechConstants.csTechConstants.cs:66
                //                 * TechConstants.ServerSkillExperienceToLearningPointsConversionMultiplier;
                var lpAtZeroLevel = experienceForKill * multiplier;
                EntityInformation.Add(new ViewModelEntityInformation("- Total LP (at skill level 0)",
                    lpAtZeroLevel.ToString("F3")));
                var lpAtMaxLevel = lpAtZeroLevel * TechConstants.SkillLearningPointMultiplierAtMaximumLevel;
                EntityInformation.Add(new ViewModelEntityInformation("- Total LP (at max skill level)",
                    lpAtMaxLevel.ToString("F3")));
            }
        }

        public RecipeViewModel Droplist { get; private set; }
    }
}