using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Common;

namespace Play.Catalog.Service.Controllers{

  [ApiController]
  [Route("items")]
  public class ItemsController : ControllerBase
  {
    public readonly IRepository<Item> _itemsRepository;

    public ItemsController(IRepository<Item> itemsRepository)
    {
      _itemsRepository = itemsRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<ItemDto>> GetAsync(){
        var items =  (await _itemsRepository.GetAllAsync())
                    .Select(item => item.AsDto());
        return items;
    }

    // GET / items/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id){
      var item = await _itemsRepository.GetAsync(id);
      
      if(item == null)
        return NotFound();
      
      return item.AsDto();
    }
    [HttpPost]
    public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
    {
        var item = new Item {
          Name = createItemDto.Name,
          Description = createItemDto.Description,
          Price = createItemDto.Price,
          CreatedDate = DateTimeOffset.UtcNow
        };

        await _itemsRepository.CreateAsync(item);

        return CreatedAtAction(nameof(GetByIdAsync), new {id = item.Id}, item);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(Guid id, UpdateIemDto updateIemDto){

      var existingItem = await _itemsRepository.GetAsync(id);

      if (existingItem == null)
        return NotFound();

      existingItem.Name = updateIemDto.Name;
      existingItem.Description = updateIemDto.Description;
      existingItem.Price = updateIemDto.Price;
      
      await _itemsRepository.UpdateAsync(existingItem);
      
      return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
      var item = await _itemsRepository.GetAsync(id);

      if (item == null)
        return NotFound();

      await _itemsRepository.RemoveAsync(item.Id);

      return NoContent();
    }
  }

}