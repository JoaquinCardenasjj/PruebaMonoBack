using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaMongoJoaquinCardenas.Modelos
{
    public class Facturas
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("CodigoFactura")]
        public string CodigoFactura { get; set; }
        [BsonElement("Cliente")]
        public string Cliente { get; set; }
        [BsonElement("Ciudad")]
        public string Ciudad { get; set; }
        [BsonElement("Nit")]
        public string Nit { get; set; }
        [BsonElement("TotalFactura")]
        public string TotalFactura { get; set; }
        [BsonElement("SubTotal")]
        public string SubTotal { get; set; }
        [BsonElement("Iva")]
        public string Iva { get; set; }
        [BsonElement("Retencion")]
        public string Retencion { get; set; }
        [BsonElement("FechaCreacion")]
        public string FechaCreacion { get; set; }
        [BsonElement("Estado")]
        public string Estado { get; set; }
        [BsonElement("Pagada")]
        public string Pagada { get; set; }
        [BsonElement("FechaPago")]
        public string FechaPago { get; set; }
    }
}
