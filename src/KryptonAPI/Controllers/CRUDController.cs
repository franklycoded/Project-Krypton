using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KryptonAPI.Data.Models;
using KryptonAPI.DataContracts;
using KryptonAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace KryptonAPI.Controllers
{
    /// <summary>
    /// Generic controller for CRUD operations
    /// </summary>
    public class CRUDController<TEntity, TDto>: Controller where TEntity: class, IEntity where TDto: class, ICRUDDto
    {
        protected readonly ICRUDManager<TEntity, TDto> Manager;

        /// <summary>
        /// Creates a new instance of the CRUDController
        /// </summary>
        /// <param name="manager">The manager to use for data retrieval and other operations</param>
        public CRUDController(ICRUDManager<TEntity, TDto> manager)
        {
            if(manager == null) throw new ArgumentNullException(nameof(manager));

            Manager = manager;
        }

        /// <summary>
        /// Gets the resource by id
        /// </summary>
        /// <param name="id">The id of the resource</param>
        /// <returns>The resource</returns>
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(long id){
            try {
                var dto = await Manager.GetByIdAsync(id);

                if(dto == null) return NotFound();

                return Ok(dto);
            }
            catch(Exception){
                // Log error
                return StatusCode(500, "Error while getting item by id " + id + ". See logs for details!");
            }
        }

        /// <summary>
        /// Creates a new instance of the resource
        /// </summary>
        /// <param name="dto">The data contract representation of the new resource instance</param>
        /// <returns>The new resource instance</returns>
        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TDto dto){
            try
            {
                var newDto = await Manager.AddAsync(dto);
                return Ok(newDto);
            }
            catch (Exception)
            {
                // log error
                return StatusCode(500, "Error while adding item. See logs for details!");
            }
        }

        /// <summary>
        /// Modifies the data of the existing resource
        /// </summary>
        /// <param name="dto">The data contract representation of the modifiable resource</param>
        /// <returns>The modified resource</returns>
        [HttpPut]
        public virtual async Task<IActionResult> Put([FromBody] TDto dto){
            try {
                var updatedDto = await Manager.UpdateAsync(dto);

                if(updatedDto == null) return NotFound();

                return Ok(updatedDto);
            }
            catch(Exception){
                // Log error
                return StatusCode(500, "Error while updating item! See logs for details!");
            }
        }

        /// <summary>
        /// Deletes a resource using the id
        /// </summary>
        /// <param name="id">The id of the resource to delete</param>
        /// <returns>True if deleted successfully, false otherwise</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id){
            try
            {
                var isDeleteSuccessful = await Manager.DeleteAsync(id);

                if(isDeleteSuccessful == false) return NotFound();

                return Ok();
            }
            catch (System.Exception)
            {
                // log error
                return StatusCode(500, "Error while deleting item with id " + id + ". See logs for details!");
            }
        }
    }
}
