using System.Diagnostics;
using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Controllers
{
    public class SchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SchedulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(DateTime date)
        {
            return View(nameof(Index), await _context.Scheduling.FirstOrDefaultAsync(d => d.Day == date.ToUniversalTime()));
        }
        public async Task<IActionResult> Index()
        {
            DateTime toDay = new DateTime(DateTime.Now.Year,DateTime.Now.Month, DateTime.Now.Day);
            Scheduling? schedule = await _context.Scheduling.FirstOrDefaultAsync(d => d.Day == toDay.ToUniversalTime());
            ViewBag.Schedule = schedule;
            return View();
        }
        [Route("pesquisa_por_data")]
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
        public async Task<IActionResult> CreateSchedule(DateTime day, ClassRoom classRoom, Professionals professor, int pos)
        {
            bool exist = await ScheduleExist(day.ToUniversalTime());
            if (exist)
            {
                Scheduling? schedule = await _context.Scheduling.FirstOrDefaultAsync(d => d.Day!.Value.ToUniversalTime() == day.ToUniversalTime());
                schedule.Professor[pos] = professor;
                schedule.ClassRoom[pos] = classRoom;
                schedule.IsScheduled[pos] = true;
                _context.Scheduling.Update(schedule);
                await _context.SaveChangesAsync();
            }
            else
            {
                Scheduling newSchedule = new();
                newSchedule.Day = day.ToUniversalTime();
                newSchedule.Professor[pos] = professor;
                newSchedule.ClassRoom[pos] = classRoom;
                newSchedule.IsScheduled[pos] = true;
                _context.Scheduling.Add(newSchedule);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
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

        [HttpPut]
        [ValidateAntiForgeryToken]
        //TODO
        public async Task<IActionResult> Edit()
        {
            return View(await _context.Scheduling.ToListAsync());
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        //TODO
        public async Task<IActionResult> Delete()
        {
            return View(await _context.Scheduling.ToListAsync());
        }

        private async Task<bool> ScheduleExist(DateTime? day)
        {
            bool result = await _context.Scheduling.FirstOrDefaultAsync(x => x.Day == day) is null ? false : true;
            return result;
        }
    }
}