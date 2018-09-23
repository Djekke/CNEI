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
        private static IReadOnlyList<IProtoEntity> allEntity;

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