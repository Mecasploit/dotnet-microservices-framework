using System;

namespace Play.Inventory.Service;

public static class Extensions
{
  public static InventoryItemDto AsDto(this InventoryItem item)
    => new InventoryItemDto(item.CatalogItemId,item.Quantity,item.AcquiredDate);
}
