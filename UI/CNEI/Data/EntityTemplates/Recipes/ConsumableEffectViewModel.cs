namespace CryoFall.CNEI.UI.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ConsumableEffectViewModel : RecipeViewModel
    {
        public override string ResourceDictionaryName => "ConsumableEffectDataTemplate.xaml";

        public override string RecipeTypeName => "Consumable Effect";

        /// <summary>
        /// Constructor for various consumable providing specific status effect.
        /// Must be used only in InitEntityRelationships phase.
        /// </summary>
        /// <param name="statusEffectViewModel">View Model of status effect.</param>
        /// <param name="conumableViewModel">View Model of consumable.</param>
        /// <param name="intensity">Status effect intensity inflicted by this consumable.</param>
        public ConsumableEffectViewModel([NotNull] ProtoEntityViewModel statusEffectViewModel,
                                         [NotNull] ProtoEntityViewModel conumableViewModel,
                                         double intensity)
            : base(statusEffectViewModel.ProtoEntity)
        {
            if (!EntityViewModelsManager.EntityDictionaryCreated)
            {
                throw new Exception("CNEI: Consumable effect constructor used before all entity VMs sets.");
            }

            StatusEffectEntity = statusEffectViewModel;

            OutputItemsList.Add(statusEffectViewModel);

            AddConsumable(conumableViewModel, intensity);
        }

        public void AddConsumable([NotNull] ProtoEntityViewModel conumableViewModel, double intensity)
        {
            ConsumableList.Add(new ConsumableInfo()
                {
                    ConsumableViewModel = conumableViewModel,
                    Name = conumableViewModel.Title,
                    Intensity = intensity,
            });
            ConsumableList = ConsumableList.OrderByDescending(c => c.Intensity).ThenBy(c => c.Name).ToList();
            InputItemsList.Add(conumableViewModel);
        }

        public ProtoEntityViewModel StatusEffectEntity { get; protected set; }

        public List<ConsumableInfo> ConsumableList { get; protected set; }
            = new List<ConsumableInfo>();

        public class ConsumableInfo
        {
            public ProtoEntityViewModel ConsumableViewModel { get; set; }

            public string Name { get; set; }

            public double Intensity { get; set; }
        }
    }
}