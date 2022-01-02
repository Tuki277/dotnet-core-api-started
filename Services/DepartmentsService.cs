using APIStarted.Models;
using Microsoft.AspNetCore.Builder;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace APIStarted.Services
{
    public class DepartmentsService
    {
        private readonly IMongoCollection<Departments> _department;
        public DepartmentsService (IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _department = database.GetCollection<Departments>(settings.DepartmentCollection);
        }

        // public List<Departments> Get() => _department.Find(dep => true).ToList();
        public List<Departments> Get ()
            => _department.Find(dep => true).ToList();

        public List<Departments> Get(string id)
            => _department.Find<Departments>(department => department.Id == id).ToList();
        public Departments GetToRemove (string id)
            => _department.FindOneAndDelete<Departments>(department => department.Id == id);
        public Departments Create (Departments department)
        {
            _department.InsertOne(department);
            return department;
        }
        public void Update(string id, Departments departmentsIn) 
            => _department.ReplaceOne(department => department.Id == id, departmentsIn);
        public void Remove(Departments departments) 
            => _department.DeleteOne(departments => departments.Id == departments.Id);
        public void Remove(string id) => _department.DeleteOne(department => department.Id == id);
    }
}