using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Data;
using LibraryApp.Models;
using System.Diagnostics;
using LibraryApp.Services;
using System.Drawing;

namespace LibraryApp.Controllers
{
    public class CardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("carteirinha/pesquisar")]
        public async Task<IActionResult> Search(string name, string classroom)
        {
            User? user = await _context.User.FirstAsync(u => u.Name!.ToUpper().Trim() == name.ToUpper() && u.Class! == classroom);
            byte[] qrCode = QRCodeGeneratorService.QRCodeCreator($"https://bibliotecawf.com.br/Users/Details/{user.UserId}");
            string img = QRCodeGeneratorService.ImageToBase64(qrCode);
            ViewBag.QRCode = img;
            return View(nameof(Index), user);
        }

        // GET: Cards
        [Route("carteirinha")]
        public IActionResult Index()
        {
            return View();
        }

        // GET: Cards/Details/5
        [Route("carteirinha/detalhes")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Card
                .FirstOrDefaultAsync(m => m.Id == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }
    }
}
