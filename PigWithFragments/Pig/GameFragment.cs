using System;
using System.Xml.Serialization;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Pig
{
    public class GameFragment : Fragment
    {
        private PigLogic pig;
        private bool isDualPane;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override void OnActivityCreated(Bundle bundle)
        {
            base.OnActivityCreated(bundle);

            if (bundle == null)
            {
                pig = new PigLogic();
                var image = Activity.FindViewById<ImageView>(Resource.Id.dieImageView);
                image.SetImageResource(Resource.Drawable.Die6);
                NewGame();
            }
            else
            {
                string savedPig = bundle.GetString("Pig");
                var pigSerializer = new XmlSerializer(typeof(PigLogic));
                pig = (PigLogic)pigSerializer.Deserialize(new StringReader(savedPig));
                ResetViews();
            }

            var startBtn = Activity.FindViewById<Button>(Resource.Id.startGameBtn);
            if (startBtn != null)
                isDualPane = true;

            var rollBtn = Activity.FindViewById<Button>(Resource.Id.rollBtn);
            var endBtn = Activity.FindViewById<Button>(Resource.Id.endBtn);
            var newGameBtn = Activity.FindViewById<Button>(Resource.Id.newGameBtn);

            rollBtn.Click += RollButtonClick;
            endBtn.Click += EndTurnButtonClick;
            newGameBtn.Click += NewGameButton_Click;
        }

        private void RollButtonClick(object sender, EventArgs e)
        {
            pig.Roll();

            SetDieImage();

            if (pig.RollScore == 0)
            {
                ((Button)sender).Enabled = false;
                pig.CanContinue = false;
            }

            var scoreView = Activity.FindViewById<TextView>(Resource.Id.currentScore);
            scoreView.Text = pig.RollScore.ToString();
        }

        private void EndTurnButtonClick(object sender, EventArgs e)
        {
            if (pig.PlayerOneTurn)
            {
                pig.PlayerOneScore += pig.RollScore;
                var pOneScore = Activity.FindViewById<TextView>(Resource.Id.playerOneScore);
                pOneScore.Text = pig.PlayerOneScore.ToString();
            }
            else
            {
                pig.PlayerTwoScore += pig.RollScore;
                var pTwoScore = Activity.FindViewById<TextView>(Resource.Id.playerTwoScore);
                pTwoScore.Text = pig.PlayerTwoScore.ToString();
            }

            if (pig.IsWinner())
            {
                var turnView = Activity.FindViewById<TextView>(Resource.Id.turnText);
                turnView.Text = pig.Winner + " Wins!";
                DisableButtons();
            }
            else
            {
                pig.NextPlayer();

                var turnView = Activity.FindViewById<TextView>(Resource.Id.turnText);
                if (pig.PlayerOneTurn)
                    turnView.Text = string.Format("{0}'s turn", pig.PlayerOneName);
                else
                    turnView.Text = string.Format("{0}'s turn", pig.PlayerTwoName);

                var scoreView = Activity.FindViewById<TextView>(Resource.Id.currentScore);
                scoreView.Text = "0";

                EnableButtons();
            }
        }

        private void DisableButtons()
        {
            var rollBtn = Activity.FindViewById<Button>(Resource.Id.rollBtn);
            var endBtn = Activity.FindViewById<Button>(Resource.Id.endBtn);

            rollBtn.Enabled = false;
            endBtn.Enabled = false;
        }

        private void EnableButtons()
        {
            var rollBtn = Activity.FindViewById<Button>(Resource.Id.rollBtn);
            var endBtn = Activity.FindViewById<Button>(Resource.Id.endBtn);
            var newGameBtn = Activity.FindViewById<Button>(Resource.Id.newGameBtn);

            rollBtn.Enabled = true;
            endBtn.Enabled = true;
            newGameBtn.Enabled = true;
        }

        private void ResetViews()
        {
            var turnView = Activity.FindViewById<TextView>(Resource.Id.turnText);
            var scoreView = Activity.FindViewById<TextView>(Resource.Id.currentScore);
            var pOneScore = Activity.FindViewById<TextView>(Resource.Id.playerOneScore);
            var pTwoScore = Activity.FindViewById<TextView>(Resource.Id.playerTwoScore);
            var playerOneNameView = Activity.FindViewById<TextView>(Resource.Id.playerOneTxtView);
            var playerTwoNameView = Activity.FindViewById<TextView>(Resource.Id.playerTwoTxtView);
            var rollBtn = Activity.FindViewById<Button>(Resource.Id.rollBtn);

            scoreView.Text = pig.RollScore.ToString();
            pOneScore.Text = pig.PlayerOneScore.ToString();
            pTwoScore.Text = pig.PlayerTwoScore.ToString();
            playerOneNameView.Text = pig.PlayerOneName;
            playerTwoNameView.Text = pig.PlayerTwoName;

            if (pig.PlayerOneTurn)
                turnView.Text = string.Format("{0}'s turn", pig.PlayerOneName);
            else
                turnView.Text = string.Format("{0}'s turn", pig.PlayerTwoName);

            if (!pig.CanContinue)
                rollBtn.Enabled = false;

            SetDieImage();
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            pig.Reset();

            if (isDualPane)
            {
                var startBtn = Activity.FindViewById<Button>(Resource.Id.startGameBtn);
                var rollBtn = Activity.FindViewById<Button>(Resource.Id.rollBtn);
                var endBtn = Activity.FindViewById<Button>(Resource.Id.endBtn);
                var newGameBtn = Activity.FindViewById<Button>(Resource.Id.newGameBtn);

                startBtn.Enabled = true;
                rollBtn.Enabled = false;
                endBtn.Enabled = false;
                newGameBtn.Enabled = false;
            }
            else
            {
                var menu = new Intent(Activity, typeof(MainActivity));
                StartActivity(menu);
            }
        }

        public void NewGame()
        {
            if (!isDualPane)
            {
                pig.PlayerOneName = Activity.Intent.GetStringExtra("Player1") ?? "Player 1";
                pig.PlayerTwoName = Activity.Intent.GetStringExtra("Player2") ?? "Player 2";
            }
            else
            {
                var playerOneNameBox = Activity.FindViewById<EditText>(Resource.Id.pOneNameTxtBox);
                var playerTwoNameBox = Activity.FindViewById<EditText>(Resource.Id.pTwoNameTxtBox);

                pig.PlayerOneName = playerOneNameBox.Text;
                pig.PlayerTwoName = playerTwoNameBox.Text;
                EnableButtons();
            }

            ResetViews();
        }

        private void SetDieImage()
        {
            var image = Activity.FindViewById<ImageView>(Resource.Id.dieImageView);

            switch (pig.CurrentRoll)
            {
                case 1:
                    image.SetImageResource(Resource.Drawable.Die1);
                    break;
                case 2:
                    image.SetImageResource(Resource.Drawable.Die2);
                    break;
                case 3:
                    image.SetImageResource(Resource.Drawable.Die3);
                    break;
                case 4:
                    image.SetImageResource(Resource.Drawable.Die4);
                    break;
                case 5:
                    image.SetImageResource(Resource.Drawable.Die5);
                    break;
                case 6:
                default:
                    image.SetImageResource(Resource.Drawable.Die6);
                    break;
            }
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            var sw = new StringWriter();
            var pigSerializer = new XmlSerializer(typeof(PigLogic));

            pigSerializer.Serialize(sw, pig);
            var xml = sw.ToString();
            outState.PutString("Pig", xml);

            base.OnSaveInstanceState(outState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.GameFrag, container, false);
        }
    }
}