// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");


//create objects of the Customer class
using CustomerApp.Models;
/*
 Tightly coupld code
Customer customer = new Customer
{
    CustomerId = Faker.RandomNumber.Next(100,10000),
    Name = new FullName { FirstName = Faker.Name.First(), LastName =Faker.Name.Last() },
    Address = new Address
    {
        DoorNo = Faker.Company.BS(),
        Street = Faker.Address.StreetSuffix(),
        City = Faker.Address.CitySuffix(),
        State = Faker.Address.CitySuffix(),
        ZipCode = Faker.Address.ZipCode()
    },
    Email = Faker.Internet.Email(),
    Password = Faker.Identification.UsPassportNumber(),
    PhoneNumber = Faker.Phone.Number()

};

Console.WriteLine($"Customer ID: {customer.CustomerId}");   
Console.WriteLine($"Customer Name: {customer.Name.FirstName} {customer.Name.LastName}");
Console.WriteLine($"Customer Address: {customer.Address.DoorNo}, {customer.Address.Street}, {customer.Address.City}, {customer.Address.State} - {customer.Address.ZipCode}");
Console.WriteLine($"Customer Email: {customer.Email}");
Console.WriteLine($"Customer Phone Number: {customer.PhoneNumber}");
Console.WriteLine($"Customer Password: {customer.Password}");
Console.WriteLine("--------------------------------------------------");

Console.ReadKey();
*/