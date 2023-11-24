using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using bytegr.candidates.manager.data.Dtos;
using bytegr.candidates.manager.data.Entities;
using bytegr.candidates.manager.data.Repositories;
using bytegr.candidates.manager.web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace bytegr.candidates.manager.web.Controllers
{
    public class DegreeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICandidatesRepository _candidatesRepository;

        public DegreeController(IMapper mapper, ICandidatesRepository candidatesRepository) {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _candidatesRepository = candidatesRepository ?? throw new ArgumentNullException(nameof(candidatesRepository));
        }

        #region Index
        [HttpGet]
        public async Task<IActionResult> Index(bool isEditMode) {
            ViewData["Title"] = "Degrees";
            ViewData["IsEditMode"] = isEditMode;
            var degreesEntities = await _candidatesRepository.GetDegreesAsync();
            return View(_mapper.Map<IEnumerable<DegreeDto>>(degreesEntities));
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int degreeId, bool isEditMode, bool isCandidate, [FromForm] CandidateDto candidateDto) {
            ViewData["Title"] = "Add degrees";
            ViewData["IsEditMode"] = isEditMode;

            if (ModelState.IsValid) {
                await _candidatesRepository.RemoveDegreeAsync(degreeId);

                if (isCandidate) {
                    var candidateEntity = await _candidatesRepository.GetCandidateAsync(candidateDto.Id, true);
                    var updatedCandidate = _mapper.Map(candidateEntity, candidateDto);

                    return RedirectToAction("Add", isEditMode);
                }
                else {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }
        #endregion
    }
}

