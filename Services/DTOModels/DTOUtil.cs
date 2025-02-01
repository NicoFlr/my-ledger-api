using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;

namespace Services.DTOModels
{
    public class DTOUtil
    {
        /* Categories */
        public static Category MapCategoryDTO(CategoryDTO categoryDTO)
        {
            Category category = new Category();

            category.Id = categoryDTO.Id;
            category.Name = categoryDTO.Name;

            return category;
        }

        public static CategoryDTO MapCategoryToDTO(Category category)
        {
            CategoryDTO categoryDTO = new CategoryDTO();

            categoryDTO.Id = category.Id;
            categoryDTO.Name = category.Name;

            if (category.Transactions != null)
            {
                categoryDTO.Transactions = MapTransactionToDTOList(category.Transactions.ToList());
            }

            return categoryDTO;
        }

        public static List<Category> MapCategoryDTOList(List<CategoryDTO> categoryDTOList)
        {
            List<Category> categoryList = new List<Category>();
            foreach (CategoryDTO categoryDTO in categoryDTOList)
            {
                categoryList.Add(MapCategoryDTO(categoryDTO));
            }

            return categoryList;
        }

        public static List<CategoryDTO> MapCategoryToDTOList(List<Category> categoryList)
        {
            List<CategoryDTO> categoryDTOList = new List<CategoryDTO>();
            foreach (Category category in categoryList)
            {
                categoryDTOList.Add(MapCategoryToDTO(category));
            }

            return categoryDTOList;
        }

        /* Roles */
        public static Role MapRoleDTO(RoleDTO roleDTO)
        {
            Role role = new Role();

            role.Id = roleDTO.Id;
            role.Name = roleDTO.Name;

            return role;
        }

        public static RoleDTO MapRoleToDTO(Role role)
        {
            RoleDTO roleDTO = new RoleDTO();

            roleDTO.Id = role.Id;
            roleDTO.Name = role.Name;

            return roleDTO;
        }

        public static RoleDTO MapRoleToDTOWithUsers(Role role)
        {
            RoleDTO roleDTO = new RoleDTO();

            roleDTO.Id = role.Id;
            roleDTO.Name = role.Name;

            if (role.Users != null)
            {
                roleDTO.Users = MapUserToDTOList(role.Users.ToList());
            }

            return roleDTO;
        }

        public static List<Role> MapRoleDTOList(List<RoleDTO> roleDTOList)
        {
            List<Role> roleList = new List<Role>();
            foreach (RoleDTO roleDTO in roleDTOList)
            {
                roleList.Add(MapRoleDTO(roleDTO));
            }

            return roleList;
        }

        public static List<RoleDTO> MapRoleToDTOList(List<Role> roleList)
        {
            List<RoleDTO> roleDTOList = new List<RoleDTO>();
            foreach (Role role in roleList)
            {
                roleDTOList.Add(MapRoleToDTO(role));
            }

            return roleDTOList;
        }

        /* Transactions */
        public static Transaction MapTransactionDTO(TransactionDTO transactionDTO)
        {
            Transaction transaction = new Transaction();

            transaction.Id = transactionDTO.Id;
            transaction.Money = transactionDTO.Money;
            transaction.DateTime = transactionDTO.DateTime;
            transaction.IsBill = transactionDTO.IsBill;
            transaction.CategoryId = transactionDTO.CategoryId;

            return transaction;
        }

        public static TransactionDTO MapTransactionToDTO(Transaction transaction)
        {
            TransactionDTO transactionDTO = new TransactionDTO();

            transactionDTO.Id = transaction.Id;
            transactionDTO.Money = transaction.Money;
            transactionDTO.DateTime = transaction.DateTime;
            transactionDTO.IsBill = transaction.IsBill;
            transactionDTO.CategoryId = transaction.CategoryId;


            if (transaction.Category != null)
            {
                transactionDTO.Category = MapCategoryToDTO(transaction.Category);
            }

            if(transaction.Users != null)
            {
                transactionDTO.Users = MapUserToDTOList(transaction.Users.ToList());
            }

            return transactionDTO;
        }

        public static List<Transaction> MapTransactionDTOList(List<TransactionDTO> transactionDTOList)
        {
            List<Transaction> transactionList = new List<Transaction>();
            foreach (TransactionDTO transactionDTO in transactionDTOList)
            {
                transactionList.Add(MapTransactionDTO(transactionDTO));
            }

            return transactionList;
        }

        public static List<TransactionDTO> MapTransactionToDTOList(List<Transaction> transactionList)
        {
            List<TransactionDTO> transactionDTOList = new List<TransactionDTO>();
            foreach (Transaction transaction in transactionList)
            {
                transactionDTOList.Add(MapTransactionToDTO(transaction));
            }

            return transactionDTOList;
        }

        /* Users */
        public static User MapUserDTO(UserDTO userDTO)
        {
            User user = new User();

            user.Id = userDTO.Id;
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.Email = userDTO.Email;
            user.RoleId = userDTO.RoleId;
            user.IsDeleted = userDTO.IsDeleted;

            return user;
        }

        public static UserDTO MapUserToDTO(User user)
        {
            UserDTO userDTO = new UserDTO();

            userDTO.Id = user.Id;
            userDTO.FirstName = user.FirstName;
            userDTO.LastName = user.LastName;
            userDTO.Email = user.Email;
            userDTO.RoleId = user.RoleId;
            userDTO.IsDeleted = user.IsDeleted;


            if (user.Role != null)
            {
                userDTO.Role = MapRoleToDTO(user.Role);
            }

            if (user.Transactions != null)
            {
                userDTO.Transactions = MapTransactionToDTOList(user.Transactions.ToList());
            }

            return userDTO;
        }

        public static List<User> MapUserDTOList(List<UserDTO> userDTOList)
        {
            List<User> userList = new List<User>();
            foreach (UserDTO userDTO in userDTOList)
            {
                userList.Add(MapUserDTO(userDTO));
            }

            return userList;
        }

        public static List<UserDTO> MapUserToDTOList(List<User> userList)
        {
            List<UserDTO> userDTOList = new List<UserDTO>();
            foreach (User user in userList)
            {
                userDTOList.Add(MapUserToDTO(user));
            }

            return userDTOList;
        }
    }
}
