using Battleships.Business.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Business.Ship
{
    public class ShipFactory
    {
        public static IBaseShip GetInstance(ShipType? shipType)
        {
            return shipType switch
            {
                ShipType.Battleship => (BaseShip<Battleship>.Instance as IBaseShip),
                ShipType.Destroyers => (BaseShip<Destroyer>.Instance as IBaseShip),
                _ => null
            };
        }
    }
}
