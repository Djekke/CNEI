namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using AtomicTorch.CBND.CoreMod.ClientComponents.Input;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Scripting;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data;

    public partial class WindowCNEIdetails : BaseUserControlWithWindow
    {
        private Stack<ProtoEntityViewModel> entityVMStack = new Stack<ProtoEntityViewModel>();

        private static ClientInputContext windowInputContext;

        public static WindowCNEIdetails Instance { get; private set; }

        public static WindowCNEIdetails Open(ProtoEntityViewModel entityViewModel)
        {
            if (Instance == null)
            {
                var instance = new WindowCNEIdetails();
                instance.entityVMStack.Push(entityViewModel);
                Instance = instance;
                Api.Client.UI.LayoutRootChildren.Add(instance);
            }
            else
            {
                if (Instance.entityVMStack.Peek() != entityViewModel)
                {
                    Instance.entityVMStack.Push(entityViewModel);
                    Instance.DataContext = Instance.entityVMStack.Peek();
                }
            }
            return Instance;
        }

        protected override void InitControlWithWindow()
        {
            base.InitControlWithWindow();
            this.Window.IsCached = false;
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            this.DataContext = this.entityVMStack.Peek();
            windowInputContext = ClientInputContext.Start("CNEI details")
                .HandleButtonDown(GameButton.CNEImenuBack, this.OnBackButtonDown);
        }

        private protected void OnBackButtonDown()
        {
            if (this.entityVMStack.Count > 1)
            {
                this.entityVMStack.Pop();
                this.DataContext = this.entityVMStack.Peek();
            }
            else
            {
                this.CloseWindow();
            }
        }

        protected override void OnUnloaded()
        {
            base.OnUnloaded();
            this.DataContext = null;
            this.entityVMStack.Clear();
            windowInputContext?.Stop();
            windowInputContext = null;
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}