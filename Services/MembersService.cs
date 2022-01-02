using APIStarted.Models;
using Microsoft.AspNetCore.Builder;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APIStarted.Services
{
    public class MembersService
    {
        private readonly IMongoCollection<Departments> _departments;
        private readonly IMongoCollection<Members> _members;
        private readonly IMongoCollection<Positions> _positions;
        public MembersService (IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _departments = database.GetCollection<Departments>(settings.DepartmentCollection);
            _members = database.GetCollection<Members>(settings.MemberCollection);
            _positions = database.GetCollection<Positions>(settings.PositionCollection);
        }

        // public List<Departments> Get() => _department.Find(dep => true).ToList();
        public List<Members> Get ()
        {
            var departmentDB = _departments.Find(departments => true).ToList();
            var memberDB = _members.Find(members => true).ToList();
            var positionDB = _positions.Find(departments => true).ToList();
            var query = (
                            from member in memberDB
                            join department in departmentDB on member.Department equals department.Id
                            join position in positionDB on member.Position equals position.Id
                            select new Members {
                                Id = member.Id,
                                Name = member.Name,
                                Username = member.Username,
                                Password = member.Password,
                                Created = member.Created,
                                Address = member.Address,
                                PhoneNumber = member.PhoneNumber,
                                Role = member.Role,
                                Department = department.Name,
                                Position = position.Name
                            }
                        ).ToList();
            return query;
        }

        public List<Members> Get(string id)
        {
            var idParameter = id;
            var departmentDB = _departments.Find(departments => true).ToList();
            var memberDB = _members.Find(members => true).ToList();
            var positionDB = _positions.Find(departments => true).ToList();
            var query = (
                            from member in memberDB
                            join department in departmentDB on member.Department equals department.Id
                            join position in positionDB on member.Position equals position.Name
                            where member.Id == idParameter
                            select new Members {
                                Id = member.Id,
                                Name = member.Name,
                                Username = member.Username,
                                Password = member.Password,
                                Created = member.Created,
                                Address = member.Address,
                                PhoneNumber = member.PhoneNumber,
                                Role = member.Role,
                                Department = department.Name,
                                Position = position.Name
                            }
                        ).ToList();
            return query;
        }
        public Members GetToRemove (string id)
            => _members.FindOneAndDelete<Members>(members => members.Id == id);
        public bool Login (Account account)
        {
            var login = false;
            var memberDB = _members.Find(x => x.Username == account.Username && x.Password == account.Password).ToList();
            if (memberDB.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Members Create (Members members)
        {
            if (members.Password == null)
            {
                members.Password = "abc@1230-=";
            }
            if ( members.Role.ToString() == null)
            {
                members.Role = 1;
            }
            if ( members.Created == null)
            {
                members.Created = DateTime.Now.ToString();
            }
            if ( members.Position == null)
            {
                members.Position = "No information";
            }
            if ( members.Department == null)
            {
                members.Department = "No information";
            }
            _members.InsertOne(members);
            return members;
        }
        public void Update(string id, Members membersIn) 
            => _members.ReplaceOne(department => department.Id == id, membersIn);
        public void Remove(Members members) 
            => _members.DeleteOne(members => members.Id == members.Id);
        public void Remove(string id) => _members.DeleteOne(member => member.Id == id);
    }
}