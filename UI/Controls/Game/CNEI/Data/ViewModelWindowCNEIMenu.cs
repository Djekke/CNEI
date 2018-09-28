﻿namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CoreMod.Characters;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures;
    using AtomicTorch.CBND.CoreMod.Systems.ServerOperator;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class ViewModelWindowCNEIMenu : BaseViewModel
    {
        private List<ProtoEntityViewModel> allEntitiesVMList;

        //private SortedDictionary<string, List<ProtoEntityViewModel>> allEntitiesVMDictionary;

        //public ViewModelTypeHierarchy TypeHierarchy { get; private set; }

        private string searchText = string.Empty;

        private bool IsCreativeModeOn => ServerOperatorSystem.ClientIsOperator();

        // Default settings.
        private bool isShowingItems = true;
        private bool isShowingStructures = true;
        private bool isShowingMobs = true;
        private bool isShowingEntityWithTemplates = true; //TODO: change to false, true for debug
        private bool isShowingAll = false;

        //public int CurrentEntityCount => this.EntityVMList.Count;

        //public ObservableCollection<ProtoEntityViewModel> EntityVMList { get; private set; }

        public FilteredObservableWithPaging<ProtoEntityViewModel> FilteredEntityVMList { get; private set; }

        //public ObservableCollection<ViewModelEntityCategory> CategoryList { get; }

        public int PageCapacity = 154;

        public BaseCommand NextPage { get; }

        public BaseCommand PrevPage { get; }

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

        private bool SettingsFilter(ProtoEntityViewModel entityVM)
        {
            return IsShowingAll ||
                   (IsShowingEntityWithTemplates && (entityVM.GetType().BaseType == typeof(ProtoEntityViewModel))) ||
                   (IsShowingItems && (entityVM.ProtoEntity is IProtoItem)) ||
                   (IsShowingStructures && (entityVM.ProtoEntity is IProtoObjectStructure)) ||
                   (IsShowingMobs && (entityVM.ProtoEntity is IProtoCharacterMob));
        }

        public ViewModelWindowCNEIMenu()
        {
            if (this.allEntitiesVMList == null)
            {
                GetAllEntitiesVMList();
            }
            //UpdateEntitiesList();

            this.FilteredEntityVMList = new FilteredObservableWithPaging<ProtoEntityViewModel>(this.allEntitiesVMList);
            this.FilteredEntityVMList.AddFilter(SettingsFilter);
            this.FilteredEntityVMList.AddFilter(SearchFilter);
            this.FilteredEntityVMList.SetPageCapacity(this.PageCapacity);

            this.NextPage = new ActionCommand(() => this.FilteredEntityVMList.NextPage());
            this.PrevPage = new ActionCommand(() => this.FilteredEntityVMList.PrevPage());

            //this.CategoryList = new ObservableCollection<ViewModelEntityCategory>();
            //foreach (KeyValuePair<string, List<ProtoEntityViewModel>> entry in allEntitiesVMDictionary)
            //{
            //    this.CategoryList.Add(new ViewModelEntityCategory(entry.Key, entry.Value));
            //}
        }

        protected override void DisposeViewModel()
        {
            base.DisposeViewModel();
            foreach (var viewModelEntity in this.allEntitiesVMList)
            {
                viewModelEntity.Dispose();
            }
            //foreach (var viewModelEntityCategory in CategoryList)
            //{
            //    viewModelEntityCategory.Dispose();
            //}
            //TypeHierarchy.Dispose();
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

        // TODO: Update this on Operator status change
        public bool IsCreativePanelVisibile
        {
            get => this.IsCreativeModeOn;
        }

        private bool SearchFilter(ProtoEntityViewModel entityVM)
        {
            return (entityVM.Title.ToLower().Contains(this.searchText.ToLower())
                || entityVM.GetType().ToString().ToLower().Contains(this.searchText.ToLower())
                || entityVM.GetType().Name.ToLower().Contains(this.searchText.ToLower()));
        }

        //private void UpdateEntitiesList()
        //{
        //    if (this.EntityVMList == null)
        //    {
        //        //this.CurrentEntityList = new List<ProtoEntityViewModel> { new ProtoEntityViewModel(Api.GetProtoEntity<ObjectConstructionSite>()) };
        //        //this.CurrentEntityList = this.allEntities.Where(VME => VME.ProtoEntity is IProtoItem).ToList();
        //        this.EntityVMList = new ObservableCollection<ProtoEntityViewModel>(this.allEntitiesVMList);
        //    }
        //}

        private void GetAllEntitiesVMList()
        {
            this.allEntitiesVMList = new List<ProtoEntityViewModel>();
            //this.allEntitiesVMDictionary = new SortedDictionary<string, List<ProtoEntityViewModel>>();
            //this.TypeHierarchy = new ViewModelTypeHierarchy();
            var allEntitiesList = EntityList.AllEntity;
            foreach (var entity in allEntitiesList)
            {
                var entityType = entity.GetType();
                bool templateFound = false;
                Type type;
                do
                {
                    type = Type.GetType("AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data."
                                + GetNameWithoutGenericArity(entityType.Name) + "ViewModel");
                    if (type != null)
                    {
                        templateFound = true;
                        var newEntityVM = (ProtoEntityViewModel) Activator.CreateInstance(type, new object[] {entity});
                        //TypeHierarchy.Add(entity.GetType(), newEntityVM);
                        this.allEntitiesVMList.Add(newEntityVM);
                        //var keyName = GetNameWithoutGenericArity(entity.GetType().BaseType.ToString());
                        //AddEntityVMToDictonary(keyName, newEntityVM);
                    }
                    entityType = entityType.BaseType;
                } while ((entityType.BaseType != null) && (!templateFound));
                if (entityType == null)
                {
                    Api.Logger.Error("Template for " + entity + "not found");
                    var newEntityVM = new ProtoEntityViewModel(entity);
                    this.allEntitiesVMList.Add(newEntityVM);
                    //TypeHierarchy.Add(entity.GetType(), newEntityVM);
                    //var keyName = GetNameWithoutGenericArity(typeof(ProtoEntity).ToString());
                    //AddEntityVMToDictonary(keyName, newEntityVM);
                }
            }
        }

        private void AddEntityVMToDictonary(string keyName, ProtoEntityViewModel entityVM)
        {
            //if (keyName != null)
            //{
            //    if (allEntitiesVMDictionary.ContainsKey(keyName))
            //    {
            //        this.allEntitiesVMDictionary[keyName].Add(entityVM);
            //    }
            //    else
            //    {
            //        this.allEntitiesVMDictionary.Add(keyName, new List<ProtoEntityViewModel>() { entityVM });
            //    }
            //}
            //else
            //{
            //    Api.Logger.Error("Can not add entityVM to Dictonary for " + entityVM.ProtoEntity);
            //}
        }

        private string GetNameWithoutGenericArity(string s)
        {
            int index = s.IndexOf('`');
            return index == -1 ? s : s.Substring(0, index);
        }
    }
}