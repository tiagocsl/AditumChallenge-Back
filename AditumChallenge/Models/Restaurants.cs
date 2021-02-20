using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AditumChallenge.Models
{
    public class Restaurant
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id {get; set;}

        [Required]
        [BsonElement("Name")]
        public string Name {get; set;}
        
        [Required]
        public string openHour {get; set;}
        
        [Required]
        public string closeHour {get; set;}
    }
}