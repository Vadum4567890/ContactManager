using ContactManager.Mappers;
using ContactManager.Models;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using System.Formats.Asn1;
using System.Globalization;

namespace ContactManager.Controllers
{
    public class ContactsController : Controller
    {
        private readonly MyDbContext _context;

        public ContactsController(MyDbContext context)
        {
            _context = context;
        }

        // Отримати всі контакти
        [HttpGet]
        public IActionResult Index()
        {
            var contacts = _context.Contacts.ToList();
            return View(contacts);
        }

        // Завантажити CSV
        [HttpPost]
        public async Task<IActionResult> UploadCSV(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return RedirectToAction("Index");
            }


            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvHelper.CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true
            }))
            {
                // Зареєструємо маппінг полів
                csv.Context.RegisterClassMap<ContactCsvMap>();
                
                // Зчитуємо дані з CSV і додаємо їх у базу даних
                var contacts = csv.GetRecords<Contact>().ToList();


                await _context.Contacts.AddRangeAsync(contacts);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
        // Додати контакт
        [HttpPost]
        public IActionResult AddContact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Contacts.Add(contact);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Видалити контакт
        [HttpPost]
        public IActionResult DeleteContact(int id)
        {
            var contact = _context.Contacts.Find(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var contact = _context.Contacts.Find(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }
        // Оновити контакт
        [HttpPost]
        public IActionResult Edit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Contacts.Update(contact);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit", contact);
        }
    }
}
