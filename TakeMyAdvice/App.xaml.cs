
using TakeMyAdvice.Pages;
using Xamarin.Forms;

namespace TakeMyAdvice
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new SignUpPage();

            //MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
