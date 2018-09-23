namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CoreMod.Characters;
    using AtomicTorch.CBND.CoreMod.Items.Tools.Axes;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures.ConstructionSite;
    using AtomicTorch.CBND.CoreMod.Systems.ServerOperator;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Data;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using AtomicTorch.GameEngine.Common.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    public class ViewModelWindowCNEIMenu : BaseViewModel
    {
        private readonly IReadOnlyList<ProtoEntityViewModel> allEntities;

        private string searchText = string.Empty;

        private bool IsCreativeModeOn => ServerOperatorSystem.ClientIsOperator();

        public int CurrentEntityCount => this.CurrentEntityList.Count;

        public IReadOnlyList<ProtoEntityViewModel> CurrentEntityList { get; private set; }

        public Visibility VisibilitySettings { get; set; } = Visibility.Collapsed;

        public Visibility VisibilityEntityList { get; set; } = Visibility.Visible;

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

                this.UpdateEntitiesList();
            }
        }

        // TODO: Update this on Operator status change
        public bool IsCreativePanelVisibile
        {
            get => IsCreativeModeOn;
        }

        public BaseCommand ShowDetails { get; }

        public ViewModelWindowCNEIMenu()
        {
            if (allEntities == null)
            {
                allEntities = GetAllEntitiesVMList();
            }
            UpdateEntitiesList();

            this.ShowDetails = new ActionCommand(() => WindowCNEIDetails.Open());
        }

        protected override void DisposeViewModel()
        {
            base.DisposeViewModel();
            foreach(var viewModelEntity in this.allEntities)
            {
                viewModelEntity.Dispose();
            }
        }

        private void UpdateEntitiesList()
        {
            if (this.CurrentEntityList == null)
            {
                //this.CurrentEntityList = new List<ProtoEntityViewModel> { new ProtoEntityViewModel(Api.GetProtoEntity<ObjectConstructionSite>()) };
                //this.CurrentEntityList = this.allEntities.Where(VME => VME.ProtoEntity is IProtoItem).ToList();
                this.CurrentEntityList = this.allEntities.ToList();
            }
        }

        private IReadOnlyList<ProtoEntityViewModel> GetAllEntitiesVMList()
        {
            var allEntitiesList = EntityList.AllEntity;
            List<ProtoEntityViewModel> allEntitiesVMList = new List<ProtoEntityViewModel>();
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
                        allEntitiesVMList.Add((ProtoEntityViewModel)Activator.CreateInstance(type, new object[] { entity }));
                    }
                    entityType = entityType.BaseType;
                } while ((entityType.BaseType != null) && (!templateFound));
                if (entityType == null)
                {
                    Api.Logger.Error("Template for " + entity + "not found");
                    allEntitiesVMList.Add(new ProtoEntityViewModel(entity));
                }
            }
            return allEntitiesVMList.AsReadOnly();
        }

        private string GetNameWithoutGenericArity(string s)
        {
            int index = s.IndexOf('`');
            return index == -1 ? s : s.Substring(0, index);
        }
    }
}