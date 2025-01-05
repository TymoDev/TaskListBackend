using Core.ValidationModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTests.User
{
    internal class UserPasswordModelTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            //Arrange
            string validPassword = "TestPas1123";
            string notValidUsername1 = "1";
            string notValidUsername2 = "000000000000000000000000000000000000000000000000000000000000";
            //Act
            var resultShouldBeValid = UserPasswordModel.Create(validPassword);
            var resultShouldBeNotValid1 = UserPasswordModel.Create(notValidUsername1);
            var resultShouldBeNotValid2 = UserPasswordModel.Create(notValidUsername2);
            //Assert
            Assert.AreEqual(true, resultShouldBeValid.Success);
            Assert.AreEqual("Weak password", resultShouldBeNotValid1.ErrorMessage);
            Assert.AreEqual("Password can not be longer than 50 sybols", resultShouldBeNotValid2.ErrorMessage);
        }
    }
}
