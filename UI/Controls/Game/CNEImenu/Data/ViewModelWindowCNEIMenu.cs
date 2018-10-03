namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Characters;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures;
    using AtomicTorch.CBND.CoreMod.Systems.ServerOperator;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;

    public class ViewModelWindowCNEIMenu : BaseViewModel
    {
        private string searchText = string.Empty;

        // Default settings.
        private bool isShowingItems = true;
        private bool isShowingStructures = true;
        private bool isShowingMobs = true;
        private bool isShowingEntityWithTemplates = true; //TODO: change to false, true for debug
        private bool isShowingAll = false;

        public FilteredObservableWithPaging<ProtoEntityViewModel> FilteredEntityVMList { get; }

        public int PageCapacity = 154;

        public BaseCommand NextPage { get; }

        public BaseCommand PrevPage { get; }

        // TODO: rewrite settings filtering (listbox of comboxes to select what types to show)
        private bool SettingsFilter(ProtoEntityViewModel entityVM)
        {
            return IsShowingAll ||
                   (IsShowingEntityWithTemplates && entityVM.GetType().IsSubclassOf(typeof(ProtoEntityViewModel))) ||
                   (IsShowingItems && (entityVM.ProtoEntity is IProtoItem)) ||
                   (IsShowingStructures && (entityVM.ProtoEntity is IProtoObjectStructure)) ||
                   (IsShowingMobs && (entityVM.ProtoEntity is IProtoCharacterMob));
        }

        private bool SearchFilter(ProtoEntityViewModel entityVM)
        {
            return (entityVM.Title.ToLower().Contains(this.searchText.ToLower())
                    || entityVM.GetType().ToString().ToLower().Contains(this.searchText.ToLower())
                    || entityVM.GetType().Name.ToLower().Contains(this.searchText.ToLower()));
        }

        public ViewModelWindowCNEIMenu()
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

        public bool IsShowingItems
        {
            get => this.isShowingItems;
            set
            {
                if (value == this.isShowingItems)
                {
                    return;
                }
                this.isShowingItems = value;
                if (!this.isShowingItems)
                {
                    this.IsShowingEntityWithTemplates = false;
                    this.IsShowingAll = false;
                }
                this.FilteredEntityVMList.Refresh();
                this.NotifyThisPropertyChanged();
            }
        }

        public bool IsShowingStructures
        {
            get => this.isShowingStructures;
            set
            {
                if (value == this.isShowingStructures)
                {
                    return;
                }
                this.isShowingStructures = value;
                if (!this.isShowingStructures)
                {
                    this.IsShowingEntityWithTemplates = false;
                    this.IsShowingAll = false;
                }
                this.FilteredEntityVMList.Refresh();
                this.NotifyThisPropertyChanged();
            }
        }

        public bool IsShowingMobs
        {
            get => this.isShowingMobs;
            set
            {
                if (value == this.isShowingMobs)
                {
                    return;
                }
                this.isShowingMobs = value;
                if (!this.isShowingMobs)
                {
                    this.IsShowingEntityWithTemplates = false;
                    this.IsShowingAll = false;
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
                    this.IsShowingItems = true;
                    this.IsShowingStructures = true;
                    this.IsShowingMobs = true;
                }
                else
                {
                    this.IsShowingAll = false;
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
                    this.IsShowingItems = true;
                    this.IsShowingStructures = true;
                    this.IsShowingMobs = true;
                    this.IsShowingEntityWithTemplates = true;
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