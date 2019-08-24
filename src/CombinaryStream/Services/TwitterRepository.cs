﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CombinaryStream.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CombinaryStream.Services
{
    public class TwitterRepository : IItemRepository
    {
        private readonly string _connectionString;
        private readonly int _limit;

        public TwitterRepository(IConfiguration configuration) {
            _connectionString = configuration["TwitterConnectionString"];
            _limit = int.Parse(configuration["TwitterLimit"] ?? "100");
        }
        
        public async Task<IEnumerable<StreamItem>> GetItemsAsync() {
            if(string.IsNullOrWhiteSpace(_connectionString)) return new StreamItem[0];

            const string query = @"
                select
                       t.url as ""Url"",
                       'twitter' as ""ItemType"",
                       '' as ""Title"",
                       t.text as ""Body"",
                       u.name || ' (@' || u.screen_name || ')' as ""AuthorName"",
                       'https://twitter.com/' || u.screen_name as ""AuthorUrl"",
                       ts as ""PublishedAt""

                from db.tweet t
                left join db.user u on t.user_id = u.id
                order by t.ts desc
                limit @limit
            ";

            using (var db = new NpgsqlConnection(_connectionString)) {
                return await db.QueryAsync<StreamItem>(query, param: new {limit = _limit});
            }
        }
    }
}
