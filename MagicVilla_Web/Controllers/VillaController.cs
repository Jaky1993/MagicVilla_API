using AutoMapper;
using Azure;
using MagicVilla_VillaAPI.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Buffers.Text;
using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.PortableExecutable;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }

        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> list = new();

            var response = await _villaService.GetAllAsync<APIResponse>();

            if (response != null && response.IsSucces)
            {
                /*
                    object obj = new SomeClass(); 
                    string str = obj.ToString();

                    convert object to String in c#
                */

                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        public async Task<IActionResult> CreateVilla()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        /*
        IActionResult is an interface that allows controller actions to return various HTTP responses dynamically.
        It provides greater flexibility by enabling actions to return different HTTP status codes and response types based
        on runtime conditions.We will get the following Benefits from Using IActionResult.
        It allows actions to return different HTTP responses (e.g., Ok(), NotFound(), BadRequest())
        from a single action method dynamically based on runtime conditions.
        It allows actions to control the exact response details, such as status codes, headers, and content.
        With IActionResult as the return type, we can easily handle and return error responses like StatusCode(500).

        IActionResult is an interface in ASP.NET Core that represents the result of an action method.
        It allows you to return various types of responses, such as views, JSON, files, and more.
        The View() method is one of the ways to return a view from an action method.

        IActionResult is an interface that defines a contract for the result of an action method.
        It allows for the return of any specific result types that implement the IActionResult interface, providing maximum flexibility
        */
        public async Task<IActionResult> CreateVilla(VillaCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.CreateAsync<APIResponse>(model);

                if (response != null && response.IsSucces)
                {
                    TempData["success"] = "Villa created succesfully";

                    return RedirectToAction(nameof(IndexVilla));
                }
            }

            TempData["error"] = "Error encountered.";

            return View(model);
        }

        public async Task<IActionResult> UpdateVilla(int villaId)
        {
            var response = await _villaService.GetAsync<APIResponse>(villaId);

            if (response != null && response.IsSucces)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));

                VillaUpdateDTO villaUpdateDTO = _mapper.Map<VillaUpdateDTO>(model);

                return View(villaUpdateDTO);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.UpdateAsync<APIResponse>(model);

                if (response != null && response.IsSucces)
                {
                    TempData["success"] = "Villa update succesfully";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }

            TempData["error"] = "Error encountered.";

            return View(model);
        }

        public async Task<IActionResult> DeleteVilla(int villaId)
        {
            var response = await _villaService.GetAsync<APIResponse>(villaId);

            if (response != null && response.IsSucces)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));

                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaDTO model)
        {
            var response = await _villaService.DeleteAsync<APIResponse>(model.Id);

            if (response != null && response.IsSucces)
            {
                TempData["success"] = "Villa delete succesfully";

                return RedirectToAction(nameof(IndexVilla));
            }

            TempData["error"] = "Error encountered.";

            return View(model);
        }
    }
}
