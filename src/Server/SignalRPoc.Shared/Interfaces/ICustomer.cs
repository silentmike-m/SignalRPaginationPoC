namespace SignalRPoc.Shared.Interfaces;

public interface ICustomer
{
    string FirstName { get; set; }
    string LastName { get; set; }
    string Company { get; set; }
    string Address { get; set; }
    string City { get; set; }
    string County { get; set; }
    string Phone { get; set; }
    string Email { get; set; }
}