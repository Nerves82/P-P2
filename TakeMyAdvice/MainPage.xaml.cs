using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using TakeMyAdvice.Models;
using TakeMyAdvice.Pages;
using Xamarin.Forms;
using static TakeMyAdvice.DataAccess;

namespace TakeMyAdvice
{
    public partial class MainPage : ContentPage
    {
        private DataAccess.Response _currentAdvice;

        public MainPage()
        {
            InitializeComponent();

            GoodSwiper.Invoked += async (s, a) =>
            {
                await GoodSwipeInvokedAsync(s, a);
            };

            BadSwipper.Invoked += async (s, a) =>
            {
                await BadSwipeInvokedAsync(s, a);
            };

            BadMark.IsVisible = false;
            GoodMark.IsVisible = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Task.Run(async () =>
            {
                _currentAdvice = await Instance.GetInfoAsync();
                Update(_currentAdvice);
            });
        }

        private void Update(Response advice)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                AdviceNumberLabel.Text = "# " + advice.Slip.Id.ToString();
                AdviceLabel.Text = advice.Slip.Advice.ToLower();

                //var realm = Realms.Realm.GetInstance();

                //var savedAdvice = realm.All<AdviceModel>().FirstOrDefault(x => 14 == x.AdviceNumber);
                //var thisSavedAdvice = savedAdvice;

                //var savedAdvice = realm.All<AdviceModel>();
                //var thisAdvice = savedAdvice.FirstOrDefault(x => x.AdviceNumber == id);


                //if (thisAdvice != null)
                //{
                //    if (thisAdvice.AdviceType == "bad")
                //    {
                //        BadMark.IsVisible = true;
                //        GoodMark.IsVisible = false;
                //    }

                //    if (thisAdvice.AdviceType == "good")
                //    {
                //        BadMark.IsVisible = false;
                //        GoodMark.IsVisible = true;
                //    }
                //}
            });
        }

        void BadAdviceButtonTapped(System.Object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new BadAdvicePage());
        }

        void GoodAdviceButtonTapped(System.Object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new GoodAdviceListPage());
        }

        void AccountButtonTapped(System.Object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new AccountPage());
        }

        async Task BadSwipeInvokedAsync(System.Object sender, System.EventArgs e)
        {
            Console.WriteLine("Bad Swipe");
            var config = new ToastConfig($"advice #{_currentAdvice.Slip.Id} added to bad list") { Position = ToastPosition.Top };
            UserDialogs.Instance.Toast(config);

            await Instance.AddToBadAdviceAsync(_currentAdvice);

            _currentAdvice = await Instance.GetInfoAsync();
            Update(_currentAdvice);
        }

        async Task GoodSwipeInvokedAsync(System.Object sender, System.EventArgs e)
        {
            Console.WriteLine("Good Swipe");
            var config = new ToastConfig($"advice #{_currentAdvice.Slip.Id} added to good list") { Position = ToastPosition.Top };
            UserDialogs.Instance.Toast(config);

            await Instance.AddToGoodAdviceAsync(_currentAdvice);

            _currentAdvice = await Instance.GetInfoAsync();
            Update(_currentAdvice);
        }
    }
}
