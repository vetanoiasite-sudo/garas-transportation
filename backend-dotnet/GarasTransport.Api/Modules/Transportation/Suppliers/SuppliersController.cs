using GarasTransport.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace GarasTransport.Api.Modules.Transportation.Suppliers;

// Base path /api/Transportation (§6.12). Every action requires CompanyName +
// UserToken headers via the filter.
[Route("api/Transportation")]
[HeaderAuth]
public class SuppliersController : ApiControllerBase
{
    private readonly SuppliersService _service;

    public SuppliersController(SuppliersService service) => _service = service;

    [HttpGet("getSuppliers")]
    public Task<object> GetSuppliers() =>
        _service.GetSuppliers(HeaderInt("PageNo", 1), HeaderInt("NoOfItems", 20), HeaderDecoded("Name"), Header("Phone"), Header("Mobile"));

    [HttpPost("addSupplier")]
    public Task<object> AddSupplier([FromBody] SupplierBody body) => _service.AddSupplier(body ?? new());

    [HttpGet("getSupplier")]
    public Task<object> GetSupplier() => _service.GetSupplier(HeaderInt("Id", 0));

    [HttpPost("updateSupplier")]
    public Task<object> UpdateSupplier([FromBody] SupplierBody body) => _service.UpdateSupplier(body?.Id ?? 0, body ?? new());

    private AccountFilters AccountFilters() => new()
    {
        Year = HeaderIntOrNull("Year"),
        Month = HeaderIntOrNull("Month"),
        SupplierId = HeaderIntOrNull("SupplierId"),
        RouteId = HeaderIntOrNull("RouteId"),
    };

    [HttpGet("AccountsAllMonthsForSupplier")]
    public Task<object> AccountsAllMonths() =>
        _service.AccountsAllMonths(HeaderInt("PageNo", 1), HeaderInt("NoOfItems", 20), AccountFilters());

    [HttpGet("AccountsAllMonthsForSupplierExcel")]
    public Task<object> AccountsAllMonthsExcel() => _service.AccountsAllMonthsExcel(AccountFilters());

    [HttpGet("AccountsAllRoundsForSupplier")]
    public Task<object> AccountsAllRounds() =>
        _service.AccountsAllRounds(HeaderInt("PageNo", 1), HeaderInt("NoOfItems", 20), AccountFilters());

    [HttpGet("getAllSupplierPayment")]
    public Task<object> GetAllSupplierPayment() =>
        _service.GetAllSupplierPayment(HeaderInt("PageNo", 1), HeaderInt("NoOfItems", 20), Header("FromDate"), Header("ToDate"), HeaderIntOrNull("SupplierId"));

    [HttpPost("AddSupplierPayment")]
    public Task<object> AddSupplierPayment([FromBody] SupplierPaymentBody body) => _service.AddSupplierPayment(body ?? new());
}
