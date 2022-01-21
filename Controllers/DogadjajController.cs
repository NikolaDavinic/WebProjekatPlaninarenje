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
    public class DogadjajController : ControllerBase
    {
        
        public PlaninarenjeContext Context { get; set; }

        public DogadjajController(PlaninarenjeContext context)
        {
            Context=context;
        }

        [Route("PreuzmiDogadjajeZaPlaninara/{planinarID}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiDogadjajeZaPlaninara(int planinarID)
        {
            var dogadjajiPoPlaninaru = Context.PlaninariDogadjaji
                                        .Include(p => p.Planinari)
                                        .Include(p => p.Dogadjaji)
                                        .ThenInclude(p => p.Planina)
                                        .Where(p => p.Planinari.IDPlaninara==planinarID);

            var dog = await dogadjajiPoPlaninaru.ToListAsync();

            return Ok(
                dog.Select(p => 
                    new
                    {
                        Id = p.Dogadjaji.IDDogadjaja,
                        ImeDogadjaja = p.Dogadjaji.ImeDogadjaja,
                        Vrh = p.Dogadjaji.ImeVrhaDogadjaja,
                        Datum = p.Dogadjaji.DatumOdrzavanja,
                        Planina = p.Dogadjaji.Planina.ImePlanine,
                        Tezina = p.Dogadjaji.TezinaUspona,
                        BrojUcesnika = p.Dogadjaji.BrojUcesnika
                    }
                ).ToList()
            );
        }

        [Route("DodajDogadjajZaPlaninara/{IDPlaninara}/{IDDogadjaja}")]
        [HttpPost]
        public async Task<ActionResult> DodajDogadjajZaPlaninara(int IDPlaninara, int IDDogadjaja)
        {
            try
            {
            var planinar = await Context.Planinari.Where(p => p.IDPlaninara==IDPlaninara).FirstOrDefaultAsync();
            if(planinar == null)
                return BadRequest("Ne postoji Planinar sa tim ID-jem!");
            var dogadjaj = await Context.Dogadjaji.Where(p => p.IDDogadjaja==IDDogadjaja).FirstOrDefaultAsync();
            if(dogadjaj == null)
                return BadRequest("Ne postoji dogadjaj sa tim ID-em!");
            IdeNaDogadjaj n = new IdeNaDogadjaj
            {
                Dogadjaji = dogadjaj,
                Planinari = planinar
            };
            // var provera = Context.PlaninariDogadjaji.Where(p => p.Planinari==planinar && p.Dogadjaji==dogadjaj).FirstOrDefaultAsync();
            // if(provera==null){
                Context.PlaninariDogadjaji.Add(n);
                await Context.SaveChangesAsync();
            //}
            return Ok("Uspesno smo dodali dogadjaj");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzbrisiDogadjajZaPlaninara/{IDPlaninara}/{IDDogadjaja}")]
        [HttpDelete]
        public async Task<ActionResult> IzbrisiDogadjajZaPlaninara(int IDPlaninara, int IDDogadjaja)
        {
            try
            {
            var planinar = await Context.Planinari.Where(p => p.IDPlaninara==IDPlaninara).FirstOrDefaultAsync();
            if(planinar == null)
                return BadRequest("Ne postoji Planinar sa tim ID-jem!");
            var dogadjaj = await Context.Dogadjaji.Where(p => p.IDDogadjaja==IDDogadjaja).FirstOrDefaultAsync();
            if(dogadjaj == null)
                return BadRequest("Ne postoji dogadjaj sa tim ID-em!");
            var ob = Context.PlaninariDogadjaji.Where(p => p.Dogadjaji==dogadjaj && p.Planinari==planinar).FirstOrDefault();
            if(ob!=null)
            {
                Context.PlaninariDogadjaji.Remove(ob);
                await Context.SaveChangesAsync();
                
                return Ok("Uspesno smo obrisali dogadjaj");
            }
            return BadRequest("Nije izbirsan dogadjaj!");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("PreuzmiDogadjaje")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiDogadjaje()
        {
            return Ok(await Context.Dogadjaji.Select(p => 
            new
            {
                p.IDDogadjaja,
                p.ImeDogadjaja,
                p.ImeVrhaDogadjaja,
                p.DatumOdrzavanja,
                p.Planina,
                p.TezinaUspona,
                p.BrojUcesnika
            }).ToListAsync());
        }

        
        

        [Route("PrezumiDogadjajPoDatum/{imeDogadjaja}")]
        [HttpGet]
        public async Task<ActionResult> PrezumiPlaninaraPoJMBG(String imeDogadjaja)
        {
            try
            {
                // var  dog = await Context.Dogadjaji.Where(p => p.ImeDogadjaja==imeDogadjaja).FirstOrDefaultAsync();
                // return Ok(new{
                //     dog.ImeDogadjaja,
                //     dog.ImeVrhaDogadjaja,
                //     dog.DatumOdrzavanja,
                //     dog.Planina,
                //     dog.TezinaUspona,
                //     dog.BrojUcesnika
                // });
                return Ok(await Context.Dogadjaji.Where(p => p.ImeDogadjaja==imeDogadjaja).FirstOrDefaultAsync());
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        

        [Route("DodajDogadjajFromBody")]
        [HttpPost]
        public async Task<ActionResult> DodajDogadjajFromBody([FromBody] Dogadjaj dogadjaj)
        {
            if(string.IsNullOrEmpty(dogadjaj.ImeDogadjaja) || dogadjaj.ImeDogadjaja.Length>50)
            {
                return BadRequest("Ime ne moze da ima manje od 0 karaktera ili vise od 50!");
            }
            if(string.IsNullOrEmpty(dogadjaj.ImeVrhaDogadjaja) || dogadjaj.ImeVrhaDogadjaja.Length>30)
            {
                return BadRequest("Prezime ne moze da ima manje od 0 karaktera ili vise od 30!");
            }
            if(dogadjaj.TezinaUspona < 0 || dogadjaj.TezinaUspona > 10)
            {
                return BadRequest("Tezina moze da bude u opsegu od 0 do 10!");
            }
            if(dogadjaj.BrojUcesnika < 0)
            {
                return BadRequest("Broj ucesnika mora da bude broj veci od nule!");
            }
            try
            {
                Context.Dogadjaji.Add(dogadjaj);
                await Context.SaveChangesAsync();
                return Ok($"Dogadjaj sa imenom: {dogadjaj.ImeDogadjaja} je uspesno dodat!");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajDogadjaj/{ime}/{imeVrha}/{datum}/{planina}/{tezina}/{brojU}")]
        [HttpPost]
        public async Task<ActionResult> DodajDogadjaj(String ime, String imeVrha, DateTime datum, Planina planina, int tezina, int brojU)
        {
            if(string.IsNullOrEmpty(ime) || ime.Length>50)
            {
                return BadRequest("Ime ne moze da ima manje od 0 karaktera ili vise od 50!");
            }
            if(string.IsNullOrEmpty(imeVrha) || imeVrha.Length > 30)
            {
                return BadRequest("Prezime ne moze da ima manje od 0 karaktera ili vise od 30!");
            }
            if(tezina < 0 || tezina > 10)
            {
                return BadRequest("Tezina moze da bude u opsegu od 0 do 10!");
            }
            if(brojU < 0)
            {
                return BadRequest("Broj ucesnika mora da bude broj veci od nule!");
            }
            try
            {
                Dogadjaj d = new Dogadjaj
                {
                    ImeDogadjaja=ime,
                    ImeVrhaDogadjaja=imeVrha,
                    DatumOdrzavanja=datum,
                    Planina=planina,
                    TezinaUspona=tezina,
                    BrojUcesnika=brojU
                };
                Context.Dogadjaji.Add(d);
                await Context.SaveChangesAsync();
                return Ok($"Dogadjaj sa ID: {d.IDDogadjaja} je uspesno dodat, a njegovo ime je: {d.ImeDogadjaja}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzbrisiDogadjaj/{id}")]
        [HttpDelete]
        public async Task<ActionResult> IzbrisiDogadjaj(int id)
        {
            if(id<0)
            {
                return BadRequest("ID ne moze da bude negativan broj!");
            }
            try
            {
                var dogadjaj = await Context.Dogadjaji.FindAsync(id);
                String ime = dogadjaj.ImeDogadjaja;
                Context.Dogadjaji.Remove(dogadjaj);
                await Context.SaveChangesAsync();
                return Ok($"Dogadjaj sa imenom: {ime} je uspesno izbacen!");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        

        [Route("IzbrisiPlaninaraSaImenom/{ime}")]
        [HttpDelete]
        public async Task<ActionResult> IzbrisiPlaninu(String ime)
        {
            if(string.IsNullOrEmpty(ime) || ime.Length > 50)
            {
                return BadRequest("Ime mora da ima bar jedan karakter i manje od 50 karaktera!");
            }
            try
            {
                var dog = Context.Dogadjaji.Where(p => p.ImeDogadjaja==ime).FirstOrDefault();
                Context.Dogadjaji.Remove(dog);
                await Context.SaveChangesAsync();
                return Ok($"Dogadjaj sa imenom: {ime} je uspesno izbacen");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("PromeniDogadjaj/{ime}/{imeVrha}/{datum}/{planina}/{tezina}/{brojU}")]
        [HttpPut]
        public async Task<ActionResult> PromeniPlaninu(String ime, String imeVrha, DateTime datum, Planina planina, int tezina, int brojU)
        {
            if(string.IsNullOrEmpty(ime) || ime.Length>50)
            {
                return BadRequest("Ime ne moze da ima manje od 0 karaktera ili vise od 50!");
            }
            if(string.IsNullOrEmpty(imeVrha) || imeVrha.Length > 30)
            {
                return BadRequest("Prezime ne moze da ima manje od 0 karaktera ili vise od 30!");
            }
            if(tezina < 0 || tezina > 10)
            {
                return BadRequest("Tezina moze da bude u opsegu od 0 do 10!");
            }
            if(brojU < 0)
            {
                return BadRequest("Broj ucesnika mora da bude broj veci od nule!");
            }
            
            try
            {
                var dog = Context.Dogadjaji.Where(p => p.ImeDogadjaja==ime).FirstOrDefault();
                if(dog!=null)
                {
                    dog.ImeVrhaDogadjaja=imeVrha;
                    dog.DatumOdrzavanja=datum;
                    dog.Planina=planina;
                    dog.TezinaUspona=tezina;
                    dog.BrojUcesnika=brojU;

                    await Context.SaveChangesAsync();
                    return Ok($"Uspesno smo modifikovali podatke za dogadjaj sa imeno: {dog.ImeDogadjaja}");
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
        [Route("PromeniDogadjajFromBody")]
        [HttpPut]
        public async Task<ActionResult> PromeniDogadjajFromBody([FromBody] Dogadjaj dogadjaj)
        {
            if(string.IsNullOrEmpty(dogadjaj.ImeDogadjaja) || dogadjaj.ImeDogadjaja.Length > 50)
            {
                return BadRequest("Ime ne moze da ima manje od 0 karaktera ili vise od 50!");
            }
            try
            {
                var pom = Context.Dogadjaji.Where(p => p.ImeDogadjaja==dogadjaj.ImeDogadjaja).FirstOrDefault();
                pom.ImeDogadjaja=dogadjaj.ImeDogadjaja;
                pom.ImeVrhaDogadjaja=dogadjaj.ImeVrhaDogadjaja;
                pom.DatumOdrzavanja=dogadjaj.DatumOdrzavanja;
                pom.Planina=dogadjaj.Planina;
                pom.TezinaUspona=dogadjaj.TezinaUspona;
                pom.BrojUcesnika=dogadjaj.BrojUcesnika;
                await Context.SaveChangesAsync();
                return Ok($"Uspesno smo modifikovali podatke za dogadjaj sa imeno: {dogadjaj.ImeDogadjaja}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
    }
}
