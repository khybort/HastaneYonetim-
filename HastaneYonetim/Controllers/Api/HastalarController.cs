using AutoMapper;
using HastaneYonetim.Core;
using HastaneYonetim.Core.Dto;
using HastaneYonetim.Core.Models;
using System.Linq;
using System.Web.Http;

namespace HastaneYonetim.Controllers.Api
{
    public class HastalarController : ApiController
    {
        private readonly IIsBirimi _isBirimi;
        public HastalarController(IIsBirimi isBirimi)
        {
            _isBirimi = isBirimi;

        }

        public IHttpActionResult HastalariGetir()
        {
            var hastalarSorgusu = _isBirimi.Hastalar.HastalariGetir();


            var hastaDto = hastalarSorgusu.ToList()
                                          .Select(Mapper.Map<Hasta, HastaDto>);

            return Ok(hastaDto);

        }


        [HttpDelete]
        public IHttpActionResult Sil(int id)
        {
            var hasta = _isBirimi.Hastalar.HastaGetir(id);
            _isBirimi.Hastalar.Kaldir(hasta);
            _isBirimi.Tamamla();
            return Ok();
        }

    }
}
