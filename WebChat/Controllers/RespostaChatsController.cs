using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebChat.Db;
using WebChat.Models;
using WebChat.Models.Api;

namespace WebChat.Controllers
{
    public class RespostaChatsController : Controller
    {
        private readonly Contexto _context;

        public RespostaChatsController(Contexto context) => _context = context;

        public async Task<IActionResult> Index() => View(await _context.RespostaChat.ToListAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var respostaChat = await _context.RespostaChat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (respostaChat == null)
            {
                return NotFound();
            }

            return View(respostaChat);
        }


        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Resposta,Mensagem")] RespostaChat respostaChat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(respostaChat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(respostaChat);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var respostaChat = await _context.RespostaChat.FindAsync(id);

            if (respostaChat == null)
                return NotFound();

            return View(respostaChat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Resposta,Mensagem")] RespostaChat respostaChat)
        {
            if (id != respostaChat.Id)
                return NotFound();


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(respostaChat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RespostaChatExists(respostaChat.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(respostaChat);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();


            var respostaChat = await _context.RespostaChat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (respostaChat == null)
                return NotFound();

            return View(respostaChat);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var respostaChat = await _context.RespostaChat.FindAsync(id);
            _context.RespostaChat.Remove(respostaChat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("api/Chat")]
        #region Api
        public async Task<IActionResult> Chat(RequestApi request)
        {
            var respostaChat = await _context.RespostaChat
                .Where(m => m.Mensagem.ToUpper()
                .Contains(request.Mensagem.ToUpper())).FirstOrDefaultAsync();


            if (respostaChat == null)
            {
                var reposta = new ResponseApi { Resposta = "Não entendemos sua pergunta, poderia reformular?" };
                return Ok(reposta);
            }

            var resposta = new ResponseApi { Resposta = respostaChat.Mensagem };
            return Ok(resposta);
        }
        #endregion
        private bool RespostaChatExists(int id) =>  _context.RespostaChat.Any(e => e.Id == id);
        
    }
}
