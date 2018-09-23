namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CoreMod.Characters;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures;
    using AtomicTorch.CBND.GameApi.Data;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.CBND.GameApi.Scripting;
    using System.Collections.Generic;

    public class EntityList
    {
        private static IReadOnlyList<IProtoItem> allItems;

        private static IReadOnlyList<IProtoObjectStructure> allStructures;

        private static IReadOnlyList<IProtoCharacterMob> allCreatures;

        private static IReadOnlyList<IProtoEntity> allEntity;

        public static IReadOnlyList<IProtoItem> AllItems
        {
            get
            {
                if (allItems == null)
                {
                    allItems = Api.FindProtoEntities<IProtoItem>();
                }
                return allItems;
            }
        }

        public static IReadOnlyList<IProtoObjectStructure> AllStructures
        {
            get
            {
                if (allStructures == null)
                {
                    allStructures = Api.FindProtoEntities<IProtoObjectStructure>();
                }
                return allStructures;
            }
        }

        public static IReadOnlyList<IProtoCharacterMob> AllCreatures
        {
            get
            {
                if (allCreatures == null)
                {
                    allCreatures = Api.FindProtoEntities<IProtoCharacterMob>();
                }
                return allCreatures;
            }
        }

        public static IReadOnlyList<IProtoEntity> AllEntity
        {
            get
            {
                if (allEntity == null)
                {
                    allEntity = Api.FindProtoEntities<IProtoEntity>();
                }
                return allEntity;
            }
        }

        public EntityList()
        {

        }
    }
}