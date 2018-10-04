namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.GameApi.Scripting;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class FilteredObservableWithPaging<T> : FilteredObservable<T>
    {
        private int currentPage = 0;

        private int pageCount = 0;

        private int pageCapacity = 1;

        public int CurrentPage
        {
            get => currentPage;
            private set
            {
                if (value == currentPage)
                {
                    return;
                }
                currentPage = value;
                NotifyThisPropertyChanged();
                Refresh();
            }
        }

        public int PageCount
        {
            get => pageCount;
            private set
            {
                if (value == pageCount)
                {
                    return;
                }
                pageCount = value;
                if (CurrentPage > pageCount)
                {
                    CurrentPage = pageCount;
                }
                NotifyThisPropertyChanged();
                Refresh();
            }
        }

        public bool IsFirstPage => currentPage <= 1;

        public bool IsLastPage => currentPage == pageCount;

        public bool IsNotFirstPage => !IsFirstPage;

        public bool IsNotLastPage => !IsLastPage;

        public override ObservableCollection<T> Items
        {
            get
            {
                items.Clear();
                // Apply filters.
                var tempCollection = BaseCollection.Where(x => filters.All(f => f(x))).ToList();
                EntityCount = tempCollection.Count;
                RefreshPageCount();
                // Apply pagination.
                if (EntityCount > 0)
                {
                    int offset = pageCapacity * (CurrentPage - 1);
                    items = new ObservableCollection<T>(tempCollection
                            .GetRange(0 + offset, Math.Min(pageCapacity, EntityCount - offset)));
                    return items;
                }
                return items;
            }
        }

        public override void Refresh()
        {
            NotifyPropertyChange("Items");
        }

        private void RefreshPageCount()
        {
            var newCount = EntityCount / pageCapacity +
                (EntityCount % pageCapacity == 0 ? 0 : 1);
            if (PageCount != newCount)
            {
                pageCount = newCount;
                NotifyPropertyChange("PageCount");
                if (CurrentPage > pageCount)
                {
                    currentPage = pageCount;
                    NotifyPropertyChange("CurrentPage");
                }
                if (CurrentPage == 0 && pageCount > 0 )
                {
                    currentPage = 1;
                    NotifyPropertyChange("CurrentPage");
                }
            }
            NotifyPropertyChange("IsFirstPage");
            NotifyPropertyChange("IsLastPage");
            NotifyPropertyChange("IsNotFirstPage");
            NotifyPropertyChange("IsNotLastPage");
        }

        public void SetPageCapacity(int capacity)
        {
            if (capacity == 0)
            {
                Api.Logger.Error("What idiot set page capacity for " + this + " to zero?");
                return;
            }
            if (pageCapacity != capacity)
            {
                pageCapacity = capacity;
                Refresh();
            }
        }

        public void NextPage()
        {
            if (currentPage < PageCount)
            {
                CurrentPage += 1;
            }
        }

        public void PrevPage()
        {
            if (currentPage > 0)
            {
                CurrentPage -= 1;
            }
        }

        public FilteredObservableWithPaging(IEnumerable<T> collection) : base(collection) { }

        public FilteredObservableWithPaging(ObservableCollection<T> collection) : base(collection) { }

        public FilteredObservableWithPaging() : base() { }
    }
}