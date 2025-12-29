using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web_Project.Data;

namespace Web_Project.ViewComponents
{
    public class SalesBarViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public SalesBarViewComponent(ApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var now = DateTime.UtcNow;
            var start = new DateTime(now.Year, now.Month, 1);
            var end = start.AddMonths(1);

            var booksSold = await (from od in _db.OrderDetails
                                   join o in _db.Orders on od.OrderID equals o.Id
                                   where o.OrderDate >= start && o.OrderDate < end
                                   select (int?)od.Quantity).SumAsync() ?? 0;

            return View(booksSold);
        }
    }
}
