namespace SchoolAccount.Application.Organisations.GetByLaestab;

public class LaestabValue
{
    private readonly string _laestab;

    public LaestabValue(string laestab)
    {
        if (laestab.Length != 7)
        {
            throw new ArgumentException("Laestab must be 7 characters long");
        }

        _laestab = laestab;
    }

    public string LocalAuthorityCode => _laestab[..3];
    public string EstablishmentNo => _laestab[3..];
}
