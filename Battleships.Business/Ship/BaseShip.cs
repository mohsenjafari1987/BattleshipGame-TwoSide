using Battleships.Business.Enum;
using Battleships.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Business.Ship
{
    public abstract class BaseShip<T> : IBaseShip where T : class
    {
        public int Size { get; set; }
        public int Quantity { get; set; }
        protected List<List<Tuple<char, int>>> Ships { get; set; }
        public List<List<Tuple<char, int>>> AllShips => Ships;
        public List<List<Square>> Posibilities { get; set; }

        public List<List<Square>> CurrentPosibilities
        {
            get
            {
                return Posibilities;
            }
        }

        private static readonly Lazy<T> sInstance = new Lazy<T>(() => CreateInstanceOfT());
        public static T Instance { get { return sInstance.Value; } }
        private static T CreateInstanceOfT()
        {
            return Activator.CreateInstance(typeof(T), true) as T;
        }
        public BaseShip(int size, int quantity)
        {
            Size = size;
            Quantity = quantity;
        }
        internal virtual Tuple<char, int> FindCommonPosition(List<List<Square>> series)
        {
            Dictionary<Tuple<char, int>, int> cnt = new Dictionary<Tuple<char, int>, int>();
            series.ForEach(r =>
            {
                r.Where(p => p.Status == PositionStatus.Empty).ToList().ForEach(b =>
                {
                    if (cnt.ContainsKey(b.Position))
                    {
                        cnt[b.Position]++;
                    }
                    else
                    {
                        cnt.Add(b.Position, 1);
                    }
                });
            });

            return cnt.First(r => r.Value == cnt.Max(r => r.Value)).Key;

        }
        public abstract Tuple<char, int> FindNextSquer(List<Square> SquareBoard, Tuple<char, int> currentPosition);
        public abstract List<List<Square>> GeneratePosibilities(List<Square> SquareBoard, Tuple<char, int> currentPosition);
        public abstract void RefreshPosibilities(Tuple<char, int> position, PositionStatus positionStatus, bool isSinked, ShipType? shipType);
        public abstract SinkStatus CheckSink(List<Square> squares);

        public virtual void Sinked()
        {
            Posibilities = null;
        }

        public virtual List<Tuple<char, int>> SetShip(List<Square> board)
        {

            List<Tuple<char, int>> Right(Tuple<char, int> selectedPoint)
            {
                char nextColumn = selectedPoint.Item1;
                nextColumn += (char)Size;
                var endPoint = new Tuple<char, int>(nextColumn, selectedPoint.Item2);

                List<Tuple<char, int>> tempList = new List<Tuple<char, int>>();
                for (char i = selectedPoint.Item1; i <= endPoint.Item1; i++)
                {
                    var square = new Tuple<char, int>(i, selectedPoint.Item2);
                    if (board.Any(r => r.Position.Equals(square) && r.ShipType is null && !AllShips.Any(r => r.Any(rr => rr.Equals(square)))))
                    {
                        tempList.Add(new Tuple<char, int>(i, selectedPoint.Item2));
                    }
                    else
                    {
                        break;
                    }
                }

                if (tempList.Count == Size)
                {
                    return tempList;
                }

                return null;
            }
            List<Tuple<char, int>> Left(Tuple<char, int> selectedPoint)
            {
                char nextColumn = selectedPoint.Item1;
                nextColumn -= (char)Size;
                var endPoint = new Tuple<char, int>(nextColumn, selectedPoint.Item2);

                List<Tuple<char, int>> tempList = new List<Tuple<char, int>>();
                for (char i = selectedPoint.Item1; i >= endPoint.Item1; i--)
                {
                    var square = new Tuple<char, int>(i, selectedPoint.Item2);
                    if (board.Any(r => r.Position.Equals(square) && r.ShipType is null && !AllShips.Any(r => r.Any(rr => rr.Equals(square)))))
                    {
                        tempList.Add(new Tuple<char, int>(i, selectedPoint.Item2));
                    }
                    else
                    {
                        break;
                    }
                }

                if (tempList.Count == Size)
                {
                    return tempList;
                }

                return null;
            }
            List<Tuple<char, int>> Up(Tuple<char, int> selectedPoint)
            {

                var endPoint = new Tuple<char, int>(selectedPoint.Item1, selectedPoint.Item2 - (Size - 1));

                List<Tuple<char, int>> tempList = new List<Tuple<char, int>>();
                for (int i = selectedPoint.Item2; i >= endPoint.Item2; i--)
                {
                    var square = new Tuple<char, int>(selectedPoint.Item1, i);
                    if (board.Any(r => r.Position.Equals(square) && r.ShipType is null && !AllShips.Any(r => r.Any(rr => rr.Equals(square)))))
                    {
                        tempList.Add(new Tuple<char, int>(selectedPoint.Item1, i));
                    }
                    else
                    {
                        break;
                    }
                }

                if (tempList.Count == Size)
                {
                    return tempList;
                }

                return null;
            }
            List<Tuple<char, int>> Down(Tuple<char, int> selectedPoint)
            {
                var endPoint = new Tuple<char, int>(selectedPoint.Item1, selectedPoint.Item2 + (Size - 1));

                List<Tuple<char, int>> tempList = new List<Tuple<char, int>>();
                for (int i = selectedPoint.Item2; i <= endPoint.Item2; i++)
                {
                    var square = new Tuple<char, int>(selectedPoint.Item1, i);
                    if (board.Any(r => r.Position.Equals(square) && r.ShipType is null && !AllShips.Any(r => r.Any(rr => rr.Equals(square)))))
                    {
                        tempList.Add(new Tuple<char, int>(selectedPoint.Item1, i));
                    }
                    else
                    {
                        break;
                    }
                }

                if (tempList.Count == Size)
                {
                    return tempList;
                }

                return null;
            }

            List<Tuple<char, int>> blankSquars, filledSquars;

            blankSquars = board.Where(r => r.ShipType is null).Select(r => r.Position).ToList();

            Random random = new Random();
            var index = random.Next(1, blankSquars.Count);
            var startPoint = blankSquars[index];

            var methodNumber = random.Next(1, 4);
            filledSquars = methodNumber switch
            {
                1 => Right(startPoint),
                2 => Up(startPoint),
                3 => Left(startPoint),
                4 => Down(startPoint),
                _ => null
            };

            if (filledSquars is null)
            {
                filledSquars = SetShip(board);
            }

            return filledSquars;
        }

        public abstract List<List<Tuple<char, int>>> Build(List<Square> board);

    }
}
