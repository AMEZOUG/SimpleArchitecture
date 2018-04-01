using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleArchitectureDAL.Infrastructure;
using SimpleArchitectureDAL.Repositories;
using SimpleArchitectureWeb.Mappings;

namespace SimpleArchitectureTests.Controllers.Property
{
    [TestClass]
    public abstract class BaseTests<T> where T : class
    {
        protected Mock<IUnitOfWork> moqUnitOfWork;
        protected Mock<IGenericRepository<T>> moqRepository;        

        [TestInitialize]
        public void Setup()
        {
            AutoMapperConfiguration.Configure();
            moqUnitOfWork = new Mock<IUnitOfWork>();
            moqRepository = new Mock<IGenericRepository<T>>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            moqUnitOfWork = null;
            moqRepository = null;
            AutoMapper.Mapper.Reset();
        }
    }
}
