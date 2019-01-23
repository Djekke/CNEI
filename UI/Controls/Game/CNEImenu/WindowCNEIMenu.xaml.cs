namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data;

    public partial class WindowCNEImenu : BaseWindowMenu
    {
        private ViewModelWindowCNEImenu viewModel;

        protected override void WindowOpening()
        {
            base.WindowOpening();
            if (viewModel != null)
            {
                viewModel.UpdateCreativeStatus();
            }
            CreativePanel.UpdateStatus();
        }

        protected override void WindowClosing()
        {
            base.WindowClosing();
            TypeHierarchySelectView.Close();
        }

        protected override void DisposeMenu()
        {
            base.DisposeMenu();
            DataContext = null;
            viewModel.Dispose();
            viewModel = null;
        }

        protected override void InitMenu()
        {
            DataContext = viewModel = new ViewModelWindowCNEImenu();
        }
    }
}