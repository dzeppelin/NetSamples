using Microsoft.EntityFrameworkCore;
using WebApplication5.DatabaseLayer;
using TestProject.Stubs;
using WebApplication5.RepositoryLayer;
using WebApplication5.Exceptions;

namespace TestProject;

[TestClass]
public class AcademicClassRepositoryTests
{
    private PostgresContext _inMemoryDbContext;
    private AcademicClassRepository _AcademicClassRepository;

    [TestInitialize]
    public void TestInitialize()
    {
        DbContextOptions<PostgresContext> dbContextOptions = new DbContextOptionsBuilder<PostgresContext>().UseInMemoryDatabase("TestDb").Options;

        _inMemoryDbContext = new PostgresContextStub(dbContextOptions);

        _AcademicClassRepository = new AcademicClassRepository(_inMemoryDbContext);
        Assert.IsNotNull(_AcademicClassRepository);
    }

    [TestMethod]
    public async Task CreateClassSuccess()
    {
        await _AcademicClassRepository.CreateClass(classID: 1, studentID: 0);
        AcademicClass academicClass = _inMemoryDbContext.AcademicClasses.First();

        Assert.IsNotNull(academicClass);
        Assert.AreEqual(1, academicClass.Id);
    }

    [TestMethod]
    [DataRow(-1, 0)]
    [DataRow(0, -1)]
    [DataRow(-1, -1)]
    [ExpectedException(typeof(ArgumentException))]
    public async Task CreateClassFailureInvalidInputs(int customerID, int classID)
    {
        await _AcademicClassRepository.CreateClass(customerID, classID);
    }

    [TestMethod]
    [ExpectedException(typeof(CouldNotAddStudentToDatabaseException))]
    public async Task CreateStudentFailureDatabaseError()
    {
        await _AcademicClassRepository.CreateClass(classID: 0, studentID: 1);
    }
}
