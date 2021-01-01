using Battleships.Business.Enum;
using Battleships.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Business.Core
{
    public class WrapperBattleshipBoard
    {
        #region Properties
        private BattleshipBoard BattleshipBoard { get; set; }

        private const int WinnerCount = 3;
        #endregion

        #region Constractor
        public WrapperBattleshipBoard()
        {
            BattleshipBoard = new BattleshipBoard();
        }
        #endregion

        #region UserSideMethods
        public void BuildShips()
        {
            BattleshipBoard.BuildShips(ShipType.Destroyers);

            BattleshipBoard.BuildShips(ShipType.Battleship);
        }
        public bool UserWinnerCheck()
        {
            return GetComputerOceanSinkShipCount() == WinnerCount;
        }
        public int GetComputerOceanSinkShipCount()
        {
            int count = 0;

            count += BattleshipBoard.GetComputerSinkShipCount(ShipType.Battleship);
            count += BattleshipBoard.GetComputerSinkShipCount(ShipType.Destroyers);

            return count;
        }
        public int GetComputerOceanSinkShipCount(ShipType shipType)
        {
            return BattleshipBoard.GetComputerSinkShipCount(shipType);
        }
        public Square GetSquareFromComputerOcean(Tuple<char, int> position)
        {
            return BattleshipBoard.GetSquareFromComputerOcean(position);
        }
        public void SetRedFlagInComputerOcean(Tuple<char, int> position)
        {
            BattleshipBoard.SetRedSquareComputerOcean(position);
        }
        #endregion

        #region ComputerSideMethods
        public bool ComputerWinnerCheck()
        {
            return BattleshipBoard.OpponentDestroyed == WinnerCount;
        }
        public void FillRedSquare(Tuple<char, int> position, ShipType? shipType)
        {
            BattleshipBoard.FillSquare(position, PositionStatus.Red, shipType);
        }
        public void FillWhiteSquare(Tuple<char, int> position)
        {
            BattleshipBoard.FillSquare(position, PositionStatus.White, null);
        }
        public Tuple<char, int> SelectSquare()
        {

            return BattleshipBoard.SelectSquare();
        }
        public SinkStatus CheckOpponentShipSink()
        {
            return BattleshipBoard.CheckOpponentShipSink();
        }
        public void ProcessRedSquare(Tuple<char, int> position, PositionStatus positionStatus, ShipType shipType, bool isSink)
        {
            BattleshipBoard.Process(position, positionStatus, shipType, isSink);
        }
        public void ProcessWhiteSquare(Tuple<char, int> position)
        {
            BattleshipBoard.Process(position, PositionStatus.White, null, false);
        }
        #endregion


    }
}
