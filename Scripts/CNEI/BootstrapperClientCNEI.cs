namespace CryoFall.CNEI
{
    using AtomicTorch.CBND.CoreMod.Bootstrappers;
    using AtomicTorch.CBND.CoreMod.ClientComponents.Input;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core.Menu;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Game.HUD;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Game.Items.Controls;
    using AtomicTorch.CBND.GameApi.Data;
    using AtomicTorch.CBND.GameApi.Data.Characters;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using CryoFall.CNEI.Managers;
    using CryoFall.CNEI.UI;
    using CryoFall.CNEI.UI.Controls;
    using CryoFall.CNEI.UI.Data;
    using CryoFall.CNEI.UI.Helpers;

    [PrepareOrder(afterType: typeof(BootstrapperClientOptions))]
    public class BootstrapperClientCNEI: BaseBootstrapper
    {
        private static ClientInputContext gameplayInputContext;

        private static ViewModelHUDButton hudButton;

        private static HUDLayoutControl hudLayoutControl;

        public override void ClientInitialize()
        {
            ClientInputManager.RegisterButtonsEnum<CNEIButton>();

            EntityViewModelsManager.Init();

            BootstrapperClientGame.InitEndCallback += GameInitHandler;

            BootstrapperClientGame.ResetCallback += ResetHandler;
        }

        private static void GameInitHandler(ICharacter currentCharacter)
        {
            hudButton = new ViewModelHUDButton();

            foreach (var child in Api.Client.UI.LayoutRootChildren)
            {
                if (child is HUDLayoutControl layoutControl)
                {
                    hudLayoutControl = layoutControl;
                }
            }

            if (hudLayoutControl != null)
            {
                hudLayoutControl.Loaded += LayoutControl_Loaded;
            }
            else
            {
                Api.Logger.Error("CNEI: HUDLayoutControl not found.");
            }

            gameplayInputContext = ClientInputContext
                                   .Start("CNEI menu")
                                   .HandleButtonDown(CNEIButton.MenuOpen, Menu.Toggle<MainWindow>)
                                   .HandleButtonDown(CNEIButton.MenuDetails, ShowContextDetails);

            CreativePanelManager.Init();
        }

        private static void LayoutControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            HUDButtonHelper.AddHUDButton(hudButton.MenuCNEI,
                                         "/Content/Textures/Quests/Unknown.png",
                                         "/Content/Textures/Quests/Unknown.png",
                                         "CryoFall NEI",
                                         hudButton.ButtonReference);
        }

        private static void ResetHandler()
        {
            DetailsWindow.Close();

            CreativePanelManager.Reset();

            hudButton?.Dispose();
            hudButton = null;

            if (hudLayoutControl != null)
            {
                hudLayoutControl.Loaded -= LayoutControl_Loaded;
                hudLayoutControl = null;
            }

            gameplayInputContext?.Stop();
            gameplayInputContext = null;
        }

        private static void ShowContextDetails()
        {
            var hitTestResult = Api.Client.UI.GetVisualInPointedPosition();
            if (hitTestResult == null)
            {
                return;
            }

            if (VisualTreeHelperExtension.FindParentOfType(
                hitTestResult, typeof(ItemSlotControl)) is ItemSlotControl itemSlotControl)
            {
                if (itemSlotControl.Item != null)
                {
                    DetailsWindow.Open(
                        EntityViewModelsManager.GetEntityViewModel(itemSlotControl.Item.ProtoItem));
                }

                return;
            }

            if (VisualTreeHelperExtension.FindParentOfType(
                hitTestResult, typeof(RequiredItemControl)) is RequiredItemControl requiredItemControl)
            {
                if (requiredItemControl.ProtoItemWithCount != null)
                {
                    DetailsWindow.Open(
                        EntityViewModelsManager.GetEntityViewModel(requiredItemControl.ProtoItemWithCount.ProtoItem));
                }

                return;
            }

            if (VisualTreeHelperExtension.FindParentOfType(
                hitTestResult, typeof(EntitySlotControl)) is EntitySlotControl entitySlotControl)
            {
                if (entitySlotControl.DataContext is ProtoEntityViewModel entityViewModel)
                {
                    DetailsWindow.Open(entityViewModel);
                }
                else
                {
                    Api.Logger.Error("CNEI: Wrong view model for details window " + entitySlotControl.DataContext);
                }
            }
        }
    }
}