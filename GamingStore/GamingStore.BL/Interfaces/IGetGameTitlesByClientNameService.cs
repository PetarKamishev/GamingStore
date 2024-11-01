namespace GamingStore.GamingStore.BL.Interfaces
{
    public interface IGetGameTitlesByClientNameService
    {
         Task<List<string>> GetGameTitlesByClientName(string gameTitle);
    }
}
