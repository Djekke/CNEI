namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CoreMod.Systems.Construction;
    using AtomicTorch.CBND.CoreMod.Systems.Crafting;
    using AtomicTorch.CBND.GameApi.Scripting;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using Managers;

    public class RecipeViewModel : ProtoEntityViewModel
    {
        private readonly Recipe recipe = null;

        /// <summary>
        /// Constructor for basic recipes.
        /// </summary>
        public RecipeViewModel(Recipe recipe) : base(recipe, recipe.Icon)
        {
            this.recipe = recipe;
            this.RecipeType = recipe.RecipeType;

            this.IsByproduct = this.RecipeType == RecipeType.StationByproduct
                ? Visibility.Visible
                : Visibility.Collapsed;
            this.IsStationCraft = (this.RecipeType == RecipeType.Hand)
                ? Visibility.Collapsed
                : Visibility.Visible;
            this.IsHandCraft = (this.RecipeType == RecipeType.Hand)
                ? Visibility.Visible
                : Visibility.Collapsed;

            this.OriginalDuration = recipe.OriginalDuration;
            this.IsDisabled = !recipe.IsEnabled;
            this.IsAutoUnlocked = recipe.IsAutoUnlocked;
        }

        /// <summary>
        /// Constructor for build recipe for IProtoStructure.
        /// </summary>
        /// <param name="structureViewModel">View Model of IProtoStructure.</param>
        /// <param name="config">Building config.</param>
        public RecipeViewModel(ProtoObjectStructureViewModel structureViewModel,
            IConstructionStageConfigReadOnly config)
            : base(structureViewModel.ProtoEntity, structureViewModel.IconResource)
        {
            this.InputItemsVMList =
                new List<BaseViewModel>() {new ViewModelEntityWithCount(structureViewModel)}.AsReadOnly();

            this.OutputItemsVMList = config.StageRequiredItems
                .Select(item => new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(item.ProtoItem),
                    item.Count * config.StagesCount))
                .ToList().AsReadOnly();

            this.IsByproduct = Visibility.Collapsed;
            this.IsStationCraft = Visibility.Collapsed;
            this.IsHandCraft = Visibility.Collapsed;
            this.IsDisabled = false;
            this.IsAutoUnlocked = structureViewModel.IsAutoUnlocked;
            this.OriginalDuration = config.StageDurationSeconds * config.StagesCount;
        }

        /// <summary>
        /// Constructor for upgrade recipe for IProtoStructure.
        /// </summary>
        /// <param name="structureViewModel">View Model of IProtoStructure.</param>
        /// <param name="upgradeEntry">Entry of upgrade config.</param>
        public RecipeViewModel(ProtoObjectStructureViewModel structureViewModel,
            IConstructionUpgradeEntryReadOnly upgradeEntry)
            : base(structureViewModel.ProtoEntity, structureViewModel.IconResource)
        {
            var inputTempList = new List<BaseViewModel>() { structureViewModel };
            inputTempList.AddRange(upgradeEntry.RequiredItems
                .Select(item => new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(item.ProtoItem),
                    item.Count)));
            this.InputItemsVMList = inputTempList.AsReadOnly();

            this.OutputItemsVMList = new List<BaseViewModel>()
                {
                    new ViewModelEntityWithCount(
                        EntityViewModelsManager.GetEntityViewModel(upgradeEntry.ProtoStructure))
                }.AsReadOnly();

            this.IsByproduct = Visibility.Collapsed;
            this.IsStationCraft = Visibility.Collapsed;
            this.IsHandCraft = Visibility.Collapsed;
            this.IsDisabled = false;
            this.IsAutoUnlocked = structureViewModel.IsAutoUnlocked;
            this.OriginalDuration = 0;
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created.
        /// <para>You can access them by using <see cref="M:AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Managers.EntityViewModelsManager.GetEntityViewModel(AtomicTorch.CBND.GameApi.Data.IProtoEntity)" /> and
        /// <see cref="M:AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Managers.EntityViewModelsManager.GetAllEntityViewModels" />.</para>
        /// </summary>
        public override void InitEntityRelationships()
        {
            if (this.recipe == null)
            {
                return;
            }

            if (recipe is Recipe.RecipeForStationByproduct byproductRecipe)
            {
                this.InputItemsVMList = new List<BaseViewModel>()
                {
                    new ViewModelEntityWithCount(
                        EntityViewModelsManager.GetEntityViewModel(byproductRecipe.ProtoItemFuel))
                }.AsReadOnly();
            }
            else
            {
                this.InputItemsVMList = recipe.InputItems
                    .Select(i =>
                        new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(i.ProtoItem), i.Count))
                    .ToList().AsReadOnly();
            }

            this.OutputItemsVMList = recipe.OutputItems.Items
                .Select(i => new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(i.ProtoItem),
                    i.Count, i.CountRandom, i.Probability))
                .ToList().AsReadOnly();

            this.ListedInTechNodes = recipe.ListedInTechNodes
                .Select(EntityViewModelsManager.GetEntityViewModel)
                .ToList().AsReadOnly();

            if (recipe is Recipe.BaseRecipeForStation stationsRecipe)
            {
                this.StationsList = stationsRecipe.StationTypes
                    .Select(EntityViewModelsManager.GetEntityViewModel)
                    .ToList().AsReadOnly();
            }
        }

        public IReadOnlyList<BaseViewModel> InputItemsVMList { get; private set; }

        public IReadOnlyList<BaseViewModel> OutputItemsVMList { get; private set; }

        public double OriginalDuration  { get; }

        public bool IsDisabled  { get; }

        public bool IsAutoUnlocked { get; }

        public IReadOnlyList<ProtoEntityViewModel> ListedInTechNodes { get; private set; }

        public RecipeType RecipeType { get; }

        public Visibility IsStationCraft { get; }

        public Visibility IsHandCraft { get; }

        public Visibility IsByproduct { get; }

        public IReadOnlyList<ProtoEntityViewModel> StationsList { get; private set; }
    }
}