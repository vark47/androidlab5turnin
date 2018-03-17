using Android.App;
using Android.OS;

namespace Pig
{
    [Activity(Label = "New Game", MainLauncher = true, Icon = "@drawable/icon", LaunchMode = Android.Content.PM.LaunchMode.SingleInstance)]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Menu);
        }
    }
}

