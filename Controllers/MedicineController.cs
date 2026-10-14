using Microsoft.AspNetCore.Mvc;
using trpo_6.Data;
using trpo_6.Models;

namespace trpo_6.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MedicineController : Controller
    {
        private readonly WebApiDatabaseContext _context;

        public MedicineController(WebApiDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] string? manufacturer, [FromQuery] bool? available)
        {
            var query = _context.Medicines.AsQueryable();

            if (available.HasValue)
                query = query.Where(m => m.IsAvailable == available);

            if (manufacturer != null)
                query = query.Where(m => m.Manufacturer == manufacturer);

            /*List<Medicine> medicines = new List<Medicine>();

            medicines = _context.Medicines.ToList();

            if (medicines.Count == 0)
                return NoContent();*/

            var result = query.ToList();

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            Medicine? medicine;

            if (id < 1)
                return BadRequest("Id должен быть больше 1");

            medicine = _context.Medicines.Find(id);

            if (medicine == null)
                return NotFound("Препарат не найден");

            return Ok(medicine);
        }

        [HttpPost]
        public IActionResult Create(Medicine medicine)
        {
            _context.Medicines.Add(medicine);
            _context.SaveChanges();

            return Created($"api/medicine/{medicine.Id}", medicine);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateFull(int id, Medicine newMedicine)
        {
            Medicine? medicine;

            if (id < 1)
                return BadRequest("Id должен быть больше 1");

            medicine = _context.Medicines.Find(id);

            if (medicine == null)
                return NotFound("Препарат не найден");

            if (newMedicine.Id != id)
                return BadRequest("Id должно совпадать с поиском");

            medicine.Name = newMedicine.Name;
            medicine.Manufacturer = newMedicine.Manufacturer;
            medicine.Price = newMedicine.Price;
            medicine.ExpirationDate = newMedicine.ExpirationDate;
            medicine.IsAvailable = newMedicine.IsAvailable;
            
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public IActionResult Update(int id, MedicineDto newMedicine)
        {
            Medicine? medicine;

            if (id < 1)
                return BadRequest("Id должен быть больше 1");

            medicine = _context.Medicines.Find(id);

            if (medicine == null)
                return NotFound("Препарат не найден");

            if (newMedicine.Id != id)
                return BadRequest("Id должно совпадать с поиском");

            if (newMedicine.Name is not null) medicine.Name = newMedicine.Name;
            if (newMedicine.Manufacturer is not null) medicine.Manufacturer = newMedicine.Manufacturer;
            if (newMedicine.Price is not null) medicine.Price = newMedicine.Price.Value;
            if (newMedicine.ExpirationDate is not null) medicine.ExpirationDate = newMedicine.ExpirationDate.Value;
            if (newMedicine.IsAvailable is not null) medicine.IsAvailable = newMedicine.IsAvailable.Value;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (id < 1)
                return BadRequest("Id должен быть больше 1");

            if (_context.Medicines.Find(id) is Medicine medicine)
            {
                _context.Medicines.Remove(medicine);
                _context.SaveChanges();
                return NoContent();
            }

            return NotFound("Препарат не найден");
        }
    }
}
