
using System;
using System.Linq;
using TakeMyAdvice.Models;
using Xamarin.Forms;

namespace TakeMyAdvice.Pages
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
            SignUpButton.Clicked += async (s, o) => { await SignUpButton_ClickedAsync(s, o); };
            SignInButton.Clicked += SignInButton_Clicked;
        }

        private void SignInButton_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new LoginPage();
        }

        private async System.Threading.Tasks.Task SignUpButton_ClickedAsync(object sender, EventArgs e)
        {
            // check for user name
            if (string.IsNullOrEmpty(UserNameEntry.Text))
            {
                await App.Current.MainPage.DisplayAlert("Notice", "Please add a User Name", "Ok");
                return;
            }

            // check for Email
            if (string.IsNullOrEmpty(EmailEntry.Text))
            {
                await App.Current.MainPage.DisplayAlert("Notice", "Please add your email", "Ok");
                return;
            }

            // check for Password
            if (string.IsNullOrEmpty(PasswordEntry.Text))
            {
                await App.Current.MainPage.DisplayAlert("Notice", "Please add your password", "Ok");
                return;
            }

            // check for Password Confirm
            if (string.IsNullOrEmpty(ConfirmPasswordEntry.Text))
            {
                await App.Current.MainPage.DisplayAlert("Notice", "Please confirm your password", "Ok");
                return;
            }

            // Check for names
            if (string.IsNullOrEmpty(FirstNameEntry.Text) || string.IsNullOrEmpty(LastNameEntry.Text))
            {
                await App.Current.MainPage.DisplayAlert("Notice", "First and Last name are required", "Ok");
                return;
            }

            // Make sure password and password confirm match
            if (PasswordEntry.Text == ConfirmPasswordEntry.Text == false)
            {
                await App.Current.MainPage.DisplayAlert("Notice", "Password and Confirm Password must match", "Ok");
                PasswordEntry.Text =
                    ConfirmPasswordEntry.Text =
                    string.Empty;
                return;
            }

            // validate the email address
            if (EmailEntry.Text.Contains("@") == false)
            {
                await App.Current.MainPage.DisplayAlert("Notice", "Please enter a valid email", "Ok");
                return;
            }

            // check for existing user
            var realm = Realms.Realm.GetInstance();
            if (realm.All<UserModel>().Any(x => x.Email == EmailEntry.Text))
            {
                await App.Current.MainPage.DisplayAlert("Notice", "A user exists with this email address", "Ok");
                return;
            }

            // check for unigue username
            if (realm.All<UserModel>().Any(x => x.UserName == UserNameEntry.Text))
            {
                await App.Current.MainPage.DisplayAlert("Notice", "This username is taken. Please choose another one", "Ok");
                return;
            }

            // create user and store it
            realm.Write(() =>
            {
                var user = new UserModel(FirstNameEntry.Text, LastNameEntry.Text, EmailEntry.Text, UserNameEntry.Text, PasswordEntry.Text)
                {
                    // set as active user
                    IsActive = true
                };
                realm.Add(user);
            });

            // done, navigate to main page
            await App.Current.MainPage.DisplayAlert("Complete", "Thank you for signing up", "Ok");
            App.Current.MainPage = new MainPage();
        }
    }
}
