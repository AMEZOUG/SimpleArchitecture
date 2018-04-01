using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleArchitectureCore.DTO;
using SimpleArchitectureEntities.Models;
using SimpleArchitectureWeb.Controllers.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace SimpleArchitectureTests.Controllers.Property
{
    [TestClass]
    public class GroupTest : BaseTests<Group>
    {
        List<Group> groups;
        GroupDto groupDto;

        [TestInitialize]
        public void Setup()
        {
            base.Setup();
            groupDto = new GroupDto { IdGroup = 1, Label = "test_group 1" };
            groups = new List<Group>()
                {
                    new Group {IdGroup = 1, Label = "test_group 1" },
                    new Group { IdGroup = 2,Label = "test_group 2"}
                };
        }

        [TestCleanup]
        public void Cleanup()
        {
            base.Cleanup();
        }

        /// <summary>
        /// Actions of group controller
        /// </summary>
        /// <returns></returns>

        #region Get
        [TestMethod]
        public async Task Group_Get_Verify_GetAllAsyncIsCalledOnce()
        {
            // Arrange
            moqRepository.Setup(g => g.GetAllAsync(null)).ReturnsAsync(groups).Verifiable();
            moqUnitOfWork.Setup(x => x.Repository<Group>()).Returns(moqRepository.Object);

            var controller = new GroupController(moqUnitOfWork.Object);

            // Act
            var actionResult = await controller.Get();

            // Assert
            moqRepository.Verify(m => m.GetAllAsync(null), Times.Once);
            moqUnitOfWork.Verify(x => x.Repository<Group>(), Times.Once);
        }

        [TestMethod]
        public async Task Group_Get_Should_Return_InstanceOfTypeOkObjectResultIEnumerableGroupDto()
        {
            // Arrange
            moqRepository.Setup(g => g.GetAllAsync(null)).ReturnsAsync(groups).Verifiable();
            moqUnitOfWork.Setup(x => x.Repository<Group>()).Returns(moqRepository.Object);

            var controller = new GroupController(moqUnitOfWork.Object);

            // Act
            var actionResult = await controller.Get() as OkObjectResult;

            // Assert
            Assert.IsInstanceOfType(actionResult.Value, typeof(IEnumerable<GroupDto>));
        }

        [TestMethod]
        public async Task Group_Get_Should_Return_ListGroupWithSameLenght()
        {
            // Arrange
            moqRepository.Setup(g => g.GetAllAsync(null)).ReturnsAsync(groups).Verifiable();
            moqUnitOfWork.Setup(x => x.Repository<Group>()).Returns(moqRepository.Object);

            var controller = new GroupController(moqUnitOfWork.Object);

            // Act
            var contentResult = await controller.Get() as OkObjectResult;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Value);
            Assert.AreEqual(2, ((IEnumerable<GroupDto>)contentResult.Value).Count());
        }

        [TestMethod]
        public async Task Group_Get_Should_Return_InstanceOfTypeNotFoundResult()
        {
            // Arrange
            var listGroup = new List<Group>();
            moqRepository.Setup(g => g.GetAllAsync(null)).ReturnsAsync(listGroup).Verifiable();
            moqUnitOfWork.Setup(x => x.Repository<Group>()).Returns(moqRepository.Object);

            var controller = new GroupController(moqUnitOfWork.Object);

            // Act
            IActionResult actionResult = await controller.Get();

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }
        #endregion

        #region Get by id
        [TestMethod]
        public async Task Group_GetById_Verify_GetAsyncIsCalledOnce()
        {
            // Arrange
            int id = 1;
            moqRepository.Setup(g => g.GetAsync(It.IsAny<Expression<Func<Group, bool>>>()))
                .ReturnsAsync((Expression<Func<Group, bool>> predicate) => { return groups.FirstOrDefault(x => x.IdGroup == id) as Group; });

            moqUnitOfWork.Setup(x => x.Repository<Group>()).Returns(moqRepository.Object);

            var controller = new GroupController(moqUnitOfWork.Object);

            // Act
            var actionResult = await controller.Get(id);

            // Assert
            moqUnitOfWork.Verify(x => x.Repository<Group>(), Times.Once);
            moqRepository.Verify(x => x.GetAsync(It.IsAny<Expression<Func<Group, bool>>>()), Times.Once);
        }

        [TestMethod]
        public async Task Group_GetById_Should_Return_OkObjectResultGroupDto()
        {
            // Arrange
            int id = 1;
            moqRepository.Setup(g => g.GetAsync(It.IsAny<Expression<Func<Group, bool>>>()))
            .ReturnsAsync((Expression<Func<Group, bool>> predicate) => { return groups.FirstOrDefault(x => x.IdGroup == id); });
            //.ReturnsAsync(new Func<Expression<Func<Group, bool>>, Group>(expr => groups.Where(expr.Compile()).FirstOrDefault(x => x.IdGroup == id) as Group));

            moqUnitOfWork.Setup(x => x.Repository<Group>()).Returns(moqRepository.Object);

            var controller = new GroupController(moqUnitOfWork.Object);

            // Act
            var actionResult = await controller.Get(id) as OkObjectResult;

            // Assert
            Assert.IsInstanceOfType(actionResult.Value, typeof(GroupDto));
        }
        [TestMethod]
        public async Task Group_GetById_Should_Return_GroupWithSameId()
        {
            // Arrange
            int id = 1;
            moqRepository.Setup(g => g.GetAsync(It.IsAny<Expression<Func<Group, bool>>>()))
                              .ReturnsAsync((Expression<Func<Group, bool>> predicate) => { return groups.FirstOrDefault(x => x.IdGroup == id); });

            moqUnitOfWork.Setup(x => x.Repository<Group>()).Returns(moqRepository.Object);

            var controller = new GroupController(moqUnitOfWork.Object);

            // Act
            var actionResult = await controller.Get(id);
            var contentResult = actionResult as OkObjectResult;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(1, ((GroupDto)contentResult.Value).IdGroup);
        }

        [TestMethod]
        public async Task Group_GetById_Should_Return_InstanceOfTypeNotFoundResult()
        {
            // Arrange
            int id = 3;
            moqRepository.Setup(g => g.GetAsync(It.IsAny<Expression<Func<Group, bool>>>()))
                              .ReturnsAsync((Expression<Func<Group, bool>> predicate) =>
                                                { return groups.FirstOrDefault(x => x.IdGroup == id); });
            moqUnitOfWork.Setup(x => x.Repository<Group>()).Returns(moqRepository.Object);

            var controller = new GroupController(moqUnitOfWork.Object);

            // Act
            var actionResult = await controller.Get(id);
            var contentResult = actionResult as NotFoundResult;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }
        #endregion

        #region Post       
        [TestMethod]
        public async Task Group_Post_Verify_MethodSetsLocationHeader()
        {
            // Arrange
            moqUnitOfWork.Setup(x => x.Repository<Group>()).Returns(moqRepository.Object);
            moqUnitOfWork.Setup(x => x.SaveChangesAsyn()).ReturnsAsync(1);

            var controller = new GroupController(moqUnitOfWork.Object);

            // Act
            var actionResult = await controller.Post(groupDto);
            var createdResult = actionResult as CreatedAtRouteResult;// CreatedAtRouteNegotiatedContentResult<GroupDto>;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("GetGroup", createdResult.RouteName);
            Assert.AreEqual(1, createdResult.RouteValues["id"]);
        }

        [TestMethod]
        public async Task Group_Post_Should_Return_Exception()
        {
            // Arrange
            Exception expectedException = null;
            string expectedExceptionMessage = "Creating a group failed on save.";
            moqUnitOfWork.Setup(x => x.Repository<Group>()).Returns(moqRepository.Object);
            moqUnitOfWork.Setup(x => x.SaveChangesAsyn()).ReturnsAsync(0);

            var controller = new GroupController(moqUnitOfWork.Object);

            // Act
            try
            {
                var actionResult = await controller.Post(groupDto);
            }
            catch (Exception ex)
            {
                expectedException = ex;
            }

            // Assert
            Assert.IsNotNull(expectedException);
            Assert.AreEqual(expectedExceptionMessage, expectedException.Message);
        }

        #endregion

        #region Put        
        [TestMethod]
        public async Task Group_Put_Should_Return_InstanceOfTypeBadRequest()
        {
            // Arrange
            int id = 2;
            moqRepository.Setup(g => g.Get(It.IsAny<Func<Group, bool>>())).Returns(groups.FirstOrDefault());
            moqUnitOfWork.Setup(x => x.Repository<Group>()).Returns(moqRepository.Object);

            var controller = new GroupController(moqUnitOfWork.Object);

            // Act
            var actionResult = await controller.Put(id, groupDto);
            var createdResult = actionResult as BadRequestResult;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task Group_Put_Should_Return_InstanceOfTypeNotFound()
        {
            // Arrange
            int id = 1;
            moqRepository.Setup(g => g.Get(It.IsAny<Func<Group, bool>>())).Returns(null as Group);
            moqUnitOfWork.Setup(x => x.Repository<Group>()).Returns(moqRepository.Object);

            var controller = new GroupController(moqUnitOfWork.Object);

            // Act
            var actionResult = await controller.Put(id, groupDto);
            var createdResult = actionResult as NotFoundResult;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Group_Put_Should_Return_InstanceOfTypeNoContentResult()
        {
            // Arrange
            int id = 1;
            moqRepository.Setup(g => g.Get(It.IsAny<Func<Group, bool>>())).Returns(groups.FirstOrDefault());
            moqUnitOfWork.Setup(x => x.Repository<Group>()).Returns(moqRepository.Object);
            moqUnitOfWork.Setup(x => x.SaveChangesAsyn()).ReturnsAsync(1);

            var controller = new GroupController(moqUnitOfWork.Object);

            // Act
            var actionResult = await controller.Put(id, groupDto) as NoContentResult;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task Group_Put_Should_Return_Exception()
        {
            // Arrange
            int id = 1;
            Exception expectedException = null;
            string expectedExceptionMessage = "Updating a group failed on save.";
            moqRepository.Setup(g => g.Get(It.IsAny<Func<Group, bool>>())).Returns(groups.FirstOrDefault());
            moqUnitOfWork.Setup(x => x.Repository<Group>()).Returns(moqRepository.Object);
            moqUnitOfWork.Setup(x => x.SaveChangesAsyn()).ReturnsAsync(0);

            var controller = new GroupController(moqUnitOfWork.Object);

            // Act
            try
            {
                var actionResult = await controller.Put(id, groupDto);
            }
            catch (Exception ex)
            {
                expectedException = ex;
            }

            // Assert
            Assert.IsNotNull(expectedException);
            Assert.AreEqual(expectedExceptionMessage, expectedException.Message);
        }
        #endregion

        #region Delete      
        [TestMethod]
        public async Task Group_Delete_Should_Return_InstanceOfTypeNotFound()
        {
            // Arrange
            int id = 1;
            moqRepository.Setup(g => g.Get(It.IsAny<Func<Group, bool>>())).Returns(null as Group);
            moqUnitOfWork.Setup(x => x.Repository<Group>()).Returns(moqRepository.Object);

            var controller = new GroupController(moqUnitOfWork.Object);

            // Act
            var actionResult = await controller.Delete(id);
            var createdResult = actionResult as NotFoundResult;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Group_Delete_Should_Return_InstanceOfTypeNoContentResultGroupDto()
        {
            // Arrange
            int id = 1;
            moqRepository.Setup(g => g.Get(It.IsAny<Func<Group, bool>>())).Returns(groups.FirstOrDefault());
            moqUnitOfWork.Setup(x => x.Repository<Group>()).Returns(moqRepository.Object);
            moqUnitOfWork.Setup(x => x.SaveChangesAsyn()).ReturnsAsync(1);

            var controller = new GroupController(moqUnitOfWork.Object);

            // Act
            var actionResult = await controller.Delete(id) as NoContentResult;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task Group_Delete_Should_Return_Exception()
        {
            // Arrange
            int id = 1;
            Exception expectedException = null;
            string expectedExceptionMessage = "Deleting a group failed on save.";
            moqRepository.Setup(g => g.Get(It.IsAny<Func<Group, bool>>())).Returns(groups.FirstOrDefault());
            moqUnitOfWork.Setup(x => x.Repository<Group>()).Returns(moqRepository.Object);
            moqUnitOfWork.Setup(x => x.SaveChangesAsyn()).ReturnsAsync(0);

            var controller = new GroupController(moqUnitOfWork.Object);

            // Act
            try
            {
                var actionResult = await controller.Delete(id);
            }
            catch (Exception ex)
            {
                expectedException = ex;
            }

            // Assert
            Assert.IsNotNull(expectedException);
            Assert.AreEqual(expectedExceptionMessage, expectedException.Message);
        }
        #endregion
    }
}
