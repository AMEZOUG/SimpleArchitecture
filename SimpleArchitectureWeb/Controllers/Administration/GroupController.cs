using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleArchitectureCore.DTO;
using SimpleArchitectureDAL.Infrastructure;
using SimpleArchitectureEntities.Models;

namespace SimpleArchitectureWeb.Controllers.Administration
{
    [Route("api/group")]
    public class GroupController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public GroupController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet(Name = "GetGroups")]
        public async Task<IActionResult> Get()
        {
            var groups = await _unitOfWork.Repository<Group>().GetAllAsync();
            var groupsDto = Mapper.Map<IEnumerable<GroupDto>>(groups);
            if (groupsDto.Count() == 0)
            {
                return NotFound();
            }
            return Ok(groupsDto);
        }

        [HttpGet("{id}", Name = "GetGroup")]
        public async Task<IActionResult> Get(int id)
        {
            var group = await _unitOfWork.Repository<Group>().GetAsync(g => g.IdGroup == id);
            var groupDto = Mapper.Map<GroupDto>(group);
            if (groupDto == null)
            {
                return NotFound();
            }
            return Ok(groupDto);
        }

        [HttpPost(Name = "CreateGroup")]
        public async Task<IActionResult> Post([FromBody] GroupDto value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _unitOfWork.Repository<Group>().Add(Mapper.Map<Group>(value));
            var insertedRow = await _unitOfWork.SaveChangesAsyn();
            if (insertedRow > 0)
            {
                return CreatedAtRoute("GetGroup", new
                {
                    id = value.IdGroup
                }, value);
            }

            throw new Exception("Creating a group failed on save.");
        }

        [HttpPut("{id}", Name = "UpdateGroup")]
        public async Task<IActionResult> Put(int id, [FromBody] GroupDto value)
        {
            if (!ModelState.IsValid || id != value.IdGroup)
            {
                return BadRequest();
            }
            var groupToUpdate = _unitOfWork.Repository<Group>().Get(g => g.IdGroup == id);
            if (groupToUpdate == null)
            {
                return NotFound();
            }
            _unitOfWork.Repository<Group>().Update(Mapper.Map(value, groupToUpdate));
            var updatedRow = await _unitOfWork.SaveChangesAsyn();
            if (updatedRow > 0)
            {
                return NoContent();
            }

            throw new Exception("Updating a group failed on save.");
        }

        [HttpDelete("{id}", Name = "DeleteGroup")]
        public async Task<IActionResult> Delete(int id)
        {
            var group = _unitOfWork.Repository<Group>().Get(g => g.IdGroup == id);
            if (group == null)
            {
                return NotFound();
            }
            _unitOfWork.Repository<Group>().Delete(group);
            var deletedRow = await _unitOfWork.SaveChangesAsyn();
            if (deletedRow > 0)
            {
                return NoContent();
            }
            throw new Exception("Deleting a group failed on save.");
        }
    }
}