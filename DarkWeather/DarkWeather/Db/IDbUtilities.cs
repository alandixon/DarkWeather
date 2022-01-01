namespace DarkWeather.Db
{
    public interface IDbUtilities
    {
        Tt CreateDto<Tt, Ts>(Ts src);
    }
}
