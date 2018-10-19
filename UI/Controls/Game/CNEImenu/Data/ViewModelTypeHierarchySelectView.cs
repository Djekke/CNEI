namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using System.Collections.ObjectModel;

    public class ViewModelTypeHierarchySelectView : BaseViewModel
    {
        public ObservableCollection<TypeHierarchy> TypeHierarchyPlaneList { get; }

        public TypeHierarchy MainNode { get; }

        public ViewModelTypeHierarchySelectView()
        {
            TypeHierarchyPlaneList = EntityViewModelsManager.TypeHierarchyPlaneCollection;
            MainNode = EntityViewModelsManager.EntityTypeHierarchy;
            Save = new ActionCommand(SaveChanges);
            Cancel = new ActionCommand(TypeHierarchySelectView.Close);
        }

        public BaseCommand Save { get; }

        public BaseCommand Cancel { get; }

        /// <summary>
        /// Save changes in IsChecked state for every node in TypeHierarchy.
        /// </summary>
        public static void SaveChanges()
        {
            foreach (TypeHierarchy node in EntityViewModelsManager.TypeHierarchyPlaneCollection)
            {
                node.IsCheckedSavedState = node.IsChecked;
            }

            EntityViewModelsManager.SaveViewPreset();
            TypeHierarchySelectView.Close();
        }
    }
}