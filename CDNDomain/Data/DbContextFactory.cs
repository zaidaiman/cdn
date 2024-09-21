using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var config = JsonConfiguration.LoadJson();
        string? connection;
        if (config?.Values?.Environment == "prod")
        {
            connection = config?.Values?.DefaultConnectionProd;
        }
        else
        {
            connection = config?.Values?.DefaultConnectionDev;
        }

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(connection);
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}

public static class JsonConfiguration
{
    public static IJsonConfiguration LoadJson()
    {
        IJsonConfiguration? item = new IJsonConfiguration();
        using (StreamReader r = new StreamReader("local.settings.json"))
        {
            string json = r.ReadToEnd();
            item = JsonSerializer.Deserialize<IJsonConfiguration>(json);
        }

        return item ?? new IJsonConfiguration();
    }
}

public class IJsonConfiguration
{
    public IValues? Values { get; set; }
}

public class IValues
{
    public string? DefaultConnectionDev { get; set; }
    public string? DefaultConnectionProd { get; set; }
    public string? Environment { get; set; }
}
