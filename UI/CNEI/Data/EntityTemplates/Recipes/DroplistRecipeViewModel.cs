namespace CryoFall.CNEI.UI.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class DroplistRecipeViewModel: RecipeViewModel
    {
        public override string ResourceDictionaryName => "DroplistRecipeDataTemplate.xaml";

        public override string RecipeTypeName => "Drop";

        /// <summary>
        /// Constructor for entity with droplist.
        /// Must be used only in InitEntityRelationships phase.
        /// </summary>
        /// <param name="entityViewModel">View Model of entity with droplist.</param>
        /// <param name="droplist">Droplist</param>
        public DroplistRecipeViewModel([NotNull] ProtoEntityViewModel entityViewModel,
            [NotNull] IEnumerable<IProtoItem> droplist)
            : base(entityViewModel.ProtoEntity)
        {
            if (!EntityViewModelsManager.EntityDictionaryCreated)
            {
                throw new Exception("CNEI: Droplist constructor used before all entity VMs sets.");
            }

            InputItemsList.Add(entityViewModel);

            TargetEntity = entityViewModel;

            HashSet<IProtoItem> uniqueDroplist = new HashSet<IProtoItem>(droplist);

            OutputItemsList = uniqueDroplist.Select(EntityViewModelsManager.GetEntityViewModel).ToList();

            OutputItemsVMList = uniqueDroplist
                .Select(item => new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(item)))
                .ToList().AsReadOnly();

            if (entityViewModel is ProtoCharacterMobViewModel protoCharacterMobViewModel)
            {
                icon = protoCharacterMobViewModel.Icon;
            }
        }

        public ProtoEntityViewModel TargetEntity { get; protected set; }

        public IReadOnlyList<BaseViewModel> OutputItemsVMList { get; protected set; }
    }
}