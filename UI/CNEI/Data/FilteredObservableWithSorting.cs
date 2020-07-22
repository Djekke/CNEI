namespace CryoFall.CNEI.UI.Data
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using CryoFall.CNEI.Scripts;

    public class FilteredObservableWithSorting<T> : FilteredObservable<T>
    {
        private protected string sortPropertyName = "";

        private protected bool sortDirection;

        public string SortPropertyName
        {
            get => sortPropertyName;
            set
            {
                if (sortPropertyName == value)
                {
                    return;
                }

                sortPropertyName = value;
                Refresh();
            }
        }

        public bool SortDirection
        {
            get => sortDirection;
            set
            {
                if (sortDirection == value)
                {
                    return;
                }

                sortDirection = value;
                Refresh();
            }
        }

        /// <summary>
        /// Filtered item collection.
        /// </summary>
        public override ObservableCollection<T> Items
        {
            get
            {
                items.Clear();
                if (sortPropertyName != "" && Utils.IsPropertyExist(typeof(T), sortPropertyName))
                {
                    if (sortDirection)
                    {
                        items = new ObservableCollection<T>(baseCollection
                          .Where(x => filters.All(f => f(x)))
                          .OrderBy(x => Utils.GetPropertyValueFromPath(x, sortPropertyName)));
                    }
                    else
                    {
                        items = new ObservableCollection<T>(baseCollection
                          .Where(x => filters.All(f => f(x)))
                          .OrderByDescending(x => Utils.GetPropertyValueFromPath(x, sortPropertyName)));
                    }
                }
                else
                {
                    items = new ObservableCollection<T>(baseCollection
                        .Where(x => filters.All(f => f(x))));
                }
                EntityCount = items.Count;
                return items;
            }
        }

        public FilteredObservableWithSorting(ObservableCollection<T> collection) : base(collection) { }

        public FilteredObservableWithSorting(IEnumerable<T> collection) : base(collection) { }

        public FilteredObservableWithSorting() : base() { }
    }
}