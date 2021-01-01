using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Battleships.Business.extension;
using Battleships.Business.Core;
using Battleships.Business.Enum;

namespace Battleships.Test
{
    [TestClass]
    public class BattleshipTest
    {
        [TestMethod]
        public void CheckTupleFormat_WithValidInput()
        {
            // Arrange
            string position = "D5";
            var expected = true;

            // Act            
            var actual = position.IsTruePosition();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckTupleFormat_WithInvalidInput()
        {
            // Arrange
            string position = "WRONG";
            var expected = false;

            //Act
            try
            {

                var actual = position.IsTruePosition();
                // Assert
                Assert.AreEqual(expected, actual);
            }
            catch (Exception)
            {
                Assert.Fail("unexpected result");
            }

        }

        [TestMethod]
        public void ConvertToTuple_WithValidInput()
        {
            // Arrange
            string position = "D5";
            var expected = new Tuple<char, int>('D', 5);


            // Act
            Tuple<char, int> actual;
            position.ToTuple(out actual);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertToTuple_WithInvalidInput()
        {
            // Arrange
            string position = "M11";
            var expected = false;


            // Act
            var actual = position.ToTuple(out Tuple<char, int> output); ;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetSquare_WithInvalidPosition()
        {
            // Arrange
            WrapperBattleshipBoard wrp = new WrapperBattleshipBoard();
            wrp.BuildShips();
            Tuple<char, int> position = new Tuple<char, int>('M', 10);

            //  Act and assert
            Assert.ThrowsException<InvalidOperationException>(() => wrp.GetSquareFromComputerOcean(position));
        }

        [TestMethod]
        public void BattleShipOperation_CreateAndSink()
        {
            // Arrange
            var expected = 1;
            BattleshipBoard btl = new BattleshipBoard();
            btl.BuildShips(ShipType.Battleship);
            for (int i = 1; i <= 10; i++)
            {
                for (char c = 'A'; c <= 'J'; c++)
                {
                    var square = new Tuple<char, int>(c, i);
                    if (btl.GetSquareFromComputerOcean(square).ShipType is ShipType.Battleship)
                    {
                        btl.SetRedSquareComputerOcean(square);
                    }
                }
            }

            //act
            var actual = btl.GetComputerSinkShipCount(ShipType.Battleship);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DestroyerOperation_CreateAndSink()
        {
            // Arrange
            var expected = 2;
            BattleshipBoard btl = new BattleshipBoard();
            btl.BuildShips(ShipType.Destroyers);
            for (int i = 1; i <= 10; i++)
            {
                for (char c = 'A'; c <= 'J'; c++)
                {
                    var square = new Tuple<char, int>(c, i);
                    if (btl.GetSquareFromComputerOcean(square).ShipType is ShipType.Destroyers)
                    {
                        btl.SetRedSquareComputerOcean(square);
                    }
                }
            }

            //act
            var actual = btl.GetComputerSinkShipCount(ShipType.Destroyers);

            // Assert
            Assert.AreEqual(expected, actual);
        }


    }
}
