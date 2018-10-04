namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Characters;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Loot;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Minerals;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Vegetation;
    using AtomicTorch.CBND.CoreMod.Systems.ServerOperator;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ViewModelWindowCNEImenu : BaseViewModel
    {
        private string searchText = string.Empty;

        // Default settings.
        private bool isDefaultViewOn = true;
        private bool isShowingEntityWithTemplates = false;
        private bool isShowingAll = false;

        private static List<Type> defaultViewTypes = new List<Type>()
        {
            typeof(IProtoItem),
            typeof(IProtoObjectStructure),
            typeof(IProtoCharacterMob),
            typeof(IProtoObjectMineral),
            typeof(IProtoObjectLoot),
            typeof(IProtoObjectVegetation),
        };

        public FilteredObservableWithPaging<ProtoEntityViewModel> FilteredEntityVMList { get; }

        public int PageCapacity = 154;

        public BaseCommand NextPage { get; }

        public BaseCommand PrevPage { get; }

        // TODO: rewrite settings filtering (listbox of comboxes to select what types to show)
        private bool SettingsFilter(ProtoEntityViewModel entityVM)
        {
            return IsShowingAll ||
                   (IsShowingEntityWithTemplates && entityVM.GetType().IsSubclassOf(typeof(ProtoEntityViewModel))) ||
                   (IsDefaultViewOn && (defaultViewTypes.Any(t => t.IsInstanceOfType(entityVM.ProtoEntity))));
        }

        private bool SearchFilter(ProtoEntityViewModel entityVM)
        {
            return (entityVM.Title.ToLower().Contains(this.searchText.ToLower())
                    || entityVM.GetType().ToString().ToLower().Contains(this.searchText.ToLower())
                    || entityVM.GetType().Name.ToLower().Contains(this.searchText.ToLower()));
        }

        public ViewModelWindowCNEImenu()
        {
            this.FilteredEntityVMList =
                new FilteredObservableWithPaging<ProtoEntityViewModel>(EntityViewModelsManager
                    .GetAllEntityViewModels());
            this.FilteredEntityVMList.AddFilter(SettingsFilter);
            this.FilteredEntityVMList.AddFilter(SearchFilter);
            this.FilteredEntityVMList.SetPageCapacity(this.PageCapacity);

            this.NextPage = new ActionCommand(() => this.FilteredEntityVMList.NextPage());
            this.PrevPage = new ActionCommand(() => this.FilteredEntityVMList.PrevPage());
        }

        public string SearchText
        {
            get => this.searchText;
            set
            {
                value = value?.TrimStart() ?? string.Empty;
                if (this.searchText == value)
                {
                    return;
                }

                this.searchText = value;
                this.NotifyThisPropertyChanged();

                this.FilteredEntityVMList.Refresh();
            }
        }

        public bool IsDefaultViewOn
        {
            get => this.isDefaultViewOn;
            set
            {
                if (value == this.isDefaultViewOn)
                {
                    return;
                }
                this.isDefaultViewOn = value;
                if (!this.isDefaultViewOn)
                {
                    this.isShowingEntityWithTemplates = false;
                    this.NotifyPropertyChanged("IsShowingEntityWithTemplates");
                    this.isShowingAll = false;
                    this.NotifyPropertyChanged("IsShowingAll");
                }
                this.FilteredEntityVMList.Refresh();
                this.NotifyThisPropertyChanged();
            }
        }

        public bool IsShowingEntityWithTemplates
        {
            get => this.isShowingEntityWithTemplates;
            set
            {
                if (value == this.isShowingEntityWithTemplates)
                {
                    return;
                }
                this.isShowingEntityWithTemplates = value;
                if (isShowingEntityWithTemplates)
                {
                    this.IsDefaultViewOn = true;
                }
                else
                {
                    this.isShowingAll = false;
                    this.NotifyPropertyChanged("IsShowingAll");
                }
                this.FilteredEntityVMList.Refresh();
                this.NotifyThisPropertyChanged();
            }
        }

        public bool IsShowingAll
        {
            get => this.isShowingAll;
            set
            {
                if (value == this.isShowingAll)
                {
                    return;
                }
                this.isShowingAll = value;
                if (isShowingAll)
                {
                    this.isDefaultViewOn = true;
                    this.NotifyPropertyChanged("IsDefaultViewOn");
                    this.isShowingEntityWithTemplates = true;
                    this.NotifyPropertyChanged("IsShowingEntityWithTemplates");
                }
                this.FilteredEntityVMList.Refresh();
                this.NotifyThisPropertyChanged();
            }
        }

        // TODO: Update this on Operator status change.
        public bool IsCreativePanelVisibile
        {
            get => ServerOperatorSystem.ClientIsOperator();
        }
    }
}