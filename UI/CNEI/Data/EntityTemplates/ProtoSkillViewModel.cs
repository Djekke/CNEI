namespace CryoFall.CNEI.UI.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.Skills;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoSkillViewModel : ProtoEntityViewModel
    {
        public override string ResourceDictionaryName => "ProtoSkillDataTemplate.xaml";

        public ProtoSkillViewModel([NotNull] IProtoSkill skill) : base(skill)
        {
            Description = skill.Description;
        }

        /// <summary>
        /// Initilize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is IProtoSkill skill)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Category", skill.Category.Name));
                EntityInformation.Add(new ViewModelEntityInformation("Max level", skill.MaxLevel));

                LevelTable = new List<List<string>>();
                List<ISkillEffect> skillEffects = null;
                if (skill.HasStatEffects)
                {
                    skillEffects = skill.GetEffects()
                                        .OrderBy(ef => ef.Level)
                                        .ToList();
                }
                if (skill.MaxLevel > 0)
                {
                    List<string> levelList = new List<string>() {"Level"};
                    List<string> expList = new List<string>() {"Exp"};
                    for (byte level = 1; level <= skill.MaxLevel; level++)
                    {
                        levelList.Add(level.ToString());
                        expList.Add(skill.GetExperienceForLevel(level).ToString(CultureInfo.CurrentCulture));
                    }
                    LevelTable.Add(levelList);
                    LevelTable.Add(expList);

                    if (skillEffects?.Count > 0)
                    {
                        foreach (var group in skillEffects.OfType<StatEffect>().GroupBy(ef => ef.StatName)
                                                          .OrderByDescending(g => skillEffects.IndexOf(g.First())))
                        {
                            List<string> statBonus = new List<string>() { group.First().Description };
                            for (byte level = 1; level <= skill.MaxLevel; level++)
                            {
                                double valueBonus = 0.0;
                                double percentBonus = 0.0;
                                foreach (StatEffect statEffect in group)
                                {
                                    if (level < statEffect.Level)
                                    {
                                        continue;
                                    }

                                    valueBonus += statEffect.CalcTotalValueBonus(level);
                                    percentBonus += statEffect.CalcTotalPercentBonus(level);
                                }
                                string text = "";
                                if (Math.Abs(valueBonus) > 0.001)
                                {
                                    text += (valueBonus > 0 ? " +" : " ") +
                                            valueBonus.ToString("0.##");
                                }

                                if (Math.Abs(percentBonus) > 0.001)
                                {
                                    text += (percentBonus > 0 ? " +" : " ") +
                                            percentBonus.ToString("0.##") + "%";
                                }
                                statBonus.Add(text);
                            }
                            LevelTable.Add(statBonus);
                        }

                        foreach (IFlagEffect flagEffect in skillEffects.OfType<IFlagEffect>())
                        {
                            List<string> flagEffectStats = new List<string>() { flagEffect.Description };
                            for (byte level = 1; level <= skill.MaxLevel; level++)
                            {
                                flagEffectStats.Add(level >= flagEffect.Level ? "+" : "");
                            }
                            LevelTable.Add(flagEffectStats);
                        }
                    }
                }
            }
        }

        public List<List<string>> LevelTable { get; private set; }

        public int SelectedTabIndex { get; set; }
    }
}