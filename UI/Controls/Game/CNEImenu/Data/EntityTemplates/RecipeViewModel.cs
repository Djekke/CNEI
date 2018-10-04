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
            this.InputItemsVMList = new List<BaseViewModel>();
            this.OutputItemsVMList = new List<BaseViewModel>();
            this.StationsList = new List<ProtoEntityViewModel>();
            this.ListedInTechNodes = new List<ProtoEntityViewModel>();
        }

        /// <summary>
        /// Constructor for basic recipes.
        /// </summary>
        public RecipeViewModel([NotNull] Recipe recipe) : this(recipe, recipe.Icon)
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

            this.InputItemsVMList = config.StageRequiredItems
                .Select(item => new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(item.ProtoItem),
                    item.Count * config.StagesCount))
                .ToList().AsReadOnly();

            this.OutputItemsVMList =
                new List<BaseViewModel>() {new ViewModelEntityWithCount(structureViewModel)}.AsReadOnly();

            // TODO: Add All VM's of toolboxes to StationsList
            //this.StationsList = new List<ProtoEntityViewModel>();
            this.ListedInTechNodes = structureViewModel.ListedInTechNodes;
            this.IsByproduct = Visibility.Collapsed;
            this.IsStationCraft = Visibility.Collapsed;
            this.IsHandCraft = Visibility.Collapsed;
            this.IsDisabled = false;
            this.IsAutoUnlocked = structureViewModel.IsAutoUnlocked;
            this.OriginalDuration = config.StageDurationSeconds * config.StagesCount;
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
            this.InputItemsVMList = inputTempList.AsReadOnly();

            if (!(EntityViewModelsManager.GetEntityViewModel(upgradeEntry.ProtoStructure)
                is ProtoObjectStructureViewModel newStructureViewModel))
            {
                throw new Exception("CNEI: Can not find ProtoObjectStructureViewModel for " +
                                    upgradeEntry.ProtoStructure);
            }
            this.OutputItemsVMList = new List<BaseViewModel>()
                {
                    new ViewModelEntityWithCount(newStructureViewModel)
                }.AsReadOnly();

            this.StationsList = new List<ProtoEntityViewModel>() { structureViewModel };
            this.ListedInTechNodes = newStructureViewModel.ListedInTechNodes;
            this.IsByproduct = Visibility.Collapsed;
            this.IsStationCraft = Visibility.Collapsed;
            this.IsHandCraft = Visibility.Collapsed;
            this.IsDisabled = false;
            this.IsAutoUnlocked = structureViewModel.IsAutoUnlocked;
            this.OriginalDuration = 0;
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
            this.InputItemsVMList =
                new List<BaseViewModel>() {new ViewModelEntityWithCount(entityViewModel)}.AsReadOnly();

            HashSet<IProtoItem> uniqueDroplist = new HashSet<IProtoItem>(droplist);
            this.OutputItemsVMList = uniqueDroplist
                .Select(item => new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(item)))
                .ToList().AsReadOnly();

            //this.StationsList = new List<ProtoEntityViewModel>();
            //this.ListedInTechNodes = new List<ProtoEntityViewModel>();
            this.IsByproduct = Visibility.Collapsed;
            this.IsStationCraft = Visibility.Collapsed;
            this.IsHandCraft = Visibility.Collapsed;
            this.IsDisabled = false;
            this.IsAutoUnlocked = true;
            this.OriginalDuration = 0;
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created.
        /// <para>You can access them by using <see cref="M:CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers.EntityViewModelsManager.GetEntityViewModel(AtomicTorch.CBND.GameApi.Data.IProtoEntity)" /> and
        /// <see cref="M:CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers.EntityViewModelsManager.GetAllEntityViewModels" />.</para>
        /// </summary>
        public override void InitAdditionalRecipes()
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