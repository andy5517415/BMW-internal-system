using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternalSystem.Models;
using System.Timers;
using System;

namespace InternalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionContextsController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public ProductionContextsController(MSIT44Context context)
        {
            _context = context;
        }

        //報工內容清單
        //GET: api/ProductionContexts/ContextsList
        [HttpGet("ContextsList")]
        public async Task<ActionResult<dynamic>> GetContextsList(string empNumber , string empName , string starDate  )
        {
            //TimeSpan TS = Convert.ToDateTime(productionContext.Date).Subtract(Convert.ToDateTime(starDate));
            //double dd = TS.TotalDays;
            //TimeSpan ES = Convert.ToDateTime(endDate).Subtract(Convert.ToDateTime(productionContext.Date));
            //double ff = ES.TotalDays;
            var query = from PC in this._context.ProductionContexts
                        join PPD in this._context.PersonnelProfileDetails on PC.EmployeeId equals PPD.EmployeeId
                        join PPL in this._context.ProductionProcessLists on PC.OrderId equals PPL.OrderId
                        join PD in this._context.PersonnelDepartmentLists on PPD.DepartmentId equals PD.DepartmentId
                        join BO in this._context.BusinessOrders on PPL.OrderId equals BO.OrderId
                        orderby PC.Date descending
                        //where dd >= 0 && ff <=0
                        select new
                        {
                            EmployeeId = PC.EmployeeId,
                            EmployeeName = PPD.EmployeeName,
                            EmployeeNumber = PPD.EmployeeNumber,
                            OrderId = PC.OrderId,
                            Date = PC.Date.ToString(),
                            PD.DepName,
                            BO.OrderNumber
                        };
           
            if (!string.IsNullOrEmpty(empNumber))
            {
                query = query.Where(a => a.EmployeeNumber.Contains(empNumber));
            }
            if (!string.IsNullOrEmpty(empName))
            {
                query = query.Where(a => a.EmployeeName.Contains(empName));
            }
            if (!string.IsNullOrEmpty(starDate))
            {
                query = query.Where(a => a.Date.Contains(starDate));
            }
            




            return await query.ToListAsync();
        }

        //報工內容檢查/修改
        //GET: api/ProductionContexts/ContextsCheck/25/1/2022-01-01
        [HttpGet("ContextsCheck/{orderid}/{empid}/{date}")]
        public async Task<ActionResult<dynamic>> GetContextsCheck(int orderid , int empid , string date)
        {
            var query = from PC in this._context.ProductionContexts
                        join PPD in this._context.PersonnelProfileDetails on PC.EmployeeId equals PPD.EmployeeId
                        join PPL in this._context.ProductionProcessLists on PC.OrderId equals PPL.OrderId
                        join PD in this._context.PersonnelDepartmentLists on PPD.DepartmentId equals PD.DepartmentId
                        join BO in this._context.BusinessOrders on PPL.OrderId equals BO.OrderId
                        join PP in this._context.ProductionProcesses on PC.ProcessId equals PP.ProcessId
                        join PA in this._context.ProductionAreas on PC.AreaId equals PA.AreaId
                        join BOD in this._context.BusinessOrderDetails on BO.OrderId equals BOD.OrderId
                        join BOP in this._context.BusinessOptionals on BOD.OptionalId equals BOP.OptionalId
                        join BC in this._context.BusinessCategories on BOP.CategoryId equals BC.CategoryId
                        where PC.OrderId == orderid && PC.EmployeeId == empid && PC.Date.ToString() == date && BOP.CategoryId == 1
                        select new
                        {
                            OrderId = PC.OrderId,
                            EmployeeId = PC.EmployeeId,
                            EmployeeName = PPD.EmployeeName,
                            EmployeeNumber = PPD.EmployeeNumber,
                            PD.DepName,
                            ProcessName = PP.ProcessName,
                            AreaName = PA.AreaName,
                            Date = PC.Date.ToString(),
                            StartTime = PC.StartTime,
                            EndTime = PC.EndTime,
                            Context = PC.Context,
                            BO.OrderNumber,
                            OptionalId = BOP.OptionalId,
                            OptionalName = BOP.OptionalName
                        };



            return await query.FirstOrDefaultAsync();
        }

        //報工內容
        //GET: api/ProductionContexts/Contexts
        [HttpGet("Contexts")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetContexts()
        {
            var query = from PC in this._context.ProductionContexts
                        join PPD in this._context.PersonnelProfileDetails on PC.EmployeeId equals PPD.EmployeeId
                        join PPL in this._context.ProductionProcessLists on PC.OrderId equals PPL.OrderId                      
                        select new
                        {
                            OrderId = PC.OrderId,
                            EmployeeId  = PC.EmployeeId,
                            EmployeeName = PPD.EmployeeName,
                            Date = PC.Date.ToString(),
                            //StartTime = PC.StartTime.ToString(),
                            //EndTime = PC.EndTime.ToString(),
                            Context = PC.Context
                        };
                       


            return await query.ToListAsync();
        }

        // GET: api/ProductionContexts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductionContext>>> GetProductionContexts()
        {
            return await _context.ProductionContexts.ToListAsync();
        }

        // GET: api/ProductionContexts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductionContext>> GetProductionContext(int id)
        {
            var productionContext = await _context.ProductionContexts.FindAsync(id);

            if (productionContext == null)
            {
                return NotFound();
            }

            return productionContext;
        }

        // PUT: api/ProductionContexts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductionContext(int id, ProductionContext productionContext)
        {
            if (id != productionContext.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(productionContext).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductionContextExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductionContexts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductionContext>> PostProductionContext(ProductionContext productionContext)
        {
            _context.ProductionContexts.Add(productionContext);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductionContextExists(productionContext.OrderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProductionContext", new { id = productionContext.OrderId }, productionContext);
        }

        // DELETE: api/ProductionContexts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductionContext(int id)
        {
            var productionContext = await _context.ProductionContexts.FindAsync(id);
            if (productionContext == null)
            {
                return NotFound();
            }

            _context.ProductionContexts.Remove(productionContext);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductionContextExists(int id)
        {
            return _context.ProductionContexts.Any(e => e.OrderId == id);
        }
    }
}
