namespace CheckersLogic
{
    public struct Position
    {
        private int m_Row;
        private int m_Col;

        public Position(int i_Row, int i_Col)
        {
            m_Row = i_Row;
            m_Col = i_Col;
        }

        public bool ValidatePositionInBoardBounds(int i_BoardSize)
        {
            return !(m_Row < 0 || m_Col < 0 || m_Row > i_BoardSize - 1 || m_Col > i_BoardSize - 1);
        }

        public int PositionCol
        {
            get
            {
                return m_Col;
            }

            set
            {
                m_Col = value;
            }
        }

        public int PositionRow
        {
            get
            {
                return m_Row;
            }

            set
            {
                m_Row = value;
            }
        }
    }
}
