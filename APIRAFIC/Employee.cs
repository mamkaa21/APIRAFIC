using System;
using System.Collections.Generic;

namespace APIRAFIC;

public partial class Employee
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public sbyte? IsBlocked { get; set; }

    public DateTime? Lastlogin { get; set; }

    public virtual Role? Role { get; set; }
}
public partial class EmployeeModel
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public sbyte? IsBlocked { get; set; }

    public DateTime? Lastlogin { get; set; }

    public static explicit operator Employee(EmployeeModel model)
    {
        return new Employee
        {

            Id = model.Id,
            Login = model.Login,
            Password = model.Password,
            RoleId = model.RoleId,
            RegistrationDate = model.RegistrationDate,
            IsBlocked = model.IsBlocked,
            Lastlogin = model.Lastlogin
        };
    }

    public static explicit operator EmployeeModel(Employee model)
    {
        return new EmployeeModel
        {

            Id = model.Id,
            Login = model.Login,
            Password = model.Password,
            RoleId = model.RoleId,
            RegistrationDate = model.RegistrationDate,
            IsBlocked = model.IsBlocked,
            Lastlogin = model.Lastlogin
        };
    }
}

