namespace CryoFall.CNEI.UI.Data
{
    using System;
    using System.Collections.Generic;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class AmmoUsageViewModel : RecipeViewModel
    {
        public override string ResourceDictionaryName => "AmmoUsageDataTemplate.xaml";

        public override string RecipeTypeName => "Ammo usage";

        /// <summary>
        /// Constructor for entity with droplist.
        /// Must be used only in InitEntityRelationships phase.
        /// </summary>
        /// <param name="ammoViewModel">View Model of ammo.</param>
        /// <param name="gunViewModel">View Model of gun that uses that ammo.</param>
        public AmmoUsageViewModel([NotNull] ProtoEntityViewModel ammoViewModel,
            [NotNull] ProtoEntityViewModel gunViewModel)
            : base(ammoViewModel.ProtoEntity)
        {
            if (!EntityViewModelsManager.EntityDictionaryCreated)
            {
                throw new Exception("CNEI: Droplist constructor used before all entity VMs sets.");
            }

            AmmoEntity = ammoViewModel;

            InputItemsList.Add(ammoViewModel);

            //GunsVMList.Add(gunViewModel);
            //GunsInformationList.Add(new ViewModelEntityWithCount(gunViewModel));
            AddAmmoUsage(gunViewModel);
        }

        public void AddAmmoUsage([NotNull] ProtoEntityViewModel gunViewModel)
        {
            //GunsVMList.Add(gunViewModel);
            GunsInformationList.Add(new ViewModelEntityWithCount(gunViewModel));
        }

        public ProtoEntityViewModel AmmoEntity { get; protected set; }

        //public List<ProtoEntityViewModel> GunsVMList { get; protected set; }
        //    = new List<ProtoEntityViewModel>();

        public List<BaseViewModel> GunsInformationList { get; protected set; }
            = new List<BaseViewModel>();
    }
}