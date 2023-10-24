using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApplication5.DatabaseLayer;
using WebApplication5.Exceptions;

namespace WebApplication5.RepositoryLayer;

public class StudentRepository
{
    private readonly PostgresContext _dbContext;

    public StudentRepository(PostgresContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CreateStudent(Student student)
    {
        if (student == null) return false;

        if (isInvalidStudent(student)) return false;

        try
        {
            using (_dbContext)
            {
                _dbContext.Students.Add(student);
                await _dbContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }

        return true;
    }

    public async Task<Student> GetStudentByFullName(string fullName)
    {
        if (isInvalidFullName(fullName)) throw new StudentNotFoundException();
        
        return await _dbContext.Students.FirstOrDefaultAsync(student => student.FirstName + " " + student.LastName == fullName) ?? throw new StudentNotFoundException();
    }

    private bool isInvalidStudent(Student student)
    {
        char[] forbiddenCharacters = { '!', '@', '#', '$', '%', '&', '*', };
        
        // Here we check if any of the Student string properties are empty or null and if they contain invalid characters
        foreach (PropertyInfo pi in student.GetType().GetProperties())
        {
            if(pi.PropertyType == typeof(string))
            {
                string? value = (string?)pi.GetValue(student);

                if (string.IsNullOrEmpty(value))
                {
                    return true;
                }

                if (pi.Name == "Email")
                {
                    continue;
                }

                if (value.Any(x => forbiddenCharacters.Contains(x)))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool isInvalidFullName(string fullName)
    {
        char[] forbiddenCharacters = { '!', '@', '#', '$', '%', '&', '*', };

        if (string.IsNullOrEmpty(fullName))
        {
            return true;
        }

        if (fullName.Any(x => forbiddenCharacters.Contains(x)))
        {
            return true;
        }

        return false;
    }
}
