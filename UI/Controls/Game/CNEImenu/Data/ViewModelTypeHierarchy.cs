namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class ViewModelTypeHierarchy : BaseViewModel
    {
        private static readonly Type MainNode = typeof(object);

        private ViewModelTypeHierarchy Parent = null;

        public bool IsChild { get; private set; }

        public Type MyType = MainNode;

        public string Name { get; }

        public ProtoEntityViewModel EntityVM { get; private set; }

        public ObservableCollection<ViewModelTypeHierarchy> Derivatives { get; private set; }

        public List<ProtoEntityViewModel> EntityVMList => this.Derivatives.Select(d => d.EntityVM).ToList();

        public bool EndNode => this.Derivatives.All(n => n.IsChild);

        public ViewModelTypeHierarchy()
        {
            this.Derivatives = new ObservableCollection<ViewModelTypeHierarchy>();
            this.Name = GetTypeNameWithoutGenericArity(this.MyType);
            this.IsChild = true;
        }

        public ViewModelTypeHierarchy(Type type)
        {
            this.Derivatives = new ObservableCollection<ViewModelTypeHierarchy>();
            this.MyType = type;
            this.Name = GetTypeNameWithoutGenericArity(type);
            this.IsChild = true;
        }

        public void Add(Type type, ProtoEntityViewModel entityVM)
        {
            if (GetTypeNameWithoutGenericArity(type) == this.Name)
            {
                return;
            }
            var localNode = this;
            var tempType = type.BaseType;
            while (GetTypeNameWithoutGenericArity(type.BaseType) != localNode.Name)
            {                
                while(GetTypeNameWithoutGenericArity(tempType.BaseType) != localNode.Name)
                {
                    tempType = tempType.BaseType;
                }
                var tempNode = localNode.Derivatives
                    .Where(n => n.Name == GetTypeNameWithoutGenericArity(tempType))
                    .FirstOrDefault();
                if (tempNode == null)
                {
                    tempNode = new ViewModelTypeHierarchy(tempType) { Parent = localNode };
                    localNode.Derivatives.Add(tempNode);
                    localNode.IsChild = false;
                }
                localNode = tempNode;
                tempType = type.BaseType;
            }
            var newNode = new ViewModelTypeHierarchy(type) { Parent = localNode, EntityVM = entityVM };
            localNode.Derivatives.Add(newNode);
            localNode.IsChild = false;
        }

        protected override void DisposeViewModel()
        {
            base.DisposeViewModel();
            foreach (var viewModelTypeHierarchy in this.Derivatives)
            {
                viewModelTypeHierarchy.Dispose();
            }
        }

        private string GetTypeNameWithoutGenericArity(Type t)
        {
            int index = t.ToString().IndexOf('`');
            return index == -1 ? t.ToString() : t.ToString().Substring(0, index);
        }
    }
}