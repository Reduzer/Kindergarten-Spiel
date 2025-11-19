using KindergartenSpiel.Enums;

namespace KindergartenSpiel.Models
{
	public class Game
	{
		//Members & Events-------------------------------------------------------------------------------------

		public delegate void GameHasEnded();
		private GameHasEnded m_dGameHasEnded;
		public event GameHasEnded EGameHasEndet
		{
			add { m_dGameHasEnded += value; }
			remove { m_dGameHasEnded -= value; }
		}

		private EGameState eGameState;
		public TimeOnly oLowestReactionTime { get; private set; }
		public TimeOnly oHighestReactionTime { get; private set; }
		public int nGameScore { get; private set; }
		public int nAmountOfRoundsToPlay { get; init; }

		//Constructors----------------------------------------------------------------------------------------

		public Game(int nAmountOfRoundsToPlay)
		{
			this.eGameState = EGameState.Started;
			this.nGameScore = 0;
			this.nAmountOfRoundsToPlay = nAmountOfRoundsToPlay;
		}

		~Game()
		{

		}

		//Methods---------------------------------------------------------------------------------------------

		private void GameLoop()
		{
			while (eGameState != EGameState.Ended) {

			}

			RaiseGameHasEndet();
		}

		public void GameStarted()
		{
			eGameState = EGameState.Running;
		}

		public void ElementKlicked(object sender)
		{
			if (/*Prüfen ob Element das richtige ist*/ true) {
				nGameScore = nGameScore++;
			}
		}

		//Event Handlers--------------------------------------------------------------------------------------

		public void RaiseGameHasEndet()
		{
			m_dGameHasEnded?.Invoke();
		}
	}
}
