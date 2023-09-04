# WebBlazorEC
Sử dụng framework Blazor và net core 7

### WebBlazorEc.Server
- Ở nhánh này ta sẽ cài các packages cho EF bao gồm các gói sau (Nó nằm ở phần project Server):
  <p>
  -> Microsoft.EntityFrameworkCore.Tools
  <br/>
  -> Microsoft.AspNetCore.Identity.EntityFrameworkCore
  </p>
- Ta sẽ sử dụng lệnh trong packages manager console:
  <p>
    add-migration CreateInitial
    <br/>
    và
    <br/>
    update-database
  </p>
