using Battleships.Business.Enum;
using Battleships.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Business.Ship
{
    public interface IBaseShip
    {
        public List<List<Square>> CurrentPosibilities { get; }
        public int Size { get; set; }
        public int Quantity { get; set; }
        public List<List<Tuple<char, int>>> AllShips { get; }
        public SinkStatus CheckSink(List<Square> squares);
        public Tuple<char, int> FindNextSquer(List<Square> SquareBoard, Tuple<char, int> currentPosition);
        public List<List<Square>> GeneratePosibilities(List<Square> SquareBoard, Tuple<char, int> currentPosition);
        public void RefreshPosibilities(Tuple<char, int> position, PositionStatus positionStatus, bool isSinked, ShipType? shipType);
        public void Sinked();
        public List<List<Tuple<char, int>>> Build(List<Square> board);
    }
}
