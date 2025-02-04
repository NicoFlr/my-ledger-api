using Data.Exceptions;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Managers.Category
{
    public class CategoryManager : ICategoryManager
    {
        private readonly UnitOfWork _unitOfWork;

        public CategoryManager(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public CategoryDTO Create(CategoryDTO newCategory)
        {
            try
            {
                Data.Models.Category category = new Data.Models.Category();
                category = DTOUtil.MapCategoryDTO(newCategory);
                _unitOfWork.CategoryRepository.Add(category);
                _unitOfWork.Save();
                CategoryDTO categoryDTO = DTOUtil.MapCategoryToDTO(category);
                return categoryDTO;
            }
            catch (SystemException)
            {
                throw new NoContentException("Category with specified Id was not found.");
            }
        }

        public CategoryDTO Get(Guid Id)
        {
            try
            {
                Data.Models.Category? category = _unitOfWork.GetContext().Categories.Where(a => a.Id == Id).FirstOrDefault();
                if (category != null)
                {
                    CategoryDTO categoryDTO = DTOUtil.MapCategoryToDTO(category);
                    return categoryDTO;
                }
                else
                {
                    throw new EntityNotFoundError("The category with the specified Id was not found.");
                }
            }
            catch (SystemException)
            {
                throw new UnexpectedError("Unexpected error.");
            }
        }

        public List<CategoryDTO> GetAll()
        {
            try
            {
                List<CategoryDTO> categoriesDTOList = new List<CategoryDTO>();
                List<Data.Models.Category> categoryList = _unitOfWork.GetContext().Categories.ToList();
                categoriesDTOList = DTOUtil.MapCategoryToDTOList(categoryList);
                return categoriesDTOList;
            }
            catch (SystemException)
            {
                throw new UnexpectedError("Couldnt retrieve categories list.");
            }
        }

        public CategoryDTO Update(CategoryDTO category, Guid id)
        {
            try
            {
                Data.Models.Category? foundCategory = _unitOfWork.GetContext().Categories.Where(a => a.Id == id).FirstOrDefault();
                if (foundCategory != null)
                {
                    _unitOfWork.CategoryRepository.Detach(foundCategory);
                    Data.Models.Category updatedCategory = DTOUtil.MapCategoryDTO(category);
                    updatedCategory.Id = id;
                    _unitOfWork.CategoryRepository.Update(updatedCategory);
                    _unitOfWork.Save();

                    CategoryDTO categoryDTO = DTOUtil.MapCategoryToDTO(updatedCategory);
                    return categoryDTO;
                }
                else
                {
                    throw new EntityNotFoundError("The category with the specified Id was not found.");
                }
            }
            catch (SystemException)
            {
                throw new UnexpectedError("The category with the specified Id couldnt be updated.");
            }
        }
        
        public CategoryDTO Delete(Guid id)
        {
            try
            {
                Data.Models.Category? categoryToDelete = _unitOfWork.GetContext().Categories.Where(a => a.Id == id).FirstOrDefault();
                if (categoryToDelete != null)
                {
                    List<Data.Models.Transaction> transactionsWithCategory = new List<Data.Models.Transaction>();
                    transactionsWithCategory = _unitOfWork.GetContext().Transactions.Where(t => t.CategoryId != null && t.CategoryId.Equals(id)).ToList();
                    if(transactionsWithCategory.Count > 0)
                    {
                        foreach(Data.Models.Transaction transactionWithCategoryToDelete in transactionsWithCategory)
                        {
                            transactionWithCategoryToDelete.CategoryId = null;
                        }

                        _unitOfWork.TransactionRepository.UpdateRange(transactionsWithCategory);
                        _unitOfWork.Save();
                    }
                    CategoryDTO categoryDTO = DTOUtil.MapCategoryToDTO(categoryToDelete);
                    _unitOfWork.CategoryRepository.Delete(categoryToDelete);
                    _unitOfWork.Save();
                    return categoryDTO;
                }
                else
                {
                    throw new EntityNotFoundError("The category with the specified Id was not found.");
                }
            }
            catch (SystemException)
            {
                throw new UnexpectedError("The category with the specified Id couldnt be deleted.");
            }
        }
    }
}
