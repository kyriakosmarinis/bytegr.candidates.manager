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

        #region Add
        [HttpPost]
        public async Task<IActionResult> Add(string? degreeName, bool isEditMode, bool isInsertMode, [FromForm] CandidateDto candidateDto) {
            ViewData["Title"] = "Add degrees";
            ViewData["IsEditMode"] = isEditMode;

            if (ModelState.IsValid) {
                if (!string.IsNullOrEmpty(degreeName)) {
                    var degreeDto = new DegreeDto { Name = degreeName, CandidateId = candidateDto.Id };
                    //HttpContext.Session.AddToSessionList($"{degreeName}", degreeDto);
                    //await _candidatesRepository.InsertDegreeAsync(_mapper.Map(degreeDto, new DegreeEntity()));
                    await _candidatesRepository.InsertCandidateDegreeAsync(_mapper.Map(degreeDto, new DegreeEntity()));
                }
                var degrees = await _candidatesRepository.GetCandidateDegreesAsync(candidateDto.Id);
                candidateDto.Degrees = _mapper.Map(degrees, new List<DegreeDto>());
                //var canditateEntity = await _candidatesRepository.GetCandidateAsync(candidateDto.Id, true);
                //var dto = candidateDto.Id > 0 ? _mapper.Map(canditateEntity, new CandidateDto()) : _mapper.Map<CandidateDto>(canditateEntity);

                return View(candidateDto);
            }
            return View();
        }
        #endregion

        #region Delete
        //todo::fix bug - better implementation
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
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        //todo::fix bug - better implementation
        [HttpGet]
        public async Task<IActionResult> Add(bool isEditMode, int candidateId = 1) {
            ViewData["Title"] = "Degrees";
            ViewData["IsEditMode"] = isEditMode;
            var candidateEntity = await _candidatesRepository.GetCandidateAsync(candidateId, true);

            return View(_mapper.Map(candidateEntity, new CandidateDto()));
            //var degreesEntities = await _candidatesRepository.GetDegreesAsync();
            //return View(_mapper.Map<IEnumerable<DegreeDto>>(degreesEntities));
        }
        #endregion
    }
}

