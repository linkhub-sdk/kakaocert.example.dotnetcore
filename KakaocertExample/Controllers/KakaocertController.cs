using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Kakaocert;


namespace KakaocertExample.Controllers
{
    public class KakaocertController : Controller
    {
        private readonly KakaocertService _kakaocertService;

        public KakaocertController(KakaocertInstance KCinstance)
        {
            //Kakaocert 서비스 객체 생성
            _kakaocertService = KCinstance.kakaocertService;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
