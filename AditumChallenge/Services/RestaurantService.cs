using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Driver;

using AditumChallenge.Models;
using AditumChallenge.Interfaces;
using System.Data;
using Csv;
using Newtonsoft.Json;

namespace AditumChallenge.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IMongoCollection<Restaurant> _restaurants;

        public RestaurantService()
        {
            var client = new MongoClient("mongodb+srv://testDB:Yjyhagw8XcNA95us@aditumchallenge.kjtlz.mongodb.net/AditumChallenge?retryWrites=true&w=majority");
            var database = client.GetDatabase("Restaurants");
            _restaurants = database.GetCollection<Restaurant>("Restaurant");
        }

        public Restaurant Create(Restaurant  restaurant)
        {
            try
            {
                _restaurants.InsertOne(restaurant);
            }
            catch
            {
                return null;
            }

            return restaurant;
        }

        public bool ConvertCSV(List<ICsvLine> csvlines)
        {
            for (int i = 0; i < csvlines.Count; i++)
            {
                var line = csvlines[i];
                var splitNameTime = line.ToString().Split(',');
                var Times = splitNameTime[1].Split("-");

                var Name = splitNameTime[0];
                var OpenHour = Times[0];
                var CloseHour = Times[1];

                Restaurant restaurant = new Restaurant();
                restaurant.Name = Name;
                restaurant.openHour = OpenHour;
                restaurant.closeHour = CloseHour;


                string json = JsonConvert.SerializeObject(restaurant);
                Console.WriteLine(json);
                _restaurants.InsertOne(restaurant);
            }
            return true;
        }

        public Restaurant GetById(string id) => _restaurants.Find(restaurant => restaurant.Id == id).FirstOrDefault();

        public List<Restaurant> Get() => _restaurants.Find(restaurant => true).ToList();

        public List<Restaurant> GetByHour(string hour)
        {
            var parsedHour = TimeSpan.ParseExact(hour, "h\\:mm", null);
            var filteredRestaurants = new List<Restaurant>();
            List<Restaurant> rests = _restaurants.Find(x => true).ToList();
            for (int i = 0; i < rests.Count; i++)
            {
                var rest = rests[i];
                if (TimeSpan.ParseExact(rest.openHour, "h\\:mm", null) <= parsedHour && TimeSpan.ParseExact(rest.closeHour, "h\\:mm", null) >= parsedHour)
                {
                    filteredRestaurants.Add(rest);
                }
            }
            return filteredRestaurants;
        }

        public void Update(string id, Restaurant restaurantIn) => _restaurants.ReplaceOne(restaurant => restaurant.Id == id, restaurantIn);

        public void Remove(string id) => _restaurants.DeleteOne(restaurant => restaurant.Id == id);

        public void RemoveAll() => _restaurants.DeleteMany(Builders<Restaurant>.Filter.Empty);
    }

}

