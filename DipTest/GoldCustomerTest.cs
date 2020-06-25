using System;
using System.Security.Cryptography.X509Certificates;
using Castle.Core.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SolidDemo.DIP.Customers;
using SolidDemo.DIP.Logger;
using SolidDemo.Validation;

namespace DipTest
{
    [TestClass]
    public class GoldCustomerTest
    {
        [TestMethod]
        public void Constructor_Throws_Exception_WhilePassingNullParams()
        {
            //Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new GoldCustomer(null, null));
        }

        [TestMethod]
        public void Constructor_Throws_Exception_WhilePassingLoggerAsNull()
        {
            //Arrange
            var mockValidation = new Mock<IValidation>();

            //Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new GoldCustomer(null, mockValidation.Object));
        }

        [TestMethod]
        public void Constructor_Throws_Exception_WhilePassingValidatorAsNull()
        {
            //Arrange
           
            var mockLogger = new Mock<SolidDemo.DIP.Logger.ILogger>();

            //Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new GoldCustomer(mockLogger.Object, null));
        }

        [TestMethod]
        public void Constructor_Pass_WhilePassingValidParams()
        {
            //Arrange
            var mockLogger = new Mock<SolidDemo.DIP.Logger.ILogger>();
            var mockValidation = new Mock<IValidation>();

            //Act & Assert
            _= new GoldCustomer(mockLogger.Object, mockValidation.Object);
        }

        [TestMethod]
        public void Add_Throws_Exception_WhilePassingNullParams()
        {
            //Arrange
            var mockLogger = new Mock<SolidDemo.DIP.Logger.ILogger>();
            var mockValidation = new Mock<IValidation>();
            mockValidation.Setup(x => x.Validate(It.IsAny<string>()))
                          .Callback(() => throw new ArgumentNullException());
            var customer = new GoldCustomer(mockLogger.Object, mockValidation.Object);

            //Act
            customer.Add(null);

            //Assert
            mockValidation.Verify(x => x.Validate(It.IsAny<string>()), Times.Exactly(1));
            mockLogger.Verify(x => x.Log(It.IsAny<string>()), Times.Exactly(1));
        }

        [TestMethod]
        public void Add_Pass_WhilePassingValidParams()
        {
            //Arrange
            var mockLogger = new Mock<SolidDemo.DIP.Logger.ILogger>();
            var mockValidation = new Mock<IValidation>();
            mockValidation.Setup(x => x.Validate(It.IsAny<string>())).Verifiable();
            var customer = new GoldCustomer(mockLogger.Object, mockValidation.Object);

            //Act
            customer.Add("Name");

            //Assert
            mockValidation.Verify(x => x.Validate(It.IsAny<string>()), Times.Exactly(1));
            mockLogger.Verify(x => x.Log(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void CalculateDiscount_Throws_Exception_WhilePassingInvaidAmount()
        {
            //Arrange
            var mockLogger = new Mock<SolidDemo.DIP.Logger.ILogger>();
            var mockValidation = new Mock<IValidation>();           
            var customer = new GoldCustomer(mockLogger.Object, mockValidation.Object);

            //Act&&Assert
            Assert.ThrowsException<ArgumentException>(() => customer.CalculateDiscount(0));
        }

        [TestMethod]
        public void CalculateDiscount_Pass_WhilePassingVaidAmount()
        {
            //Arrange
            var mockLogger = new Mock<SolidDemo.DIP.Logger.ILogger>();
            var mockValidation = new Mock<IValidation>();
            var customer = new GoldCustomer(mockLogger.Object, mockValidation.Object);
            var amount = 1000.00M;
            var expectedAmount = 800.00M;

            //Act
            var actualDiscountAmount = customer.CalculateDiscount(amount);

            //Assert
            Assert.AreEqual(expectedAmount, actualDiscountAmount);
        }
    }
}
