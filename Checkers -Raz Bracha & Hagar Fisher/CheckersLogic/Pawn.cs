using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersLogic
{
    public class Pawn
    {
        private Position m_Position;
        private eShapes m_Shape;
        private List<Move> m_PossibleMoves = new List<Move>();

        public Pawn(eShapes i_PlayerShape, Position i_Position)
        {
            m_Position = i_Position;
            m_Shape = i_PlayerShape;
        }

        public Position Position
        {
            get
            {
                return m_Position;
            }

            set
            {
                m_Position = value;
            }
        }

        public eShapes Shape
        {
            get
            {
                return m_Shape;
            }

            set
            {
                m_Shape = value;
            }
        }

        public List<Move> PossibleMoves
        {
            get
            {
                return m_PossibleMoves;
            }
        }

        public void MakeKing()
        {
            m_Shape = m_Shape == eShapes.PlayerO ? eShapes.PlayerOKing : eShapes.PlayerXKing;
        }

        public bool CheckIfKing()
        {
            bool isKing = m_Shape == eShapes.PlayerOKing || m_Shape == eShapes.PlayerXKing;

            return isKing;
        }
    }
}
