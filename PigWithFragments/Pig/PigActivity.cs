using Android.App;
using Android.OS;
using Android.Content;

namespace Pig
{
    [Activity(Label = "Pig", LaunchMode = Android.Content.PM.LaunchMode.SingleInstance)]
    public class PigActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Game);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            Intent = intent;

            var gameFrag = FragmentManager.FindFragmentById<GameFragment>(Resource.Id.gameFragment);
            gameFrag.NewGame();
        }
    }
}