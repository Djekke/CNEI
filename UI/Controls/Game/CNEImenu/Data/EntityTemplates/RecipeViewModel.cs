namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Tools.Toolboxes;
    using AtomicTorch.CBND.CoreMod.Systems.Construction;
    using AtomicTorch.CBND.CoreMod.Systems.Crafting;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Data.Items;
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

        /// <summary>
        /// Constructor for basic recipes.
        /// </summary>
        public RecipeViewModel([NotNull] Recipe recipe) : base(recipe, recipe.Icon)
        {
            this.recipe = recipe;
            RecipeType = recipe.RecipeType;

            IsByproduct = (RecipeType == RecipeType.StationByproduct)
                ? Visibility.Visible
                : Visibility.Collapsed;
            IsStationCraft = (RecipeType == RecipeType.Hand)
                ? Visibility.Collapsed
                : Visibility.Visible;
            IsHandCraft = (RecipeType == RecipeType.Hand)
                ? Visibility.Visible
                : Visibility.Collapsed;
            TimeVisibility = Visibility.Visible;
            OriginalDuration = recipe.OriginalDuration;
            IsDisabled = !recipe.IsEnabled;
            IsAutoUnlocked = recipe.IsAutoUnlocked;
            OriginText = (RecipeType == RecipeType.Hand) ? "Made by:" : "Made in:";
        }

        /// <summary>
        /// Constructor for build recipe for IProtoStructure.
        /// Must be used only in InitEntityRelationships phase.
        /// </summary>
        /// <param name="structureViewModel">View Model of IProtoStructure.</param>
        /// <param name="config">Building config.</param>
        public RecipeViewModel([NotNull] ProtoObjectStructureViewModel structureViewModel,
            [NotNull] IConstructionStageConfigReadOnly config)
            : base(structureViewModel.ProtoEntity, structureViewModel.IconResource)
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

            OriginText = "Build by:";
            IsStationCraft = Visibility.Visible;
            StationsList = EntityViewModelsManager.GetAllEntityViewModelsByType<IProtoItemToolToolbox>().AsReadOnly();
            ListedInTechNodes = structureViewModel.ListedInTechNodes;

            IsAutoUnlocked = structureViewModel.IsAutoUnlocked;
        }

        /// <summary>
        /// Constructor for upgrade recipe for IProtoStructure.
        /// Must be used only in InitEntityRelationships phase.
        /// </summary>
        /// <param name="structureViewModel">View Model of IProtoStructure.</param>
        /// <param name="upgradeEntry">Entry of upgrade config.</param>
        public RecipeViewModel([NotNull] ProtoObjectStructureViewModel structureViewModel,
            [NotNull] IConstructionUpgradeEntryReadOnly upgradeEntry)
            : base(upgradeEntry.ProtoStructure, upgradeEntry.ProtoStructure.Icon)
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

            OriginText = "Upgrade from:";
            IsStationCraft = Visibility.Visible;
            StationsList = new List<ProtoEntityViewModel>() { structureViewModel }.AsReadOnly();
            // Can not simply get it from result entityVM because it can has not initilized Tech.
            //ListedInTechNodes = newStructureViewModel.ListedInTechNodes;
            ListedInTechNodes = upgradeEntry.ProtoStructure.ListedInTechNodes
                .Select(EntityViewModelsManager.GetEntityViewModel)
                .ToList().AsReadOnly();
            IsAutoUnlocked = structureViewModel.IsAutoUnlocked;
        }

        /// <summary>
        /// Constructor for entity with droplist.
        /// Must be used only in InitEntityRelationships phase.
        /// </summary>
        /// <param name="entityViewModel">View Model of entity with droplist.</param>
        /// <param name="droplist">Droplist</param>
        public RecipeViewModel([NotNull] ProtoEntityViewModel entityViewModel, [NotNull] IEnumerable<IProtoItem> droplist)
            : base(entityViewModel.ProtoEntity, entityViewModel.IconResource)
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

            OriginVisibility = Visibility.Collapsed;
            TechVisibility = Visibility.Collapsed;
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

        public IReadOnlyList<BaseViewModel> InputItemsVMList { get; private set; } = new List<BaseViewModel>();

        public IReadOnlyList<BaseViewModel> OutputItemsVMList { get; private set; } = new List<BaseViewModel>();

        public IReadOnlyList<ProtoEntityViewModel> StationsList { get; private set; } = new List<ProtoEntityViewModel>();

        public IReadOnlyList<ProtoEntityViewModel> ListedInTechNodes { get; private set; } = new List<ProtoEntityViewModel>();

        public double OriginalDuration { get; } = 0d;

        public bool IsDisabled { get; } = false;

        public bool IsAutoUnlocked { get; }

        public RecipeType RecipeType { get; }

        public string OriginText { get; }

        public Visibility IsStationCraft { get; } = Visibility.Collapsed;

        public Visibility IsHandCraft { get; } = Visibility.Collapsed;

        public Visibility IsByproduct { get; } = Visibility.Collapsed;

        public Visibility TechVisibility { get; } = Visibility.Visible;

        public Visibility OriginVisibility { get; } = Visibility.Visible;

        public Visibility TimeVisibility { get; } = Visibility.Collapsed;
    }
}