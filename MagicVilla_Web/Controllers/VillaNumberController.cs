using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.services;
using MagicVilla_Web.services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper, IVillaService villaService)
        {
            _villaNumberService = villaNumberService;
            _villaService = villaService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list = new();

            var response = await _villaNumberService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

            if(response != null && response.IsSucces)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> CreateVillaNumber()
        {
            VillaNumberCreateVM villaNumberVM = new();

            var response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

            if (response != null && response.IsSucces)
            {
                villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

            return View(villaNumberVM);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.CreateAsync<APIResponse>(model.VillaNumber, HttpContext.Session.GetString(SD.SessionToken));

                if (response != null && response.IsSucces)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    if (response.ErrorMessage.Count > 0)
                    {
                        ModelState.AddModelError("CustomError", response.ErrorMessage.FirstOrDefault());
                    }
                }
            }

            var res = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

            if (res != null && res.IsSucces)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(res.Result)).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
        {
            VillaNumberUpdateVM VillaNumberUpdateVM = new();

            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionToken));

            if (response != null && response.IsSucces)
            {
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));

                VillaNumberUpdateVM.VillaNumber = _mapper.Map<VillaNumberUpdateDTO>(model);
            }

            var responseVillaList = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

            if (responseVillaList != null && responseVillaList.IsSucces)
            {
                //Il metodo JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result)) deserializza la risposta
                //response.Result in una lista di oggetti di tipo VillaDTO. La funzione Convert.ToString()
                //viene utilizzata per convertire il risultato in una stringa prima della deserializzazione

                //La lista deserializzata viene trasformata in una collezione di oggetti SelectListItem.
                //Utilizzando il metodo Select(), ogni elemento della lista (i di tipo VillaDTO) viene mappato
                //a un nuovo oggetto SelectListItem

                //La proprietà Text del SelectListItem viene impostata sul valore della proprietà Name dell'oggetto VillaDTO.

                //La proprietà Value del SelectListItem viene impostata sul valore della proprietà Id dell'oggetto VillaDTO,
                //convertito in stringa con il metodo ToString().

                VillaNumberUpdateVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(responseVillaList.Result)).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });

                return View(VillaNumberUpdateVM);
            }

            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.UpdateAsync<APIResponse>(model.VillaNumber, HttpContext.Session.GetString(SD.SessionToken));

                if (response != null && response.IsSucces)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    if (response.ErrorMessage.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessage", response.ErrorMessage.FirstOrDefault());
                    }
                }
            }

            var errorResponse = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

            if (errorResponse != null && errorResponse.IsSucces)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(errorResponse.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {
            VillaNumberDeleteVM villaNumberDeleteVM = new();

            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionToken));

            if (response != null && response.IsSucces)
            {
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));

                villaNumberDeleteVM.VillaNumber = model;
            }

            var responseVillaList = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

            if (responseVillaList != null && responseVillaList.IsSucces)
            {
                //Il metodo JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result)) deserializza la risposta
                //response.Result in una lista di oggetti di tipo VillaDTO. La funzione Convert.ToString()
                //viene utilizzata per convertire il risultato in una stringa prima della deserializzazione

                //La lista deserializzata viene trasformata in una collezione di oggetti SelectListItem.
                //Utilizzando il metodo Select(), ogni elemento della lista (i di tipo VillaDTO) viene mappato
                //a un nuovo oggetto SelectListItem

                //La proprietà Text del SelectListItem viene impostata sul valore della proprietà Name dell'oggetto VillaDTO.

                //La proprietà Value del SelectListItem viene impostata sul valore della proprietà Id dell'oggetto VillaDTO,
                //convertito in stringa con il metodo ToString().

                villaNumberDeleteVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(responseVillaList.Result)).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });

                return View(villaNumberDeleteVM);
            }

            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
        {
            var response = await _villaNumberService.DeleteAsync<APIResponse>(model.VillaNumber.VillaNo, HttpContext.Session.GetString(SD.SessionToken));

            if (response != null && response.IsSucces)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }

            return View(model);
        }
    }
}
