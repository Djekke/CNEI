namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class TypeHierarchy
    {
        private static readonly Type MainNode = typeof(object);

        private TypeHierarchy Parent = null;

        public bool IsChild { get; private set; }

        public Type MyType = MainNode;

        public string Name { get; }

        public ProtoEntityViewModel EntityViewModel { get; private set; }

        public ObservableCollection<TypeHierarchy> Derivatives { get; private set; }

        public List<ProtoEntityViewModel> EntityViewModelsList => Derivatives.Select(d => d.EntityViewModel).ToList();

        public bool EndNode => Derivatives.All(n => n.IsChild);

        public TypeHierarchy()
        {
            Derivatives = new ObservableCollection<TypeHierarchy>();
            Name = GetTypeNameWithoutGenericArity(MyType);
            IsChild = true;
        }

        public TypeHierarchy(Type type)
        {
            Derivatives = new ObservableCollection<TypeHierarchy>();
            MyType = type;
            Name = GetTypeNameWithoutGenericArity(type);
            IsChild = true;
        }

        public void Add(Type type, ProtoEntityViewModel entityViewModel)
        {
            if (GetTypeNameWithoutGenericArity(type) == Name)
            {
                return;
            }
            var localNode = this;
            var tempType = type.BaseType;
            while (GetTypeNameWithoutGenericArity(type.BaseType) != localNode.Name)
            {
                while(GetTypeNameWithoutGenericArity(tempType?.BaseType) != localNode.Name)
                {
                    tempType = tempType?.BaseType;
                }
                var tempNode = localNode.Derivatives
                    .FirstOrDefault(n => n.Name == GetTypeNameWithoutGenericArity(tempType));
                if (tempNode == null)
                {
                    tempNode = new TypeHierarchy(tempType) { Parent = localNode };
                    localNode.Derivatives.Add(tempNode);
                    localNode.IsChild = false;
                }
                localNode = tempNode;
                tempType = type.BaseType;
            }
            var newNode = new TypeHierarchy(type) { Parent = localNode, EntityViewModel = entityViewModel };
            localNode.Derivatives.Add(newNode);
            localNode.IsChild = false;
        }

        private static string GetTypeNameWithoutGenericArity(Type t)
        {
            int index = t.ToString().IndexOf('`');
            return index == -1 ? t.ToString() : t.ToString().Substring(0, index);
        }
    }
}