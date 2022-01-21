import { Planinarenje } from "./Planinarenje.js";
import { Dogadjaj } from "./Dogadjaj.js";
import { PlaninarskoDrustvo } from "./PlaninarskoDrustvo.js";
import { Planinar } from "./Planinar.js";

var listaDogadjaja = [];
var listaJMBG = [];
var listaDrustva = [];

fetch("https://localhost:5001/Dogadjaj/PreuzmiDogadjaje")
.then(p => {
    p.json().then(dogadjaji => {
        dogadjaji.forEach(dogadjaj => {
            //console.log(dogadjaj.idDogadjaja);
            var d = new Dogadjaj(dogadjaj.idDogadjaja, dogadjaj.imeDogadjaja, dogadjaj.imeVrha, dogadjaj.datum, dogadjaj.planina, dogadjaj.tezina, dogadjaj.brojUcesnika);
            listaDogadjaja.push(d);
        });

        fetch("https://localhost:5001/Planinar/PreuzmiPlaninare")
        .then(p => {
            p.json().then(planinari => {
                planinari.forEach(planinar => {
                    //console.log(planinar.idPlaninara)
                    var p = new Planinar(planinar.idPlaninara,planinar.ime,planinar.prezime,planinar.jmbg,planinar.spremnost,planinar.grad,planinar.drzava)
                    listaJMBG.push(p);
                });
                fetch("https://localhost:5001/PlaninarskoDrustvo/PreuzmiPlaninarskaDrustva")
                .then(p => {
                    p.json().then(drustva => {
                        drustva.forEach(drustvo => {
                            //console.log(drustvo);
                            var dr = new PlaninarskoDrustvo(drustvo.idPlaninarskogDrustva,drustvo.imePlaninarskogDrustva,drustvo.grad,drustvo.drzava,drustvo.brojClana,drustvo.clanarina)
                            listaDrustva.push(dr);
                        });
                        var f = new Planinarenje(listaJMBG, listaDogadjaja,listaDrustva);
                        // console.log(listaDrustva);
                        // console.log(listaJMBG);
                        // console.log(listaDogadjaja);
                        f.crtaj(document.body);
                    })
                })
            })
        })
    })
})

