namespace CryoFall.CNEI.UI
{
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using AtomicTorch.CBND.CoreMod.ClientComponents.Input;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using CryoFall.CNEI.Managers;
    using CryoFall.CNEI.UI.Data;

    public partial class DetailsWindow : BaseUserControlWithWindow
    {
        private Stack<ProtoEntityViewModel> entityVMStack = new Stack<ProtoEntityViewModel>();

        private Run pageCountText;

        private Button buttonBack;

        private static ClientInputContext windowInputContext;

        public static DetailsWindow Instance { get; private set; }

        public static void Open(ProtoEntityViewModel entityViewModel)
        {
            if (Instance == null)
            {
                var instance = new DetailsWindow();
                instance.entityVMStack.Push(entityViewModel);
                Instance = instance;
                Api.Client.UI.LayoutRootChildren.Add(instance);
            }
            else
            {
                if (Instance.entityVMStack.Peek() != entityViewModel)
                {
                    Instance.entityVMStack.Push(entityViewModel);
                    Instance.UpdateCount();
                    Instance.DataContext = Instance.entityVMStack.Peek();
                }
            }
        }

        public static void Close()
        {
            if (Instance?.IsOpened == true)
            {
                Instance.CloseWindow();
            }
        }

        protected override void InitControlWithWindow()
        {
            base.InitControlWithWindow();
            Resources.MergedDictionaries.Add(EntityViewModelsManager.AllEntityTemplatesResourceDictionary);
            pageCountText = FindName("PageCountText") as Run;
            buttonBack = FindName("ButtonBack") as Button;
            buttonBack.Command = new ActionCommand(OnBackButtonDown);
            UpdateCount();
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            DataContext = entityVMStack.Peek();
            windowInputContext = ClientInputContext.Start("CNEI details")
                .HandleButtonDown(CNEIButton.MenuBack, OnBackButtonDown);
        }

        private protected void OnBackButtonDown()
        {
            if (entityVMStack.Count > 1)
            {
                entityVMStack.Pop();
                UpdateCount();
                DataContext = entityVMStack.Peek();
            }
            else
            {
                CloseWindow();
            }
        }

        protected override void OnUnloaded()
        {
            base.OnUnloaded();
            DataContext = null;
            entityVMStack.Clear();
            windowInputContext?.Stop();
            windowInputContext = null;
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void UpdateCount()
        {
            pageCountText.Text = "(" + entityVMStack.Count.ToString() + ")";
            if (entityVMStack.Count > 1)
            {
                buttonBack.Content = "Go back";
            }
            else
            {
                buttonBack.Content = "Close";
            }
        }
    }
}