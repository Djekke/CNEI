namespace AtomicTorch.CBND.CoreMod.ClientComponents.Input
{
    using AtomicTorch.CBND.GameApi.ServicesClient;

    public enum GameButton
    {
        [ButtonInfo(
            "Cancel/close",
            "Cancel or close menu/window (usually Escape)",
            InputKey.Escape)]
        CancelOrClose,

        [ButtonInfo("Move up", InputKey.W)]
        MoveUp,

        [ButtonInfo("Move left", InputKey.A)]
        MoveLeft,

        [ButtonInfo("Move down", InputKey.S)]
        MoveDown,

        [ButtonInfo("Move right", InputKey.D)]
        MoveRight,

        [ButtonInfo(
            "Run (held)",
            "Run temporary, while the key is held",
            InputKey.Shift)]
        RunTemporary,

        [ButtonInfo(
            "Run (toggle)",
            "Switches between run/walk mode")]
        RunToggle,

        [ButtonInfo(
            "Action - use item",
            "Use the current item (from the active hotbar slot)",
            InputKey.MouseLeftButton)]
        ActionUseCurrentItem,

        [ButtonInfo(
            "Action - interact",
            "Interaction with the world object under the mouse cursor",
            InputKey.MouseRightButton)]
        ActionInteract,

        [ButtonInfo(
            "Reload item",
            "Reload item (from current hotbar slot)",
            InputKey.R)]
        ItemReload,

        [ButtonInfo(
            "Switch item mode",
            "Switch item mode (for example, switch/alternate the ammo type for current weapon)",
            InputKey.B)]
        ItemSwitchMode,

        [ButtonInfo(
            "Helmet light toggle",
            "Toggle currently equipped helmet light (if you have equipped a helmet with the light source)",
            InputKey.F)]
        HeadEquipmentLightToggle,

        [ButtonInfo(
            "Slot #1",
            "Select hotbar slot #1",
            InputKey.D1)]
        HotbarSelectSlot1,

        [ButtonInfo(
            "Slot #2",
            "Select hotbar slot #2",
            InputKey.D2)]
        HotbarSelectSlot2,

        [ButtonInfo(
            "Slot #3",
            "Select hotbar slot #3",
            InputKey.D3)]
        HotbarSelectSlot3,

        [ButtonInfo(
            "Slot #4",
            "Select hotbar slot #4",
            InputKey.D4)]
        HotbarSelectSlot4,

        [ButtonInfo(
            "Slot #5",
            "Select hotbar slot #5",
            InputKey.D5)]
        HotbarSelectSlot5,

        [ButtonInfo(
            "Slot #6",
            "Select hotbar slot #6",
            InputKey.D6)]
        HotbarSelectSlot6,

        [ButtonInfo(
            "Slot #7",
            "Select hotbar slot #7",
            InputKey.D7)]
        HotbarSelectSlot7,

        [ButtonInfo(
            "Slot #8",
            "Select hotbar slot #8",
            InputKey.D8)]
        HotbarSelectSlot8,

        [ButtonInfo(
            "Slot #9",
            "Select hotbar slot #9",
            InputKey.D9)]
        HotbarSelectSlot9,

        [ButtonInfo(
            "Slot #10",
            "Select hotbar slot #10",
            InputKey.D0)]
        HotbarSelectSlot10,

        [ButtonInfo("Chat", InputKey.Enter)]
        OpenChat,

        [ButtonInfo(
            "Inventory",
            "Inventory and equipment menu",
            InputKey.E)]
        EquipmentAndInventoryMenu,

        [ButtonInfo(
            "Crafting",
            "Crafting menu",
            InputKey.C)]
        CraftingMenu,

        [ButtonInfo(
            "Map",
            "World map",
            InputKey.M)]
        MapMenu,

        [ButtonInfo(
            "Construction",
            "Construction menu",
            InputKey.Tab)]
        ConstructionMenu,

        [ButtonInfo(
            "Skills",
            "Skills menu",
            InputKey.K)]
        SkillsMenu,

        [ButtonInfo(
            "Technologies",
            "Technologies menu",
            InputKey.G)]
        TechnologiesMenu,

        [ButtonInfo(
            "Social",
            "Social menu",
            InputKey.H)]
        SocialMenu,

        [ButtonInfo(
            "Quests",
            "Quests menu",
            InputKey.J)]
        QuestsMenu,

        [ButtonInfo(
            "Debug tools",
            "Toggle debug tools overlay",
            InputKey.F5)]
        ToggleDebugToolsOverlay,

        [ButtonInfo(
            "Toggle fullscreen",
            "Toggle fullscreen mode",
            InputKey.F11)]
        ToggleFullscreen,

        [ButtonInfo(
            "Capture screenshot",
            "(See the screenshot settings in the General options tab)",
            InputKey.F12)]
        CaptureScreenshot,

        [ButtonInfo(
            "Sort container",
            "Point on an items container and press this button to sort its content",
            InputKey.MouseScrollButton,
            InputKey.Z)]
        ContainerSort,

        [ButtonInfo(
            "Take all items",
            "Containers exchange: take all items",
            InputKey.R,
            Category = "Container menu")]
        ContainerTakeAll,

        [ButtonInfo(
            "Match items down",
            "Containers exchange: match items down",
            InputKey.T,
            Category = "Container menu")]
        ContainerMoveItemsMatchDown,

        [ButtonInfo(
            "Match items up",
            "Containers exchange: match items up",
            InputKey.Y,
            Category = "Container menu")]
        ContainerMoveItemsMatchUp,

        [ButtonInfo(
            "Display land claim",
            "Hold to display the land claim zones (or you can also just hold the Alt key)",
            InputKey.L)]
        DisplayLandClaim,

        [ButtonInfo(
            "Open CNEI",
            "Open CryoFall NEI",
            InputKey.N,
            Category = "CNEI")]
        CNEImenuOpen,

        [ButtonInfo(
            "Show item info",
            "Open CryoFall NEI Details window with information about item under mouse coursor",
            InputKey.I,
            Category = "CNEI")]
        CNEImenuDetails,

        [ButtonInfo(
            "Previous info",
            "Show previous information in Details window or close it",
            InputKey.Back,
            Category = "CNEI")]
        CNEImenuBack,
    }
}