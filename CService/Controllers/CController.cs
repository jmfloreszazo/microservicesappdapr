using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Object = Models.Object;

namespace CService.Controllers
{
    [ApiController]   
    public class CController : ControllerBase
    {
        private static readonly ObjectGroup[] og = new[]
        {
           new ObjectGroup
           {
               Id=new Guid("86c82a5e-fc10-4d85-b725-4dd5c14140fb"),
               Objects = new List<Object>{
                   new Object{Id=new Guid("4d32d3d9-002e-49bd-9e19-dde29db0b835"),Name="Object 1.1"},
                   new Object{Id=new Guid("56ee065e-b897-4571-a1dd-a16cc0405fef"),Name="Object 1.2"},
               }
           },
           new ObjectGroup
           {
               Id=new Guid("98c04dfd-94f3-4ec7-82bd-01aba17a4ce3"),
               Objects = new List<Object>{
                   new Object{Id=new Guid("b2b6d3ed-71af-4b3c-bd95-c15cf0dfced9"),Name="Object 2.1"}                   
               }
           }
        };

        private readonly ILogger<CController> _logger;

        public CController(ILogger<CController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            string s = id.ToString();
            _logger.LogInformation(s);
            _logger.LogInformation("checking for order {id}");
            var o = og.Where(x => x.Id.Equals(id));
            if (!o.Any())
            {
                _logger.LogInformation("order not found");
                return NotFound();
            }
                
            return Ok(o.FirstOrDefault());
        }
    }
}
