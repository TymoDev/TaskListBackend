using Aplication.Core.Models.TaskModel;
using Core.ResultModels;

namespace CoreTests
{
    public class TaskModelTests
    {
        [SetUp]
        public void Setup()
        {
                     
        }

        [Test]
        public void Test1()
        {
            //Arrange
            string validTaskName = "TestTask1";
            string validTaskStatus = "ValidTaskStatus";
            string notValidTaskName = "";
            string notValidTaskStatus = "";
            //Act
            var shoudlBeValidResult = TaskModel.Create(validTaskName,validTaskStatus);
            var shoudlBeNotValidResult = TaskModel.Create(notValidTaskName, notValidTaskStatus);
            //Assert
            Assert.AreEqual(true,shoudlBeValidResult.Success);
            Assert.AreEqual(false, shoudlBeNotValidResult.Success);
        }
    }
}