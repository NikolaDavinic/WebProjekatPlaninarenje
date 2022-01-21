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
    public class PlaninarController : ControllerBase
    {
        public PlaninarenjeContext Context { get; set; }
        public PlaninarController(PlaninarenjeContext context)
        {
            Context=context;
        }
        [Route("PreuzmiPlaninare")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiPlaninare()
        {
            return Ok(await Context.Planinari.Select(p => 
            new
            {
                p.IDPlaninara,
                p.Ime,
                p.Prezime,
                p.JMBG,
                p.Spremnost,
                p.Grad,
                p.Drzava,
                p.IDPlaninarskogDrustva
            }).ToListAsync());
        }


        [Route("PreuzmiPlaninareIzPlaninarskogDrustva/{drustvoID}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiPlaninareIzPlaninarskogDrustva(int drustvoID)
        {
            var d = Context.PlaninarskaDrustva
                            .Include(p => p.Planinari)
                            .Where(p => p.IDPlaninarskogDrustva==drustvoID);
            var pom = await d.FirstOrDefaultAsync();
            
            return Ok(
                pom.Planinari.Select(p => 
                new
                {
                        JMBG = p.JMBG,
                        Ime = p.Ime,
                        Prezime = p.Prezime,
                        Spremnost = p.Spremnost,
                        Grad = p.Grad,
                        Drzava = p.Drzava,
                        id = p.IDPlaninara
                })
            );         
        }

        [Route("PreuzmiPlaninareZaDogadjaj/{dogID}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiPlaninareZaDogadjaj(int dogID)
        {
            var planinariPoDogadjaju = Context.PlaninariDogadjaji
                                        .Include(p => p.Planinari)
                                        .Include(p => p.Dogadjaji)
                                        .Where(p => p.Dogadjaji.IDDogadjaja==dogID);

            var pla = await planinariPoDogadjaju.ToListAsync();

            return Ok(
                pla.Select(p => 
                    new
                    {
                        JMBG = p.Planinari.JMBG,
                        Ime = p.Planinari.Ime,
                        Prezime = p.Planinari.Prezime,
                        Spremnost = p.Planinari.Spremnost,
                        Grad = p.Planinari.Grad,
                        Drzava = p.Planinari.Drzava,
                        id = p.Planinari.IDPlaninara
                    }
                ).ToList()
            );
        }


        [Route("PrezumiPlaninaraPoJMBG/{jmbg}")]
        [HttpGet]
        public async Task<ActionResult> PrezumiPlaninaraPoJMBG(int jmbg)
        {
            if(jmbg < 0)
            {
                return BadRequest("Pogresan JMBG!");
            }
            try
            {
                return Ok(await Context.Planinari.Where(p => p.JMBG==jmbg).FirstOrDefaultAsync());
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajPlaninaraFromBody")]
        [HttpPost]
        public async Task<ActionResult> DodajPlaninara([FromBody] Planinar planinar)
        {
            if(string.IsNullOrEmpty(planinar.Ime) || planinar.Ime.Length>30)
            {
                return BadRequest("Ime ne moze da ima manje od 0 karaktera ili vise od 30!");
            }
            if(string.IsNullOrEmpty(planinar.Prezime) || planinar.Prezime.Length>30)
            {
                return BadRequest("Prezime ne moze da ima manje od 0 karaktera ili vise od 30!");
            }
            foreach(var a in Context.Planinari)
            {
                if(a.JMBG == planinar.JMBG)
                {
                    return BadRequest("Ovaj jmbg vec postoji!");
                }
            }
            if(planinar.Spremnost<0 || planinar.Spremnost>10)
            {
                return BadRequest("Spremnost moze da bude u opsegu od 0 do 10!");
            }
            if(string.IsNullOrEmpty(planinar.Grad) || planinar.Grad.Length>40)
            {
                return BadRequest("Mora da se unese grad, ali ne sme da ima vise od 40 karaktera!");
            }
            if(string.IsNullOrEmpty(planinar.Drzava) || planinar.Drzava.Length>40)
            {
                return BadRequest("Mora da se unese drzava, ali ne sme da ima vise od 40 karaktera!");
            }
            try
            {
                Context.Planinari.Add(planinar);
                await Context.SaveChangesAsync();
                return Ok($"Planinar je uspesno dodat sa ID: {planinar.IDPlaninara} i JMBG: {planinar.JMBG}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("DodajPlaninara/{ime}/{prezime}/{jmbg}/{spremnost}/{grad}/{drzava}")]
        [HttpPost]
        public async Task<ActionResult> DodajPlaninaraParametri(String ime, String prezime, int jmbg, int spremnost, String grad, String drzava)
        {
            if(string.IsNullOrEmpty(ime) || ime.Length>30)
            {
                return BadRequest("Ime ne moze da ima manje od 0 karaktera ili vise od 30!");
            }
            if(string.IsNullOrEmpty(prezime) || prezime.Length>30)
            {
                return BadRequest("Prezime ne moze da ima manje od 0 karaktera ili vise od 30!");
            }
            foreach(var a in Context.Planinari)
            {
                if(a.JMBG == jmbg)
                {
                    return BadRequest("Ovaj jmbg vec postoji!");
                }
            }
            if(spremnost<0 || spremnost>10)
            {
                return BadRequest("Spremnost moze da bude u opsegu od 0 do 10!");
            }
            if(string.IsNullOrEmpty(grad) || grad.Length>40)
            {
                return BadRequest("Mora da se unese grad, ali ne sme da ima vise od 40 karaktera!");
            }
            if(string.IsNullOrEmpty(drzava) || drzava.Length>40)
            {
                return BadRequest("Mora da se unese drzava, ali ne sme da ima vise od 40 karaktera!");
            }
            try
            {
                Planinar p = new Planinar
                {
                    Ime=ime,
                    Prezime=prezime,
                    JMBG=jmbg,
                    Spremnost=spremnost,
                    Grad=grad,
                    Drzava=drzava,
                };
                Context.Planinari.Add(p);
                await Context.SaveChangesAsync();
                return Ok($"Planinar je uspesno dodat sa ID: {p.IDPlaninara} i JMBG: {p.JMBG}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("IzbrisiPlaninara/{id}")]
        [HttpDelete]
        public async Task<ActionResult> IzbrisiPlaninara(int id)
        {
            if(id<0)
            {
                return BadRequest("ID ne moze da bude negativan broj!");
            }
            try
            {
                var planinar = await Context.Planinari.FindAsync(id);
                String ime = planinar.Ime;
                String prezime = planinar.Prezime;
                Context.Planinari.Remove(planinar);
                await Context.SaveChangesAsync();
                return Ok($"Planinar sa imenom:{ime} i prezmenom:{prezime} je uspesno izbrisan.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("IzbrisiPlaninaraSaJMBG/{jmbg}")]
        [HttpDelete]
        public async Task<ActionResult> IzbrisiPlaninu(int jmbg)
        {
            if(jmbg<0)
            {
                return BadRequest("ID ne moze da bude negativan broj!");
            }
            try
            {
                var planinar = Context.Planinari.Where(p => p.JMBG==jmbg).FirstOrDefault();
                String ime = planinar.Ime;
                String prezime = planinar.Prezime;
                Context.Planinari.Remove(planinar);
                await Context.SaveChangesAsync();
                return Ok($"Planinar sa imenom:{ime} i prezmenom:{prezime} je uspesno izbrisan.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("PromeniPlaninara/{ime}/{prezime}/{jmbg}/{spremnost}/{grad}/{drzava}")]
        [HttpPut]
        public async Task<ActionResult> PromeniPlaninu(String ime, String prezime, int jmbg, int spremnost, String grad, String drzava)
        {
            if(string.IsNullOrEmpty(ime) || ime.Length>30)
            {
                return BadRequest("Ime ne moze da ima manje od 0 karaktera ili vise od 30!");
            }
            if(string.IsNullOrEmpty(prezime) || prezime.Length>30)
            {
                return BadRequest("Prezime ne moze da ima manje od 0 karaktera ili vise od 30!");
            }
            if(jmbg<0)
            {
                return BadRequest("JMBG mora da bude pozitivan broj!");
            }
            if(spremnost < 0 || spremnost > 10)
            {
                return BadRequest("Spremnost moze da bude u opsegu od 0 do 10!");
            }
            if(string.IsNullOrEmpty(grad) || grad.Length>40)
            {
                return BadRequest("Mora da se unese grad, ali ne sme da ima vise od 40 karaktera!");
            }
            if(string.IsNullOrEmpty(drzava) || drzava.Length>40)
            {
                return BadRequest("Mora da se unese drzava, ali ne sme da ima vise od 40 karaktera!");
            }
            try
            {
                var planinar = Context.Planinari.Where(p => p.JMBG==jmbg).FirstOrDefault();
                if(planinar!=null)
                {
                    planinar.Ime=ime;
                    planinar.Prezime=prezime;
                    planinar.Spremnost=spremnost;
                    planinar.Grad=grad;
                    planinar.Drzava=drzava;

                    await Context.SaveChangesAsync();
                    return Ok($"Uspesno smo obavili izmenu podataka za planinara sa imenom: {ime} i prezimenom: {prezime}");
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
        [Route("PromeniPlaninaraFromBody")]
        [HttpPut]
        public async Task<ActionResult> PromeniPlaninaraFromBody([FromBody] Planinar planinar)
        {
            if(planinar.JMBG < 0)
            {
                return BadRequest("JMBG ne sme da bude manje od nule!");
            }
            try
            {
                var pom = Context.Planinari.Where(p => p.JMBG==planinar.JMBG).FirstOrDefault();
                pom.Ime=planinar.Ime;
                pom.Prezime=planinar.Prezime;
                pom.Spremnost=planinar.Spremnost;
                pom.Grad=planinar.Grad;
                pom.Drzava=planinar.Drzava;
                await Context.SaveChangesAsync();
                return Ok($"Uspesno smo obavili izmenu podataka za planinara sa imenom: {planinar.Ime} i prezimenom: {planinar.Prezime}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
