using AditumChallenge.Models;
using Csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AditumChallenge.Interfaces
{
    public interface IRestaurantService
    {
        Restaurant Create(Restaurant restaurant);
        bool ConvertCSV(List<ICsvLine> csvlines);
        List<Restaurant> Get(); 
        List<Restaurant> GetByHour(string hour);
        Restaurant GetById(string id);
        void Update(string id, Restaurant restaurantIn);
        void Remove(string id);
        void RemoveAll();        
    }
}