using MongoDB.Driver;
using PruebaMongoJoaquinCardenas.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaMongoJoaquinCardenas.Servicios
{
    public class ClienteService
    {

        private readonly IMongoCollection<Clientes> _Clientes;

        public ClienteService(IPruebaMonostoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _Clientes = database.GetCollection<Clientes>(settings.ClientesCollectionName);
        }

        public List<Clientes> Get() =>
            _Clientes.Find(Cliente => true).ToList();

        public Clientes Get(string id) =>
            _Clientes.Find<Clientes>(Cliente => Cliente.Id == id).FirstOrDefault();

        public Clientes Create(Clientes Cliente)
        {
            _Clientes.InsertOne(Cliente);
            return Cliente;
        }

        public void Update(string id, Clientes ClienteIn) =>
            _Clientes.ReplaceOne(Cliente => Cliente.Id == id, ClienteIn);

        public void Remove(Clientes ClienteIn) =>
            _Clientes.DeleteOne(Cliente => Cliente.Id == ClienteIn.Id);

        public void Remove(string id) =>
            _Clientes.DeleteOne(Cliente => Cliente.Id == id);
    }
}
