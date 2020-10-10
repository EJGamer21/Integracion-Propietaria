using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Iso810.Entities;

namespace Iso810.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly MainContext _context;

        public ApiController(MainContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<IEnumerable<StudentsView>> Get()
        {
            return await _context.StudentsView.ToListAsync();
        }
    }
}