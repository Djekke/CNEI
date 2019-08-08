namespace CryoFall.CNEI.UI
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using CryoFall.CNEI.UI.Data;

    public partial class MainWindow : BaseWindowMenu
    {
        private ViewModelMainWindow viewModel;

        protected override void WindowOpening()
        {
            base.WindowOpening();
            if (viewModel != null)
            {
                viewModel.UpdateCreativeStatus();
            }
        }

        protected override void WindowClosing()
        {
            base.WindowClosing();
            TypeHierarchySelectView.Close();
            HelpWindow.Close();
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
            DataContext = viewModel = new ViewModelMainWindow();
        }
    }
}