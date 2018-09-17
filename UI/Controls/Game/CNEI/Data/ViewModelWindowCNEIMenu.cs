namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CoreMod.Characters;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.GameEngine.Common.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ViewModelWindowCNEIMenu : BaseViewModel
    {
        public int ItemsCount => this.ItemsList.Count;

        public int StructuresCount => this.StructuresList.Count;

        public int CreaturesCount => this.CreaturesList.Count;

        public IReadOnlyList<ViewModelItem> ItemsList { get; }

        public IReadOnlyList<ViewModelStructure> StructuresList { get; }

        public IReadOnlyList<ViewModelCreature> CreaturesList { get; }
        
        public ViewModelWindowCNEIMenu()
        {
            this.ItemsList = GetAllItems()
                                .Select(item => new ViewModelItem(item))
                                .ToList();
            this.StructuresList = GetAllStructures()
                                    .Select(structure => new ViewModelStructure(structure))
                                    .ToList();
            this.CreaturesList = GetAllCreatures()
                                    .Select(creature => new ViewModelCreature(creature))
                                    .ToList();
        }

        protected override void DisposeViewModel()
        {
            base.DisposeViewModel();
            foreach(var viewModelItem in this.ItemsList)
            {
                viewModelItem.Dispose();
            }
            foreach (var viewModelStructure in this.StructuresList)
            {
                viewModelStructure.Dispose();
            }
            foreach (var viewModelCreature in this.CreaturesList)
            {
                viewModelCreature.Dispose();
            }
        }

        private List<IProtoItem> GetAllItems()
        {
            var list = Api.FindProtoEntities<IProtoItem>().ToList();
            list.SortBy(r => r.Id, StringComparer.Ordinal);
            return list;
        }

        private List<IProtoObjectStructure> GetAllStructures()
        {
            var list = Api.FindProtoEntities<IProtoObjectStructure>().ToList();
            list.SortBy(r => r.Id, StringComparer.Ordinal);
            return list;
        }

        private List<IProtoCharacterMob> GetAllCreatures()
        {
            var list = Api.FindProtoEntities<IProtoCharacterMob>().ToList();
            list.SortBy(r => r.Id, StringComparer.Ordinal);
            return list;
        }
    }
}