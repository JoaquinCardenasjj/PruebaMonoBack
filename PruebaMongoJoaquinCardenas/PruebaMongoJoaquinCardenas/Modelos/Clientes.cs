using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaMongoJoaquinCardenas.Modelos
{
    public class Clientes
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Documento")]
        public string Documento { get; set; }
        [BsonElement("Nombres")]
        public string Nombres { get; set; }
        [BsonElement("Apellidos")]
        public string Apellidos { get; set; }
        [BsonElement("Celular")]
        public string Celular { get; set; }
        [BsonElement("Correo")]
        public string Correo { get; set; }
        [BsonElement("Direccion")]
        public string Direccion { get; set; }

    }
}
