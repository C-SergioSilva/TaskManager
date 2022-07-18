using System;
using TaskManagerAPI.Infra.Interfaces;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Infra.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ContextDB context;
        public UserRepository(ContextDB context)
        {
            this.context = context; 
        }           
        public void SaveUser(User user)
        {
            try
            {
                context.User.Add(user);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
