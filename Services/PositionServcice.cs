using APIStarted.Models;
using Microsoft.AspNetCore.Builder;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace APIStarted.Services
{
    public class PositionsService
    {
        private readonly IMongoCollection<Positions> _positions;
        public PositionsService (IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _positions = database.GetCollection<Positions>(settings.PositionCollection);
        }

        public List<Positions> Get ()
            => _positions.Find(dep => true).ToList();

        public List<Positions> Get(string id)
            => _positions.Find<Positions>(positions => positions.Id == id).ToList();
        public Positions GetToRemove (string id)
            => _positions.FindOneAndDelete<Positions>(positions => positions.Id == id);
        public Positions Create (Positions positions)
        {
            _positions.InsertOne(positions);
            return positions;
        }
        public void Update(string id, Positions PositionsIn) 
            => _positions.ReplaceOne(position => position.Id == id, PositionsIn);
        public void Remove(Positions Positions) 
            => _positions.DeleteOne(Positions => Positions.Id == Positions.Id);
        public void Remove(string id) => _positions.DeleteOne(position => position.Id == id);
    }
}