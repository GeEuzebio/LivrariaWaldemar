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
        [Route("agendamentos/pesquisa")]
        public async Task<IActionResult> Search(DateTime date)
        {
            return View(nameof(Index), await _context.Scheduling.FirstOrDefaultAsync(d => d.Day == date.ToUniversalTime()));
        }

        [Route("agendamentos")]
        public async Task<IActionResult> Index()
        {
            DateTime toDay = new DateTime(DateTime.Now.Year,DateTime.Now.Month, DateTime.Now.Day);
            Scheduling? schedule = await _context.Scheduling.FirstOrDefaultAsync(d => d.Day == toDay.ToUniversalTime());
            ViewBag.Schedule = schedule;
            return View();
        }
        [Route("agendamentos/novo_agendamento/pesquisa_por_data")]
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

        [Route("agendamentos/novo_agendamento")]
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
        [Route("agendamentos/novo_agendamento")]
        public async Task<IActionResult> CreateSchedule(DateTime day, ClassRoom classRoom, Professionals professor, int pos)
        {
            bool exist = await ScheduleExist(day.ToUniversalTime());
            if (exist)
            {
                Scheduling? schedule = await _context.Scheduling.FirstOrDefaultAsync(d => d.Day!.Value.ToUniversalTime() == day.ToUniversalTime());
                schedule!.Professor[pos] = professor;
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
        [Route("agendamentos/novo_agendamento/criar_agendamento")]
        public IActionResult Schedule(DateTime day, int position)
        {
            Scheduling schedule = new Scheduling
            {
                Day = day
            };
            ViewBag.Position = position;
            return View(schedule);
        }

        [Route("agendamentos/editar")]
        public async Task<IActionResult> Edit(int? id, int pos)
        {
            if(id == null)
            {
                return NotFound();
            }
            ViewBag.Pos = pos;
            return View(await _context.Scheduling.FirstAsync( s => s.Id == id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("agendamentos/editar")]
        public async Task<IActionResult> Edit(DateTime day, ClassRoom classRoom, Professionals professor, int pos)
        {
            Scheduling? schedule = await _context.Scheduling.FirstOrDefaultAsync(s => s.Day == day.ToUniversalTime());
            schedule!.ClassRoom[pos] = classRoom;
            schedule!.Professor[pos] = professor;
            _context.Update(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Route("agendamentos/deletar")]
        public async Task<IActionResult> Delete(int? id, int pos)
        {
            if(id == null)
            {
                return NotFound();
            }
            ViewBag.Pos = pos;
            return View(await _context.Scheduling.FirstAsync(s => s.Id == id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("agendamentos/deletar")]
        //TODO
        public async Task<IActionResult> Delete(DateTime day, int pos)
        {
            Scheduling? schedule = _context.Scheduling.FirstOrDefault(s => s.Day == day.ToUniversalTime());
            schedule!.ClassRoom[pos] = null;
            schedule!.Professor[pos] = null;
            schedule!.IsScheduled[pos] = false;
            _context.Update(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ScheduleExist(DateTime? day)
        {
            bool result = await _context.Scheduling.FirstOrDefaultAsync(x => x.Day == day) is null ? false : true;
            return result;
        }
    }
}