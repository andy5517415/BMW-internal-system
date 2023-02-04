using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternalSystem.Models;
using System.Timers;
using System;
using System.Collections;

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

        //報工內容清單(普通表單及複合式查詢)
        //GET: api/ProductionContexts/ContextsList
        [HttpGet("ContextsList")]
        public async Task<ActionResult<dynamic>> GetContextsList(string empNumber , string empName , string starDate )
        {

            var query = from PC in this._context.ProductionContexts
                        join PPD in this._context.PersonnelProfileDetails on PC.EmployeeId equals PPD.EmployeeId
                        join PD in this._context.PersonnelDepartmentLists on PPD.DepartmentId equals PD.DepartmentId
                        join BO in this._context.BusinessOrders on PC.OrderId equals BO.OrderId
                        orderby PC.Date descending
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
           
            if (!string.IsNullOrWhiteSpace(empNumber))
            {
                query = query.Where(a => a.EmployeeNumber.Contains(empNumber));
            }
            if (!string.IsNullOrWhiteSpace(empName))
            {
                query = query.Where(a => a.EmployeeName.Contains(empName));
            }
            if (!string.IsNullOrWhiteSpace(starDate))
            {
                query = query.Where(a => a.Date.Contains(starDate));
            }
           

            return await query.ToListAsync();
        }

        //報工內容清單(日期複合式查詢)
        //GET: api/ProductionContexts/ContextsListSearch/start2023-01-01/end2023-02-01
        [HttpGet("ContextsListSearch/start{startDate}/end{endDate}")]
        public async Task<ActionResult<dynamic>> GetContextDatesList(string empNumber, string empName, string startDate ,string endDate)
        {
            var std = DateTime.Parse(startDate);
            var end = DateTime.Parse(endDate);

            var query = from PC in this._context.ProductionContexts
                        join PPD in this._context.PersonnelProfileDetails on PC.EmployeeId equals PPD.EmployeeId
                        join PD in this._context.PersonnelDepartmentLists on PPD.DepartmentId equals PD.DepartmentId
                        join BO in this._context.BusinessOrders on PC.OrderId equals BO.OrderId
                        orderby PC.Date descending
                        where PC.Date >= std && PC.Date <= end
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
                            ProcessId = PP.ProcessId,
                            ProcessName = PP.ProcessName,
                            AreaId = PA.AreaId,
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

        //訂單的總時間
        // GET: api/ProductionContexts/test
        [HttpGet("test")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetProductionContextstest()
        {
            List<dynamic> apple = new List<dynamic>();

            var process = (from PPL in this._context.ProductionProcessLists
                          join PP in this._context.ProductionProcesses on PPL.ProcessId equals PP.ProcessId
                          join PPSN in this._context.ProductionProcessStatusNames on PPL.StatusId equals PPSN.StatusId
                          join BO in this._context.BusinessOrders on PPL.OrderId equals BO.OrderId
                          join BOD in this._context.BusinessOrderDetails on BO.OrderId equals BOD.OrderId
                          join BOP in this._context.BusinessOptionals on BOD.OptionalId equals BOP.OptionalId
                          join BC in this._context.BusinessCategories on BOP.CategoryId equals BC.CategoryId
                          where BOP.CategoryId == 1 
                          select new
                          {
                              OrderId = PPL.OrderId,
                              ProcessName = PP.ProcessName,
                              StarDate = PPL.StarDate.ToString(),
                              EndDate = PPL.EndDate.ToString(),
                              BO.OrderNumber,
                              OptionalId = BOP.OptionalId,
                              OptionalName = BOP.OptionalName,
                              StatusId = PPSN.StatusId,
                              StatusName = PPSN.StatusName,
                          }).ToList();

            

            var query = from BO in this._context.BusinessOrders
                        join BOD in this._context.BusinessOrderDetails on BO.OrderId equals BOD.OrderId
                        join BOP in this._context.BusinessOptionals on BOD.OptionalId equals BOP.OptionalId
                        join BC in this._context.BusinessCategories on BOP.CategoryId equals BC.CategoryId
                        where BOP.CategoryId == 1 && BO.IsAccepted == true
                        select new
                        {
                            OrderId = BO.OrderId,
                            BO.OrderNumber,
                            OptionalId = BOP.OptionalId,
                            OptionalName = BOP.OptionalName,
                            process
                        };

            return await query.ToListAsync();
        }

        //訂單的總時間
        // GET: api/ProductionContexts/ListTotal
        [HttpGet("ListTotal")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetProductionContextsListTotal()
        {


            var query = from BO in this._context.BusinessOrders
                        join BOD in this._context.BusinessOrderDetails on BO.OrderId equals BOD.OrderId
                        join BOP in this._context.BusinessOptionals on BOD.OptionalId equals BOP.OptionalId
                        join BC in this._context.BusinessCategories on BOP.CategoryId equals BC.CategoryId
                        where BOP.CategoryId == 1 && BO.IsAccepted == true 
                        select new
                        {
                            OrderId = BO.OrderId,
                            BO.OrderNumber,
                            OptionalId = BOP.OptionalId,
                            OptionalName = BOP.OptionalName,
                            
                        };

            return await query.ToListAsync();
        }


        //訂單的總時間
        // GET: api/ProductionContexts/Total?id=25
        [HttpGet("Total")]
        public async Task<ActionResult<dynamic>> GetProductionContextsTotal(int id)
        {
          

            //抓取 製程A總時
            var proessesA = from o in _context.ProductionContexts
                            where  o.ProcessId == 1 && o.OrderId == id
                            select o;

            double Atotal = 0;
            foreach (var pro in proessesA)
            {

                TimeSpan a = Convert.ToDateTime(pro.EndTime).Subtract(Convert.ToDateTime(pro.StartTime));
                
                if (a.TotalMinutes >= 570)
                {
                    Atotal += a.TotalMinutes - 90;
                }
                else if (a.TotalMinutes >= 300)
                {
                    Atotal += a.TotalMinutes - 60;
                }else  
                {
                    Atotal += a.TotalMinutes;
                }

            }

                Atotal = Atotal / 60;
            

            

            //抓取 製程B 總時
            var proessesB = from o in _context.ProductionContexts
                            where  o.ProcessId == 2 && o.OrderId == id
                            select o;

            double Btotal = 0;
            foreach (var proB in proessesB)
            {

                TimeSpan b = Convert.ToDateTime(proB.EndTime).Subtract(Convert.ToDateTime(proB.StartTime));
                
                if (b.TotalMinutes >= 570)
                {
                    Btotal += b.TotalMinutes - 90;
                }
                else if (b.TotalMinutes >= 300)
                {
                    Btotal += b.TotalMinutes - 60;
                }

            }
            Btotal = Btotal / 60;

            //抓取 製程C 總時
            var proessesC = from o in _context.ProductionContexts
                            where  o.ProcessId == 3 && o.OrderId == id
                            select o;

            double Ctotal = 0;
            foreach (var proC in proessesC)
            {

                TimeSpan c = Convert.ToDateTime(proC.EndTime).Subtract(Convert.ToDateTime(proC.StartTime));
                if (c.TotalMinutes >= 570)
                {
                    Ctotal += c.TotalMinutes - 90;
                }
                else if (c.TotalMinutes >= 300)
                {
                    Ctotal += c.TotalMinutes - 60;
                }

            }
            Ctotal = Ctotal / 60;

            double Total = Atotal + Btotal + Ctotal;



            // 訂單內製程每個製程及製程總時
            var proess = (from PPL in this._context.ProductionProcessLists
                        join PP in this._context.ProductionProcesses on PPL.ProcessId equals PP.ProcessId
                        join PPSN in this._context.ProductionProcessStatusNames on PPL.StatusId equals PPSN.StatusId
                        join BO in this._context.BusinessOrders on PPL.OrderId equals BO.OrderId
                        join BOD in this._context.BusinessOrderDetails on BO.OrderId equals BOD.OrderId
                        join BOP in this._context.BusinessOptionals on BOD.OptionalId equals BOP.OptionalId
                        join BC in this._context.BusinessCategories on BOP.CategoryId equals BC.CategoryId
                        where BOP.CategoryId == 1 && PPL.OrderId == id
                        select new
                        {
                            OrderId = PPL.OrderId,
                            ProcessName = PP.ProcessName,
                            StarDate = PPL.StarDate.ToString(),
                            EndDate = PPL.EndDate.ToString(),
                            BO.OrderNumber,
                            OptionalId = BOP.OptionalId,
                            OptionalName = BOP.OptionalName,
                            StatusId = PPSN.StatusId,
                            StatusName = PPSN.StatusName,

                            Atotal = (float)Math.Round(Atotal, 1),
                            Btotal = (float)Math.Round(Btotal, 1),
                            Ctotal = (float)Math.Round(Ctotal, 1),
                        }).ToList();

            // 抓訂單的開始日期及結束日期
            var startDate = (from o in this._context.ProductionProcessLists
                            where o.ProcessId == 1 && o.OrderId == id && (o.StatusId == 2 || o.StatusId == 3)
                            select o).FirstOrDefault();
            var endDate = (from o in this._context.ProductionProcessLists
                            where o.ProcessId == 3 && o.OrderId == id && o.StatusId == 3
                           select o).FirstOrDefault();



            if (endDate == null)
            {
                var query = from BO in this._context.BusinessOrders 
                        join BOD in this._context.BusinessOrderDetails on BO.OrderId equals BOD.OrderId
                        join BOP in this._context.BusinessOptionals on BOD.OptionalId equals BOP.OptionalId
                        join BC in this._context.BusinessCategories on BOP.CategoryId equals BC.CategoryId
                        where BOP.CategoryId == 1  && BO.IsAccepted == true && BO.OrderId == id
                        select new
                        {
                            OrderId = BO.OrderId,
                            BO.OrderNumber,
                            OptionalId = BOP.OptionalId,
                            OptionalName = BOP.OptionalName,
                            Total,
                            StarDate = startDate.StarDate.ToString("yyyy-MM-dd"),
                            EndDate = "",
                            proess

                        };
                return await query.FirstOrDefaultAsync();
            }
            else 
            {
                var query = from BO in this._context.BusinessOrders
                            join BOD in this._context.BusinessOrderDetails on BO.OrderId equals BOD.OrderId
                            join BOP in this._context.BusinessOptionals on BOD.OptionalId equals BOP.OptionalId
                            join BC in this._context.BusinessCategories on BOP.CategoryId equals BC.CategoryId
                            where BOP.CategoryId == 1 && BO.IsAccepted == true && BO.OrderId == id
                            select new
                            {
                                OrderId = BO.OrderId,
                                BO.OrderNumber,
                                OptionalId = BOP.OptionalId,
                                OptionalName = BOP.OptionalName,
                                Total,
                                StarDate = startDate.StarDate.ToString("yyyy-MM-dd"),
                                EndDate = endDate.EndDate,
                                proess
                            };
                return await query.FirstOrDefaultAsync();
            }
            

        }


        //每個製程的總時間
        // GET: api/ProductionContexts/ProcessesTotal
        [HttpGet("ProcessesTotal")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetProductionContextsProcessesTotal()
        {
            //var bb = (from o in _context.ProductionContexts
            //         where o.OrderId == 25
            //         select o).Count();

            //List<double> TA = new List<double> ();
            //List<> QQ = new ();
            //for (int i = 1; i <= bb; i++)
            //{
            //    var query = from PPL in this._context.ProductionProcessLists
            //                join PP in this._context.ProductionProcesses on PPL.ProcessId equals PP.ProcessId
            //                join BO in this._context.BusinessOrders on PPL.OrderId equals BO.OrderId
            //                join BOD in this._context.BusinessOrderDetails on BO.OrderId equals BOD.OrderId
            //                join BOP in this._context.BusinessOptionals on BOD.OptionalId equals BOP.OptionalId
            //                join BC in this._context.BusinessCategories on BOP.CategoryId equals BC.CategoryId
            //                where BOP.CategoryId == 1 && PPL.OrderId == 25 && PPL.ProcessId == i
            //                select new
            //                {
            //                    OrderId = PPL.OrderId,
            //                    ProcessName = PP.ProcessName,
            //                    StarDate = PPL.StarDate.ToString(),
            //                    EndDate = PPL.EndDate.ToString(),
            //                    BO.OrderNumber,
            //                    OptionalId = BOP.OptionalId,
            //                    OptionalName = BOP.OptionalName,
            //                    TA
            //                    //Atotal = (float)Math.Round(Atotal,1),
            //                    //Btotal = (float)Math.Round(Btotal, 1),
            //                    //Ctotal = (float)Math.Round(Ctotal, 1)
            //                };

            //    double total = 0;
            //    var aa = from o in _context.ProductionContexts
            //                    where o.OrderId == 25 && o.ProcessId == i
            //                    select o;
            //    foreach (var item in aa)
            //    {
            //        TimeSpan a = Convert.ToDateTime(item.EndTime).Subtract(Convert.ToDateTime(item.StartTime));
            //        total += a.TotalMinutes;
            //    }
            //    TA.Add(total);
            //}


            //抓取 製程A總時
            var proessesA = from o in _context.ProductionContexts
                            where o.OrderId == 1 && o.ProcessId == 1 
                            select o;

            double Atotal = 0;
            foreach (var pro in proessesA)
            {

                TimeSpan a = Convert.ToDateTime(pro.EndTime).Subtract(Convert.ToDateTime(pro.StartTime));
                if (a.TotalMinutes >= 570 )
                {
                     Atotal += a.TotalMinutes-90;
                } else if (a.TotalMinutes >= 300)
                {
                    Atotal += a.TotalMinutes-60;
                }

            }
            Atotal = Atotal / 60;

            //抓取 製程B 總時
            var proessesB = from o in _context.ProductionContexts
                            where o.OrderId == 1 && o.ProcessId == 2
                            select o;

            double Btotal = 0;
            foreach (var proB in proessesB)
            {

                TimeSpan b = Convert.ToDateTime(proB.EndTime).Subtract(Convert.ToDateTime(proB.StartTime));

                if (b.TotalMinutes >= 570)
                {
                    Btotal += b.TotalMinutes - 90;
                }
                else if (b.TotalMinutes >= 300)
                {
                    Btotal += b.TotalMinutes - 60;
                }


            }
            Btotal = Btotal / 60;

            //抓取 製程C 總時
            var proessesC = from o in _context.ProductionContexts
                            where o.OrderId == 1 && o.ProcessId == 3
                            select o;

            double Ctotal = 0;
            foreach (var proC in proessesC)
            {

                TimeSpan c = Convert.ToDateTime(proC.EndTime).Subtract(Convert.ToDateTime(proC.StartTime));

                if (c.TotalMinutes >= 570)
                {
                    Ctotal += c.TotalMinutes - 90;
                }
                else if (c.TotalMinutes >= 300)
                {
                    Ctotal += c.TotalMinutes - 60;
                }

            }
            Ctotal = Ctotal / 60;

            double Total = Atotal + Btotal + Ctotal;

            var query = from PPL in this._context.ProductionProcessLists
                        join PP in this._context.ProductionProcesses on PPL.ProcessId equals PP.ProcessId
                        join PPSN in this._context.ProductionProcessStatusNames on PPL.StatusId equals PPSN.StatusId
                        join BO in this._context.BusinessOrders on PPL.OrderId equals BO.OrderId
                        join BOD in this._context.BusinessOrderDetails on BO.OrderId equals BOD.OrderId
                        join BOP in this._context.BusinessOptionals on BOD.OptionalId equals BOP.OptionalId
                        join BC in this._context.BusinessCategories on BOP.CategoryId equals BC.CategoryId
                        where BOP.CategoryId == 1 && PPL.OrderId == 1
                        select new
                        {
                            OrderId = PPL.OrderId,
                            ProcessName = PP.ProcessName,
                            StarDate = PPL.StarDate.ToString(),
                            EndDate = PPL.EndDate.ToString(),
                            BO.OrderNumber,
                            OptionalId = BOP.OptionalId,
                            OptionalName = BOP.OptionalName,
                            StatusId =PPSN.StatusId,
                            StatusName = PPSN.StatusName,
                           
                            Atotal = (float)Math.Round(Atotal, 1),
                            Btotal = (float)Math.Round(Btotal, 1),
                            Ctotal = (float)Math.Round(Ctotal, 1),
                            Total = (float)Math.Round(Total, 1)    
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
        [HttpPut("as/{orderid}/{empid}/{date}")]
        public async Task<IActionResult> PutProductionContext(int orderid, int empid, string date, ProductionContext productionContext)
        {
            if (orderid != productionContext.OrderId)
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
                if (!ProductionContextExists(orderid))
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
        public string  PostProductionContext(ProductionContext productionContext)
        {
            //結束時間不能大於開始時間
            TimeSpan endtime = Convert.ToDateTime(productionContext.EndTime).Subtract(Convert.ToDateTime(productionContext.StartTime));
            double tt = endtime.TotalMinutes;
            if (tt>0)
            {
                _context.ProductionContexts.Add(productionContext);
                try
                {
                     _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (ProductionContextExists(productionContext.OrderId))
                    {
                        return Conflict().ToString();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                return "開始時間不能大於結束時間";
            }

            return "成功";
        }



        // POST: api/ProductionContexts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public string PostProductionContextUpdate(int id, ProductionContext productionContext)
        {
            //結束時間不能大於開始時間
            TimeSpan endtime = Convert.ToDateTime(productionContext.EndTime).Subtract(Convert.ToDateTime(productionContext.StartTime));
            double tt = endtime.TotalMinutes;
            if (tt > 0)
            {
                _context.Entry(productionContext).State = EntityState.Modified;
                //_context.ProductionContexts.Add(productionContext);
                try
                {
                    _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (ProductionContextExists(productionContext.OrderId))
                    {
                        return Conflict().ToString();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                return "開始時間不能大於結束時間";
            }

            return "成功";
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
