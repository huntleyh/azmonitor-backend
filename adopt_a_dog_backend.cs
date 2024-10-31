using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace contoso.sample
{
    public class adopt_a_dog_backend
    {
        private readonly ILogger<adopt_a_dog_backend> _logger;

        public adopt_a_dog_backend(ILogger<adopt_a_dog_backend> logger)
        {
            _logger = logger;
        }

        private static readonly List<Dog> Dogs = new List<Dog>
        {
            new Dog { Id = 1, Name = "Buddy", Description = "A friendly dog who loves to play and bring joy to your home." },
            new Dog { Id = 2, Name = "Bella", Description = "A playful companion who enjoys outdoor activities and keeps you active." },
            new Dog { Id = 3, Name = "Charlie", Description = "A curious dog who loves to explore and bring excitement to your life." },
            new Dog { Id = 4, Name = "Lucy", Description = "A loyal friend who provides comfort and security to your family." },
            new Dog { Id = 5, Name = "Max", Description = "An energetic dog who keeps you entertained and brings happiness to your home." },
            new Dog { Id = 6, Name = "Daisy", Description = "A gentle companion who offers love and affection to everyone." },
            new Dog { Id = 7, Name = "Rocky", Description = "A brave dog who protects your home and offers companionship." },
            new Dog { Id = 8, Name = "Luna", Description = "A smart dog who learns quickly and brings intelligence to your household." }
        };

        [Function("get_all_dogs")]
        public IActionResult get_all_dogs([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            return new OkObjectResult(Dogs);
        }

        [Function("get_dog_detail")]
        public async Task<IActionResult> get_dog_detail([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            // Add a delay (just for TESTING purposes)
            var currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (currentTimestamp % 2 != 0)
            {
                await Task.Delay(5000);
            }

            if (!int.TryParse(req.Query["id"], out int id))
            {
                return new BadRequestObjectResult("Please pass a valid dog ID on the query string.");
            }

            var dog = Dogs.FirstOrDefault(d => d.Id == id);
            if (dog == null)
            {
                return new NotFoundObjectResult("Dog not found.");
            }

            return new OkObjectResult(dog);
        }
    }
}
