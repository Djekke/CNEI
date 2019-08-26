namespace CryoFall.CNEI.UI.Data
{
    using System;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class PlantRecipeViewModel : RecipeViewModel
    {
        public override string ResourceDictionaryName => "PlantRecipeDataTemplate.xaml";

        public override string RecipeTypeName => "Plant growth";

        /// <summary>
        /// Constructor for entity with droplist.
        /// Must be used only in InitEntityRelationships phase.
        /// </summary>
        /// <param name="seedEntityViewModel">View Model of seed entity.</param>
        /// <param name="plantEntityViewModel">View Model of grown plant entity.</param>
        public PlantRecipeViewModel([NotNull] ProtoEntityViewModel seedEntityViewModel,
            [NotNull] ProtoEntityViewModel plantEntityViewModel)
            : base(seedEntityViewModel.ProtoEntity)
        {
            if (!EntityViewModelsManager.EntityDictonaryCreated)
            {
                throw new Exception("CNEI: Droplist constructor used before all entity VMs sets.");
            }

            InputItemsList.Add(seedEntityViewModel);

            SeedEntity = seedEntityViewModel;

            OutputItemsList.Add(plantEntityViewModel);

            PlantEntity = plantEntityViewModel;
        }

        public ProtoEntityViewModel SeedEntity { get; protected set; }

        public ProtoEntityViewModel PlantEntity { get; protected set; }
    }
}