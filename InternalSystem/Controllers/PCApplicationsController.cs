using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternalSystem.Models;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.CodeAnalysis;

namespace InternalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PCApplicationsController : ControllerBase
    {
        private readonly MSIT44Context _context;

        public PCApplicationsController(MSIT44Context context)
        {
            _context = context;
        }

        //申請表
        //GET: api/PCApplications/applicationlist
        [HttpGet("applicationlist/{id}")]
        public async Task<ActionResult<dynamic>> GetApplicationsList(int id)
        {
            var list = from AP in this._context.PcApplications
                       join PD in this._context.PersonnelProfileDetails on AP.EmployeeId equals PD.EmployeeId
                       join PDL in this._context.PersonnelDepartmentLists on PD.DepartmentId equals PDL.DepartmentId
                       where PD.EmployeeId == id
                       select new
                       {
                           PurchaseId = AP.PurchaseId,
                           EmployeeId = PD.EmployeeId,
                           Department = PDL.DepName,
                           Total = AP.Total,
                           ApplicationStatus = AP.ApplicationStatus,
                           ApplicationRejectStatus = AP.ApplicationRejectStatus,
                           DeliveryStatus = AP.DeliveryStatus,
                           DeliveryRejectStatus = AP.DeliveryRejectStatus,
                           AcceptanceStatus = AP.AcceptanceStatus,
                           AcceptanceRejectStatus = AP.AcceptanceRejectStatus,
                       };

            return await list.FirstOrDefaultAsync();
        }

        //申請表
        //GET: api/PCApplications/applist
        [HttpGet("applist")]
        public async Task<ActionResult<dynamic>> GetApplications()
        {
            var list = from AP in this._context.PcApplications
                       join PD in this._context.PersonnelProfileDetails on AP.EmployeeId equals PD.EmployeeId
                       join PDL in this._context.PersonnelDepartmentLists on PD.DepartmentId equals PDL.DepartmentId
                       select new
                       {
                           EmployeeId = PD.EmployeeId,
                           EmployeeName = PD.EmployeeName,
                           OrderId = AP.OrderId,
                           Department = PDL.DepName,
                           Date = AP.Date,
                           PurchaseId = AP.PurchaseId,
                           Comment = AP.Comment,
                           Total = AP.Total,
                           ApplicationStatus = AP.ApplicationStatus,
                           ApplicationRejectStatus = AP.ApplicationRejectStatus,
                           DeliveryStatus = AP.DeliveryStatus,
                           DeliveryRejectStatus = AP.DeliveryRejectStatus,
                           AcceptanceStatus = AP.AcceptanceStatus,
                           AcceptanceRejectStatus = AP.AcceptanceRejectStatus,
                       };

            return await list.ToListAsync();
        }

        //採購審核訂單細項
        //用於PC_OrderCheck.html 送出成功拋值
        //GET: api/PCApplications/OrderCheck
        [HttpGet("OrderCheck/{id}")]
        public async Task<ActionResult<dynamic>> GetOrderCheck(int id)
        {
            var list = from AP in this._context.PcApplications
                       join PD in this._context.PersonnelProfileDetails on AP.EmployeeId equals PD.EmployeeId
                       join PDL in this._context.PersonnelDepartmentLists on PD.DepartmentId equals PDL.DepartmentId
                       where AP.OrderId == id
                       select new
                       {
                           EmployeeId = PD.EmployeeId,
                           EmployeeName = PD.EmployeeName,
                           OrderId = AP.OrderId,
                           Department = PDL.DepName,
                           Date = AP.Date,
                           PurchaseId = AP.PurchaseId,
                           Comment = AP.Comment,
                           Total = AP.Total,
                           ApplicationStatus = AP.ApplicationStatus,
                           ApplicationRejectStatus = AP.ApplicationRejectStatus,
                           DeliveryStatus = AP.DeliveryStatus,
                           DeliveryRejectStatus = AP.DeliveryRejectStatus,
                           AcceptanceStatus = AP.AcceptanceStatus,
                           AcceptanceRejectStatus = AP.AcceptanceRejectStatus,
                           RejectReason = AP.RejectReason
                       };

            return await list.FirstOrDefaultAsync();
        }

        // 物品
        // GET: api/PCApplications/goods
        [HttpGet("goods")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetPcPurchaseItemSearches()
        {
            var i = from PS in this._context.PcGoodLists
                    where PS.Classification == "通用"
                    select new
                    {
                        ProductId = PS.ProductId,
                        Goods = PS.Goods,
                        Unit = PS.Unit,
                        UnitPrice = PS.UnitPrice,
                    };

            return await i.ToListAsync();
        }

        // 進階物品篩選
        // GET: api/PCApplications/selectgoods
        [HttpGet("selectgoods/{Dep}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetPcPurchaseItemSearchesDepartment(string Dep)
        {
            var i = from PS in this._context.PcGoodLists
                    where PS.Classification == Dep
                    select new
                    {
                        ProductId = PS.ProductId,
                        Goods = PS.Goods,
                        Unit = PS.Unit,
                        UnitPrice = PS.UnitPrice,
                    };

            return await i.ToListAsync();
        }

        //// 申請表物品
        //// GET: api/PCApplications/apgoods
        //[HttpGet("apgoods")]
        //public async Task<ActionResult<IEnumerable<dynamic>>> GetPcapgoods()
        //{
        //    var i = from PS in this._context.PcGoodLists
        //            select new
        //            {
        //                ProductId = PS.ProductId,
        //                Goods = PS.Goods,
        //                Unit = PS.Unit,
        //                UnitPrice = PS.UnitPrice,
        //                PS.sub
        //            };

        //    return await i.ToListAsync();
        //}

        // 驗收專用
        // 用於 PC_Acceptance
        // GET: api/PCApplications/acceptance
        [HttpGet("acceptance")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetPCAcceptance()
        {
            var List = from AP in this._context.PcApplications
                       join PD in this._context.PersonnelProfileDetails on AP.EmployeeId equals PD.EmployeeId
                       join PDL in this._context.PersonnelDepartmentLists on PD.DepartmentId equals PDL.DepartmentId
                       where AP.DeliveryStatus == true && AP.AcceptanceStatus == false && AP.AcceptanceRejectStatus == false && AP.DeliveryRejectStatus == false
                       select new
                       {
                           PurchaseId = AP.PurchaseId,
                           OrderId = AP.OrderId,
                           EmployeeName = PD.EmployeeName,
                           Department = PDL.DepName,
                           Total = AP.Total,
                           AcceptanceStatus = AP.AcceptanceStatus,
                           DeliveryStatus = AP.DeliveryStatus,
                           Date = AP.Date.ToString(),
                           Comment = AP.Comment
                       };


            return await List.ToListAsync();
        }

        // 驗收確認專用
        // 用於PC_AcceptanceOrderCheck
        // GET: api/PCApplications/acceptanceordercheck
        [HttpGet("acceptanceordercheck/{id}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetPcAcceptanceOrderCheck(int id)
        {
            var List = from AP in this._context.PcApplications
                       join PD in this._context.PersonnelProfileDetails on AP.EmployeeId equals PD.EmployeeId
                       join PDL in this._context.PersonnelDepartmentLists on PD.DepartmentId equals PDL.DepartmentId
                       join OD in this._context.PcOrderDetails on AP.OrderId equals OD.OrderId
                       join PIS in this._context.PcGoodLists on OD.ProductId equals PIS.ProductId
                       where AP.DeliveryStatus == true && AP.PurchaseId == id


                       select new
                       {
                           PurchaseId = AP.PurchaseId,
                           EmployeeName = PD.EmployeeName,
                           Department = PDL.DepName,
                           Total = AP.Total,
                           ApplicationStatus = AP.ApplicationStatus,
                           DeliveryStatus = AP.DeliveryStatus,
                           OrderId = AP.OrderId,
                           ProductId = OD.ProductId,
                           Goods = OD.Goods,
                           Quantiy = OD.Quantiy,
                           Unit = OD.Unit,
                           UnitPrice = OD.UnitPrice,
                           Subtotal = OD.Subtotal
                       };


            return await List.ToListAsync();
        }

        // 代辦事項
        // 用於 PC_ToDoItems
        // GET: api/PCApplications/todoitem
        [HttpGet("todoitem")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetPCApplicationTodoItem()
        {
            var List = from AP in this._context.PcApplications
                       join PD in this._context.PersonnelProfileDetails on AP.EmployeeId equals PD.EmployeeId
                       join PDL in this._context.PersonnelDepartmentLists on PD.DepartmentId equals PDL.DepartmentId
                       orderby AP.Date descending
                       where AP.ApplicationStatus == false && AP.ApplicationRejectStatus == false
                       select new
                       {
                           PurchaseId = AP.PurchaseId,
                           OrderId = AP.OrderId,
                           EmployeeName = PD.EmployeeName,
                           Department = PDL.DepName,
                           Total = AP.Total,
                           ApplicationStatus = AP.ApplicationStatus,
                           ApplicationRejectStatus = AP.ApplicationRejectStatus,
                           RejectReason = AP.RejectReason,
                           Date = AP.Date.ToString(),
                           Comment = AP.Comment
                       };


            return await List.ToListAsync();
        }

        // 用於PC_ordercheck
        // GET: api/PCApplications/todoitemdetail
        [HttpGet("todoitemdetail/{id}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetPcPurchasetodoitem(int id)
        {
            var List = from AP in this._context.PcApplications
                       join PD in this._context.PersonnelProfileDetails on AP.EmployeeId equals PD.EmployeeId
                       join PDL in this._context.PersonnelDepartmentLists on PD.DepartmentId equals PDL.DepartmentId
                       join OD in this._context.PcOrderDetails on AP.OrderId equals OD.OrderId
                       join PIS in this._context.PcGoodLists on OD.ProductId equals PIS.ProductId
                       where AP.DeliveryStatus == false && AP.PurchaseId == id


                       select new
                       {
                           PurchaseId = AP.PurchaseId,
                           EmployeeName = PD.EmployeeName,
                           Department = PDL.DepName,
                           Total = AP.Total,
                           ApplicationStatus = AP.ApplicationStatus,
                           DeliveryStatus = AP.DeliveryStatus,
                           OrderId = AP.OrderId,
                           ProductId = OD.ProductId,
                           Goods = OD.Goods,
                           Quantiy = OD.Quantiy,
                           Unit = OD.Unit,
                           UnitPrice = OD.UnitPrice,
                           Subtotal = OD.Subtotal
                       };


            return await List.ToListAsync();
        }



        // 物品查詢專用  部門搜尋
        // 用於 PC_ApplicationRecordSearch
        // GET: api/PCApplications/recordsearch
        [HttpGet("recordsearch")]
        public async Task<ActionResult<dynamic>> GetPCrecordsearch(string id)
        {
            var List = from AP in this._context.PcApplications
                       join PD in this._context.PersonnelProfileDetails on AP.EmployeeId equals PD.EmployeeId
                       join PDL in this._context.PersonnelDepartmentLists on PD.DepartmentId equals PDL.DepartmentId
                       orderby AP.Date
                       select new
                       {
                           PurchaseId = AP.PurchaseId,
                           EmployeeName = PD.EmployeeName,
                           DepartmentId = PDL.DepartmentId,
                           Department = PDL.DepName,
                           Date = AP.Date.ToString(),
                           Comment = AP.Comment,
                           Total = AP.Total,
                           ApplicationStatus = AP.ApplicationStatus,
                           ApplicationRejectStatus = AP.ApplicationRejectStatus,
                           DeliveryStatus = AP.DeliveryStatus,
                           DeliveryRejectStatus = AP.DeliveryRejectStatus,
                           AcceptanceStatus = AP.AcceptanceStatus,
                           AcceptanceRejectStatus = AP.AcceptanceRejectStatus,
                           RejectReason = AP.RejectReason
                       };

            if (!string.IsNullOrWhiteSpace(id)) {
                List = List.Where(a => a.Department.Contains(id));
            }

            return await List.ToListAsync();
        }

        // 物品查詢專用  部門+日期搜尋
        // 用於 PC_ApplicationRecordSearch
        // GET: api/PCApplications/recordsearch/{startdate}/{enddate}
        [HttpGet("recordsearch/{startdate}/{enddate}")]
        public async Task<ActionResult<dynamic>> GetPCrecordsearchDate(string id,string startdate,string enddate)
        {
            var Mystartdate = DateTime.Parse(startdate);
            var Myenddate = DateTime.Parse(enddate);
            var List = from AP in this._context.PcApplications
                       join PD in this._context.PersonnelProfileDetails on AP.EmployeeId equals PD.EmployeeId
                       join PDL in this._context.PersonnelDepartmentLists on PD.DepartmentId equals PDL.DepartmentId
                       orderby AP.Date
                       where AP.Date >= Mystartdate && AP.Date <= Myenddate
                       select new
                       {
                           PurchaseId = AP.PurchaseId,
                           EmployeeName = PD.EmployeeName,
                           DepartmentId = PDL.DepartmentId,
                           Department = PDL.DepName,
                           Date = AP.Date.ToString(),
                           Comment = AP.Comment,
                           Total = AP.Total,
                           ApplicationStatus = AP.ApplicationStatus,
                           ApplicationRejectStatus = AP.ApplicationRejectStatus,
                           DeliveryStatus = AP.DeliveryStatus,
                           DeliveryRejectStatus = AP.DeliveryRejectStatus,
                           AcceptanceStatus = AP.AcceptanceStatus,
                           AcceptanceRejectStatus = AP.AcceptanceRejectStatus,
                           RejectReason = AP.RejectReason,
                       };

            if (!string.IsNullOrWhiteSpace(id))
            {
                List = List.Where(a => a.Department.Contains(id));
            }

            return await List.ToListAsync();
        }

        // 物品查詢細項專用
        // 用於 PC_ApplicationRecordDetails
        // GET: api/PCApplications/recordsearchdetail
        [HttpGet("recordsearchdetail/{id}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetPCrecordsearchdetail(int id)
        {
            var List = from AP in this._context.PcApplications
                       join PD in this._context.PersonnelProfileDetails on AP.EmployeeId equals PD.EmployeeId
                       join PDL in this._context.PersonnelDepartmentLists on PD.DepartmentId equals PDL.DepartmentId
                       join OD in this._context.PcOrderDetails on AP.OrderId equals OD.OrderId
                       join GL in this._context.PcGoodLists on OD.ProductId equals GL.ProductId
                       where AP.PurchaseId == id
                       select new
                       {
                           PurchaseId = AP.PurchaseId,
                           EmployeeName = PD.EmployeeName,
                           Department = PDL.DepName,
                           Date = AP.Date.ToString(),
                           Comment = AP.Comment,
                           Total = AP.Total,
                           Goods = OD.Goods,
                           Unit = OD.Unit,
                           Quantiy = OD.Quantiy,
                           UnitPrice = OD.UnitPrice,
                           Subtotal = OD.Subtotal,
                           RejectReason = AP.RejectReason,

                       };


            return await List.ToListAsync();
        }

        // 物品資料查詢+圖片
        // 用於 PC_ItemSearch
        // GET: api/PCApplications/itemsearch
        [HttpGet("itemsearch")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetPCitemsearch()
        {
            var List = from OL in this._context.PcGoodLists
                       select new
                       {
                           ProductId = OL.ProductId,
                           Goods = OL.Goods,
                           Unit = OL.Unit,
                           UnitPrice = OL.UnitPrice,
                           Image = OL.Image,
                           Classification = OL.Classification
                       };


            return await List.ToListAsync();
        }

        // 物品查詢專用  部門搜尋
        // 用於 PC_ItemSearch
        // GET: api/PCApplications/departmentsearch
        [HttpGet("departmentsearch")]
        public async Task<ActionResult<dynamic>> GetGoodsDepartmentSearch(string id)
        {
            var List = from GL in this._context.PcGoodLists 
                       select new
                       {
                           ProductId = GL.ProductId,
                           Goods = GL.Goods,
                           Unit = GL.Unit,
                           UnitPrice = GL.UnitPrice,
                           Image = GL.Image,
                           Classification = GL.Classification
                       };

            if (!string.IsNullOrWhiteSpace(id))
            {
                List = List.Where(a => a.Classification.Contains(id));
            }

            return await List.ToListAsync();
        }

        //採購清單查詢用
        //GET: api/PCApplications/department
        [HttpGet("department/{depId}")]
        public async Task<ActionResult<dynamic>> GetDepartment(int depid)
        {
            var list = from PD in this._context.PersonnelProfileDetails
                       join DL in this._context.PersonnelDepartmentLists on PD.DepartmentId equals DL.DepartmentId
                       join AP in this._context.PcApplications on PD.EmployeeId equals AP.EmployeeId
                       where DL.DepartmentId == depid
                       select new
                       {
                           DepartmentId = DL.DepartmentId,
                           DepartmentnName = DL.DepName,
                           Date = AP.Date.ToString("yyyy-MM-dd")
                       };

            return await list.ToListAsync();
        }

        //用部門尋找 採購細項查詢
        // GET: api/PCApplications/department/5
        [HttpGet("department/{depId}")]
        public async Task<ActionResult<dynamic>> GetDepartmentLeave(int depId)
        {

            var list = from AP in this._context.PcApplications
                       join PD in this._context.PersonnelProfileDetails on AP.EmployeeId equals PD.EmployeeId
                       join PDL in this._context.PersonnelDepartmentLists on PD.DepartmentId equals PDL.DepartmentId
                       where PDL.DepartmentId == depId
                       select new
                       {
                           DepName = PDL.DepName,
                       };


            if (list == null)
            {
                return NotFound();
            }

            return await list.ToListAsync();
        }

        //用日期尋找 採購細項查詢
        // GET: api/PCApplications/department/{y}-{m}
        [HttpGet("department/{y}-{m}")]
        public async Task<ActionResult<dynamic>> GetDateLeave(int y, int m)
        {

            var list = from AP in this._context.PcApplications
                       join PD in this._context.PersonnelProfileDetails on AP.EmployeeId equals PD.EmployeeId
                       join PDL in this._context.PersonnelDepartmentLists on PD.DepartmentId equals PDL.DepartmentId
                       where AP.Date.Year == y && AP.Date.Month == m
                       select new
                       {
                           Year = AP.Date.Year,
                           Month = AP.Date.Month,
                       };


            if (list == null)
            {
                return NotFound();
            }

            return await list.ToListAsync();
        }

        // GET: api/PCApplications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PcApplication>> GetPcApplication(int id)
        {
            var pcApplication = await _context.PcApplications.FindAsync(id);

            if (pcApplication == null)
            {
                return NotFound();
            }

            return pcApplication;
        }

        // PUT: api/PCApplications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPcApplication(int id, PcApplication pcApplication)
        {
            if (id != pcApplication.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(pcApplication).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PcApplicationExists(id))
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

        //// POST: api/PCApplications
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<PcApplication>> PostPcApplication(PcApplication pcApplication)
        //{
        //    _context.PcApplications.Add(pcApplication);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetPcApplication", new { id = pcApplication.PurchaseId }, pcApplication);
        //}

        // POST: api/PCApplications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult PostPcApplication([FromBody] PcApplication pcApplication)
        {
            if (pcApplication.Total != 0) { 
            PcApplication insert = new PcApplication {
                PurchaseId = pcApplication.PurchaseId,
                EmployeeId = pcApplication.EmployeeId,
                Department = pcApplication.Department,
                Date = pcApplication.Date,
                Comment = pcApplication.Comment,
                RejectReason = pcApplication.RejectReason,
                Total = pcApplication.Total,
                ApplicationStatus = false,
                ApplicationRejectStatus = false,
                DeliveryStatus = false,
                DeliveryRejectStatus = false,
                AcceptanceStatus = false,
                AcceptanceRejectStatus = false,
                PcOrderDetails = pcApplication.PcOrderDetails
            };
            _context.PcApplications.Add(insert);
            _context.SaveChanges();
            return Content("申請完成");
            }else {
                return Content("請輸入至少一個品項");
            }
        }

        //// POST: api/PCApplications
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost("PcOrderDetail")]
        //public ActionResult PostPcApplicationGoods(int OrderId,[FromBody] PcOrderDetail pcOrderDetail)
        //{
        //    PcOrderDetail insert = new PcOrderDetail
        //    {
        //        OrderId = OrderId,
        //        ProductId = pcOrderDetail.ProductId,
        //        Goods = pcOrderDetail.Goods,
        //        Quantiy = pcOrderDetail.Quantiy,
        //        Unit = pcOrderDetail.Unit,
        //        UnitPrice = pcOrderDetail.UnitPrice,
        //        Subtotal = pcOrderDetail.Subtotal
        //    };
        //    _context.PcOrderDetails.Add(insert);
        //    _context.SaveChanges();
        //    return Content("ok");
        //}

        // DELETE: api/PCApplications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePcApplication(int id)
        {
            var pcApplication = await _context.PcApplications.FindAsync(id);
            if (pcApplication == null)
            {
                return NotFound();
            }

            _context.PcApplications.Remove(pcApplication);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PcApplicationExists(int id)
        {
            return _context.PcApplications.Any(e => e.OrderId == id);
        }
    }
}
