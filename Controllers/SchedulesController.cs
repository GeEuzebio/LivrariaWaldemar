using System.Diagnostics;
using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Controllers
{
    public class SchedulesController : Controller
    {
        List<Schedule> ListOfSchedules = [];
        private readonly ApplicationDbContext _context;

        public SchedulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Schedule.ToListAsync());
        }

        public async Task<IActionResult> SearchByDate(DateTime date)
        {
            Scheduling? schedules = await _context.Scheduling.FirstOrDefaultAsync(s => s.Day!.Value.ToUniversalTime() == date.ToUniversalTime());
            if(schedules is null)
            {
                schedules = new Scheduling
                {
                    Day = date,
                };
                return View(nameof(Create), schedules);
            }
            return View(nameof(Create), schedules);
        }

        public IActionResult Create(Scheduling schedule)
        {
            if(schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSchedule([Bind("Day,ClassHour,Professor,classRoom")] Schedule schedule)
        {
            if(ModelState.IsValid)
            {
                _context.Schedule.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Schedule(DateTime day, int position)
        {
            Scheduling schedule = new Scheduling
            {
                Day = day
            };
            ViewBag.Position = position;
            return View(schedule);
        }
    }
}