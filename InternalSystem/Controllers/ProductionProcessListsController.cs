﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternalSystem.Models;


namespace InternalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionProcessListsController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public ProductionProcessListsController(MSIT44Context context)
        {
            _context = context;
        }

        //車子型號
        // GET: api/ProductionProcessLists/model
        [HttpGet("model")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetModel()
        {
            var processList = from BO in this._context.BusinessOptionals
                              join BC in this._context.BusinessCategories on BO.CategoryId equals BC.CategoryId
                              where BC.CategoryId == 1
                              select new
                              {
                                  OptionalId = BO.OptionalId,
                                  OptionalName = BO.OptionalName
                              };

            return await processList.ToListAsync();
        }

        //製程單
        // GET: api/ProductionProcessLists/process
        [HttpGet("process")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetProcess()
        {
            var processList = from PP in this._context.ProductionProcesses
                              select PP;

            return await processList.ToListAsync();
        }

        //報工的大表單
        // GET: api/ProductionProcessLists/Processor/{id}/{carid}
        [HttpGet("Processor/{id}/{carid}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetProductionProcessLists(int id, int carid)
        {
            var List = from PPL in this._context.ProductionProcessLists
                       join PA in this._context.ProductionAreas on PPL.AreaId equals PA.AreaId
                       join PP in this._context.ProductionProcesses on PPL.ProcessId equals PP.ProcessId
                       join PPSN in this._context.ProductionProcessStatusNames on PPL.StatusId equals PPSN.StatusId
                       join BO in this._context.BusinessOrders on PPL.OrderId equals BO.OrderId
                       join BOD in this._context.BusinessOrderDetails on BO.OrderId equals BOD.OrderId
                       join BOT in this._context.BusinessOptionals on BOD.OptionalId equals BOT.OptionalId
                       join BC in this._context.BusinessCategories on BOT.CategoryId equals BC.CategoryId
                       where BC.CategoryId == 1 && PPSN.StatusId == 2 && PP.ProcessId == id && BOT.OptionalId == carid
                       select new
                       {
                           OrderId = PPL.OrderId,
                           OrderNumber = BO.OrderNumber,
                           AreaId = PPL.AreaId,
                           AreaName = PA.AreaName,
                           ProcessId = PP.ProcessId,
                           ProcessName = PP.ProcessName,
                           StarDate = PPL.StarDate.ToString(),
                           OptionalId = BOT.OptionalId,
                           OptionalName = BOT.OptionalName,
                           StatusId = PPL.StatusId,
                           StatusName = PPSN.StatusName,

                       };

            return await List.ToListAsync();
        }

        // 現場主管代辦事項
        // GET: api/ProductionProcessLists/BusinessOrderProcessor
        [HttpGet("BusinessOrderProcessor")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetBusinessOrderTodo()
        {
            var List = from BO in this._context.BusinessOrders
                       join BOD in this._context.BusinessOrderDetails on BO.OrderId equals BOD.OrderId
                       join BOT in this._context.BusinessOptionals on BOD.OptionalId equals BOT.OptionalId
                       join BC in this._context.BusinessCategories on BOT.CategoryId equals BC.CategoryId
                       where BO.IsAccepted == false && BC.CategoryId == 1
                       select new
                       {
                           OrderId = BO.OrderId,
                           OrderNumber = BO.OrderNumber,
                           OptionalId = BOT.OptionalId,
                           OptionalName = BOT.OptionalName,
                           IsAccepted = BO.IsAccepted
                       };

            return await List.ToListAsync();
        }

        //訂單內容
        // GET: api/ProductionProcessLists/BusinessOrderProcessor
        [HttpGet("BusinessOrderProcessor/{id}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetBusinessOrder(int id)
        {
            var List = from BO in this._context.BusinessOrders          
                       join BOD in this._context.BusinessOrderDetails on BO.OrderId equals BOD.OrderId
                       join BOT in this._context.BusinessOptionals on BOD.OptionalId equals BOT.OptionalId
                       join BC in this._context.BusinessCategories on BOT.CategoryId equals BC.CategoryId
                       join BA in this._context.BusinessAreas on BO.AreaId equals BA.AreaId
                       where BO.IsAccepted == false && BO.OrderId == id  
                       select new
                       {
                           OrderId = BO.OrderId,
                           OrderNumber = BO.OrderNumber,
                           OptionalId = BOT.OptionalId,
                           OptionalName = BOT.OptionalName,
                           Price = BOT.Price
                       };

            return await List.ToListAsync();
        }


        // GET: api/ProductionProcessLists
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<ProductionProcessList>>> GetProductionProcessLists()
        //{
        //    return await _context.ProductionProcessLists.ToListAsync();
        //}

        // GET: api/ProductionProcessLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductionProcessList>> GetProductionProcessList(int id)
        {
            var productionProcessList = await _context.ProductionProcessLists.FindAsync(id);

            if (productionProcessList == null)
            {
                return NotFound();
            }

            return productionProcessList;
        }

        // PUT: api/ProductionProcessLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductionProcessList(int id, ProductionProcessList productionProcessList)
        {
            if (id != productionProcessList.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(productionProcessList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductionProcessListExists(id))
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

        // POST: api/ProductionProcessLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductionProcessList>> PostProductionProcessList(ProductionProcessList productionProcessList)
        {
            _context.ProductionProcessLists.Add(productionProcessList);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductionProcessListExists(productionProcessList.OrderId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProductionProcessList", new { id = productionProcessList.OrderId }, productionProcessList);
        }

        // DELETE: api/ProductionProcessLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductionProcessList(int id)
        {
            var productionProcessList = await _context.ProductionProcessLists.FindAsync(id);
            if (productionProcessList == null)
            {
                return NotFound();
            }

            _context.ProductionProcessLists.Remove(productionProcessList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductionProcessListExists(int id)
        {
            return _context.ProductionProcessLists.Any(e => e.OrderId == id);
        }
    }
}
