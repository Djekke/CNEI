﻿namespace CryoFall.CNEI.UI.Data
{
    using System.Collections.ObjectModel;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using CryoFall.CNEI.Managers;

    public class ViewModelMainWindow : BaseViewModel
    {
        private string searchText = string.Empty;

        public FilteredObservableWithPaging<ProtoEntityViewModel> EntityViewModelCollection { get; }

        public ObservableCollection<TypeHierarchy> CurrentViewTypesCollection { get; }

        public int PageCapacity = 154;

        public BaseCommand NextPage { get; }

        public BaseCommand PrevPage { get; }

        public BaseCommand ChangeViewPreset { get; }

        public BaseCommand ToggleSettings { get; }

        public BaseCommand ShowInfo => HelpWindow.CommandOpenMenu;

        private bool SearchFilter(ProtoEntityViewModel entityViewModel)
        {
            var searchTextLowCase = searchText.ToLower();
            return entityViewModel.TypeLower.Contains(searchTextLowCase)
                || entityViewModel.TitleLower.Contains(searchTextLowCase);
        }

        public ViewModelMainWindow()
        {
            EntityViewModelCollection = EntityViewModelsManager.CurrentView;
            EntityViewModelCollection.RemoveAllFilters();
            EntityViewModelCollection.AddFilter(SearchFilter);
            EntityViewModelCollection.SetPageCapacity(PageCapacity);

            CurrentViewTypesCollection = EntityViewModelsManager.TypeHierarchyPlaneCollection;

            NextPage = new ActionCommand(() => EntityViewModelCollection.NextPage());
            PrevPage = new ActionCommand(() => EntityViewModelCollection.PrevPage());
            ChangeViewPreset = new ActionCommand(TypeHierarchySelectView.Open);
            ToggleSettings = new ActionCommandWithParameter(isChecked =>
            {
                if ((bool) isChecked == false)
                {
                    EntityViewModelsManager.SaveSettings();
                }
            });
        }

        public string SearchText
        {
            get => searchText;
            set
            {
                value = value?.TrimStart() ?? string.Empty;
                if (searchText == value)
                {
                    return;
                }
                searchText = value;
                NotifyThisPropertyChanged();
                EntityViewModelCollection.Refresh();
            }
        }

        public bool IsDefaultViewOn
        {
            get => EntityViewModelsManager.IsDefaultViewOn;
            set
            {
                if (value == IsDefaultViewOn)
                {
                    return;
                }
                EntityViewModelsManager.IsDefaultViewOn = value;

                NotifyThisPropertyChanged();
            }
        }

        public bool IsShowingEntityWithTemplates
        {
            get => EntityViewModelsManager.IsShowingEntityWithTemplates;
            set
            {
                if (value == IsShowingEntityWithTemplates)
                {
                    return;
                }
                EntityViewModelsManager.IsShowingEntityWithTemplates = value;

                NotifyThisPropertyChanged();
            }
        }

        public bool IsShowingAll
        {
            get => EntityViewModelsManager.IsShowingAll;
            set
            {
                if (value == IsShowingAll)
                {
                    return;
                }
                EntityViewModelsManager.IsShowingAll = value;

                NotifyThisPropertyChanged();
            }
        }

        public bool IsTypeVisible
        {
            get => EntityViewModelsManager.IsTypeVisible;
            set
            {
                if (value == IsTypeVisible)
                {
                    return;
                }

                EntityViewModelsManager.IsTypeVisible = value;
                NotifyThisPropertyChanged();
            }
        }

        public bool HideUnreachableObjects
        {
            get => EntityViewModelsManager.HideUnreachableObjects;
            set
            {
                if (value == HideUnreachableObjects)
                {
                    return;
                }

                EntityViewModelsManager.HideUnreachableObjects = value;
                NotifyThisPropertyChanged();
            }
        }
    }
}