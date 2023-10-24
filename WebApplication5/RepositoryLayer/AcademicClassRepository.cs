using WebApplication5.DatabaseLayer;
using WebApplication5.Exceptions;

namespace WebApplication5.RepositoryLayer;

public class AcademicClassRepository
{
    private readonly PostgresContext _context;

    public AcademicClassRepository(PostgresContext context)
    {
        _context = context;
    }

    public async Task CreateClass(int classID, int studentID)
    {
        if (studentID < 0 || classID < 0)
        {
            Console.WriteLine($"Argument Exception in AddClassToStudent! studentID = {studentID}, classID = {classID}");
            throw new ArgumentException("Invalid arguments provided");
        }

        AcademicClass tempClass = new AcademicClass()
        {
            Id = classID,
            ClassName = classID.ToString(),
        };

        try
        {
            _context.AcademicClasses.Add(tempClass);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during database query: {ex.Message}");
            throw new CouldNotAddStudentToDatabaseException();
        }
    }
}
