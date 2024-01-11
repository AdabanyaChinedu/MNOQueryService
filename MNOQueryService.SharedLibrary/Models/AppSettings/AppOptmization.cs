
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MNOQueryService.SharedLibrary.Model.AppSettings
{
    
    public class AppOptimization
    {
        public RedisSettings RedisSettings { get; set; }
    }

    public class RedisSettings
    {
        public bool OptimizeOperatorQuery {  get; set; }

        public int CacheTimeInMinute { get; set; }
    }
}
