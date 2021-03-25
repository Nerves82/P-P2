
using System.Collections.ObjectModel;
using System.Linq;
using TakeMyAdvice.Models;
using Xamarin.Forms;

namespace TakeMyAdvice.Pages
{
    public partial class BadAdvicePage : ContentPage
    {
        private ObservableCollection<AdviceListViewModel> _adviceCollection = new ObservableCollection<AdviceListViewModel>();

        public BadAdvicePage()
        {
            InitializeComponent();

            _adviceCollection = new ObservableCollection<AdviceListViewModel>();
            BadAdviceList.ItemsSource = _adviceCollection;
        }

        void MenuItem_Clicked(System.Object sender, System.EventArgs e)
        {
            var mi = (MenuItem)sender;
            var realm = Realms.Realm.GetInstance();

            var advice = realm.All<AdviceModel>();
            var thisAdvice = advice.FirstOrDefault(x => x.AdviceNumber == ((AdviceListViewModel)mi.CommandParameter).AdviceNumber);

            if (thisAdvice != null)
            {
                realm.Write(() =>
                {
                    realm.Remove(thisAdvice);
                });
            }

            UpdateFromCache();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdateFromCache();
        }

        private void UpdateFromCache()
        {
            var realm = Realms.Realm.GetInstance();

            var advice = realm.All<AdviceModel>().Where(x => x.AdviceType == "bad");

            _adviceCollection.Clear();
            foreach (var item in advice)
            {
                _adviceCollection.Add(new AdviceListViewModel { AdviceNumber = item.AdviceNumber, AdviceMessage = item.AdviceMessage });
            }
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}
