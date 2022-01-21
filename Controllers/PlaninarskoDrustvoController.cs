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
    public class PlaninarskoDrustvoController : ControllerBase
    {
        public PlaninarenjeContext Context { get; set; }
        public PlaninarskoDrustvoController(PlaninarenjeContext context)
        {
            Context=context;
        }

        [Route("PreuzmiPlaninarskaDrustva")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiPlaninarskaDrustva()
        {
            return Ok(await Context.PlaninarskaDrustva.Select(p => 
            new
            {
                p.IDPlaninarskogDrustva,
                p.ImePlaninarskogDrustva,
                p.Grad,
                p.Drzava,
                p.BrojClana,
                p.GodisnjaClanarina,
            }).ToListAsync());
        }

        [Route("PreuzmiPlaninarskoDrustvoPoImenu/{ime}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiPlaninarskoDrustvoPoImenu(String ime)
        {
            if(string.IsNullOrEmpty(ime) || ime.Length > 50)
            {
                return BadRequest("Ime mora da ima manje od 50 karaktera, ali mora da ima bar jedan karakter!");
            }
            try
            {
                var drustvo = await Context.PlaninarskaDrustva.Where(p => p.ImePlaninarskogDrustva==ime).FirstOrDefaultAsync();
                return Ok(drustvo);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [Route("DodajPlaninaraUPlaninarskoDrustvo/{jmbg}/{drustvoNaziv}")]
        [HttpPost]
        public async Task<ActionResult> DodajPlaninaraUPlaninarskoDrustvo(int jmbg, String drustvoNaziv)
        {
            if(jmbg<0)
            {
                return BadRequest("Pogresan jmbg!");
            }
            if(string.IsNullOrEmpty(drustvoNaziv) || drustvoNaziv.Length > 50)
            {
                return BadRequest("Naziv mora da ima vise od 50 karaktera i ne sme da bude bez karaktera!");
            }
            var planinar = await Context.Planinari.Where(p => p.JMBG==jmbg).FirstOrDefaultAsync();
            var drustvo = await Context.PlaninarskaDrustva.Where(p => p.ImePlaninarskogDrustva==drustvoNaziv).FirstOrDefaultAsync();

            if(planinar.IDPlaninarskogDrustva !=null)
            {
                return BadRequest("Ovaj planinar je vec uclanjen!");
            }
            planinar.IDPlaninarskogDrustva=drustvo;
            drustvo.Planinari.Add(planinar);
            await Context.SaveChangesAsync();

            return Ok(
                drustvo.Planinari.Select(p => 
                new
                {
                        JMBG = p.JMBG,
                        Ime = p.Ime,
                        Prezime = p.Prezime,
                        Spremnost = p.Spremnost,
                        Grad = p.Grad,
                        Drzava = p.Drzava,
                })
            );     
        }

        

        [Route("DodajPlaninarskoDrustvoFromBody")]
        [HttpPost]
        public async Task<ActionResult> DodajPlaninarskoDrustvoFromBody([FromBody] PlaninarskoDrustvo planinarskoDrustvo)
        {
            if(string.IsNullOrEmpty(planinarskoDrustvo.ImePlaninarskogDrustva) || planinarskoDrustvo.ImePlaninarskogDrustva.Length > 50)
            {
                return BadRequest("Ime drustva ne sme da bude prazno i ne sme da ima vise od 50 karaktera!");
            }
            if(string.IsNullOrEmpty(planinarskoDrustvo.Grad) || planinarskoDrustvo.Grad.Length > 50)
            {
                return BadRequest("Grad drustva ne sme da bude prazno i ne sme da ima vise od 50 karaktera!");
            }
            if(string.IsNullOrEmpty(planinarskoDrustvo.Drzava) || planinarskoDrustvo.Drzava.Length > 50)
            {
                return BadRequest("Drzava drustva ne sme da bude prazno i ne sme da ima vise od 50 karaktera!");
            }
            if(planinarskoDrustvo.BrojClana < 0)
            {
                return BadRequest("Broj clana mora da bude pozitivan broj!");
            }
            if(planinarskoDrustvo.GodisnjaClanarina < 0)
            {
                return BadRequest("Clanarina ne sme da bude negativan broj!");
            }
            try
            {
                Context.PlaninarskaDrustva.Add(planinarskoDrustvo);
                await Context.SaveChangesAsync();
                return Ok($"Planinarsko drustvo sa ID: {planinarskoDrustvo.IDPlaninarskogDrustva} i imenom: {planinarskoDrustvo.ImePlaninarskogDrustva} je uspesno dodato.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajPlaninarskoDrustvoParametri/{ime}/{grad}/{drzava}/{brClana}/{clanarina}")]
        [HttpPost]
        public async Task<ActionResult> DodajPlaninarskoDrustvoParametri(String ime, String grad, String drzava, int brClana, int clanarina)
        {
            if(string.IsNullOrEmpty(ime) || ime.Length > 50)
            {
                return BadRequest("Ime drustva ne sme da bude prazno i ne sme da ima vise od 50 karaktera!");
            }
            if(string.IsNullOrEmpty(grad) || grad.Length > 50)
            {
                return BadRequest("Grad drustva ne sme da bude prazno i ne sme da ima vise od 50 karaktera!");
            }
            if(string.IsNullOrEmpty(drzava) || drzava.Length > 50)
            {
                return BadRequest("Drzava drustva ne sme da bude prazno i ne sme da ima vise od 50 karaktera!");
            }
            if(brClana < 0)
            {
                return BadRequest("Broj clana mora da bude pozitivan broj!");
            }
            if(clanarina < 0)
            {
                return BadRequest("Clanarina ne sme da bude negativan broj!");
            }
            try
            {
                PlaninarskoDrustvo p = new PlaninarskoDrustvo
                {
                    ImePlaninarskogDrustva=ime,
                    Grad=grad,
                    Drzava=drzava,
                    BrojClana=brClana,
                    GodisnjaClanarina=clanarina
                };
                Context.PlaninarskaDrustva.Add(p);
                await Context.SaveChangesAsync();
                return Ok($"Planinarsko drustvo sa ID: {p.IDPlaninarskogDrustva} i imenom: {p.ImePlaninarskogDrustva} je uspesno dodato.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [Route("PromeniPlaninarskoDrustvoFromBody")]
        [HttpPut]
        public async Task<ActionResult> PromeniPlaninarskoDrustvoFromBody([FromBody] PlaninarskoDrustvo planinarskoDrustvo)
        {
            if(string.IsNullOrWhiteSpace(planinarskoDrustvo.ImePlaninarskogDrustva) || planinarskoDrustvo.ImePlaninarskogDrustva.Length > 50)
            {
                return BadRequest("Pogrsno ime koje se trazi!");
            }
            if(string.IsNullOrEmpty(planinarskoDrustvo.Grad) || planinarskoDrustvo.Grad.Length > 50)
            {
                return BadRequest("Grad drustva ne sme da bude prazno i ne sme da ima vise od 50 karaktera!");
            }
            if(string.IsNullOrEmpty(planinarskoDrustvo.Drzava) || planinarskoDrustvo.Drzava.Length > 50)
            {
                return BadRequest("Drzava drustva ne sme da bude prazno i ne sme da ima vise od 50 karaktera!");
            }
            if(planinarskoDrustvo.BrojClana < 0)
            {
                return BadRequest("Broj clana mora da bude pozitivan broj!");
            }
            if(planinarskoDrustvo.GodisnjaClanarina < 0)
            {
                return BadRequest("Clanarina ne sme da bude negativan broj!");
            }
            try
            {
                var pom = Context.PlaninarskaDrustva.Where(p => p.ImePlaninarskogDrustva==planinarskoDrustvo.ImePlaninarskogDrustva).FirstOrDefault();
                pom.Grad=planinarskoDrustvo.Grad;
                pom.Drzava=planinarskoDrustvo.Drzava;
                pom.BrojClana=planinarskoDrustvo.BrojClana;
                pom.GodisnjaClanarina=planinarskoDrustvo.GodisnjaClanarina;
                await Context.SaveChangesAsync();
                return Ok($"Uspesno smo obavili izmenu podataka za planinarsko drustvo sa imenom: {planinarskoDrustvo.ImePlaninarskogDrustva}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("PromeniPlaninu/{ime}/{grad}/{drzava}/{brClana}/{clanarina}")]
        [HttpPut]
        public async Task<ActionResult> PromeniPlaninu(String ime, String grad, String drzava, int brClana, int clanarina)
        {
            if(string.IsNullOrEmpty(ime) || ime.Length > 50)
            {
                return BadRequest("Ime drustva ne sme da bude prazno i ne sme da ima vise od 50 karaktera!");
            }
            if(string.IsNullOrEmpty(grad) || grad.Length > 50)
            {
                return BadRequest("Grad drustva ne sme da bude prazno i ne sme da ima vise od 50 karaktera!");
            }
            if(string.IsNullOrEmpty(drzava) || drzava.Length > 50)
            {
                return BadRequest("Drzava drustva ne sme da bude prazno i ne sme da ima vise od 50 karaktera!");
            }
            if(brClana < 0)
            {
                return BadRequest("Broj clana mora da bude pozitivan broj!");
            }
            if(clanarina < 0)
            {
                return BadRequest("Clanarina ne sme da bude negativan broj!");
            }
            try
            {
                var drustvo = Context.PlaninarskaDrustva.Where(p => p.ImePlaninarskogDrustva==ime).FirstOrDefault();
                if(drustvo!=null)
                {
                    drustvo.Grad=grad;
                    drustvo.Drzava=drzava;
                    drustvo.BrojClana=brClana;
                    drustvo.GodisnjaClanarina=clanarina;

                    await Context.SaveChangesAsync();
                    return Ok($"Uspesno smo obavili izmenu podataka za planinarsko drustvo sa imenom: {ime}");
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

        [Route("IzbrisiPlaninarskoDrustvo/{id}")]
        [HttpDelete]
        public async Task<ActionResult> IzbrisiPlaninarskoDrustvo(int id)
        {
            if(id<0)
            {
                return BadRequest("Pogrsan id!");
            }
            try
            {
                var drustvo = await Context.PlaninarskaDrustva.FindAsync(id);
                string ime = drustvo.ImePlaninarskogDrustva;
                Context.PlaninarskaDrustva.Remove(drustvo);
                await Context.SaveChangesAsync();
                return Ok($"Uspeno izbrisano planinarsko drsutvo sa imenom: {ime}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzbrisiPlaninarskoDrustvoPoImenu/{ime}")]
        [HttpDelete]
        public async Task<ActionResult> IzbrisiPlaninarskoDrustvoPoImenu(String ime)
        {
            if(string.IsNullOrWhiteSpace(ime) || ime.Length > 50)
            {
                return BadRequest("Pogrsno ime koje se trazi!");
            }
            try
            {
                var drustvo = Context.PlaninarskaDrustva.Where(p => p.ImePlaninarskogDrustva==ime).FirstOrDefault();
                Context.PlaninarskaDrustva.Remove(drustvo);
                await Context.SaveChangesAsync();
                return Ok($"Uspeno izbrisano planinarsko drustvo sa imenom: {ime}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
