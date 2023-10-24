using Microsoft.EntityFrameworkCore;
using WebApplication5.DatabaseLayer;
using WebApplication5.Exceptions;
using WebApplication5.RepositoryLayer;

namespace TestProject;

[TestClass]
public class StudentRepositoryTests
{
    private PostgresContext _inMemoryDbContext = null!;
    private StudentRepository _studentRepository = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        DbContextOptions<PostgresContext> dbContextOptions = new DbContextOptionsBuilder<PostgresContext>().UseInMemoryDatabase("TestDb").Options;

        _inMemoryDbContext = new PostgresContext(dbContextOptions);
        Assert.IsNotNull(_inMemoryDbContext);

        _studentRepository = new StudentRepository(_inMemoryDbContext);
        Assert.IsNotNull(_studentRepository);
    }

    [TestMethod]
    public async Task CreateStudentSuccess()
    {
        Student student = new Student()
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@email.com",
            Gender = "Male",
            IpAddress = "192.168.0.1",
        };
        bool result = await _studentRepository.CreateStudent(student);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task CreateStudentFailureNull()
    {
        Student student = null!;
        bool result = await _studentRepository.CreateStudent(student);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task CreateStudentFailureInvalidEmptyProperties()
    {
        Student student = new Student()
        {
            Id = 2,
            Email = "",
            FirstName = "",
            Gender = "",
            IpAddress = "",
            LastName = "",
        };
        bool result = await _studentRepository.CreateStudent(student);
        Assert.IsFalse(result);
    }

    [TestMethod]
    [DataRow('#')]
    [DataRow('$')]
    [DataRow('%')]
    [DataRow('&')]
    [DataRow('*')]
    public async Task CreateStudentFailureInvalidCharacters(char invalidCharacter)
    {
        Student student = new Student()
        {
            Id = 3,
            Email = "email@email.com",
            FirstName = "Tom" + invalidCharacter,
            Gender = "Male",
            IpAddress = "192.168.0.1",
            LastName = "Neiderman",
        };
        bool result = await _studentRepository.CreateStudent(student);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task GetStudentByNameSuccess()
    {
        // (448, 'Neila', 'Morrish', 'nmorrishcf@mit.edu', 'Female', '68.238.226.97')
        Student testStudent = new Student()
        {
            Id = 448,
            FirstName = "Neila",
            LastName = "Morrish",
            Email = "nmorrishcf@mit.edu",
            Gender = "Female",
            IpAddress = "68.238.226.97",
        };
        _inMemoryDbContext.Students.Add(testStudent);
        await _inMemoryDbContext.SaveChangesAsync();

        // Checking if the objects we got is an equivalent of what we saved
        Student receivedStudent = await _studentRepository.GetStudentByFullName("Neila Morrish");
        Assert.AreEqual(testStudent, receivedStudent);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [DataRow("#")]
    [DataRow("$")]
    [DataRow("%")]
    [DataRow("&")]
    [DataRow("*")]
    [ExpectedException(typeof(StudentNotFoundException))]
    public async Task GetStudentByFullNameFailure(string invalidValue)
    {
        await _studentRepository.GetStudentByFullName(invalidValue);
    }
}
