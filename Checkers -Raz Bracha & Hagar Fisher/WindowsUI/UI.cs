using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CheckersLogic;

namespace WindowsUI
{
    internal class UI
    {
        private static Game s_Game;
        private static bool s_Exit = false;
        private static Button s_LastClickedButton;
        private FormBoard m_FormBoard;

        public void initializeGame()
        {
            FormSettings settings;

            Application.EnableVisualStyles();
            settings = new FormSettings();
            settings.ShowDialog();
            while (settings.DialogResult != DialogResult.OK)
            {
                settings.ShowDialog();
            }

            startGame(settings);
        }

        public void startGame(FormSettings i_FormSettings)
        {
            getGameSettings(i_FormSettings);
            manageGameState();
        }

        public void getGameSettings(FormSettings i_FormSettings)
        {
            Board board;
            string firstPlayerName;
            string secondPlayerName;
            Player player1;
            Player player2;

            eBoardSizeOption sizeOfBoard = i_FormSettings.BoardSize;
            ePlayerType secondPlayerType = i_FormSettings.PlayerType;
            board = new Board(sizeOfBoard);
            firstPlayerName = i_FormSettings.PlayerOneName;
            secondPlayerName = (secondPlayerType == ePlayerType.Computer) ? "Computer" : i_FormSettings.PlayerTwoName;
            player1 = new Player(firstPlayerName, eShapes.PlayerX, ePlayerType.User);
            player2 = new Player(secondPlayerName, eShapes.PlayerO, secondPlayerType);
            s_Game = new Game(player1, player2, board, eGameState.Start, player1);
        }

        public void manageGameState()
        {
            while (!s_Exit)
            {
                while (s_Game.State == eGameState.Start)
                {
                    s_Game.Board.InitPawnsPositions(s_Game.Player1, s_Game.Player2);
                    m_FormBoard = new FormBoard(s_Game.Board);
                    m_FormBoard.Player1Label = s_Game.Player1.Name;
                    m_FormBoard.Player2Label = s_Game.Player2.Name;
                    m_FormBoard.Player1Score = s_Game.Player1.Score.ToString();
                    m_FormBoard.Player2Score = s_Game.Player2.Score.ToString();
                    m_FormBoard.BoardButtonClicked += S_Game_BoardButtonClicked;
                    s_Game.PawnMoved += S_Game_PawnMoved;
                    s_Game.GameEnded += S_Game_GameEnded;
                    s_Game.TurnSwichedToComputer += S_Game_TurnSwichedToComputer;
                    m_FormBoard.FormClosing += M_FormBoard_FormClosing;
                    m_FormBoard.ShowDialog();
                }
            }
        }

        private void M_FormBoard_FormClosing(object sender, FormClosingEventArgs e)
        {
            s_Game.State = eGameState.Start;
            s_Game.CurrentPlayer = s_Game.Player1;
            m_FormBoard.BoardButtonClicked -= S_Game_BoardButtonClicked;
            s_Game.PawnMoved -= S_Game_PawnMoved;
            s_Game.GameEnded -= S_Game_GameEnded;
            s_Game.TurnSwichedToComputer -= S_Game_TurnSwichedToComputer;
        }

        private void S_Game_TurnSwichedToComputer(EventArgs eventArgs)
        {
            Move newMove;

            s_Game.GenerateComputerMove(out newMove);
            s_Game.PlayMove(newMove);
        }

        private void S_Game_GameEnded(EventArgs eventArgs)
        {
            showEndGameMessages();
        }

        private void S_Game_PawnMoved(Pawn i_Start, Pawn i_Destination, Pawn i_Eaten, EventArgs i_EventArgs)
        {
            Button startButton = m_FormBoard.r_Buttons[i_Start.Position.PositionRow, i_Start.Position.PositionCol];
            Button destinationButton = m_FormBoard.r_Buttons[i_Destination.Position.PositionRow, i_Destination.Position.PositionCol];
            Button eatenButton = null;
            Move currentMove = new Move(i_Start.Position, i_Destination.Position);
            Image destinationImage;

            if (i_Eaten != null)
            {
                eatenButton = m_FormBoard.r_Buttons[i_Eaten.Position.PositionRow, i_Eaten.Position.PositionCol];
                eatenButton.BackgroundImage = null;
            }

            if (i_Destination.CheckIfKing())
            {
                destinationImage = i_Destination.Shape == eShapes.PlayerOKing ? Properties.Resources.WhiteKing : Properties.Resources.BlackKing;
            }
            else
            {
                destinationImage = startButton.BackgroundImage;
            }

            destinationButton.BackgroundImage = destinationImage;
            startButton.BackgroundImage = null;
            startButton.BackColor = Color.White;

            if (s_Game.CurrentPlayer.HasEaten)
            {
                s_Game.CreatePawnsPossibleMoves(s_Game.Board.BoardMatrix[currentMove.DestinationPosition.PositionRow, currentMove.DestinationPosition.PositionCol]);
            }

            checkIfNeedToSwitchPlayer();
        }

        private void S_Game_BoardButtonClicked(object i_Sender, EventArgs i_Args)
        {
            Button clickedButton = (Button)i_Sender;
            Pawn currentPawn = clickedButton.Tag as Pawn;
            Pawn lastPlayedPawn;
            string message;
            string caption;
            DialogResult result;

            if (s_LastClickedButton != null)
            {
                lastPlayedPawn = s_LastClickedButton.Tag as Pawn;
                Position startPosition = lastPlayedPawn.Position;
                Position destinationPosition = currentPawn.Position;
                Move newMove = new Move(startPosition, destinationPosition);

                if (s_LastClickedButton != clickedButton)
                {
                    if (!s_Game.CheckIfPossiblePawnMove(newMove))
                    {
                        message = "Invalid move, try again";
                        caption = "Invalid Move";
                        result = MessageBox.Show(
                            message,
                            caption,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Question);
                    }
                    else
                    {
                        s_Game.PlayMove(newMove);
                    }
                }

                clickedButton.BackColor = Color.White;
                s_LastClickedButton.BackColor = Color.White;
                s_LastClickedButton = null;
            }
            else
            {
                s_LastClickedButton = clickedButton;
            }
        }

        private void checkIfNeedToSwitchPlayer()
        {
            if (!s_Game.CurrentPlayer.HasEaten || !s_Game.CurrentPlayer.CanEat)
            {
                s_Game.SwitchPlayer();
            }
            else if(s_Game.CurrentPlayer.Type == ePlayerType.Computer)
            {
                S_Game_TurnSwichedToComputer(EventArgs.Empty);
            }
        }

        private void showEndGameMessages()
        {
            StringBuilder message = new StringBuilder();
            string caption = "Game Over";
            DialogResult dialogResult;
            MessageBoxButtons userChoice;
            int playerNumber;

            if (s_Game.State == eGameState.Won)
            {
                playerNumber = s_Game.Winner == s_Game.Player1 ? 1 : 2;
                message.AppendFormat($"Player {playerNumber} Won!");
            }

            if (s_Game.State == eGameState.Tie)
            {
                message.Append("It's a tie!");
            }

            message.AppendLine().Append("Another Round?");
            userChoice = MessageBoxButtons.YesNo;
            dialogResult = MessageBox.Show(
                message.ToString(),
                caption,
                userChoice,
                MessageBoxIcon.Question);
            suggestAnotherGame(dialogResult);
        }

        private void suggestAnotherGame(DialogResult i_Result)
        {
            if (i_Result == DialogResult.Yes)
            {
                m_FormBoard.Close();
                s_Game.State = eGameState.Start;
                s_Game.CurrentPlayer = s_Game.Player1;
                m_FormBoard.BoardButtonClicked -= S_Game_BoardButtonClicked;
                s_Game.PawnMoved -= S_Game_PawnMoved;
                s_Game.GameEnded -= S_Game_GameEnded;
                s_Game.TurnSwichedToComputer -= S_Game_TurnSwichedToComputer;
            }
            else
            {
                m_FormBoard.Close();
                s_Game.State = eGameState.Quit;
                s_Exit = true;
            }
        }
    }
}
