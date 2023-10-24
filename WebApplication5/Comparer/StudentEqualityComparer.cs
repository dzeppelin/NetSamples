using System.Security.Cryptography;
using WebApplication5.DatabaseLayer;
using WebApplication5.Exceptions;

namespace WebApplication5.Comparer;

internal class StudentEqualityComparer: EqualityComparer<Student>
{
    public override bool Equals(Student? x, Student? y)
    {
        // In case both of the values is null
        if (x == null && y == null) return true;
        // In case one of the values is null
        if (x == null || y == null) return false;

        // We use all the keys except FKs of any kind
        return x.Id == y.Id
            && x.FirstName == y.LastName
            && x.LastName == y.LastName
            && x.Email == y.Email
            && x.Gender == y.Gender
            && x.IpAddress == y.IpAddress;
    }

    public override int GetHashCode(Student obj)
    {
        if(obj == null) throw new NullValueException();

        int randomNumber = RandomNumberGenerator.GetInt32(int.MaxValue / 2);

        return (obj.Id + obj.FirstName.Length + randomNumber).GetHashCode();
    }
}
