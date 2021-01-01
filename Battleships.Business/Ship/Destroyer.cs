using Battleships.Business.Enum;
using Battleships.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Business.Ship
{
    class Destroyer : BaseShip<Destroyer>
    {
        public Destroyer() : base(4, 2)
        {

        }

        public override List<List<Tuple<char, int>>> Build(List<Square> board)
        {
            Ships = new List<List<Tuple<char, int>>>();
            for (int i = 0; i < Quantity; i++)
            {
                Ships.Add(SetShip(board));
            }
            return Ships;
        }
        public override Tuple<char, int> FindNextSquer(List<Square> SquareBoard, Tuple<char, int> currentPosition)
        {
            Posibilities ??= GeneratePosibilities(SquareBoard, currentPosition);
            if (Posibilities is not null)
            {
                var minimumCost = Posibilities.Min(serries => serries.Count(r => r.Status is PositionStatus.Empty));
                var minimumSeries = Posibilities.Where(series => series.Count(r => r.Status is PositionStatus.Empty) == minimumCost).ToList();
                if (minimumSeries.Count() == 1)
                {
                    return minimumSeries.Single().FirstOrDefault(r => r.Status is PositionStatus.Empty).Position;
                }
                else
                {
                    return FindCommonPosition(minimumSeries);
                }
            }
            return null;
        }

        public override void RefreshPosibilities(Tuple<char, int> position, PositionStatus positionStatus, bool isSinked, ShipType? shipType)
        {
            if (Posibilities is not null)
            {
                if (positionStatus is PositionStatus.White || shipType is not ShipType.Destroyers)
                {
                    Posibilities.RemoveAll(r => r.Any(rr => rr.Position.Equals(position)));
                }
                else
                {
                    Posibilities.Where(r => r.Any(rr => rr.Position.Equals(position))).ToList().ForEach(series =>
                    {
                        series.Single(r => r.Position.Equals(position)).Status = PositionStatus.Red;
                    });

                    if (isSinked)
                    {
                        Posibilities.RemoveAll(r => r.Any(rr => rr.Status != PositionStatus.Red));
                    }
                    else
                    {
                        Posibilities.RemoveAll(r => r.All(rr => rr.Status is PositionStatus.Red));
                    }
                }
            }
        }

        public override List<List<Square>> GeneratePosibilities(List<Square> SquareBoard, Tuple<char, int> currentPosition)
        {
            List<List<Square>> list = new List<List<Square>>();
            list.AddRange(Vertical(currentPosition, SquareBoard));
            list.AddRange(Horizontal(currentPosition, SquareBoard));
            return list;
        }

        private List<List<Square>> Vertical(Tuple<char, int> currentPosition, List<Square> SquareBoard)
        {
            List<List<Square>> VerticalPosibilities = new List<List<Square>>();


            for (int i = 0; i < this.Size; i++)
            {

                var firstPoint = new Tuple<char, int>(currentPosition.Item1, currentPosition.Item2 + i);
                var lastPoint = new Tuple<char, int>(currentPosition.Item1, currentPosition.Item2 - (Size - 1) + i);

                if (SquareBoard.Any(r => r.Position.Equals(firstPoint) && (r.Status == PositionStatus.Empty || (r.Status == PositionStatus.Red && r.ShipType == ShipType.Destroyers)))
                    && SquareBoard.Any(r => r.Position.Equals(firstPoint) && (r.Status == PositionStatus.Empty || (r.Status == PositionStatus.Red && r.ShipType == ShipType.Destroyers))))
                {
                    var tempList = new List<Tuple<char, int>>();
                    tempList.Add(firstPoint);
                    for (int j = firstPoint.Item2 - 1; j > lastPoint.Item2; j--)
                    {
                        var tempPoint = new Tuple<char, int>(currentPosition.Item1, j);
                        if (!SquareBoard.Any(r => r.Position.Equals(tempPoint) && (r.Status == PositionStatus.Empty || (r.Status == PositionStatus.Red && r.ShipType == ShipType.Destroyers))))
                        {
                            tempList = null;
                            break;
                        }
                        tempList.Add(tempPoint);
                    }
                    if (tempList is not null)
                    {
                        tempList.Add(lastPoint);
                        VerticalPosibilities.Add(SquareBoard.Where(r => tempList.Contains(r.Position)).ToList());
                    }
                }
            }

            return VerticalPosibilities;
        }

        private List<List<Square>> Horizontal(Tuple<char, int> currentPosition, List<Square> SquareBoard)
        {
            List<List<Square>> HorizontalPosibilities = new List<List<Square>>();
            for (int i = 0; i < this.Size; i++)
            {
                char firstAlphabet = currentPosition.Item1;
                firstAlphabet += (char)i;
                char lastAlphabet = currentPosition.Item1;
                lastAlphabet -= (char)((Size - 1) - i);
                var firstPoint = new Tuple<char, int>(firstAlphabet, currentPosition.Item2);
                var lastPoint = new Tuple<char, int>(lastAlphabet, currentPosition.Item2);

                if (SquareBoard.Any(r => r.Position.Equals(firstPoint) && (r.Status == PositionStatus.Empty || (r.Status == PositionStatus.Red && r.ShipType == ShipType.Destroyers)))
                    && SquareBoard.Any(r => r.Position.Equals(lastPoint) && (r.Status == PositionStatus.Empty || (r.Status == PositionStatus.Red && r.ShipType == ShipType.Destroyers))))
                {
                    var tempList = new List<Tuple<char, int>>();
                    tempList.Add(firstPoint);
                    char a = firstPoint.Item1;
                    a -= (char)1;
                    for (char j = a; j > lastPoint.Item1; j--)
                    {
                        var tempPoint = new Tuple<char, int>(j, currentPosition.Item2);
                        if (!SquareBoard.Any(r => r.Position.Equals(tempPoint) && (r.Status == PositionStatus.Empty || (r.Status == PositionStatus.Red && r.ShipType == ShipType.Destroyers))))
                        {
                            tempList = null;
                            break;
                        }
                        tempList.Add(tempPoint);
                    }
                    if (tempList is not null)
                    {
                        tempList.Add(lastPoint);
                        HorizontalPosibilities.Add(SquareBoard.Where(r => tempList.Contains(r.Position)).ToList());
                    }
                }

            }

            return HorizontalPosibilities;
        }

        public override SinkStatus CheckSink(List<Square> squares)
        {
            if (squares.Where(r => r.ShipType is ShipType.Destroyers).Count() == 4)
            {
                if (squares.Any(r => r.ShipType is ShipType.Destroyers && r.IsSinked))
                {
                    return SinkStatus.Sink;
                }
                return SinkStatus.SinkPossible;
            }
            return SinkStatus.NotSink;
        }

        
    }
}
