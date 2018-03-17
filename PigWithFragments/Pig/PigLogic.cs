using System;

namespace Pig
{
    public class PigLogic
    {
        private Random rng;

        public PigLogic()
        {
            rng = new Random();
            PlayerOneTurn = true;
            CanContinue = true;
        }

        public bool PlayerOneTurn { get; set; }

        public bool CanContinue { get; set; }

        public int PlayerOneScore { get; set; }

        public int PlayerTwoScore { get; set; }

        public int RollScore { get; set; }

        public string PlayerOneName { get; set; }

        public string PlayerTwoName { get; set; }

        public string Winner { get; set; }

        public int CurrentRoll { get; set; }

        public bool IsWinner()
        {
            bool val = false;

            if(!PlayerOneTurn)
            {
                if (PlayerOneScore >= 100 && PlayerOneScore > PlayerTwoScore)
                {
                    Winner = PlayerOneName;
                    val = true;
                }
                else if (PlayerTwoScore >= 100 && PlayerTwoScore > PlayerOneScore)
                {
                    Winner = PlayerTwoName;
                    val = true;
                }
            }

            return val;
        }

        public void NextPlayer()
        {
            if (PlayerOneTurn)
            {
                RollScore = 0;
                PlayerOneTurn = false;
            }
            else
            {
                RollScore = 0;
                PlayerOneTurn = true;
            }
            CanContinue = true;
        }

        public void Roll()
        {
            CurrentRoll = rng.Next(6) + 1;
            if(CurrentRoll == 1)
                RollScore = 0;
            else
                RollScore += CurrentRoll;
        }

        public void Reset()
        {
            PlayerOneScore = 0;
            PlayerOneTurn = true;
            PlayerTwoScore = 0;
            RollScore = 0;
            Winner = "";
            CanContinue = true;
        }
    }
}