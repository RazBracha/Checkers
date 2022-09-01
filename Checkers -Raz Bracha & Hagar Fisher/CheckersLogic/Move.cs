using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersLogic
{
    public struct Move
    {
        private Position m_StartPosition;
        private Position m_DestinationPosition;

        public Move(Position i_StartPosition, Position i_DestinationPosition)
        {
            m_StartPosition = i_StartPosition;
            m_DestinationPosition = i_DestinationPosition;
        }

        public Position StartPosition
        {
            get
            {
                return m_StartPosition;
            }

            set
            {
                m_StartPosition = value;
            }
        }

        public Position DestinationPosition
        {
            get
            {
                return m_DestinationPosition;
            }

            set
            {
                m_DestinationPosition = value;
            }
        }

        public eMoveType GetMoveType()
        {
            return (eMoveType)Math.Abs(m_DestinationPosition.PositionCol - m_StartPosition.PositionCol);
        }

        public Position GetEatenPawnPosition()
        {
            int row, column;
            Position eatenPawnPosition;

            row = (m_DestinationPosition.PositionRow - m_StartPosition.PositionRow) / 2;
            column = (m_DestinationPosition.PositionCol - m_StartPosition.PositionCol) / 2;
            eatenPawnPosition = new Position(m_StartPosition.PositionRow + row, m_StartPosition.PositionCol + column);

            return eatenPawnPosition;
        }
    }
}
