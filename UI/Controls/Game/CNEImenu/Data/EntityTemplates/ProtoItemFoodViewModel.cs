namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Food;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System;

    public class ProtoItemFoodViewModel : ProtoItemViewModel
    {
        public ProtoItemFoodViewModel([NotNull] IProtoItemFood food) : base(food)
        {
        }

        /// <summary>
        /// Initilize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is IProtoItemFood food)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Stay fresh",
                    food.FreshnessDuration == TimeSpan.Zero ? "forever" : food.FreshnessDuration.ToString("g")));
                EntityInformation.Add(new ViewModelEntityInformation("Food restore", food.FoodRestore));
                EntityInformation.Add(new ViewModelEntityInformation("Water restore", food.WaterRestore));
                EntityInformation.Add(new ViewModelEntityInformation("Health restore", food.HealthRestore));
                EntityInformation.Add(new ViewModelEntityInformation("Energy restore", food.EnergyRestore));
            }
        }
    }
}