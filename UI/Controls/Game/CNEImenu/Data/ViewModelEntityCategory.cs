namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;

    public class ViewModelEntityCategory : BaseViewModel
    {
        //private readonly IReadOnlyList<ProtoEntityViewModel> allEntitiesInCurrentCategory;

        public ViewModelEntityCategory(string title, List<ProtoEntityViewModel> entitiesVMList)
        {
            Title = title;
            //this.allEntitiesInCurrentCategory = entitiesVMList;
            EntityVMList = new ObservableCollection<ProtoEntityViewModel>(entitiesVMList);
        }

        public string Title { get; }

        public ObservableCollection<ProtoEntityViewModel> EntityVMList { get; }

        public Visibility Visibility { get; set; } = Visibility.Visible;
    }
}