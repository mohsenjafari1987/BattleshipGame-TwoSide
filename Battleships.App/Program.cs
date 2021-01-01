﻿using Battleships.Business.Core;
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
        static void Main(string[] args)
        {
            WrapperBattleshipBoard wprBattleshipBoard = new WrapperBattleshipBoard();

            wprBattleshipBoard.BuildShips();

            Console.WriteLine("I built 3 ships, Are you ready?");


            while (true)
            {
                var userInput = GetPositionFromUser();
                var squareDetail = wprBattleshipBoard.GetSquareFromComputerOcean(userInput);

                Console.WriteLine($"Result of your choice: {squareDetail.ShipType.GetMessageFromShipType()}");

                if (squareDetail.ShipType is ShipType.Destroyers || squareDetail.ShipType is ShipType.Battleship)
                {
                    wprBattleshipBoard.SetRedFlagInComputerOcean(squareDetail.Position);
                    var destoyed = wprBattleshipBoard.GetComputerSinkShipCount(squareDetail.ShipType.Value);
                    if (destoyed > 0)
                    {
                        Console.WriteLine($"You sunk the ship type= {squareDetail.ShipType}- count= {destoyed}");

                        if (wprBattleshipBoard.UserWinnerCheck())
                        {
                            Console.WriteLine("You won the game");
                            break;
                        }
                    }
                }


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
                        wprBattleshipBoard.Process(position, positionStatus, shipType, sinkStatus == SinkStatus.Sink);
                    }
                    else //SinkStatus.SinkPossible
                    {                        
                        var isSink = GetTrueFalseFromUser();
                        wprBattleshipBoard.Process(position, positionStatus, shipType, isSink);
                    }
                }
                else
                {
                    wprBattleshipBoard.FillWhiteSquare(position);
                    wprBattleshipBoard.Process(position, PositionStatus.White, null, false);
                }
            }

            Console.WriteLine("Computer destroyed all your ships...");
            Console.ReadKey();
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
