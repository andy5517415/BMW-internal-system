using System;
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
    public class BusinessOrderDetailsController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public BusinessOrderDetailsController(MSIT44Context context)
        {
            _context = context;
        }

        //找到第一筆訂單細項的OdId
        // GET: api/BusinessOrderDetails/fatherandson/25
        [HttpGet("fatherandson/{ordid}")]
        public int GetOdIdfatherandson(int ordid)
        {
            var q = _context.BusinessOrderDetails
                .Where(a => a.OrderId == ordid)
                .Select(x => x.OdId)
                .First();
                    
            return Convert.ToInt32(q);
        }



        //自己寫的
        // GET: api/BusinessOrderDetails/25/1
        [HttpGet("{ordid}/{oplid}")]
        public async Task<ActionResult<dynamic>> GetOdId(int ordid, int oplid)
        {
            var q = from od in _context.BusinessOrderDetails
                    where od.OrderId == ordid && od.OptionalId == oplid
                    select od.OdId;
            return await q.SingleOrDefaultAsync();
        }


        //新增父子資料
        // POST: api/BusinessOrderDetails
        [HttpPost]
        public string PostOrderDetail(int OrderId , [FromBody] BusinessOrderDetail bod)
        {
            if (!_context.BusinessOrderDetails.Any(a => a.OrderId == OrderId))
            {
                return "沒有該筆資料";
            }

            BusinessOrderDetail insert = new BusinessOrderDetail
            {
                OrderId = bod.OrderId,
                OptionalId = bod.OptionalId
            };

            _context.BusinessOrderDetails.Add(insert);
            _context.SaveChanges();

            return "orderdetail小表新增成功";
        }






        //修改子資料(目前未成功)(已在order控制器作掉)
        // PUT: api/BusinessOrderDetails
        [HttpPut]
        public string PutOrderDetail(int OrderId, [FromBody] BusinessOrderDetail bodput)
        {
            if (!_context.BusinessOrderDetails.Any(a => a.OrderId == OrderId))
            {
                return "沒有該筆資料";
            }

            BusinessOrderDetail update = new BusinessOrderDetail
            {
                OrderId = bodput.OrderId,
                OptionalId = bodput.OptionalId
            };

            _context.BusinessOrderDetails.Update(update);
            _context.SaveChanges();

            return "orderdetail小表修改成功";
        }








        ////網路做法，foreach
        //// PUT: api/BusinessOrderDetails/25/3
        //[HttpPut("{ordid}/{oplid}")]
        //public async Task<ActionResult<dynamic>> PutOrderDetail(int ordid, int oplid)
        //{


        //    var data = _context.BusinessOrderDetails
        //        .Where(od => od.OrderId == ordid);

        //    foreach (var item in data)
        //    {
        //        item.OptionalId = oplid;
        //        _context.BusinessOrderDetails.Update(item);
        //    }
        //    await _context.SaveChangesAsync();


        //    //if (id != businessOrderDetail.OdId)
        //    //{
        //    //    return BadRequest();
        //    //}

        //    //_context.Entry(businessOrderDetail).State = EntityState.Modified;

        //    //try
        //    //{
        //    //    await _context.SaveChangesAsync();
        //    //}
        //    //catch (DbUpdateConcurrencyException)
        //    //{
        //    //    if (!BusinessOrderDetailExists(id))
        //    //    {
        //    //        return NotFound();
        //    //    }
        //    //    else
        //    //    {
        //    //        throw;
        //    //    }
        //    //}

        //    return data.ToList();
        //}





        //刪除父子資料
        // Delete: api/BusinessOrderDetails/order/X021674138417
        [HttpDelete("order/{ordernum}")]
        public string DeleteOrder(string ordernum)
        {

            var ord = (from bo in _context.BusinessOrders
                          where bo.OrderNumber == ordernum
                          select bo).SingleOrDefault();

            //先判斷是否已接單
            if (ord.IsAccepted==true)
            {
                return "已接單開始生產，無法刪除";
            }
            else
            {
                //先刪除訂單小表
                var detail = from bod in _context.BusinessOrderDetails
                              join bo in _context.BusinessOrders on bod.OrderId equals bo.OrderId
                              where bo.OrderNumber == ordernum
                              select bod;
                _context.BusinessOrderDetails.RemoveRange(detail.ToList());
                _context.SaveChanges();

                //再刪除訂單大表
                _context.BusinessOrders.Remove(ord);
                _context.SaveChanges();
                return $"訂單編號 : {ordernum} 刪除成功!";
            }
        }













        // GET: api/BusinessOrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusinessOrderDetail>>> GetBusinessOrderDetails()
        {
            return await _context.BusinessOrderDetails.ToListAsync();
        }

        // GET: api/BusinessOrderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessOrderDetail>> GetBusinessOrderDetail(int id)
        {
            var businessOrderDetail = await _context.BusinessOrderDetails.FindAsync(id);

            if (businessOrderDetail == null)
            {
                return NotFound();
            }

            return businessOrderDetail;
        }

        // PUT: api/BusinessOrderDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBusinessOrderDetail(int id, BusinessOrderDetail businessOrderDetail)
        {
            if (id != businessOrderDetail.OdId)
            {
                return BadRequest();
            }

            _context.Entry(businessOrderDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessOrderDetailExists(id))
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

        // POST: api/BusinessOrderDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BusinessOrderDetail>> PostBusinessOrderDetail(BusinessOrderDetail businessOrderDetail)
        {
            _context.BusinessOrderDetails.Add(businessOrderDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBusinessOrderDetail", new { id = businessOrderDetail.OdId }, businessOrderDetail);
        }

        // DELETE: api/BusinessOrderDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusinessOrderDetail(int id)
        {
            var businessOrderDetail = await _context.BusinessOrderDetails.FindAsync(id);
            if (businessOrderDetail == null)
            {
                return NotFound();
            }

            _context.BusinessOrderDetails.Remove(businessOrderDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BusinessOrderDetailExists(int id)
        {
            return _context.BusinessOrderDetails.Any(e => e.OdId == id);
        }
    }
}
