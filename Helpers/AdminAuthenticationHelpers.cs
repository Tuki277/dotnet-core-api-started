using System;
using System.Collections.Generic;
using System.Linq;
using APIStarted;
using MongoDB.Driver;
using APIStarted.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace APIStarted.Helpers
{
    public class AdminAuthenticationHelpers
    {
        private readonly IMongoCollection<Members> _member;
        public AdminAuthenticationHelpers (IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _member = database.GetCollection<Members>(settings.MemberCollection);
        }

        public int CheckPermission(string id) 
        {
            int permission = -1;
            try
            {
                var idAccount = id;
                var memberDB = _member.Find(x  => true).ToList();
                var query = (
                    from member in memberDB
                    where member.Id == idAccount
                    select new Members {
                        Id = member.Id,
                        Role = member.Role
                    }
                ).ToList();
                if ( query[0].Role == 0)
                {
                    permission = 0;
                }
                else {
                    permission = 1;
                }
            }
            catch (System.Exception ex)
            {
                // json.Message = ex.Message;
                // json.Code = false;
            }
            return permission;
        }
    }
}