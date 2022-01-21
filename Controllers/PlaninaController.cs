using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;

namespace Projekat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlaninaController : ControllerBase
    {
        public PlaninarenjeContext Context { get; set; }

        public PlaninaController(PlaninarenjeContext context)
        {
            Context=context;
        }
        [Route("Planine")]
        [HttpGet]
        public async Task<ActionResult> Preuzmi()
        {
            return Ok(await Context.Planine.Select(p=>
            new
            {
                p.IDPlanine,
                p.ImePlanine,
                p.Drzava,
                p.MaksimalnaVisina,
                p.TezinaPlanine,
                p.ImeNajvisegVrha
            }).ToListAsync());
        }

        [Route("PrikaziPlaninuIme/{ime}")]
        [HttpGet]
        public async Task<ActionResult> PrikaziPlaninuIme(String ime)
        {
            if(string.IsNullOrWhiteSpace(ime) || ime.Length>50)
            {
                return BadRequest("Ime je prazno ili ima vise od 50 karaktera!");
            }
            try
            {
                return Ok(await Context.Planine.Where(p => p.ImePlanine==ime).FirstOrDefaultAsync());
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [Route("DodajPlaninu")]
        [HttpPost]
        public async Task<ActionResult> DodajPlaninu([FromBody] Planina planina)
        {
            if(string.IsNullOrWhiteSpace(planina.ImePlanine) || planina.ImePlanine.Length >50 )
            {
                return BadRequest("Pogresno ime!");
            }
            if(string.IsNullOrWhiteSpace(planina.Drzava) || planina.Drzava.Length >50 )
            {
                return BadRequest("Presli ste granicu za slova kod drzave!");
            }
            if(string.IsNullOrWhiteSpace(planina.ImeNajvisegVrha) || planina.ImeNajvisegVrha.Length >50 )
            {
                return BadRequest("Pogresno ime najviseg vrha!");
            }
            if(planina.TezinaPlanine<0 || planina.TezinaPlanine>10)
            {
                return BadRequest("Tezina planine moze da bude samo u rasponu od 0 do 10!");
            }
            if(planina.MaksimalnaVisina<0 || planina.MaksimalnaVisina>8850)
            {
                return BadRequest("Najvisi vrh na svetu je 8848m pa ne postoji visi vrh!");
            }
            try
            {
                Context.Planine.Add(planina);
                await Context.SaveChangesAsync();
                return Ok($"Planina je uspeno dodata! ID te planine je: {planina.IDPlanine}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajPlaninuParametri/{ime}/{drzava}/{maxVisina}/{vrh}/{tezina}")]
        [HttpPost]
        public async Task<ActionResult> DodajPlaninuParametri(String ime, String drzava, int maxVisina, String vrh, int tezina)
        {
            if(string.IsNullOrWhiteSpace(ime) || ime.Length >50 )
            {
                return BadRequest("Pogresno ime!");
            }
            if(string.IsNullOrWhiteSpace(drzava) || drzava.Length >50 )
            {
                return BadRequest("Presli ste granicu za slova kod drzave!");
            }
            if(string.IsNullOrWhiteSpace(vrh) || vrh.Length >50 )
            {
                return BadRequest("Pogresno ime najviseg vrha!");
            }
            if(tezina<0 || tezina>10)
            {
                return BadRequest("Tezina planine moze da bude samo u rasponu od 0 do 10!");
            }
            if(maxVisina<0 || maxVisina>8850)
            {
                return BadRequest("Najvisi vrh na svetu je 8848m pa ne postoji visi vrh!");
            }
            try
            {
                Planina p = new Planina
                {
                    ImePlanine=ime,
                    Drzava=drzava,
                    MaksimalnaVisina=maxVisina,
                    ImeNajvisegVrha=vrh,
                    TezinaPlanine=tezina
                };
                Context.Planine.Add(p);
                await Context.SaveChangesAsync();
                return Ok($"Planina je uspeno dodata! ID te planine je: {p.IDPlanine}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzbrisiPlaninu/{id}")]
        [HttpDelete]
        public async Task<ActionResult> IzbrisiPlaninu(int id)
        {
            if(id<0)
            {
                return BadRequest("Pogrsan id!");
            }
            try
            {
                var planina = await Context.Planine.FindAsync(id);
                string ime = planina.ImePlanine;
                Context.Planine.Remove(planina);
                await Context.SaveChangesAsync();
                return Ok($"Uspeno izbrisana planina sa imenom: {ime}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzbrisiPlaninuIme/{ime}")]
        [HttpDelete]
        public async Task<ActionResult> IzbrisiPlaninu(String ime)
        {
            if(string.IsNullOrWhiteSpace(ime) || ime.Length > 50)
            {
                return BadRequest("Pogrsno ime koje se trazi!");
            }
            try
            {
                var planina = Context.Planine.Where(p => p.ImePlanine==ime).FirstOrDefault();
                Context.Planine.Remove(planina);
                await Context.SaveChangesAsync();
                return Ok($"Uspeno izbrisana planina sa imenom: {ime}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [Route("PromeniPlaninu/{ime}/{visina}/{drzava}/{vrh}/{tezina}")]
        [HttpPut]
        public async Task<ActionResult> PromeniPlaninu(String ime, int visina, String drzava, String vrh, int tezina)
        {
            if(string.IsNullOrWhiteSpace(ime) || ime.Length > 50)
            {
                return BadRequest("Pogrsno ime koje se trazi!");
            }
            if(visina < 0 || visina > 8850)
            {
                return BadRequest("Pogresna visina!");
            }
            if(string.IsNullOrWhiteSpace(drzava) || drzava.Length > 50)
            {
                return BadRequest("Pogresna drzava!");
            }
            if(string.IsNullOrWhiteSpace(vrh) || vrh.Length > 50)
            {
                return BadRequest("Pogresan vrh!");
            }
            if(tezina<0 || tezina > 10)
            {
                return BadRequest("Pogrsna tezina!");
            }
            try
            {
                var planina = Context.Planine.Where(p => p.ImePlanine==ime).FirstOrDefault();
                if(planina!=null)
                {
                    planina.MaksimalnaVisina=visina;
                    planina.Drzava=drzava;
                    planina.ImeNajvisegVrha=vrh;
                    planina.TezinaPlanine=tezina;

                    await Context.SaveChangesAsync();
                    return Ok($"Uspesno smo obavili izmenu podataka za planinu sa imenom: {ime}");
                }
                else
                {
                    return BadRequest("Planina sa ovim imenom nije pronadjena!");
                }
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [Route("PromeniPlaninuFromBody")]
        [HttpPut]
        public async Task<ActionResult> PromeniPlaninuFromBody([FromBody] Planina planina)
        {
            if(string.IsNullOrWhiteSpace(planina.ImePlanine) || planina.ImePlanine.Length > 50)
            {
                return BadRequest("Pogrsno ime koje se trazi!");
            }
            try
            {
                var pom = Context.Planine.Where(p => p.ImePlanine==planina.ImePlanine).FirstOrDefault();
                pom.MaksimalnaVisina=planina.MaksimalnaVisina;
                pom.Drzava=planina.Drzava;
                pom.ImeNajvisegVrha=planina.ImeNajvisegVrha;
                pom.TezinaPlanine=planina.TezinaPlanine;
                await Context.SaveChangesAsync();
                return Ok($"Uspesno smo obavili izmenu podataka za planinu sa imenom: {planina.ImePlanine}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        

        
    }
}
