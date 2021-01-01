using Battleships.Business.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Business.Model
{
    public class Square
    {
        public Tuple<char, int> Position { get; set; }
        public PositionStatus Status { get; set; }
        public bool IsSinked { get; set; }
        public ShipType? ShipType { get; set; }

        public Square(Tuple<char, int> position)
        {
            Position = position;
        }

        public Square(char row, int column)
        {
            Position = new Tuple<char, int>(row, column);
        }
    }
}
