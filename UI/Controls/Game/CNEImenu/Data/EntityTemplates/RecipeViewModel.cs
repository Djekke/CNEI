namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Systems.Construction;
    using AtomicTorch.CBND.CoreMod.Systems.Crafting;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Data;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.CBND.GameApi.Resources;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    public class RecipeViewModel : ProtoEntityViewModel
    {
        private readonly Recipe recipe;

        public override string ResourceDictonaryName => "RecipeDataTemplate.xaml";

        private RecipeViewModel([NotNull] IProtoEntity entity, [NotNull] ITextureResource icon) : base(entity, icon)
        {
            InputItemsVMList = new List<BaseViewModel>();
            OutputItemsVMList = new List<BaseViewModel>();
            StationsList = new List<ProtoEntityViewModel>();
            ListedInTechNodes = new List<ProtoEntityViewModel>();
        }

        /// <summary>
        /// Constructor for basic recipes.
        /// </summary>
        public RecipeViewModel([NotNull] Recipe recipe) : this(recipe, recipe.Icon)
        {
            this.recipe = recipe;
            RecipeType = recipe.RecipeType;

            IsByproduct = RecipeType == RecipeType.StationByproduct
                ? Visibility.Visible
                : Visibility.Collapsed;
            IsStationCraft = (RecipeType == RecipeType.Hand)
                ? Visibility.Collapsed
                : Visibility.Visible;
            IsHandCraft = (RecipeType == RecipeType.Hand)
                ? Visibility.Visible
                : Visibility.Collapsed;

            OriginalDuration = recipe.OriginalDuration;
            IsDisabled = !recipe.IsEnabled;
            IsAutoUnlocked = recipe.IsAutoUnlocked;
        }

        /// <summary>
        /// Constructor for build recipe for IProtoStructure.
        /// Must be used only in InitEntityRelationships phase.
        /// </summary>
        /// <param name="structureViewModel">View Model of IProtoStructure.</param>
        /// <param name="config">Building config.</param>
        public RecipeViewModel([NotNull] ProtoObjectStructureViewModel structureViewModel,
            [NotNull] IConstructionStageConfigReadOnly config)
            : this(structureViewModel.ProtoEntity, structureViewModel.IconResource)
        {
            if (!EntityViewModelsManager.EntityDictonaryCreated)
            {
                throw new Exception("CNEI: Build constructor used before all entity VMs sets.");
            }

            InputItemsVMList = config.StageRequiredItems
                .Select(item => new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(item.ProtoItem),
                    item.Count * config.StagesCount))
                .ToList().AsReadOnly();

            OutputItemsVMList =
                new List<BaseViewModel>() {new ViewModelEntityWithCount(structureViewModel)}.AsReadOnly();

            // TODO: Add All VM's of toolboxes to StationsList
            //this.StationsList = new List<ProtoEntityViewModel>();
            ListedInTechNodes = structureViewModel.ListedInTechNodes;
            IsByproduct = Visibility.Collapsed;
            IsStationCraft = Visibility.Collapsed;
            IsHandCraft = Visibility.Collapsed;
            IsDisabled = false;
            IsAutoUnlocked = structureViewModel.IsAutoUnlocked;
            OriginalDuration = config.StageDurationSeconds * config.StagesCount;
        }

        /// <summary>
        /// Constructor for upgrade recipe for IProtoStructure.
        /// Must be used only in InitEntityRelationships phase.
        /// </summary>
        /// <param name="structureViewModel">View Model of IProtoStructure.</param>
        /// <param name="upgradeEntry">Entry of upgrade config.</param>
        public RecipeViewModel([NotNull] ProtoObjectStructureViewModel structureViewModel,
            [NotNull] IConstructionUpgradeEntryReadOnly upgradeEntry)
            : this(structureViewModel.ProtoEntity, structureViewModel.IconResource)
        {
            if (!EntityViewModelsManager.EntityDictonaryCreated)
            {
                throw new Exception("CNEI: Upgrade constructor used before all entity VMs sets.");
            }

            var inputTempList = new List<BaseViewModel>() { new ViewModelEntityWithCount(structureViewModel) };
            inputTempList.AddRange(upgradeEntry.RequiredItems
                .Select(item => new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(item.ProtoItem),
                    item.Count)));
            InputItemsVMList = inputTempList.AsReadOnly();

            if (!(EntityViewModelsManager.GetEntityViewModel(upgradeEntry.ProtoStructure)
                is ProtoObjectStructureViewModel newStructureViewModel))
            {
                throw new Exception("CNEI: Can not find ProtoObjectStructureViewModel for " +
                                    upgradeEntry.ProtoStructure);
            }
            OutputItemsVMList = new List<BaseViewModel>()
                {
                    new ViewModelEntityWithCount(newStructureViewModel)
                }.AsReadOnly();

            StationsList = new List<ProtoEntityViewModel>() { structureViewModel };
            ListedInTechNodes = newStructureViewModel.ListedInTechNodes;
            IsByproduct = Visibility.Collapsed;
            IsStationCraft = Visibility.Collapsed;
            IsHandCraft = Visibility.Collapsed;
            IsDisabled = false;
            IsAutoUnlocked = structureViewModel.IsAutoUnlocked;
            OriginalDuration = 0;
        }

        /// <summary>
        /// Constructor for entity with droplist.
        /// Must be used only in InitEntityRelationships phase.
        /// </summary>
        /// <param name="entityViewModel">View Model of entity with droplist.</param>
        /// <param name="droplist">Droplist</param>
        public RecipeViewModel([NotNull] ProtoEntityViewModel entityViewModel, [NotNull] IEnumerable<IProtoItem> droplist)
            : this(entityViewModel.ProtoEntity, entityViewModel.IconResource)
        {
            if (!EntityViewModelsManager.EntityDictonaryCreated)
            {
                throw new Exception("CNEI: Droplist constructor used before all entity VMs sets.");
            }
            InputItemsVMList =
                new List<BaseViewModel>() {new ViewModelEntityWithCount(entityViewModel)}.AsReadOnly();

            HashSet<IProtoItem> uniqueDroplist = new HashSet<IProtoItem>(droplist);
            OutputItemsVMList = uniqueDroplist
                .Select(item => new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(item)))
                .ToList().AsReadOnly();

            //this.StationsList = new List<ProtoEntityViewModel>();
            //this.ListedInTechNodes = new List<ProtoEntityViewModel>();
            IsByproduct = Visibility.Collapsed;
            IsStationCraft = Visibility.Collapsed;
            IsHandCraft = Visibility.Collapsed;
            IsDisabled = false;
            IsAutoUnlocked = true;
            OriginalDuration = 0;
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (recipe == null)
            {
                return;
            }

            if (recipe is Recipe.RecipeForStationByproduct byproductRecipe)
            {
                InputItemsVMList = new List<BaseViewModel>()
                {
                    new ViewModelEntityWithCount(
                        EntityViewModelsManager.GetEntityViewModel(byproductRecipe.ProtoItemFuel))
                }.AsReadOnly();
            }
            else
            {
                InputItemsVMList = recipe.InputItems
                    .Select(i =>
                        new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(i.ProtoItem), i.Count))
                    .ToList().AsReadOnly();
            }

            OutputItemsVMList = recipe.OutputItems.Items
                .Select(i => new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(i.ProtoItem),
                    i.Count, i.CountRandom, i.Probability))
                .ToList().AsReadOnly();

            ListedInTechNodes = recipe.ListedInTechNodes
                .Select(EntityViewModelsManager.GetEntityViewModel)
                .ToList().AsReadOnly();

            if (recipe is Recipe.BaseRecipeForStation stationsRecipe)
            {
                StationsList = stationsRecipe.StationTypes
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