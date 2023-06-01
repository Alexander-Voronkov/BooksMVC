using AutoMapper;
using BooksMVC.Data;
using BooksMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BooksMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private BookContext context;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, BookContext context, IMapper mapper)
        {
            _logger = logger;
            this.context = context;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View(context.Books.Include(x=>x.Authors).ToList());
        }

        public IActionResult Authors()
        {
            return View(context.Authors.Include(x => x.Books).ToList());
        }

        public IActionResult BookDetails(int? id)
        {
            var book = context.Books.Include(x=>x.Authors).Where(x=>x.Id == id).ToList();
            if (book.Count == 0)
                return NotFound();
            return View("BookView", book[0]);
        }

        public IActionResult BookEdit(int? id)
        {
            var book = context.Books.Include(x => x.Authors).First(q => q.Id == id);
            if (book == null)
                return NotFound();
            var vM = _mapper.Map<EditBookVM>(book);
            return View(vM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookEdit(EditBookVM vm)
        {
            try
            {
                var temp = _mapper.Map<Book>(vm);
                if (vm.TempCover is not null)
                {
                    var stream = vm.TempCover.OpenReadStream();
                    byte[] buff = new byte[stream.Length];
                    stream.Read(buff, 0, (int)stream.Length);
                    temp.Cover = buff;
                }
                context.Update(temp);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || context.Books == null)
            {
                return NotFound();
            }
            var book = await context.Books
                .Include(c => c.Authors)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        public IActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBook([Bind]EditBookVM book)
        {
            try
            {
                var added = _mapper.Map<Book>(book);
                if (book.TempCover is null)
                    context.Add(added);
                else
                {
                    var stream = book.TempCover.OpenReadStream();
                    byte[] buff = new byte[stream.Length];
                    stream.Read(buff, 0, (int)stream.Length);
                    added.Cover = buff;
                    context.Add(added);
                }
                context.SaveChanges();
                return View("BookDetails", added);
            }
            catch
            {
                return Problem("Error while adding a book.");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (context.Books == null)
            {
                return Problem();
            }
            var book = await context.Books.FindAsync(id);
            if (book != null)
            {
                context.Books.Remove(book);
            }
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}