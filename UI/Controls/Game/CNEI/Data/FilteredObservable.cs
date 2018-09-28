namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class FilteredObservable<T> : INotifyPropertyChanged
    {
        private protected ObservableCollection<T> baseCollection;

        private protected ObservableCollection<T> items = new ObservableCollection<T>();

        private protected List<Predicate<T>> Filters = new List<Predicate<T>>();

        private int entityCount = 0;

        public ObservableCollection<T> BaseCollection
        {
            get => this.baseCollection;
            set
            {
                // emulate VB's "with events" properties.
                if (null != this.baseCollection)
                    this.baseCollection.CollectionChanged -= this.BaseCollectionOnChange;

                this.baseCollection = value;
                this.baseCollection.CollectionChanged += this.BaseCollectionOnChange;

                this.NotifyThisPropertyChanged();
                this.NotifyPropertyChange("Items");
            }
        }

        public virtual ObservableCollection<T> Items
        {
            get
            {
                this.items.Clear();
                this.items = new ObservableCollection<T>(this.baseCollection.Where(x => this.Filters.All(f => f(x))));
                this.EntityCount = this.items.Count;
                return this.items;
            }
        }

        public int EntityCount
        {
            get => this.entityCount;
            private protected set
            {
                if (value == this.entityCount)
                {
                    return;
                }
                this.entityCount = value;
                this.NotifyThisPropertyChanged();
            }
        }

        public bool AddFilter(Predicate<T> filter)
        {
            if (!this.Filters.Contains(filter))
            {
                this.Filters.Add(filter);
                Refresh();
                return true;
            }
            return false;
        }

        public bool RemoveFilter(Predicate<T> filter)
        {
            bool removed = this.Filters.Remove(filter);
            if (removed)
            {
                Refresh();
            }
            return removed;
        }

        public virtual void Refresh()
        {
            NotifyPropertyChange("Items");
        }

        private void BaseCollectionOnChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChange("Items");
        }

        public FilteredObservable(ObservableCollection<T> collection)
        {
            this.BaseCollection = collection;
            this.EntityCount = collection.Count;
        }

        public FilteredObservable(IEnumerable<T> collection)
        {
            this.BaseCollection = new ObservableCollection<T>(collection);
            this.EntityCount = this.BaseCollection.Count;
        }

        public FilteredObservable()
        {
            this.BaseCollection = new ObservableCollection<T>();
        }

        protected void NotifyPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void NotifyThisPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}