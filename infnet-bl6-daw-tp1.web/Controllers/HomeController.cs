using infnet_bl6_daw_tp1.Domain.Entities;
using infnet_bl6_daw_tp1.Domain.Interfaces;
using infnet_bl6_daw_tp1.Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Data.Entity.Infrastructure;

namespace infnet_bl6_daw_tp1.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<HomeController> _logger;
        private readonly IAmigoService _amigoService;

        public HomeController(ILogger<HomeController> logger, IAmigoService amigoService, IMemoryCache memoryCache)
        {
            _logger = logger;
            _amigoService = amigoService;
            _memoryCache = memoryCache;

        }

        public IActionResult Index(string nomePesquisa, List<int> Selecionado)
        {
            if (Selecionado.Count > 0)
            {
                this.SetSessionList(Selecionado);
            }

            var amigosSelecionados = this.GetSessionList();
            var amigos = GetOrCreateFromCache();

            if (!string.IsNullOrEmpty(nomePesquisa))
            {
                amigos = amigos.Where(amigo => amigo.NomeCompletoPossui(nomePesquisa)).ToList();
            }
            amigos.Sort(delegate (AmigoViewModel x, AmigoViewModel y)
            {
                return x.DiasFaltantes.CompareTo(y.DiasFaltantes);
            });

            foreach (var item in amigos)
            {
                if (amigosSelecionados.Contains(item.Id))
                {
                    item.amigoSelecionado = true;
                }
                else
                {
                    item.amigoSelecionado = false;
                }
            }

            return View(amigos);
        }

        public IActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Incluir([Bind("Id,Nome,Sobrenome,Email,Nascimento")] Amigo amigo)
        {
            if (ModelState.IsValid)
            {
                _amigoService.Add(amigo);
                return RedirectToAction(nameof(Index));
            }
            return View(amigo);
        }

        public IActionResult Alterar(int? id)
        {
            if (id == null || _amigoService == null)
            {
                return NotFound();
            }

            var amigos = _amigoService.GetAll();
            var indice = amigos.FindIndex(amigo => amigo.Id == id);
            if (indice < 0) { return NotFound(); }

            return View(amigos[indice]);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Alterar(int id, [Bind("Id,Nome,Sobrenome,Email,Nascimento")] Amigo amigo)
        {
            if (id != amigo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _amigoService.Save(amigo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExisteAmigo(amigo.Id))
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
            return View(amigo);
        }

        private bool ExisteAmigo(int id)
        {
            return _amigoService.GetAll().Any(e => e.Id == id);
        }


        public IActionResult Exibir(int? id)
        {
            if (id == null || _amigoService == null)
            {
                return NotFound();
            }

            var amigos = _amigoService.GetAll();
            var indice = amigos.FindIndex(amigo => amigo.Id == id);
            if (indice < 0) { return NotFound(); }

            return View(amigos[indice]);
        }

        public IActionResult Excluir(int? id)
        {
            if (id == null || _amigoService == null)
            {
                return NotFound();
            }

            var amigos = _amigoService.GetAll();
            var indice = amigos.FindIndex(amigo => amigo.Id == id);
            if (indice < 0) { return NotFound(); }

            return View(amigos[indice]);
        }

        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarExcluir(int id)
        {
            if (_amigoService == null)
            {
                return Problem("Entity set 'infnet_bl6_fdaN_atContext.Amigo'  is null.");
            }
            var amigos = _amigoService.GetAll();
            var indice = amigos.FindIndex(amigo => amigo.Id == id);
            if (indice < 0) { return NotFound(); }
            var amigoViewModel = amigos[indice];

            Amigo amigo = new 
                Amigo();
            amigo.Id = amigoViewModel.Id;
            amigo.Nome = amigoViewModel.Nome;
            amigo.Sobrenome = amigoViewModel.Sobrenome;
            amigo.Nascimento = amigoViewModel.Nascimento;
            amigo.Email = amigoViewModel.Email;

            _amigoService.Remove(amigo);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("/home/voltaAmigosSelecionados")]
        public IActionResult VoltaAmigosSelecionados(List<int> Selecionado)
        {
            if (Selecionado.Count > 0)
            {
                this.SetSessionList(Selecionado);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("/home/amigosSelecionados")]
        public IActionResult AmigosSelecionados(List<int> Selecionado)
        {
            if (Selecionado.Count > 0)
            {
                this.SetSessionList(Selecionado);
            }

            var amigosSelecionados = this.GetSessionList();
            var amigos = GetOrCreateFromCache();

            foreach (var item in amigos)
            {
                if (amigosSelecionados.Contains(item.Id))
                {
                    item.amigoSelecionado = true;
                }
                else
                {
                    item.amigoSelecionado = false;
                }
            }

            return View(amigos);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        private void SetSessionList(List<int> amigosSelecionados)
        {
            HttpContext.Session.SetString("Selected", JsonConvert.SerializeObject(amigosSelecionados));
        }

        private List<int> GetSessionList()
        {
            var selectedList = HttpContext.Session.GetString("Selected");
            if (!string.IsNullOrEmpty(selectedList))
            {
                return JsonConvert.DeserializeObject<List<int>>(HttpContext.Session.GetString("Selected"));
            }
            return new List<int>();
        }
        private List<AmigoViewModel> GetOrCreateFromCache()
        {
            return _memoryCache.GetOrCreate("AmigosCache", settings =>
            {
                settings.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20);
                settings.SlidingExpiration = TimeSpan.FromSeconds(1);

                return _amigoService.GetAll();
            });
        }

    }
}