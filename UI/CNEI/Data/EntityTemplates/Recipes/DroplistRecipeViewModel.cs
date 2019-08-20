namespace CryoFall.CNEI.UI.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class DroplistRecipeViewModel: RecipeViewModel
    {
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
            if (!EntityViewModelsManager.EntityDictonaryCreated)
            {
                throw new Exception("CNEI: Droplist constructor used before all entity VMs sets.");
            }
            InputItemsVMList =
                new List<BaseViewModel>() { new ViewModelEntityWithCount(entityViewModel) }.AsReadOnly();

            HashSet<IProtoItem> uniqueDroplist = new HashSet<IProtoItem>(droplist);
            OutputItemsVMList = uniqueDroplist
                .Select(item => new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(item)))
                .ToList().AsReadOnly();

            OriginVisibility = Visibility.Collapsed;
            TechVisibility = Visibility.Collapsed;

            if (entityViewModel is ProtoCharacterMobViewModel protoCharacterMobViewModel)
            {
                icon = protoCharacterMobViewModel.Icon;
            }
        }
    }
}