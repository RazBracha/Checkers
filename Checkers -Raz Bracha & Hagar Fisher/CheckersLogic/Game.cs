using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersLogic
{
    public class Game
    {
        private readonly Board m_Board;
        private readonly Player m_Player1;
        private readonly Player m_Player2;
        private eGameState m_GameState;
        private Player m_CurrentPlayer;
        private Player m_Winner;
        private Dictionary<string, Position> m_Directions;

        public delegate void PawnMovedEventHandler(Pawn start, Pawn destination, Pawn eaten, EventArgs eventArgs);

        public event PawnMovedEventHandler PawnMoved;

        public delegate void GameEndedEventHandler(EventArgs eventArgs);

        public event GameEndedEventHandler GameEnded;

        public delegate void TurnSwitchedToComputerEventHandler(EventArgs eventArgs);

        public event TurnSwitchedToComputerEventHandler TurnSwichedToComputer;

        public Game(Player i_Player1, Player i_Player2, Board i_Board, eGameState i_GameState, Player i_CurrentPlayer)
        {
            m_Player1 = i_Player1;
            m_Player2 = i_Player2;
            m_Board = i_Board;
            m_GameState = i_GameState;
            m_CurrentPlayer = i_CurrentPlayer;
            m_Winner = null;
            initDirectionDictionary();
        }

        private void initDirectionDictionary()
        {
            m_Directions = new Dictionary<string, Position>
            {
                { "UpRight", new Position(-1, 1) },
                { "UpLeft", new Position(-1, -1) },
                { "DownRight", new Position(1, 1) },
                { "DownLeft", new Position(1, -1) },
                { "UpRightJump", new Position(-2, 2) },
                { "UpLeftJump", new Position(-2, -2) },
                { "DownRightJump", new Position(2, 2) },
                { "DownLeftJump", new Position(2, -2) },
            };
        }

        public Player Player1
        {
            get
            {
                return m_Player1;
            }
        }

        public Player Player2
        {
            get
            {
                return m_Player2;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return m_CurrentPlayer;
            }

            set
            {
                m_CurrentPlayer = value;
            }
        }

        public Player Winner
        {
            get
            {
                return m_Winner;
            }
        }

        public eGameState State
        {
            get
            {
                return m_GameState;
            }

            set
            {
                m_GameState = value;
            }
        }

        public Board Board
        {
            get
            {
                return m_Board;
            }
        }

        public void PlayMove(Move i_NewMove)
        {
            Pawn startPositionPawn = m_Board.BoardMatrix[i_NewMove.StartPosition.PositionRow, i_NewMove.StartPosition.PositionCol];
            Pawn destinationPositionPawn = m_Board.BoardMatrix[i_NewMove.DestinationPosition.PositionRow, i_NewMove.DestinationPosition.PositionCol];
            Pawn eatenPawn = null;
            if (i_NewMove.GetMoveType() == eMoveType.Jump)
            {
                CurrentPlayer.HasEaten = true;
                eatenPawn = deleteEatenPawn(i_NewMove);
            }

            destinationPositionPawn.Shape = startPositionPawn.Shape;
            startPositionPawn.Shape = eShapes.Empty;
            CurrentPlayer.LastPlayedPawn = destinationPositionPawn;
            CurrentPlayer.PlayerPawns.Remove(startPositionPawn);
            CurrentPlayer.PlayerPawns.Add(destinationPositionPawn);

            if (checkIfShouldBeKing(destinationPositionPawn))
            {
                destinationPositionPawn.MakeKing();
            }

            PawnMoved.Invoke(startPositionPawn, destinationPositionPawn, eatenPawn, EventArgs.Empty);
        }

        public void ChangeGameState()
        {
            Player opponent = GetOpponent();
            bool isOpponentPlayerMovesIsEmpty = false;
            bool isCurrentPlayerMovesIsEmpty = false;

            if (CurrentPlayer.PlayerPawns.Count == 0)
            {
                m_GameState = eGameState.Won;
                m_Winner = opponent;
                calculateWinnersScore();
                GameEnded.Invoke(EventArgs.Empty);
            }
            else
            {
                isCurrentPlayerMovesIsEmpty = !checkIfThereAreLegalMovesLeft(CurrentPlayer);
                m_CurrentPlayer = opponent;
                isOpponentPlayerMovesIsEmpty = !checkIfThereAreLegalMovesLeft(opponent);
                m_CurrentPlayer = GetOpponent();

                if (isCurrentPlayerMovesIsEmpty)
                {
                    if (isOpponentPlayerMovesIsEmpty)
                    {
                        m_GameState = eGameState.Tie;
                        GameEnded.Invoke(EventArgs.Empty);
                    }
                    else
                    {
                        m_GameState = eGameState.Won;
                        m_Winner = opponent;
                        calculateWinnersScore();
                        GameEnded.Invoke(EventArgs.Empty);
                    }
                }
            }
        }

        private void calculateWinnersScore()
        {
            m_Winner.Score += m_Winner.CalculatePlayerPoints() - CurrentPlayer.CalculatePlayerPoints();
        }

        private bool checkIfThereAreLegalMovesLeft(Player i_Player)
        {
            bool thereAreLegalMovesLeft = false;

            if (i_Player.PlayerPawns.Count != 0)
            {
                foreach (Pawn pawn in i_Player.PlayerPawns)
                {
                    CreatePawnsPossibleMoves(pawn);

                    if (pawn.PossibleMoves.Count() > 0)
                    {
                        thereAreLegalMovesLeft = true;
                    }
                }
            }

            return thereAreLegalMovesLeft;
        }

        public bool ValidateMove(Move i_Move)
        {
            bool isDestinationEmpty = false;
            bool isCurrentPlayerPawn = false;
            bool isLegalMove = false;
            bool isInBoardBounds = i_Move.StartPosition.ValidatePositionInBoardBounds((int)m_Board.BoardSize) &&
                                   i_Move.DestinationPosition.ValidatePositionInBoardBounds((int)m_Board.BoardSize);

            if (isInBoardBounds)
            {
                isDestinationEmpty = m_Board.IsEmptyCell(i_Move.DestinationPosition);
                isCurrentPlayerPawn = m_CurrentPlayer.CheckInPlayerList(i_Move.StartPosition, out Pawn pawn);
                isLegalMove = checkIfLegalMove(i_Move);
            }

            return isInBoardBounds && isDestinationEmpty && isCurrentPlayerPawn && isLegalMove;
        }

        private bool checkIfLegalMove(Move i_Move)
        {
            return checkIfUpRight(i_Move) || checkIfUpLeft(i_Move) || checkIfDownRight(i_Move) || checkIfDownLeft(i_Move);
        }

        private bool checkIfUpRight(Move i_Move)
        {
            bool isLegalMove = false;
            Pawn startPositionPawn = m_Board.BoardMatrix[i_Move.StartPosition.PositionRow, i_Move.StartPosition.PositionCol];

            if (m_CurrentPlayer == m_Player1 || startPositionPawn.CheckIfKing())
            {
                if (checkMoveByOffset(i_Move, -1, 1))
                {
                    isLegalMove = true;
                }
                else if (checkMoveByOffset(i_Move, -2, 2))
                {
                    isLegalMove = checkIfCanJump(i_Move, -1, 1);
                }
            }

            return isLegalMove;
        }

        private bool checkIfUpLeft(Move i_Move)
        {
            bool isLegalMove = false;
            Pawn startPositionPawn = m_Board.BoardMatrix[i_Move.StartPosition.PositionRow, i_Move.StartPosition.PositionCol];

            if (m_CurrentPlayer == m_Player1 || startPositionPawn.CheckIfKing())
            {
                if (checkMoveByOffset(i_Move, -1, -1))
                {
                    isLegalMove = true;
                }
                else if (checkMoveByOffset(i_Move, -2, -2))
                {
                    isLegalMove = checkIfCanJump(i_Move, -1, -1);
                }
            }

            return isLegalMove;
        }

        private bool checkIfDownRight(Move i_Move)
        {
            bool isLegalMove = false;
            Pawn startPositionPawn = m_Board.BoardMatrix[i_Move.StartPosition.PositionRow, i_Move.StartPosition.PositionCol];

            if (m_CurrentPlayer == m_Player2 || startPositionPawn.CheckIfKing())
            {
                if (checkMoveByOffset(i_Move, 1, 1))
                {
                    isLegalMove = true;
                }
                else if (checkMoveByOffset(i_Move, 2, 2))
                {
                    isLegalMove = checkIfCanJump(i_Move, 1, 1);
                }
            }

            return isLegalMove;
        }

        private bool checkIfDownLeft(Move i_Move)
        {
            bool isLegalMove = false;
            Pawn startPositionPawn = m_Board.BoardMatrix[i_Move.StartPosition.PositionRow, i_Move.StartPosition.PositionCol];

            if (m_CurrentPlayer == m_Player2 || startPositionPawn.CheckIfKing())
            {
                if (checkMoveByOffset(i_Move, 1, -1))
                {
                    isLegalMove = true;
                }
                else if (checkMoveByOffset(i_Move, 2, -2))
                {
                    isLegalMove = checkIfCanJump(i_Move, 1, -1);
                }
            }

            return isLegalMove;
        }

        private bool checkMoveByOffset(Move i_Move, int i_RowOffset, int i_ColOffset)
        {
            bool legalMove = false;

            if (i_Move.DestinationPosition.PositionRow == i_Move.StartPosition.PositionRow + i_RowOffset &&
                i_Move.DestinationPosition.PositionCol == i_Move.StartPosition.PositionCol + i_ColOffset)
            {
                legalMove = true;
            }

            return legalMove;
        }

        public void SwitchPlayer()
        {
            m_CurrentPlayer.HasEaten = false;
            m_CurrentPlayer = m_CurrentPlayer == m_Player1 ? m_Player2 : m_Player1;
            ChangeGameState();

            if(m_GameState == eGameState.Start)
            {
                if(m_CurrentPlayer.Type == ePlayerType.Computer)
                {
                    TurnSwichedToComputer.Invoke(EventArgs.Empty);
                }
            }
        }

        public Player GetOpponent()
        {
            Player opponent = m_CurrentPlayer == m_Player1 ? m_Player2 : m_Player1;

            return opponent;
        }

        private bool checkIfCanJump(Move i_Move, int i_RowOffset, int i_ColOffset)
        {
            Player opponent = GetOpponent();

            return m_Board.BoardMatrix[i_Move.StartPosition.PositionRow + i_RowOffset, i_Move.StartPosition.PositionCol + i_ColOffset].Shape == opponent.Shape
                || m_Board.BoardMatrix[i_Move.StartPosition.PositionRow + i_RowOffset, i_Move.StartPosition.PositionCol + i_ColOffset].Shape == opponent.GetKingShape();
        }

        private Pawn deleteEatenPawn(Move i_Move)
        {
            Position eatenPawnPosition = i_Move.GetEatenPawnPosition();
            Player opponent = GetOpponent();
            Pawn eatenPawn;

            m_Board.BoardMatrix[eatenPawnPosition.PositionRow, eatenPawnPosition.PositionCol].Shape = eShapes.Empty;
            opponent.CheckInPlayerList(eatenPawnPosition, out eatenPawn);
            opponent.PlayerPawns.Remove(eatenPawn);
            return eatenPawn;
        }

        public void CreatePawnsPossibleMoves(Pawn i_CurrentPawn)
        {
            i_CurrentPawn.PossibleMoves.Clear();

            foreach (KeyValuePair<string, Position> direction in m_Directions)
            {
                int row = i_CurrentPawn.Position.PositionRow + direction.Value.PositionRow;
                int column = i_CurrentPawn.Position.PositionCol + direction.Value.PositionCol;
                Position destinationPosition = new Position(row, column);
                Move newMove = new Move(i_CurrentPawn.Position, destinationPosition);

                if (ValidateMove(newMove))
                {
                    if (CurrentPlayer.HasEaten)
                    {
                        if (newMove.GetMoveType() == eMoveType.Jump)
                        {
                            i_CurrentPawn.PossibleMoves.Add(newMove);
                            CurrentPlayer.CanEat = true;
                        }
                    }
                    else
                    {
                        i_CurrentPawn.PossibleMoves.Add(newMove);
                    }
                }
            }

            if (CurrentPlayer.HasEaten && i_CurrentPawn.PossibleMoves.Count == 0)
            {
                CurrentPlayer.CanEat = false;
            }
        }

        private void createComputerPossibleMovesLists(List<Move> i_StepMoves, List<Move> i_JumpMoves)
        {
            foreach (Pawn pawn in CurrentPlayer.PlayerPawns)
            {
                CreatePawnsPossibleMoves(pawn);

                foreach (Move move in pawn.PossibleMoves)
                {
                    if (move.GetMoveType() == eMoveType.Step)
                    {
                        i_StepMoves.Add(move);
                    }
                    else
                    {
                        i_JumpMoves.Add(move);
                    }
                }
            }
        }

        public void GenerateComputerMove(out Move o_Move)
        {
            List<Move> stepMoves = new List<Move>();
            List<Move> jumpMoves = new List<Move>();
            Random random = new Random();
            int randomMoveIndex;
            Move? computerMove = null;

            createComputerPossibleMovesLists(stepMoves, jumpMoves);

            if (jumpMoves.Count != 0)
            {
                randomMoveIndex = random.Next(0, jumpMoves.Count);
                computerMove = jumpMoves[randomMoveIndex];
            }
            else if (stepMoves.Count != 0)
            {
                randomMoveIndex = random.Next(0, stepMoves.Count);
                computerMove = stepMoves[randomMoveIndex];
            }

            o_Move = (Move)computerMove;
        }

        public bool CheckIfPossiblePawnMove(Move i_Move)
        {
            Pawn pawn;
            bool canJumpFromAnotherPawn = false;
            bool jumpMove = false;
            bool isPossibleMove = false;
            bool inPlayerList = CurrentPlayer.CheckInPlayerList(i_Move.StartPosition, out pawn);

            if (inPlayerList)
            {
                CreatePawnsPossibleMoves(pawn);
                jumpMove = pawn.PossibleMoves.Exists(x => findMove(x, i_Move) && x.GetMoveType() == eMoveType.Jump);

                if (!CurrentPlayer.HasEaten)
                {
                    if (jumpMove)
                    {
                        isPossibleMove = true;
                    }
                    else
                    {
                        canJumpFromAnotherPawn = checkIfCanJump(CurrentPlayer);
                        if (canJumpFromAnotherPawn)
                        {
                            isPossibleMove = false;
                        }
                        else
                        {
                            isPossibleMove = pawn.PossibleMoves.Exists(x => findMove(x, i_Move));
                        }
                    }
                }
                else
                {
                    if (CurrentPlayer.CanEat)
                    {
                        isPossibleMove = pawn == CurrentPlayer.LastPlayedPawn && jumpMove;
                    }
                    else
                    {
                        isPossibleMove = false;
                    }
                }
            }

            return isPossibleMove;
        }

        private bool checkIfCanJump(Player i_Player)
        {
            bool canJump = false;

            foreach (Pawn pawn in i_Player.PlayerPawns)
            {
                CreatePawnsPossibleMoves(pawn);

                foreach (Move move in pawn.PossibleMoves)
                {
                    if (move.GetMoveType() == eMoveType.Jump)
                    {
                        canJump = true;
                    }
                }
            }

            return canJump;
        }

        private bool findMove(Move i_X, Move i_Move)
        {
            int startRow = i_Move.StartPosition.PositionRow;
            int destinationRow = i_Move.DestinationPosition.PositionRow;
            int startCol = i_Move.StartPosition.PositionCol;
            int destinationCol = i_Move.DestinationPosition.PositionCol;

            return i_X.StartPosition.PositionRow == startRow &&
                   i_X.StartPosition.PositionCol == startCol &&
                   i_X.DestinationPosition.PositionRow == destinationRow &&
                   i_X.DestinationPosition.PositionCol == destinationCol;
        }

        private bool checkIfShouldBeKing(Pawn i_Pawn)
        {
            int rowPosition = i_Pawn.Shape == eShapes.PlayerO ? (int)m_Board.BoardSize - 1 : 0;

            return i_Pawn.Position.PositionRow == rowPosition;
        }
    }
}
