using AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data;

namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Game.Items.Controls.Tooltips;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Game.Items.Data;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Game.Items.Managers;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;

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
            get => (bool)this.GetValue(IsBackgroundEnabledProperty);
            set => this.SetValue(IsBackgroundEnabledProperty, value);
        }


        public bool IsSelectable { get; set; } = true;

        public void SetupCustomMouseClickHandler(EntitySlotControlEventsHelper.EntitySlotMouseClickDelegate handler)
        {
            this.itemSlotControlEventsHelper.CustomMouseClickHandler = handler;
        }

        protected override void InitControl()
        {
            this.itemSlotControlEventsHelper = new EntitySlotControlEventsHelper(this);
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
            this.border = templateRoot.GetByName<Border>("Border");

            this.ResetStates();
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
            this.SetCurrentSecondaryState("Normal", false);
        }

        private void SetCurrentSecondaryState(string stateName, bool withTransition)
        {
            if (this.currentSecondaryStateName == stateName)
            {
                return;
            }

            VisualStateManager.GoToElementState(this.border, stateName, withTransition);
            this.currentSecondaryStateName = stateName;
        }

        public class EntitySlotControlEventsHelper
        {
            private static WeakReference weakReferenceMousePressedControl;

            public EntitySlotMouseClickDelegate CustomMouseClickHandler;

            private readonly EntitySlotControl entitySlotControl;

            public EntitySlotControlEventsHelper(EntitySlotControl entitySlotControl)
            {
                this.entitySlotControl = entitySlotControl;
                entitySlotControl.MouseLeftButtonDown += this.SlotMouseButtonDownHandler;
                entitySlotControl.MouseLeftButtonUp += this.SlotMouseButtonUpHandler;
                entitySlotControl.MouseEnter += this.SlotMouseEnterHandler;
                entitySlotControl.MouseLeave += this.SlotMouseLeaveHandler;
            }

            public delegate bool EntitySlotMouseClickDelegate(bool isDown);

            private void SlotMouseButtonDownHandler(object sender, MouseButtonEventArgs e)
            {
                e.Handled = true;

                if (this.CustomMouseClickHandler?.Invoke(isDown: true) ?? false)
                {
                    return;
                }

                // remember that mouse was pressed on this control
                weakReferenceMousePressedControl = new WeakReference(this);
            }

            private void SlotMouseButtonUpHandler(object sender, MouseButtonEventArgs e)
            {
                e.Handled = true;

                if (this.CustomMouseClickHandler?.Invoke(isDown: false) ?? false)
                {
                    return;
                }

                var lastMousePressedControl = weakReferenceMousePressedControl?.Target;
                weakReferenceMousePressedControl = null;
                if (lastMousePressedControl == this)
                {
                    WindowCNEIDetails.Open((ProtoEntityViewModel)entitySlotControl.DataContext);
                }
            }

            private void SlotMouseEnterHandler(object sender, MouseEventArgs e)
            {
                this.entitySlotControl.SetCurrentSecondaryState("MouseOver", true);
            }

            private void SlotMouseLeaveHandler(object sender, MouseEventArgs e)
            {
                this.entitySlotControl.SetCurrentSecondaryState("Normal", true);
            }
        }
    }
}