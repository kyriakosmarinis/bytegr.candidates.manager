
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
        private static byte[]? tempCvBlob;
        private static CandidateDto underCreateCandidateDto = new();

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
        public IActionResult Add(bool isUnderCreate = false) {
            ViewData["Title"] = "Add candidate";
            if (isUnderCreate) return View(underCreateCandidateDto);
            return View(new CandidateDto { Id = _candidatesRepository.GetCandidateId() + 1 });
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int candidateId) {
            ViewData["Title"] = "Edit candidate";
            ViewData["IsEditMode"] = true;

            var entity = await _candidatesRepository.GetCandidateAsync(candidateId, true);
            var candidateDto = _mapper.Map(entity, new CandidateDto());
            candidateDto = await GetCandidateCvFile(candidateDto);

            return View(candidateDto);
        }
        #endregion
        
        #region Insert
        [HttpPost]
        public async Task<IActionResult> Insert(bool isEditMode, string selectedOptions, [FromForm] CandidateDto candidateDto) {
            if (!string.IsNullOrEmpty(selectedOptions)) {
                var options = JsonConvert.DeserializeObject<List<string>>(selectedOptions);

                if (isEditMode) {
                    await _candidatesRepository.RemoveDegreesAsync(candidateDto.Id);
                }
                if (options != null) {
                    foreach (var item in options) {
                        var degreeDto = new DegreeDto { Name = item, CandidateId = candidateDto.Id };
                        await _candidatesRepository.InsertCandidateDegreeAsync(_mapper.Map(degreeDto, new DegreeEntity()));
                    }
                }
            }
            candidateDto = await GetCandidateCvFile(candidateDto);

            if (await _candidatesRepository.ExistsCandidateAsync(candidateDto.Id)) {
                await _candidatesRepository.UpdateCandidateAsync(_mapper.Map<CandidateEntity>(candidateDto));//_mapper.Map(candidateDto, entity));
                return RedirectToAction(nameof(Index));
            }

            if (underCreateCandidateDto.Id == 0)
                await _candidatesRepository.InsertCandidateAsync(_mapper.Map(candidateDto, new CandidateEntity()));
            else
            {
                await _candidatesRepository.InsertCandidateAsync(_mapper.Map(underCreateCandidateDto, new CandidateEntity()));
                underCreateCandidateDto = new();
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region File
        [HttpPost]
        public async Task<IActionResult> UploadFile(bool isEditMode, string selectedOptions, IFormFile formFile, [FromForm] CandidateDto candidateDto)
        {
            ViewData["Id"] = candidateDto.Id;
            ViewData["IsEditMode"] = isEditMode;

            if (formFile != null && formFile.ContentType.ToLower() == "application/pdf") {//todo doc, docx
                candidateDto.CvBlob = await formFile.ToByteArrayAsync(ModelState);
                candidateDto.CvFile = formFile;

                if (isEditMode) {//todo better implementation - repeated code
                    ViewData["Title"] = "Edit candidate";

                    if (!string.IsNullOrEmpty(selectedOptions))
                    {
                        await _candidatesRepository.RemoveDegreesAsync(candidateDto.Id);
                        var options = JsonConvert.DeserializeObject<List<string>>(selectedOptions);

                        if (options != null)
                        {
                            foreach (var item in options)
                            {
                                var degreeDto = new DegreeDto { Name = item, CandidateId = candidateDto.Id };
                                await _candidatesRepository.InsertCandidateDegreeAsync(_mapper.Map(degreeDto, new DegreeEntity()));
                            }
                        }
                    }
                    await _candidatesRepository.UpdateCandidateAsync(_mapper.Map<CandidateEntity>(candidateDto));
                    return RedirectToAction(nameof(Edit), new { candidateId = candidateDto.Id });

                }
                underCreateCandidateDto = candidateDto;

                if (!string.IsNullOrEmpty(selectedOptions)) {//todo better implementation - repeated code
                    var options = JsonConvert.DeserializeObject<List<string>>(selectedOptions);

                    if (options != null) {
                        foreach (var item in options) {
                            var degree = new DegreeDto { Name = item, CandidateId = candidateDto.Id };
                            underCreateCandidateDto.Degrees.Add(degree);
                        }
                    }
                }
                return RedirectToAction(nameof(Add), new { isUnderCreate = true });
            }
            return BadRequest($"File error, please retry");
        }

        [HttpPost]
        public async Task<IActionResult> File([FromForm] CandidateDto candidateDto) {
            ViewData["Title"] = "download cv";

            candidateDto = await GetCandidateCvFile(candidateDto);

            if (candidateDto.CvFile.Length > 0) {
                
                return File(candidateDto.CvFile.OpenReadStream(), candidateDto.CvFile.ContentType, candidateDto.CvFile.FileName);
            }
            return BadRequest($"File download error, please retry");
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

        #region Delete
        public async Task<IActionResult> Delete(int candidateId)
        {
            await _candidatesRepository.RemoveCandidateAsync(candidateId);
            return RedirectToAction("Index");
        }
        #endregion
    }
}

