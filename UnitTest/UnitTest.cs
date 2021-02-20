using System;
using Xunit;
using Moq;
using AditumChallenge.Interfaces;
using AditumChallenge.Models;
using AditumChallenge.Controllers;
using AditumChallenge.Services;
using Csv;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest
{
    public class UnitTest
    {       

        [Fact]
        public void CreateTest()
        {
            Mock<IRestaurantService> serviceM = new Mock<IRestaurantService>();
            Restaurant FakeRestaurant = new Restaurant();
            serviceM.Setup(x => x.Create(It.IsAny<Restaurant>())).Returns(FakeRestaurant);
                        
            FakeRestaurant.Name = "Bobs";
            FakeRestaurant.openHour = "09:00";
            FakeRestaurant.closeHour = "23:00";

            RestaurantService restServ = new RestaurantService();
            Restaurant op = restServ.Create(FakeRestaurant);

            Assert.Equal(FakeRestaurant.Name, op.Name);
            Assert.Equal(FakeRestaurant.openHour, op.openHour);
            Assert.Equal(FakeRestaurant.closeHour, op.closeHour);
        }

        [Fact]
        public void ConvertArchive()
        {
            Mock<IRestaurantService> serviceM = new Mock<IRestaurantService>();
            Restaurant FakeRestaurant = new Restaurant();
            serviceM.Setup(x => x.ConvertCSV(It.IsAny<List<ICsvLine>>())).Returns(true);

            var csvLines1 = CsvReader.ReadFromText("Bobs,09:00-23:00");
            var csvLines2 = CsvReader.ReadFromText("X-Touro,10:30-23:00");
            var csvLines3 = CsvReader.ReadFromText("Bar Da Breja,17:00-01:00");
            
            List<ICsvLine> list = csvLines1.ToList();
            list = csvLines2.ToList();
            list = csvLines3.ToList();

            RestaurantService restServ = new RestaurantService();

            bool op = restServ.ConvertCSV(list);
            Assert.True(op);
        }

        [Fact]
        public void GetUsingID()
        {
            Mock<IRestaurantService> serviceM = new Mock<IRestaurantService>();
            Restaurant FakeRestaurant = new Restaurant();
            serviceM.Setup(x => x.GetById(It.IsAny<string>())).Returns(FakeRestaurant);

            FakeRestaurant.Id = "60300c17110e645a38747e56";

            string iddoido = "60300c17110e645a38747e56";

            RestaurantService restServ = new RestaurantService();
            Restaurant op = restServ.GetById(iddoido);

            Assert.Equal(FakeRestaurant.Id, op.Id);
        }
        [Fact]
        public void GetUsingHour()
        {
            Mock<IRestaurantService> serviceM = new Mock<IRestaurantService>();
            List<Restaurant> FakesRestaurants = new List<Restaurant>();
            Restaurant FakeRestaurant = new Restaurant();
            serviceM.Setup(x => x.GetByHour(It.IsAny<string>())).Returns(FakesRestaurants);

            string hour = "12:00";

            RestaurantService restServ = new RestaurantService();
            List<Restaurant> op = restServ.GetByHour(hour);

            Assert.True(op.Count > 0);
        }
    }
}
