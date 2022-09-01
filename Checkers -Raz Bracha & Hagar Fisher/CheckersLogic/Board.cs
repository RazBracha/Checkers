using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersLogic
{
    public class Board
    {
        private const int k_BlankRows = 2;
        private Pawn[,] m_Board;
        private eBoardSizeOption m_BoardSize;

        public Board(eBoardSizeOption i_BoardSize)
        {
            int rowAndColSize = (int)i_BoardSize;

            m_Board = new Pawn[rowAndColSize, rowAndColSize];
            m_BoardSize = i_BoardSize;
        }

        public eBoardSizeOption BoardSize
        {
            get
            {
                return m_BoardSize;
            }

            set
            {
                m_BoardSize = value;
            }
        }

        public Pawn[,] BoardMatrix
        {
            get
            {
                return m_Board;
            }
        }

        private void fillBoard(Player i_Player, int i_StartLoopValue, int i_EndLoopValue, int i_BoardSize)
        {
            for (int i = i_StartLoopValue; i < i_EndLoopValue; i++)
            {
                for (int j = 0; j < i_BoardSize; j++)
                {
                    bool isEvenRow = i % 2 == 0;
                    bool isEvenCol = j % 2 == 0;
                    bool togglePawnPlacement = (isEvenRow && !isEvenCol) || (!isEvenRow && isEvenCol);
                    Pawn newPawn;

                    if (togglePawnPlacement)
                    {
                        newPawn = new Pawn(i_Player.Shape, new Position(i, j));
                        m_Board[i, j] = newPawn;
                        i_Player.PlayerPawns.Add(newPawn);
                    }
                    else
                    {
                        newPawn = new Pawn(eShapes.Empty, new Position(i, j));
                        m_Board[i, j] = newPawn;
                    }
                }
            }
        }

        private void initBlankRows(int i_StartLoopValue, int i_BoardSize)
        {
            Pawn newPawn;

            for (int i = i_StartLoopValue; i < (i_StartLoopValue + k_BlankRows); i++)
            {
                for (int j = 0; j < i_BoardSize; j++)
                {
                    newPawn = new Pawn(eShapes.Empty, new Position(i, j));
                    m_Board[i, j] = newPawn;
                }
            }
        }

        public void InitPawnsPositions(Player i_Player1, Player i_Player2)
        {
            int boardSize = (int)m_BoardSize;

            i_Player1.PlayerPawns.Clear();
            i_Player2.PlayerPawns.Clear();
            fillBoard(i_Player2, 0, (boardSize / 2) - 1, boardSize);
            fillBoard(i_Player1, (boardSize / 2) + 1, boardSize, boardSize);
            initBlankRows((boardSize / 2) - 1, boardSize);
        }

        public bool IsEmptyCell(Position i_Position)
        {
            return m_Board[i_Position.PositionRow, i_Position.PositionCol].Shape == eShapes.Empty;
        }
    }
}
