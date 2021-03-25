using System;
using System.Collections.Generic;
using System.Linq;
using TakeMyAdvice.Models;
using Xamarin.Forms;

namespace TakeMyAdvice.Pages
{
    public partial class AccountPage : ContentPage
    {
        public AccountPage()
        {
            InitializeComponent();

            SignOutButton.Clicked += SignOutButton_Clicked;


        }

        private void SignOutButton_Clicked(object sender, EventArgs e)
        {
            var realm = Realms.Realm.GetInstance();
            var user = realm.All<UserModel>().FirstOrDefault(x => x.IsActive == true);

            realm.Write(() =>
            {
                user.IsActive = false;
            });

            App.Current.MainPage = new LoginPage();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var realm = Realms.Realm.GetInstance();
            var user = realm.All<UserModel>().FirstOrDefault(x => x.IsActive == true);

            UsernameLabel.Text = user.UserName;
            NameLabel.Text = $"{ user.FirstName} {user.LastName}";
            EmailLabel.Text = user.Email;
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}
