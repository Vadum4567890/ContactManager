using ContactManager.Models;
using CsvHelper.Configuration;

namespace ContactManager.Mappers
{
    public sealed class ContactCsvMap : ClassMap<Contact>
    {
        public ContactCsvMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.DateOfBirth).Name("Date of Birth");
            Map(m => m.Married).Name("Married");
            Map(m => m.Phone).Name("Phone");
            Map(m => m.Salary).Name("Salary");
        }
    }
} 
