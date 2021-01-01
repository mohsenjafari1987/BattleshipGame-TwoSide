using Battleships.Business.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Business.extension
{
    public static class Extension
    {
        public static bool IsTruePosition(this string input)
        {
            if (input.Length < 2 || input.Length > 3)
                return false;

            if (input[0] < 'A' || input[0] > 'J')
                return false;

            if (!int.TryParse(input.Substring(1, input.Length - 1), out int number))
                return false;

            if (number < 1 || number > 10)
                return false;

            return true;
        }

        public static bool ToTuple(this string input, out Tuple<char, int> tuple)
        {
            if (input.IsTruePosition())
            {
                tuple = new Tuple<char, int>(input[0], int.Parse(input.Substring(1, input.Length - 1)));
                return true;
            }
            else
            {
                tuple = null;
                return false;
            }
        }

        public static bool ToPositionStatus(this string input, out PositionStatus? positionStatus)
        {
            positionStatus = null;
            if (!int.TryParse(input, out int number))
                return false;

            if (number != 1 && number != 2)
                return false;

            positionStatus = (PositionStatus)number;
            return true;
        }

        public static bool ToShipType(this string input, out ShipType? shipType)
        {
            shipType = null;
            if (!int.TryParse(input, out int number))
                return false;

            if (number != 0 && number != 1)
                return false;

            shipType = (ShipType)number;
            return true;
        }

        public static string GetMessageFromShipType(this ShipType? shipType)
        {
            string msg = shipType switch
            {
                ShipType.Battleship => "hit a Battleship",
                ShipType.Destroyers => "hit a Destroyer",
                _ => "miss"
            };
            return msg;
        }

        public static string GetMessageFromSinkStatus(this SinkStatus sinkStatus)
        {
            string msg = sinkStatus switch
            {
                SinkStatus.NotSink => "Your ship is alive!",
                SinkStatus.Sink => "Your Ship was destroyed!",
                _ => ""
            };
            return msg;
        }
    }
}
