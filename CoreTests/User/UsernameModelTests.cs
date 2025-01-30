using Aplication.Core.Models.TaskModel;
using Core.ValidationModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTests
{
    internal class UsernameModelTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void UserModelUsernameValidationTests()
        {
            //Arrange
            string validUsername = "TestTask1";
            string notValidUsername1 = "1";
            string notValidUsername2 = "TestTask1TestTask1TestTask1TestTask1TestTask1";
            //Act
            var resultShouldBeValid =UsernameModel.Create(validUsername);
            var resultShouldBeNotValid1 = UsernameModel.Create(notValidUsername1);
            var resultShouldBeNotValid2 = UsernameModel.Create(notValidUsername2);
            //Assert
            Assert.AreEqual(true, resultShouldBeValid.Success);
            Assert.AreEqual(false, resultShouldBeNotValid1.Success);
            Assert.AreEqual(false, resultShouldBeNotValid2.Success);

        }
    }
}

