using Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Managers.Category
{
    public interface ICategoryManager
    {
        CategoryDTO Get(Guid Id);
        List<CategoryDTO> GetAll();
        CategoryDTO Create(CategoryDTO newCategory);
        CategoryDTO Update(CategoryDTO categoryToUpdate, Guid id);
        CategoryDTO Delete(Guid id);
    }
}
