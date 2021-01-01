using Battleships.Business.Enum;
using Battleships.Business.Model;
using Battleships.Business.Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Business.Core
{

    public class BattleshipBoard
    {
        private List<Square> ComputerOcean { get; set; }

        private List<Square> OpponentOcean { get; set; }
        private int opponentDestroyed { get; set; }
        public int OpponentDestroyed
        {
            get
            {
                return opponentDestroyed;
            }
        }

        private Square CurrentRedSquare { get; set; }

        public BattleshipBoard()
        {
            OpponentOcean = new List<Square>();
            for (int i = 1; i <= 10; i++)
            {
                for (char c = 'A'; c <= 'J'; c++)
                {
                    OpponentOcean.Add(new Square(c, i));
                }
            }

            ComputerOcean = new List<Square>();
            for (int i = 1; i <= 10; i++)
            {
                for (char c = 'A'; c <= 'J'; c++)
                {
                    ComputerOcean.Add(new Square(c, i));
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Tuple<char, int> SelectSquare()
        {

            if (CurrentRedSquare is not null)
            {
                var ship = ShipFactory.GetInstance(CurrentRedSquare.ShipType);

                return ship.FindNextSquer(OpponentOcean, CurrentRedSquare.Position);
            }
            else
            {
                List<Tuple<char, int>> blankSquares;

                if (OpponentOcean.Any(r => r.Status is PositionStatus.Red && !r.IsSinked))
                {
                    blankSquares = OpponentOcean.Where(r => r.Status is PositionStatus.Red && !r.IsSinked).Select(r => r.Position).ToList();
                }
                else
                {
                    blankSquares = OpponentOcean.Where(r => r.Status == PositionStatus.Empty).Select(r => r.Position).ToList();
                }

                Random random = new Random();
                var index = random.Next(1, blankSquares.Count);
                return blankSquares[index];
            }
        }

        public SinkStatus CheckOpponentShipSink()
        {
            var ship = ShipFactory.GetInstance(CurrentRedSquare.ShipType);

            return ship.CheckSink(OpponentOcean);
        }

        public void FillSquare(Tuple<char, int> position, PositionStatus positionStatus, ShipType? shipType)
        {
            var Square = OpponentOcean.Single(r => r.Position.Equals(position));
            Square.Status = positionStatus;
            Square.ShipType = shipType;
            if (positionStatus is PositionStatus.Red && CurrentRedSquare is null)
            {
                CurrentRedSquare = Square;
            }
        }

        public void Process(Tuple<char, int> position, PositionStatus positionStatus, ShipType? shipType, bool isSinked)
        {
            if (CurrentRedSquare is not null)
            {
                var ship = ShipFactory.GetInstance(CurrentRedSquare.ShipType);

                ship.RefreshPosibilities(position, positionStatus, isSinked, shipType);

                if (isSinked)
                {
                    Sinked(ship);
                }
            }
        }

        private void Sinked(IBaseShip ship)
        {
            opponentDestroyed++;

            var sinkedPoint = ship.CurrentPosibilities.Single();

            OpponentOcean.Where(r => sinkedPoint.Any(sp => r.Position.Equals(sp.Position))).Select(r => r.IsSinked = true).ToList();

            CurrentRedSquare = null;
            ship.Sinked();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipType"></param>
        /// <returns></returns>

        public void BuildShips(ShipType shipType)
        {
            void setInBoard(List<List<Tuple<char, int>>> ships, ShipType shipType)
            {
                ships.ForEach(ship =>
                {
                    ship.ForEach(position =>
                    {
                        ComputerOcean.Single(r => r.Position.Equals(position)).ShipType = shipType;
                    });

                });
            }

            var ships = ShipFactory.GetInstance(shipType).Build(ComputerOcean);
            setInBoard(ships, shipType);

        }

        public Square GetSquareFromComputerOcean(Tuple<char, int> position)
        {
            return ComputerOcean.Single(r => r.Position.Equals(position));
        }

        public void SetRedSquareComputerOcean(Tuple<char, int> position)
        {
            var square = GetSquareFromComputerOcean(position);
            square.Status = PositionStatus.Red;
        }

        public int GetRedSquareCount(List<Tuple<char, int>> shipSquares)
        {

            return ComputerOcean.Where(r => r.Status is PositionStatus.Red && shipSquares.Any(p => r.Position.Equals(p))).Count();

        }

        public int GetComputerSinkShipCount(ShipType shipType)
        {
            int count = 0;
            ShipFactory.GetInstance(shipType).AllShips.ForEach(ship =>
            {
                if (GetRedSquareCount(ship) == ship.Count)
                {
                    count++;
                }
            });
            return count;
        }

    }
}
