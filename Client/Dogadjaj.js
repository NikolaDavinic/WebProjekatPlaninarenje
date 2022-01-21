export class Dogadjaj{

    constructor(id,imeDogadjaja,imeVrha,datum,planina,tezina,brojUCesnika){

        this.id=id;
        this.imeDogadjaja=imeDogadjaja;
        this.imeVrha=imeVrha;
        this.datum=datum;
        this.planina=planina;
        this.tezina=tezina;
        this.brojUCesnika=brojUCesnika;
    }

    dodajSelectFju(red)
    {
        let tabela = document.getElementById("glavnaTabela");
        
        red.addEventListener("click", () => {
            tabela.childNodes.forEach(p => {
                if (p.className != "zaglavlje") {
                    p.className = "redTabela";
                    p.id = "";
                }
            });
            red.classList += " selRed";
            red.id = "selRed";
            console.log("radi");
        });
    }

    crtaj(host){

        var tr = document.createElement("tr");
        tr.className="redTabela";
        tr.value=this.id;

        this.dodajSelectFju(tr);
        host.appendChild(tr);

        var el = document.createElement("td");
        el.innerHTML=this.imeDogadjaja;
        tr.appendChild(el);

        el = document.createElement("td");
        el.innerHTML=this.imeVrha;
        tr.appendChild(el);

        el = document.createElement("td");
        el.innerHTML=this.datum;
        tr.appendChild(el);

        el = document.createElement("td");
        el.innerHTML=this.planina;
        tr.appendChild(el);

        el = document.createElement("td");
        el.innerHTML=this.tezina;
        tr.appendChild(el);

        el = document.createElement("td");
        el.innerHTML=this.brojUCesnika;
        tr.appendChild(el);
    }
}