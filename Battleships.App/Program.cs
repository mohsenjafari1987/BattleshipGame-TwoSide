using Battleships.Business.Core;
using Battleships.Business.Enum;
using Battleships.Business.extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.App
{
    class Program
    {
        public static WrapperBattleshipBoard wprBattleshipBoard { get; set; }
        static void Main(string[] args)
        {
            wprBattleshipBoard = new WrapperBattleshipBoard();

            wprBattleshipBoard.BuildShips();

            Console.WriteLine("I built 3 ships, Are you ready?");

            while (true)
            {
                if (UserPlay())
                    break;
                
                if (ComputerPlay())
                    break;
            }

            Console.ReadKey();
        }
        static bool UserPlay()
        {
            var userInput = GetPositionFromUser();
            var squareDetail = wprBattleshipBoard.GetSquareFromComputerOcean(userInput);

            Console.WriteLine($"Result of your choice: {squareDetail.ShipType.GetMessageFromShipType()}");

            if (squareDetail.ShipType is ShipType.Destroyers || squareDetail.ShipType is ShipType.Battleship)
            {
                wprBattleshipBoard.SetRedFlagInComputerOcean(squareDetail.Position);
                var destoyed = wprBattleshipBoard.GetComputerOceanSinkShipCount(squareDetail.ShipType.Value);
                if (destoyed > 0)
                {
                    Console.WriteLine($"You sunk the ship type= {squareDetail.ShipType}- count= {destoyed}");

                    if (wprBattleshipBoard.UserWinnerCheck())
                    {
                        Console.WriteLine("You won the game");
                        return true;
                    }
                }
            }
            return false;
        }

        static bool ComputerPlay()
        {
            var position = wprBattleshipBoard.SelectSquare();

            Console.WriteLine($"This is my choice: {position}");

            var positionStatus = GetStatusFromUser();

            if (positionStatus is PositionStatus.Red)
            {
                var shipType = GetShipTypeFromUser();

                wprBattleshipBoard.FillRedSquare(position, shipType);

                var sinkStatus = wprBattleshipBoard.CheckOpponentShipSink();
                if (sinkStatus is SinkStatus.NotSink || sinkStatus is SinkStatus.Sink)
                {
                    Console.WriteLine(sinkStatus.GetMessageFromSinkStatus());
                    wprBattleshipBoard.ProcessRedSquare(position, positionStatus, shipType, sinkStatus == SinkStatus.Sink);
                }
                else //SinkStatus.SinkPossible
                {
                    var isSink = GetTrueFalseFromUser();
                    wprBattleshipBoard.ProcessRedSquare(position, positionStatus, shipType, isSink);
                }

                if (wprBattleshipBoard.ComputerWinnerCheck())
                {
                    Console.WriteLine("Computer won the game");
                    return true;
                }
            }
            else
            {
                wprBattleshipBoard.FillWhiteSquare(position);
                wprBattleshipBoard.ProcessWhiteSquare(position);
            }
            return false;
        }


        static Tuple<char, int> GetPositionFromUser()
        {
            Console.WriteLine("Guess a square between A-J and 1-10, for example, A1");
            var userInput = Console.ReadLine();
            if (userInput.ToTuple(out Tuple<char, int> userTuple))
            {
                return userTuple;
            }
            else
            {
                Console.WriteLine("Your input is wrong! Pleas try again...");
                return GetPositionFromUser();
            }
        }

        static PositionStatus GetStatusFromUser()
        {
            Console.WriteLine("Please enter the color/ WHITE = 1, RED = 2");
            var userInput = Console.ReadLine();
            if (userInput.ToPositionStatus(out PositionStatus? positionStatus))
            {
                return positionStatus.Value;
            }
            else
            {
                Console.WriteLine("Your input is wrong! Pleas try again...");
                return GetStatusFromUser();
            }
        }

        static ShipType GetShipTypeFromUser()
        {
            Console.WriteLine("Please enter the ship type/ Battleship = 0, Destroyer = 1");
            var userInput = Console.ReadLine();
            if (userInput.ToShipType(out ShipType? shipType))
            {
                return shipType.Value;
            }
            else
            {
                Console.WriteLine("Your input is wrong! Pleas try again...");
                return GetShipTypeFromUser();
            }
        }

        static bool GetTrueFalseFromUser()
        {
            Console.WriteLine("Was your ship sunk? 0= false, 1= True");
            var userInput = Console.ReadLine();
            if (userInput is "1")
            {
                return true;
            }
            else if (userInput is "0")
            {
                return false;
            }
            else
            {
                Console.WriteLine("Your input is wrong! Pleas try again...");
                return GetTrueFalseFromUser();
            }
        }
    }
}
