namespace CryoFall.CNEI.UI.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class TypeHierarchy : INotifyPropertyChanged
    {
        private const int IndentSize = 20;  // For listbox.
        //private const int IndentSize = 22;  // For treeview.

        private const int WindowWidth = 465; // 500 - 35

        private static readonly Type MainNode = typeof(object);

        private bool? isChecked = false;

        public TypeHierarchy Parent { get; } = null;

        public bool IsChild { get; private set; } = true;

        public Type MyType { get; }

        public string FullName { get; }

        public string ShortName { get; }

        // First node is 'object' type and never shown.
        public int Indent { get; } = -IndentSize;  // For listbox.
        //public int Indent { get; } = 0;  // For treeview.

        public int ListMaxWidth => WindowWidth - Indent;

        public bool? IsCheckedSavedState { get; set; } = false;

        public ProtoEntityViewModel EntityViewModel { get; }

        public List<TypeHierarchy> Derivatives { get; } =
            new List<TypeHierarchy>();

        /// <summary>
        /// List of view models from derivatives 1 level down.
        /// </summary>
        public List<ProtoEntityViewModel> EntityViewModelsList { get; } =
            new List<ProtoEntityViewModel>();

        /// <summary>
        /// List of all view models from all derivatives all the way down.
        /// </summary>
        public List<ProtoEntityViewModel> EntityViewModelsFullList { get; } =
            new List<ProtoEntityViewModel>();

        public bool EndNode => Derivatives.All(d => d.IsChild);

        public TypeHierarchy(Type type)
        {
            MyType = type;
            FullName = GetNameWithoutGenericArity(MyType.ToString());
            ShortName = GetNameWithoutGenericArity(MyType.Name);
        }

        public TypeHierarchy() : this(MainNode)
        {
        }

        public TypeHierarchy(Type type, TypeHierarchy parent) : this(type)
        {
            Parent = parent;
            Indent = parent.Indent + IndentSize;
        }

        public TypeHierarchy(Type type, TypeHierarchy parent, ProtoEntityViewModel entityViewModel)
            : this(type, parent)
        {
            EntityViewModel = entityViewModel;
        }

        public void Add(Type type, ProtoEntityViewModel entityViewModel)
        {
            if (GetNameWithoutGenericArity(type.ToString()) == FullName)
            {
                return;
            }

            var localNode = this;
            var tempType = type.BaseType;
            while (GetNameWithoutGenericArity(type.BaseType?.ToString()) != localNode.FullName)
            {
                while (GetNameWithoutGenericArity(tempType?.BaseType?.ToString()) != localNode.FullName)
                {
                    tempType = tempType?.BaseType;
                }

                var tempNode = localNode.Derivatives
                    .FirstOrDefault(n => n.FullName == GetNameWithoutGenericArity(tempType?.ToString()));
                if (tempNode == null)
                {
                    tempNode = new TypeHierarchy(tempType, localNode);
                    localNode.Derivatives.Add(tempNode);
                    localNode.IsChild = false;
                }

                localNode = tempNode;
                tempType = type.BaseType;
            }

            var newNode = new TypeHierarchy(type, localNode, entityViewModel);
            localNode.Derivatives.Add(newNode);
            localNode.IsChild = false;
            localNode.EntityViewModelsList.Add(entityViewModel);
            do
            {
                localNode.EntityViewModelsFullList.Add(entityViewModel);
                localNode = localNode.Parent;
            } while (localNode != null);
        }

        public bool? IsChecked
        {
            get => isChecked;
            set
            {
                if (IsChecked == value)
                {
                    return;
                }

                // Do not care about value, we have our own logic here.
                if (isChecked == true)
                {
                    isChecked = false;
                    // Set All children false.
                    foreach (TypeHierarchy derivative in Derivatives)
                    {
                        derivative.SetAllDerivativesIsChecked(false);
                    }

                    Parent?.RecalculateParentIsChecked();
                }
                else
                {
                    isChecked = true;
                    // Set all children true.
                    foreach (TypeHierarchy derivative in Derivatives)
                    {
                        derivative.SetAllDerivativesIsChecked(true);
                    }

                    Parent?.RecalculateParentIsChecked();
                }
                OnPropertyChanged();
            }
        }

        public void SetAllDerivativesIsChecked(bool? value)
        {
            isChecked = value;
            OnPropertyChanged(nameof(IsChecked));
            foreach (TypeHierarchy derivative in Derivatives)
            {
                derivative.SetAllDerivativesIsChecked(value);
            }
        }

        public void RecalculateParentIsChecked()
        {
            var allTrue = true;
            isChecked = false;
            foreach (TypeHierarchy derivative in Derivatives)
            {
                if (derivative.IsChecked == true)
                {
                    isChecked = null;
                }
                else if (derivative.IsChecked == false)
                {
                    allTrue = false;
                }
                else if (derivative.IsChecked == null)
                {
                    allTrue = false;
                    isChecked = null;
                    break;
                }
            }

            if (allTrue)
            {
                isChecked = true;
            }
            OnPropertyChanged(nameof(IsChecked));

            Parent?.RecalculateParentIsChecked();
        }

        /// <summary>
        /// Call this on main node to reset whole tree to previus saved state.
        /// </summary>
        public void ResetState()
        {
            isChecked = IsCheckedSavedState;
            OnPropertyChanged(nameof(IsChecked));
            foreach (TypeHierarchy derivative in Derivatives)
            {
                derivative.ResetState();
            }
        }

        private static string GetNameWithoutGenericArity(string s)
        {
            int index = s.IndexOf('`');
            return index == -1 ? s : s.Substring(0, index);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}