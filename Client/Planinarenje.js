import { Planinar } from "./Planinar.js";
import { Dogadjaj } from "./Dogadjaj.js";
import { PlaninarskoDrustvo } from "./PlaninarskoDrustvo.js";
import { Planina } from "./Planina.js";

export class Planinarenje{

    constructor(listaPlaninara, listaDogadjaja, listaplaninarskihDrustva){

        this.listaPlaninara=listaPlaninara;
        this.listaDogadjaja=listaDogadjaja;
        this.listaplaninarskihDrustva=listaplaninarskihDrustva;
        this.kontejner=null;
    }

    crtaj(host){
        
        this.kontejner=document.createElement("div");
        this.kontejner.className = "GlavniKontejner";
        host.appendChild(this.kontejner);

        let kontForma = document.createElement("div");
        kontForma.className = "Forma";
        this.kontejner.appendChild(kontForma);

        this.crtajFormu(kontForma);
        this.crtajPrikaz(this.kontejner);
    }

    crtajRed(host){

        let red = document.createElement("div");
        red.className="red";
        host.appendChild(red);
        return red;
    }

    dodajPrikazZaTabeluDogadjaj()
    {
        let tabela = document.getElementById("glavnaTabela");
        while(tabela.hasChildNodes())
        {
            tabela.removeChild(tabela.firstChild);
        }
        

        let tr = document.createElement("tr");
        tr.className="zaglavlje";
        tabela.appendChild(tr);


        let zaglavlje;
        let th;
        zaglavlje = ["IME DOGADJAJA", "VRH", "DATUM", "PLANINA", "TEZINA", "BROJ UCESNIKA"];
        zaglavlje.forEach(el => {
            th = document.createElement("th");
            th.innerHTML=el;
            tr.appendChild(th);
        })
    }

    dodajPrikazZaTabeluPlaninar()
    {
        let tabela = document.getElementById("glavnaTabela");
        while(tabela.hasChildNodes())
        {
            tabela.removeChild(tabela.firstChild);
        }

        
        let tr = document.createElement("tr");
        tr.className="zaglavlje";
        tabela.appendChild(tr);

        

        let th;
        let zaglavlje = ["JMBG", "IME", "PREZIME", "GRAD", "DRZAVA", "SPREMNOST"];
        zaglavlje.forEach(el => {
            th = document.createElement("th");
            th.innerHTML=el;
            tr.appendChild(th);
        })

    }

    crtajPrikaz(host){

        let kontPrikaz = document.createElement("div");
        kontPrikaz.className="Prikaz";
        this.kontejner.appendChild(kontPrikaz);

        let tabela = document.createElement("table");
        tabela.className="tabela";
        tabela.id="glavnaTabela";
        kontPrikaz.appendChild(tabela);

        let tabelaHead = document.createElement("thead");
        tabela.appendChild(tabelaHead);

        let tr = document.createElement("tr");
        tabelaHead.appendChild(tr);

        let tabelaBody = document.createElement("tbody");
        tabelaBody.className="TabelaPodaci";
        tabela.appendChild(tabelaBody);

        
    }

    crtajFormu(host){

        let red = this.crtajRed(host);
        let l = document.createElement("label");
        l.innerHTML = "Planinarsko drustvo:";
        red.appendChild(l);

        let se = document.createElement("select");
        se.className="PlaninarskoDrustvoSelect";
        red.appendChild(se);

        let op;
        this.listaplaninarskihDrustva.forEach(drustvo => {
            op = document.createElement("option");
            op.innerHTML=drustvo.imePlaninarskogDrustva;
            op.value=drustvo.iDPlaninarskogDrustva;
            se.appendChild(op);
        });

        
        let btnClanoveDrustva = document.createElement("button");
        btnClanoveDrustva.className="ClanoveDrustva";
        btnClanoveDrustva.innerHTML="Prikazi clanove drustva";
        btnClanoveDrustva.onclick=(ev) => {
            this.nadjiPlaninareKojiPripadajuDrustvu();
            this.dodajPrikazZaTabeluPlaninar();
        }
        red.appendChild(btnClanoveDrustva);



        red = this.crtajRed(host);
        l = document.createElement("label");
        l.innerHTML = "Dogadjaj:";
        red.appendChild(l);

        se = document.createElement("select");
        se.className="DogadjajSelect";
        red.appendChild(se);

        this.listaDogadjaja.forEach(dogadjaj => {
            op = document.createElement("option");
            op.innerHTML=dogadjaj.imeDogadjaja;
            op.value=dogadjaj.id;
            se.appendChild(op);
        });

        let btnPrikaziPlaninare = document.createElement("button");
        btnPrikaziPlaninare.className="PrikaziPlaninare";
        btnPrikaziPlaninare.innerHTML="Prikazi planinare";
        btnPrikaziPlaninare.onclick=(ev) => {
            this.nadjiPlaninareZaDogadjaj();
            this.dodajPrikazZaTabeluPlaninar();
        }
        let btnUpisi = document.createElement("button");
        btnUpisi.className="Upisi";
        btnUpisi.innerHTML="Upisi dogadjaj za planinara";
        btnUpisi.onclick=(ev)=>{
            this.upisiDogadjaj();
            this.dodajPrikazZaTabeluDogadjaj();
        }

        let btnObrisi = document.createElement("button");
        btnObrisi.className="Obrisi";
        btnObrisi.innerHTML="Obrisi dogadjaj za planinara";
        btnObrisi.onclick=(ev)=>{
            this.obrisiDogadjaj();
            this.dodajPrikazZaTabeluDogadjaj();
        }
        
        red.appendChild(btnPrikaziPlaninare);
        red.appendChild(btnUpisi);


        red = this.crtajRed(host);
        red.className+="zadnjiRed";
        l = document.createElement("label");
        l.innerHTML = "Planinar:";
        red.appendChild(l);

        se = document.createElement("select");
        se.className="PlaninarSelect";
        red.appendChild(se);

        this.listaPlaninara.forEach(planinar => {
            op = document.createElement("option");
            op.innerHTML=planinar.ime + " " + planinar.prezime;
            op.value=planinar.iDPlaninara;
            se.appendChild(op);
        });

        let btnPrikaziDogadjajeZaPlaninara = document.createElement("button");
        btnPrikaziDogadjajeZaPlaninara.className = "PrikaziDogadjajeZaPlaninara";
        btnPrikaziDogadjajeZaPlaninara.innerHTML="Prikazi dogadjaje";
        btnPrikaziDogadjajeZaPlaninara.onclick=(ev) => {
            this.nadjiDogadjajeZaPlaninara();
            this.dodajPrikazZaTabeluDogadjaj();
        }
        red.appendChild(btnPrikaziDogadjajeZaPlaninara);
        
        red.appendChild(btnObrisi);
        
    }

    obrisiDogadjaj(){
        let selRe = document.getElementById("selRed");
        if(selRe===null)
        {
            window.alert("Nije selektovan dogadjaj koji zelite da obrisete!!!");
        }
        let optEl = this.kontejner.querySelector(".PlaninarSelect");
        let planinarID = optEl.options[optEl.selectedIndex].value;

        fetch("https://localhost:5001/Dogadjaj/IzbrisiDogadjajZaPlaninara/"+ planinarID +"/" + selRe.value,{
            method:"DELETE",
            headers:{
                "Content-Type": "application/json"
            }
        }).then(s => {
            if(s.ok){
                s.json().then(data => {
                    console.log(selRe.value);
                    console.log(planinarID);
                    data.forEach(pl => {
                        const dg = new Dogadjaj(pl.id, pl.imeDogadjaja, pl.vrh, pl.datum, pl.planina, pl.tezina, pl.brojUcesnika);
                        dg.crtaj(teloTabele);
                    })
                });
                this.ucitajDogadjajeZaPlaninare(selRe.value);
            }
        })
    }

    upisiDogadjaj(){
        let selektovanRed = document.getElementById("selektovanRed");
        console.log(selektovanRed);
        console.log(selektovanRed.value);
        if(selektovanRed===null)
        {
            window.alert("Nije selektovan planinar za koga zelite da dodate dogadjaj!!!");
        }
        let optionEl = this.kontejner.querySelector(".DogadjajSelect");
        let dogID = optionEl.options[optionEl.selectedIndex].value;
        
        fetch("https://localhost:5001/Dogadjaj/DodajDogadjajZaPlaninara/"+ selektovanRed.value + "/" + dogID,{
            method:"POST"
        }).then(s => {
            if(s.ok){
                s.json().then(data => {
                    data.forEach(pl => {
                        const dg = new Dogadjaj(pl.id, pl.imeDogadjaja, pl.vrh, pl.datum, pl.planina, pl.tezina, pl.brojUcesnika);
                        dg.crtaj(teloTabele);
                    })
                    
                });
                
                this.ucitajDogadjajeZaPlaninare(selektovanRed.value);
            }
        })
    }

    nadjiDogadjajeZaPlaninara(){

        let optionEl = this.kontejner.querySelector(".PlaninarSelect");
        let planinarID = optionEl.options[optionEl.selectedIndex].value;
        console.log(planinarID);

        this.ucitajDogadjajeZaPlaninare(planinarID);
    }

    ucitajDogadjajeZaPlaninare(planinarID){

        fetch("https://localhost:5001/Dogadjaj/PreuzmiDogadjajeZaPlaninara/" + planinarID,{
            method: "GET"
        }).then(s => {
            if(s.ok)
            {
                let teloTabele = document.getElementById("glavnaTabela");
                s.json().then(data => {
                    data.forEach(p => {
                        let bl = new Dogadjaj(p.id, p.imeDogadjaja, p.vrh, p.datum, p.planina, p.tezina, p.brojUcesnika);
                        bl.crtaj(teloTabele);
                    })
                })
            }
        })
    }


    nadjiPlaninareZaDogadjaj(){

        let optionEl = this.kontejner.querySelector(".DogadjajSelect");
        let dogID = optionEl.options[optionEl.selectedIndex].value;
        console.log(dogID);

        this.ucitajPlaninareZaDogadjaj(dogID);
    }

    ucitajPlaninareZaDogadjaj(dogID){

        fetch("https://localhost:5001/Planinar/PreuzmiPlaninareZaDogadjaj/" + dogID,{
            method: "GET"
        }).then(s => {
            if(s.ok)
            {
                let teloTabele = document.getElementById("glavnaTabela");
                s.json().then(data => {
                    data.forEach(p => {
                        console.log(p.id);
                        let pl = new Planinar(p.id, p.ime, p.prezime, p.jmbg, p.spremnost, p.grad, p.drzava);
                        pl.crtaj(teloTabele);
                    })
                })
            }
        })
    }

    nadjiPlaninareKojiPripadajuDrustvu(){

        let optionEl = this.kontejner.querySelector(".PlaninarskoDrustvoSelect");
        let drustvoID = optionEl.options[optionEl.selectedIndex].value;
        console.log(drustvoID);

        this.ucitajPlaninareKojiPripadajuDrustvu(drustvoID);
    }

    ucitajPlaninareKojiPripadajuDrustvu(drustvoID){

        fetch("https://localhost:5001/Planinar/PreuzmiPlaninareIzPlaninarskogDrustva/" + drustvoID,{
            method: "GET"
        }).then(s => {
            if(s.ok)
            {
                let teloTabele = document.getElementById("glavnaTabela");
                s.json().then(data => {
                    data.forEach(p => {
                        let pl = new Planinar(p.id, p.ime, p.prezime, p.jmbg, p.spremnost, p.grad, p.drzava);
                        pl.crtaj(teloTabele);
                    })
                })
            }
        })
    }

    
}