using MagicVilla_Web.Models.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.VM
{
    public class VillaNumberDeleteVM
    {
        public VillaNumberDeleteVM()
        {
            VillaNumber = new VillaNumberDTO();
        }

        public VillaNumberDTO VillaNumber { get; set; }

        /*
        Il codice [ValidateNever] è un attributo di data annotation che fa parte di ASP.NET Core.
        Questo attributo viene utilizzato per indicare che una proprietà specifica non dovrebbe essere
        convalidata durante il binding del modello. È particolarmente utile quando si desidera escludere 
        alcune proprietà dalla convalida del modello per evitare errori durante la binding  
        */
        [ValidateNever]
        /*
        La proprietà public IEnumerable<SelectListItem> VillaList è una dichiarazione in C# che rappresenta un elenco di
        elementi selezionabili, tipicamente utilizzato nelle applicazioni web ASP.NET per popolare controlli drop-down (select).
        Ecco una spiegazione dettagliata:
        IEnumerable<SelectListItem>: IEnumerable è un'interfaccia che rappresenta una collezione di elementi su cui
        è possibile iterare. SelectListItem è una classe che rappresenta un elemento in una lista di selezione,
        con proprietà come Text (il testo visualizzato), Value (il valore dell'elemento)
        e Selected (un booleano che indica se l'elemento è selezionato).
        */
        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
}
