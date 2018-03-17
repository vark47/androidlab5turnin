using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Pig
{
    public class MenuFragment : Fragment
    {
        private bool isDualPane;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override void OnActivityCreated(Bundle bundle)
        {
            base.OnActivityCreated(bundle);

            var startBtn = Activity.FindViewById<Button>(Resource.Id.startGameBtn);
            var playerOneNameBox = Activity.FindViewById<EditText>(Resource.Id.pOneNameTxtBox);
            var playerTwoNameBox = Activity.FindViewById<EditText>(Resource.Id.pTwoNameTxtBox);

            if (bundle != null)
            {
                playerOneNameBox.Text = bundle.GetString("Player1");
                playerTwoNameBox.Text = bundle.GetString("Player2");
                startBtn.Enabled = bundle.GetBoolean("enabled");
            }

            var rollBtn = Activity.FindViewById<Button>(Resource.Id.rollBtn);
            if (rollBtn != null)
            {
                isDualPane = true;
                var endBtn = Activity.FindViewById<Button>(Resource.Id.endBtn);
                var newGameBtn = Activity.FindViewById<Button>(Resource.Id.newGameBtn);

                rollBtn.Enabled = !startBtn.Enabled;
                endBtn.Enabled = !startBtn.Enabled;
                newGameBtn.Enabled = !startBtn.Enabled;
            }

            startBtn.Click += StartButton_Click;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {

            if (isDualPane)
            {
                ((Button)sender).Enabled = false;
                var gameFrag = FragmentManager.FindFragmentById<GameFragment>(Resource.Id.gameFragment);
                gameFrag.NewGame();
            }
            else
            {
                var playerOneNameBox = Activity.FindViewById<EditText>(Resource.Id.pOneNameTxtBox);
                var playerTwoNameBox = Activity.FindViewById<EditText>(Resource.Id.pTwoNameTxtBox);
                var game = new Intent(Activity, typeof(PigActivity));

                game.PutExtra("Player1", playerOneNameBox.Text);
                game.PutExtra("Player2", playerTwoNameBox.Text);

                StartActivity(game);
            }
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            var playerOneNameBox = Activity.FindViewById<EditText>(Resource.Id.pOneNameTxtBox);
            var playerTwoNameBox = Activity.FindViewById<EditText>(Resource.Id.pTwoNameTxtBox);
            var startBtn = Activity.FindViewById<Button>(Resource.Id.startGameBtn);

            outState.PutString("Player1", playerOneNameBox.Text);
            outState.PutString("Player2", playerTwoNameBox.Text);
            outState.PutBoolean("enabled", startBtn.Enabled);
            base.OnSaveInstanceState(outState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.MenuFrag, container, false);
        }
    }
}