using Task_Management_System.DTOs;
using Task_Management_System.Models;
using Task_Management_System.Repositories;

namespace Task_Management_System.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;


        public UserService(IUserRepository UserRepository)
        {
            _userRepository = UserRepository;


        }

     




    }
}

