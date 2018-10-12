namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using System.Collections.ObjectModel;

    public class ViewModelTypeHierarchySelectView : BaseViewModel
    {
        public ObservableCollection<TypeHierarchy> TypeHierarchyPlaneList { get; }

        public ViewModelTypeHierarchySelectView()
        {
            TypeHierarchyPlaneList =
                new ObservableCollection<TypeHierarchy>(EntityViewModelsManager.TypeHierarchyPlaneCollection);
            Save = new ActionCommand(SaveChanges);
            Cancel = new ActionCommand(TypeHierarchySelectView.Close);
        }

        public BaseCommand Save { get; }

        public BaseCommand Cancel { get; }

        public void SaveChanges()
        {
            Api.Logger.Dev("Save");
            foreach (TypeHierarchy node in TypeHierarchyPlaneList)
            {
                node.IsCheckedSavedState = node.IsChecked;
            }

            EntityViewModelsManager.SaveViewPreset();
        }
    }
}