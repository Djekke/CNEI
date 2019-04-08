namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Controls
{
    using AtomicTorch.CBND.CoreMod.Characters;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.CBND.GameApi.ServicesClient;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public class EntitySlotControl : BaseControl
    {
        public static readonly DependencyProperty IsBackgroundEnabledProperty =
            DependencyProperty.Register(nameof(IsBackgroundEnabled),
                                        typeof(bool),
                                        typeof(EntitySlotControl),
                                        new PropertyMetadata(defaultValue: true));

        private Border border;

        private string currentSecondaryStateName;

        private EntitySlotControlEventsHelper itemSlotControlEventsHelper;

        static EntitySlotControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(EntitySlotControl),
                new FrameworkPropertyMetadata(typeof(EntitySlotControl)));
        }

        public EntitySlotControl()
        {
        }

        public bool IsBackgroundEnabled
        {
            get => (bool)GetValue(IsBackgroundEnabledProperty);
            set => SetValue(IsBackgroundEnabledProperty, value);
        }


        public bool IsSelectable { get; set; } = true;

        public void SetupCustomMouseClickHandler(EntitySlotControlEventsHelper.EntitySlotMouseClickDelegate handler)
        {
            itemSlotControlEventsHelper.CustomMouseClickHandler = handler;
        }

        protected override void InitControl()
        {
            itemSlotControlEventsHelper = new EntitySlotControlEventsHelper(this);
        }

        protected override void OnLoaded()
        {
            if (IsDesignTime)
            {
                return;
            }

            base.OnLoaded();

            // this cannot be done in InitControl, as later control might be removed and re-added
            var templateRoot = (FrameworkElement)VisualTreeHelper.GetChild(this, 0);
            border = templateRoot.GetByName<Border>("Border");

            ResetStates();
        }

        protected override void OnUnloaded()
        {
            if (IsDesignTime)
            {
                return;
            }

            base.OnUnloaded();
        }

        private void ResetStates()
        {
            SetCurrentSecondaryState("Normal", false);
        }

        private void SetCurrentSecondaryState(string stateName, bool withTransition)
        {
            if (currentSecondaryStateName == stateName)
            {
                return;
            }

            VisualStateManager.GoToElementState(border, stateName, withTransition);
            currentSecondaryStateName = stateName;
        }

        public class EntitySlotControlEventsHelper
        {
            private static WeakReference weakReferenceMousePressedControl;

            public EntitySlotMouseClickDelegate CustomMouseClickHandler;

            private readonly EntitySlotControl entitySlotControl;

            public EntitySlotControlEventsHelper(EntitySlotControl entitySlotControl)
            {
                this.entitySlotControl = entitySlotControl;
                entitySlotControl.MouseLeftButtonDown += SlotMouseButtonDownHandler;
                entitySlotControl.MouseLeftButtonUp += SlotMouseButtonUpHandler;
                entitySlotControl.MouseEnter += SlotMouseEnterHandler;
                entitySlotControl.MouseLeave += SlotMouseLeaveHandler;
            }

            public delegate bool EntitySlotMouseClickDelegate(bool isDown);

            private void SlotMouseButtonDownHandler(object sender, MouseButtonEventArgs e)
            {
                e.Handled = true;

                if (CustomMouseClickHandler?.Invoke(isDown: true) ?? false)
                {
                    return;
                }

                // remember that mouse was pressed on this control
                weakReferenceMousePressedControl = new WeakReference(this);
            }

            private void SlotMouseButtonUpHandler(object sender, MouseButtonEventArgs e)
            {
                e.Handled = true;

                if (CustomMouseClickHandler?.Invoke(isDown: false) ?? false)
                {
                    return;
                }

                var lastMousePressedControl = weakReferenceMousePressedControl?.Target;
                weakReferenceMousePressedControl = null;
                if (lastMousePressedControl != this)
                {
                    return;
                }

                if (entitySlotControl.DataContext is ProtoEntityViewModel entityViewModel)
                {
                    if (CreativePanelManager.IsCreativeModeOn)
                    {
                        switch (entityViewModel.ProtoEntity)
                        {
                            case IProtoItem item:
                                var itemCount = Api.Client.Input.IsKeyHeld(InputKey.Control, evenIfHandled: true)
                                    ? item.MaxItemsPerStack
                                    : 1;
                                CreativePanelManager.ExecuteCommand("/player.items.add " + item.ShortId + " " + itemCount);
                                break;
                            case IProtoCharacterMob mob:
                                CreativePanelManager.ExecuteCommand("/mobs.spawn " + mob.ShortId);
                                break;
                            default:
                                WindowCNEIdetails.Open(entityViewModel);
                                break;
                        }
                    }
                    else
                    {
                        WindowCNEIdetails.Open(entityViewModel);
                    }
                }
                else
                {
                    Api.Logger.Error("CNEI: Wrong view model for details window " + entitySlotControl.DataContext);
                }
            }

            private void SlotMouseEnterHandler(object sender, MouseEventArgs e)
            {
                entitySlotControl.SetCurrentSecondaryState("MouseOver", true);
            }

            private void SlotMouseLeaveHandler(object sender, MouseEventArgs e)
            {
                entitySlotControl.SetCurrentSecondaryState("Normal", true);
            }
        }
    }
}