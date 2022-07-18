using TaskManagerAPI.Models;

namespace TaskManagerAPI.Infra.Interfaces 
{
    public interface IUserRepository
    {
        public void SaveUser(User user);
    }
}
