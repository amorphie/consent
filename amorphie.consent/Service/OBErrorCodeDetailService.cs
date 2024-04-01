using amorphie.consent.core.Model;
using amorphie.consent.data;
using amorphie.consent.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace amorphie.consent.Service;

public class OBErrorCodeDetailService : IOBErrorCodeDetailService
{
    private readonly ConsentDbContext _context;
    private readonly ILogger<OBErrorCodeDetailService> _logger;

    public OBErrorCodeDetailService(ConsentDbContext context,
        ILogger<OBErrorCodeDetailService> logger)
    {
        _context = context;
        _logger = logger;
    }


    public async Task<List<OBErrorCodeDetail>> GetErrorCodeDetailsAsync()
    {
        return await _context.OBErrorCodeDetails.AsNoTracking().ToListAsync();
    }
}