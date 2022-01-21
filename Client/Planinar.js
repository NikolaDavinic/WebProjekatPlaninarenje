export class Planinar{

    constructor(iDPlaninara,ime,prezime,jmbg,spremnost,grad,drzava){
        this.iDPlaninara=iDPlaninara;
        this.ime=ime;
        this.prezime=prezime;
        this.jmbg=jmbg;
        this.spremnost=spremnost;
        this.grad=grad;
        this.drzava=drzava;
    }

    dodajSelectFju(red)
    {
        let tabela = document.getElementById("glavnaTabela");
        
        red.addEventListener("click", () => {
            console.log("radi");
            tabela.childNodes.forEach(p => {
                if (p.className != "zaglavlje") {
                    p.className = "redUTabeli";
                    p.id = "";
                }
            });
            red.classList += " selektovanRed";
            red.id = "selektovanRed";
        });
    }

    crtaj(host){

        var tr = document.createElement("tr");
        tr.className="redUTabeli";
        tr.value = this.iDPlaninara;

        this.dodajSelectFju(tr);
        host.appendChild(tr);

        var el = document.createElement("td");
        el.innerHTML=this.jmbg;
        tr.appendChild(el);

        el = document.createElement("td");
        el.innerHTML=this.ime;
        tr.appendChild(el);

        el = document.createElement("td");
        el.innerHTML=this.prezime;
        tr.appendChild(el);

        el = document.createElement("td");
        el.innerHTML=this.grad;
        tr.appendChild(el);

        el = document.createElement("td");
        el.innerHTML=this.drzava;
        tr.appendChild(el);

        el = document.createElement("td");
        el.innerHTML=this.spremnost;
        tr.appendChild(el);
    }
}