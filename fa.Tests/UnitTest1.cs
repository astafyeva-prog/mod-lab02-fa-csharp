using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using fans;

namespace fa.Tests
{
    [TestClass]
    public class FA1Tests
    {
        private FA1 fa1 = null!;
        
        [TestInitialize]
        public void Setup()
        {
            fa1 = new FA1();
        }
        
        // Положительные тесты для FA1 (должны приниматься)
        [TestMethod]
        public void FA1_Accept_01()
        {
            Assert.IsTrue(fa1.Run("01") == true);
        }
        
        [TestMethod]
        public void FA1_Accept_10()
        {
            Assert.IsTrue(fa1.Run("10") == true);
        }
        
        [TestMethod]
        public void FA1_Accept_011()
        {
            Assert.IsTrue(fa1.Run("011") == true);
        }
        
        [TestMethod]
        public void FA1_Accept_101()
        {
            Assert.IsTrue(fa1.Run("101") == true);
        }
        
        [TestMethod]
        public void FA1_Accept_1101()
        {
            Assert.IsTrue(fa1.Run("1101") == true);
        }
        
        [TestMethod]
        public void FA1_Accept_11111101()
        {
            Assert.IsTrue(fa1.Run("11111101") == true);
        }
        
        // Отрицательные тесты для FA1 (должны отвергаться)
        [TestMethod]
        public void FA1_Reject_NoZero()
        {
            Assert.IsFalse(fa1.Run("1") == true);
            Assert.IsFalse(fa1.Run("11") == true);
            Assert.IsFalse(fa1.Run("111") == true);
        }
        
        [TestMethod]
        public void FA1_Reject_NoOne()
        {
            Assert.IsFalse(fa1.Run("0") == true);
            Assert.IsFalse(fa1.Run("00") == true);
            Assert.IsFalse(fa1.Run("000") == true);
        }
        
        [TestMethod]
        public void FA1_Reject_TwoZeros()
        {
            Assert.IsFalse(fa1.Run("001") == true);
            Assert.IsFalse(fa1.Run("010") == true);
            Assert.IsFalse(fa1.Run("100") == true);
            Assert.IsFalse(fa1.Run("1010") == true);
        }
        
        [TestMethod]
        public void FA1_Reject_EmptyString()
        {
            Assert.IsFalse(fa1.Run("") == true);
        }
    }
    
    [TestClass]
    public class FA2Tests
    {
        private FA2 fa2 = null!;
        
        [TestInitialize]
        public void Setup()
        {
            fa2 = new FA2();
        }
        
        // Положительные тесты для FA2 (нечетное кол-во 0 и нечетное кол-во 1)
        [TestMethod]
        public void FA2_Accept_01()
        {
            // 0:1 шт (нечет), 1:1 шт (нечет)
            Assert.IsTrue(fa2.Run("01") == true);
        }
        
        [TestMethod]
        public void FA2_Accept_10()
        {
            // 0:1 шт (нечет), 1:1 шт (нечет)
            Assert.IsTrue(fa2.Run("10") == true);
        }
        
        [TestMethod]
        public void FA2_Accept_000111()
        {
            // 0:3 шт (нечет), 1:3 шт (нечет)
            Assert.IsTrue(fa2.Run("000111") == true);
        }
        
        [TestMethod]
        public void FA2_Accept_111000()
        {
            // 0:3 шт (нечет), 1:3 шт (нечет)
            Assert.IsTrue(fa2.Run("111000") == true);
        }
        
        [TestMethod]
        public void FA2_Accept_010101()
        {
            // 0:3 шт (нечет), 1:3 шт (нечет)
            Assert.IsTrue(fa2.Run("010101") == true);
        }
        
        [TestMethod]
        public void FA2_Accept_101010()
        {
            // 0:3 шт (нечет), 1:3 шт (нечет)
            Assert.IsTrue(fa2.Run("101010") == true);
        }
        
        [TestMethod]
        public void FA2_Accept_0()
        {
            // Только для проверки - должно быть false, так как нет нечетного количества 1
            Assert.IsFalse(fa2.Run("0") == true);
        }
        
        // Отрицательные тесты для FA2
        [TestMethod]
        public void FA2_Reject_EvenZerosEvenOnes()
        {
            // 0:2 шт (чет), 1:2 шт (чет)
            Assert.IsFalse(fa2.Run("0011") == true);
            Assert.IsFalse(fa2.Run("1100") == true);
            Assert.IsFalse(fa2.Run("0101") == true);
            Assert.IsFalse(fa2.Run("1010") == true);
        }
        
        [TestMethod]
        public void FA2_Reject_OddZerosEvenOnes()
        {
            // 0:1 шт (нечет), 1:2 шт (чет)
            Assert.IsFalse(fa2.Run("011") == true);
            // 0:3 шт (нечет), 1:2 шт (чет)
            Assert.IsFalse(fa2.Run("00011") == true);
        }
        
        [TestMethod]
        public void FA2_Reject_EvenZerosOddOnes()
        {
            // 0:2 шт (чет), 1:1 шт (нечет)
            Assert.IsFalse(fa2.Run("001") == true);
            // 0:2 шт (чет), 1:3 шт (нечет)
            Assert.IsFalse(fa2.Run("00111") == true);
        }
        
        [TestMethod]
        public void FA2_Reject_EmptyString()
        {
            Assert.IsFalse(fa2.Run("") == true);
        }
    }
    
    [TestClass]
    public class FA3Tests
    {
        private FA3 fa3 = null!;
        
        [TestInitialize]
        public void Setup()
        {
            fa3 = new FA3();
        }
        
        // Положительные тесты для FA3 (содержат "11")
        [TestMethod]
        public void FA3_Accept_11()
        {
            Assert.IsTrue(fa3.Run("11") == true);
        }
        
        [TestMethod]
        public void FA3_Accept_011()
        {
            Assert.IsTrue(fa3.Run("011") == true);
        }
        
        [TestMethod]
        public void FA3_Accept_110()
        {
            Assert.IsTrue(fa3.Run("110") == true);
        }
        
        [TestMethod]
        public void FA3_Accept_1011()
        {
            Assert.IsTrue(fa3.Run("1011") == true);
        }
        
        [TestMethod]
        public void FA3_Accept_111()
        {
            Assert.IsTrue(fa3.Run("111") == true);
        }
        
        [TestMethod]
        public void FA3_Accept_0110()
        {
            Assert.IsTrue(fa3.Run("0110") == true);
        }
        
        [TestMethod]
        public void FA3_Accept_10110()
        {
            Assert.IsTrue(fa3.Run("10110") == true);
        }
        
        // Отрицательные тесты для FA3 (не содержат "11")
        [TestMethod]
        public void FA3_Reject_No11()
        {
            Assert.IsFalse(fa3.Run("1") == true);
            Assert.IsFalse(fa3.Run("0") == true);
            Assert.IsFalse(fa3.Run("10") == true);
            Assert.IsFalse(fa3.Run("01") == true);
            Assert.IsFalse(fa3.Run("101") == true);
            Assert.IsFalse(fa3.Run("010") == true);
            Assert.IsFalse(fa3.Run("10101") == true);
        }
        
        [TestMethod]
        public void FA3_Reject_EmptyString()
        {
            Assert.IsFalse(fa3.Run("") == true);
        }
    }
}
