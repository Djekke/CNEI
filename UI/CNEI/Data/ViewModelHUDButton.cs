namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core.Menu;
    using CryoFall.CNEI.UI.Controls;

    public class ViewModelHUDButton: BaseViewModel
    {
        public Menu MenuCNEI { get; }

        public IButtonReference ButtonReference { get; set; }

        public ViewModelHUDButton()
        {
            MenuCNEI = Menu.Register<MainWindow>();
            ButtonReference = new CNEIButtonReference() { Button = CNEIButton.MenuOpen };
        }

        protected override void DisposeViewModel()
        {
            base.DisposeViewModel();

            MenuCNEI?.Dispose();
            ButtonReference = null;
        }
    }
}