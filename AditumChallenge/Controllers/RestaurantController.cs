using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Csv;

using AditumChallenge.Models;
using AditumChallenge.Interfaces;

namespace AditumChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : Controller
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }
        

        [HttpGet("csv")]
        public ActionResult<List<Restaurant>> Get() => _restaurantService.Get();

        [HttpGet("{id:length(24)}", Name = "GetRestaurant")]
        public ActionResult<Restaurant> GetById(string id)
        {
            var restaurant = _restaurantService.GetById(id);

            if (restaurant == null)
                return NotFound();

            return restaurant;
        }

        
        [HttpGet("filter")]
        public ActionResult<List<Restaurant>> GetByHour(string hour)
        {
            var restaurants = _restaurantService.GetByHour(hour);
            return restaurants;
        }

        [HttpPost]
        public ActionResult<Restaurant> Create(Restaurant restaurant)
        {
            _restaurantService.Create(restaurant);
            return CreatedAtRoute("GetRestaurant", new { id = restaurant.Id.ToString() }, restaurant);
        }

        [HttpPost("csv")]
        [Consumes("multipart/form-data")]
        public IActionResult PostFile([FromForm(Name = "file")] IFormFile file)
        {
            
            var csvlines = CsvReader.ReadFromStream(file.OpenReadStream()).ToList();
            if (file.FileName.EndsWith(".csv"))
            {
                try
                {
                    _restaurantService.ConvertCSV(csvlines);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return Content("Ocorreu um erro ao armazenar os dados!");
                }
            }
            else
            {
                return Content("Formato de arquivo inválido!");
            }
            return Content("Solicitação aprovada com sucesso!");
        }

        [HttpPut]
        public IActionResult Update(string id, Restaurant restaurantIn)
        {
            var restaurant = _restaurantService.GetById(id);

            if (restaurant == null)
                return NotFound();

            _restaurantService.Update(id, restaurantIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var restaurant = _restaurantService.GetById(id);

            if (restaurant == null)
                return NotFound();

            _restaurantService.Remove(restaurant.Id);

            return NoContent();
        }

        [HttpDelete("delete-all")]
        public IActionResult DeleteMany()
        {
            _restaurantService.RemoveAll();

            return NoContent();
        }

    }
}