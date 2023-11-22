
using System.Text;
using System.Xml.Linq;
using AutoMapper;
using bytegr.candidates.manager.data.Dtos;
using bytegr.candidates.manager.data.Entities;
using bytegr.candidates.manager.data.Repositories;
using bytegr.candidates.manager.web.Extensions;
using bytegr.candidates.manager.web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;

namespace bytegr.candidates.manager.web.Controllers
{
    public class CandidateController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICandidatesRepository _candidatesRepository;

        #region ctor
        public CandidateController(IMapper mapper, ICandidatesRepository candidatesRepository) {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _candidatesRepository = candidatesRepository ?? throw new ArgumentNullException(nameof(candidatesRepository));
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Candidates";
            var candidateEntities = await _candidatesRepository.GetCandidatesAsync(true);
            return View(_mapper.Map<IEnumerable<CandidateDto>>(candidateEntities));
        }
        #endregion

        #region Add
        [HttpGet]
        public IActionResult Add() {
            ViewData["Title"] = "Add candidate";
            return View(new CandidateDto { Id = _candidatesRepository.GetId() + 1 });
        }

        [HttpPost]
        public async Task<IActionResult> Add(bool isEditMode, [FromForm] CandidateDto candidateDto) {
            ViewData["Title"] = "Add candidate";
            ViewData["IsEditMode"] = isEditMode;

            if (ModelState.IsValid) {
                var degrees = await _candidatesRepository.GetCandidateDegreesAsync(candidateDto.Id);
                candidateDto.Degrees = _mapper.Map(degrees, new List<DegreeDto>());

                candidateDto = await GetCandidateCvFile(candidateDto);/////////

                return View(candidateDto);
            }
            return View(new CandidateDto { Id = _candidatesRepository.GetId() + 1 });
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int candidateId) {
            ViewData["Title"] = "Edit candidate";
            ViewData["IsEditMode"] = true;

            if (!await _candidatesRepository.ExistsCandidateAsync(candidateId))
                //return View(new CandidateDto());
                return RedirectToAction("Add");

            var entity = await _candidatesRepository.GetCandidateAsync(candidateId, true);
            var candidateDto = _mapper.Map(entity, new CandidateDto());
            candidateDto = await GetCandidateCvFile(candidateDto);/////////
            return View(candidateDto);
        }
        #endregion
        
        #region Insert
        [HttpPost]
        public async Task<IActionResult> Insert([FromForm] CandidateDto candidateDto) {
            if (await _candidatesRepository.ExistsCandidateAsync(candidateDto.Id)) {
                //var entity = await _candidatesRepository.GetCandidateAsync(candidateDto.Id, true);
                await _candidatesRepository.UpdateCandidateAsync(_mapper.Map<CandidateEntity>(candidateDto));//_mapper.Map(candidateDto, entity));
                return RedirectToAction("index");
                //return RedirectToAction("index", "Home");
            }
            await _candidatesRepository.InsertCandidateAsync(_mapper.Map(candidateDto, new CandidateEntity()));
            return RedirectToAction("index");
            //return RedirectToAction("index", "Home");  
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int candidateId) {
            await _candidatesRepository.RemoveCandidateAsync(candidateId);
            return RedirectToAction("Index");
        }
        #endregion

        #region File
        [HttpPost]
        public IActionResult UploadFile(IFormFile formFile) {
            try {
                if (formFile != null && formFile.ContentType.ToLower() == "application/pdf") {//todo doc, docx
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                    var filePath = Path.Combine("wwwroot", "uploads", fileName);

                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    formFile.CopyTo(fileStream);

                    var fileDetails = new {
                        FileName = fileName,
                        FilePath = filePath,
                        FileSize = formFile.Length
                    };

                    return Ok(fileDetails);

                }
                else {
                    return BadRequest("Invalid file. Please upload a PDF file.");
                }
            }
            catch (Exception ex) {
                throw new FileLoadException(nameof(formFile));
                //StatusCode(500, $"Internal server error: {ex.Message}");
            }
            finally { }
        }


        [HttpPost]
        public IActionResult File([FromForm] CandidateDto candidateDto) {
            return RedirectToAction("Add", "Candidate", candidateDto);
        }

        public CandidateDto UpdateFile(string name, CandidateDto candidateDto) {
            return candidateDto;
        }

        private async Task<CandidateDto> GetCandidateCvFile(CandidateDto candidateDto)
        {
            _ = new CandidateDto();
            var entity = await _candidatesRepository.GetCandidateAsync(candidateDto.Id, true);
            if (entity is not null) _ = _mapper.Map(entity, new CandidateDto());
            else _ = candidateDto;

            if (await _candidatesRepository.ExistsCandidateCvAsync(candidateDto.Id))
            {
                if (await _candidatesRepository.HasCandidateCvChanged(candidateDto.Id, entity.CvBlob))
                {
                    using MemoryStream memoryStream = new MemoryStream(entity.CvBlob);
                    IFormFile file = new FormFile(memoryStream, 0, entity.CvBlob.Length, "file", string.Format("{0}", entity.LastName));
                    candidateDto.CvBlob = entity.CvBlob;
                    candidateDto.CvFile = file;
                }
                candidateDto.CvBlob = entity.CvBlob;
                candidateDto.CvFile = entity.CvBlob.ToIFormFile($"{candidateDto.LastName}", "application/pdf");//"text/plain"
            }
            return candidateDto;
        }
        #endregion
    }
}

