using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Iso810.Entities;
using Microsoft.AspNetCore.Http;
using System.IO;
using Iso810.Services.Contracts;

namespace Iso810.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly MainContext _context;
        private readonly ICsvService _csvService;

        public ApiController(MainContext context, ICsvService csvService)
        {
            _context = context;
            _csvService = csvService;
        }
        
        [HttpGet]
        public async Task<IEnumerable<StudentsView>> Get()
        {
            return await _context.StudentsView.ToListAsync();
        }

        [HttpPost("upload")]
        public async Task<IEnumerable<StudentsView>> Upload(IFormFile file)
        {
            try
            {
                StreamReader reader = new StreamReader(file.OpenReadStream());
                string content = await reader.ReadToEndAsync();
                return await _csvService.UploadData(content);
            }
            catch (Exception ex)
            {
                throw new Exception("An error has been detected.", ex);
            }
        }

        [HttpGet("download")]
        public async Task<object> Download()
        {
            try
            {
                var header = _csvService.GetHeaders();
                var data = await _csvService.DownloadData();
                return new { header = header, data = data };
            }
            catch (Exception ex)
            {
                throw new Exception("An error has been detected.", ex);
            }
        }
    }
}