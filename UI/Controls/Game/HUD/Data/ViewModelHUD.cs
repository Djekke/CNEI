namespace AtomicTorch.CBND.CoreMod.UI.Controls.Game.HUD.Data
{
    using AtomicTorch.CBND.CoreMod.Systems.Construction;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core.Menu;
    using AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Game.Map;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Game.Player;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Game.Quests;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Game.Skills;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Game.Social;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Game.Technologies;

    public class ViewModelHUD : BaseViewModel
    {
        public ViewModelHUD()
        {
            this.Stats = new ViewModelHUDStats();

            if (IsDesignTime)
            {
                return;
            }

            this.MenuConstruction = Menu.Register(ConstructionPlacementSystem.Instance);

            this.MenuCrafting =
                Menu.Register<WindowPlayerMenu, WindowPlayerMenuTab>(WindowPlayerMenuTab.InventoryCrafting);

            this.MenuInventoryEquipment =
                Menu.Register<WindowPlayerMenu, WindowPlayerMenuTab>(WindowPlayerMenuTab.InventoryEquipment);

            this.MenuMap = Menu.Register<WindowWorldMap>();
            this.MenuSkills = Menu.Register<WindowSkills>();
            this.MenuSocial = Menu.Register<WindowSocial>();
            this.MenuTechTree = Menu.Register<WindowTechnologies>();
            this.MenuQuests = Menu.Register<WindowQuests>();
            this.MenuCNEI = Menu.Register<WindowCNEIMenu>();
        }

        public Menu MenuConstruction { get; }

        public Menu MenuCrafting { get; }

        public Menu MenuInventoryEquipment { get; }

        public Menu MenuMap { get; }

        public Menu MenuQuests { get; }

        public Menu MenuSkills { get; }

        public Menu MenuSocial { get; }

        public Menu MenuTechTree { get; }

        public Menu MenuCNEI { get; }

        public ViewModelHUDStats Stats { get; }

        protected override void DisposeViewModel()
        {
            base.DisposeViewModel();

            this.MenuConstruction.Dispose();
            this.MenuCrafting.Dispose();
            this.MenuInventoryEquipment.Dispose();
            this.MenuMap.Dispose();
            this.MenuSkills.Dispose();
            this.MenuSocial.Dispose();
            this.MenuTechTree.Dispose();
            this.MenuQuests.Dispose();
            this.MenuCNEI.Dispose();
        }
    }
}