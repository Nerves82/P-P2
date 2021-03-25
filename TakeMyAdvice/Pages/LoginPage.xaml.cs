
using System;
using System.Linq;
using TakeMyAdvice.Models;
using Xamarin.Forms;

namespace TakeMyAdvice.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            CreateAccountButton.Clicked += CreateAccountButton_Clicked;
            SignInButton.Clicked += async (o, s) => { await SignInButton_ClickedAsync(o, s); };
            ForgotPasswordButton.Clicked += async (o, s) => { await ForgotPasswordButton_ClickedAsync(o, s); };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var realm = Realms.Realm.GetInstance();
            var users = realm.All<UserModel>().Where(x => x.IsActive == true);

            if (users != null && users.Count() > 0)
            {
                var user = users.FirstOrDefault(x => x.IsActive == true);

                if (user != null)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await App.Current.MainPage.DisplayActionSheet($"{user.UserName} is already signed in. Sign in with this user?", "Yes", "No");

                        if (result == "Yes")
                        {
                            App.Current.MainPage = new MainPage();
                        }
                    });
                }
            }
        }

        private async System.Threading.Tasks.Task ForgotPasswordButton_ClickedAsync(object sender, EventArgs e)
        {
            // get the username
            var result = await App.Current.MainPage.DisplayPromptAsync("Forgot Password", "Enter your user name.");

            // null check
            if (string.IsNullOrEmpty(result))
            {
                return;
            }

            // try get the user
            var realm = Realms.Realm.GetInstance();
            var user = realm.All<UserModel>().FirstOrDefault(x => x.UserName == result);

            // display results
            if (user == null)
            {
                await App.Current.MainPage.DisplayAlert("Notice", "No user found with that username", "Ok");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Notice", $"Hint: {user.Password}", "Ok");
            }

        }

        private async System.Threading.Tasks.Task SignInButton_ClickedAsync(object sender, EventArgs e)
        {
            // null checks
            if (string.IsNullOrEmpty(UsernameEntry.Text))
            {
                await App.Current.MainPage.DisplayAlert("Notice", "Please add your email", "Ok");
                return;
            }

            if (string.IsNullOrEmpty(PasswordEntry.Text))
            {
                await App.Current.MainPage.DisplayAlert("Notice", "Please add your password", "Ok");
                return;
            }

            // check if user exists
            bool hasPassword = false;
            bool hasUsername = false;
            var realm = Realms.Realm.GetInstance();
            if (realm.All<UserModel>().Any(x => x.UserName == UsernameEntry.Text))
            {
                hasUsername = true;
            }

            if (realm.All<UserModel>().Any(x => x.Password == PasswordEntry.Text))
            {
                hasPassword = true;
            }

            // no user, ask to sign up
            if (hasUsername == false)
            {
                var result = await App.Current.MainPage.DisplayActionSheet("The user does not exist. Would you like to sign up?", "Yes", "No");
                if (result == "Yes")
                {
                    App.Current.MainPage = new SignUpPage();
                }

                return;
            }

            // incorrect password
            if (hasUsername == true && hasPassword == false)
            {
                await App.Current.MainPage.DisplayAlert("Notice", "Username/password combination is not correct,", "Ok");
                return;
            }

            // good info, log in
            if (hasUsername && hasPassword)
            {
                var all = realm.All<UserModel>();
                foreach (var u in all)
                {
                    realm.Write(() =>
                    {
                        // set this user as active
                        u.IsActive = u.UserName == UsernameEntry.Text;
                    });
                }
            }

            App.Current.MainPage = new MainPage();
        }

        private void CreateAccountButton_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new SignUpPage();
        }
    }
}
