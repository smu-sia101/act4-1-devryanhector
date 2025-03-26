dsusing Microsoft.AspNetCore.Mvc;

namespace PetNamerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetNamerController : ControllerBase
    {
        private static readonly Dictionary<string, List<string>> petNames = new()
        {
            { "dog", new List<string> { "Zephyr", "Tundra", "Onyx", "Jinx", "Quasar" } },
            { "cat", new List<string> { "Nebula", "Saffron", "Zelda", "Orion", "Fable" } },
            { "bird", new List<string> { "Solstice", "Nimbus", "Echo", "Lyric", "Vortex" } }
        };

        [HttpPost]
        public IActionResult Generate([FromBody] PetNamerRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.AnimalType))
            {
                return BadRequest(new { error = "The 'animalType' field is required." });
            }

            string animalType = request.AnimalType.ToLower();

            if (!petNames.ContainsKey(animalType))
            {
                return BadRequest(new { error = "Invalid animal type. Allowed values: dog, cat, bird." });
            }

            if (request.TwoPart.HasValue && request.TwoPart.GetType() != typeof(bool))
            {
                return BadRequest(new { error = "The 'twoPart' field must be a boolean (true or false)." });
            }

            Random rnd = new Random();
            List<string> selectedNames = petNames[animalType];
            string generatedName = selectedNames[rnd.Next(selectedNames.Count)];

            if (request.TwoPart == true)
            {
                string secondName = selectedNames[rnd.Next(selectedNames.Count)];
                generatedName = generatedName + secondName;
            }

            return Ok(new { name = generatedName });
        }
    }

    public class PetNamerRequest
    {
        public string AnimalType { get; set; } = string.Empty;
        public bool? TwoPart { get; set; }
    }
}
