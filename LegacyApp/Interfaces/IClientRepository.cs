using LegacyApp.Models;

namespace LegacyApp.Interfaces
{
    public interface IClientRepository
    {
        Client GetById(int id);
    }
}
