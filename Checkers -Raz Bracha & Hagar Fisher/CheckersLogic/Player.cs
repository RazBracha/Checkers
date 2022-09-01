using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersLogic
{
    public class Player
    {
        private string m_Name;
        private int m_Score;
        private bool m_HasEaten = false;
        private bool m_CanEat = false;
        private ePlayerType m_Type;
        private eShapes m_Shape;
        private List<Pawn> m_PlayerPawns;
        private Pawn m_LastPlayedPawn = null;

        public Player(string i_Name, eShapes i_Shape, ePlayerType i_Type)
        {
            m_Name = i_Name;
            m_Shape = i_Shape;
            m_Type = i_Type;
            m_Score = 0;
            m_PlayerPawns = new List<Pawn>();
        }

        public List<Pawn> PlayerPawns
        {
            get
            {
                return m_PlayerPawns;
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }

            set
            {
                m_Name = value;
            }
        }

        public ePlayerType Type
        {
            get
            {
                return m_Type;
            }

            set
            {
                m_Type = value;
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

        public bool HasEaten
        {
            get
            {
                return m_HasEaten;
            }

            set
            {
                m_HasEaten = value;
            }
        }

        public bool CanEat
        {
            get
            {
                return m_CanEat;
            }

            set
            {
                m_CanEat = value;
            }
        }

        public int Score
        {
            get
            {
                return m_Score;
            }

            set
            {
                m_Score = value;
            }
        }

        public Pawn LastPlayedPawn
        {
            get
            {
                return m_LastPlayedPawn;
            }

            set
            {
                m_LastPlayedPawn = value;
            }
        }

        public bool CheckInPlayerList(Position i_Position, out Pawn o_WantedPawn)
        {
            bool isExist = false;

            o_WantedPawn = m_PlayerPawns.Find(x => x.Position.PositionCol == i_Position.PositionCol && x.Position.PositionRow == i_Position.PositionRow);

            if (o_WantedPawn != null)
            {
                isExist = true;
            }

            return isExist;
        }

        public int CalculatePlayerPoints()
        {
            int pawnPoints = 1;
            int kingPoints = 4;
            int sum = 0;

            foreach (Pawn pawn in m_PlayerPawns)
            {
                if (pawn.CheckIfKing())
                {
                    sum += kingPoints;
                }
                else
                {
                    sum += pawnPoints;
                }
            }

            return sum;
        }

        public eShapes GetKingShape()
        {
            return m_Shape == eShapes.PlayerO ? eShapes.PlayerOKing : eShapes.PlayerXKing;
        }
    }
}
