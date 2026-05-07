using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace ITV_Luis_Vives.Config;

/// <summary>
///     Clase de configuración que lee desde appsettings.json.
/// </summary>
public record AppConfig {
    static AppConfig() {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", false, true)
            .Build();
    }

    public static IConfiguration Configuration { get; }

    public static CultureInfo Cultura =>
        CultureInfo.GetCultureInfo("es-ES");

    public static string DataFolder => Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        Configuration.GetValue<string>("Repository:Directory") ?? "data");

    public static string ConnectionString =>
        Configuration.GetValue<string>("Repository:ConnectionString") ??
        "Data Source=data/itv.db";

    public static string StorageType =>
        Configuration.GetValue<string>("Storage:Type") ?? "json";

    public static string RepositoryType {
        get {
            var type = Configuration.GetValue<string>("Repository:Type") ?? "memory";
            return type.ToLower() switch {
                "json" => "json",
                "dapper" => "dapper",
                "efcore" => "efcore",
                _ => "memory"
            };
        }
    }

    public static string ItvFile {
        get {
            var extension = StorageType.ToLower() switch {
                "json" => "json",
                "csv" => "csv",
                "bin" => "bin",
                _ => "json"
            };
            return Path.Combine(DataFolder, $"itv.{extension}");
        }
    }

    public static int CacheSize =>
        Configuration.GetValue("Cache:Size", 10);

    public static bool DropData =>
        Configuration.GetValue("Repository:DropData", false);

    public static bool SeedData =>
        Configuration.GetValue("Repository:SeedData", true);

    public static bool LogicalDelete =>
        Configuration.GetValue("Repository:LogicalDelete", true);

    public static string BackupDirectory => Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        Configuration.GetValue<string>("Backup:Directory") ?? "backup");

    public static string BackupFormat {
        get {
            var format = Configuration.GetValue<string>("Backup:Format") ?? "json";
            return format.ToLower() switch {
                "csv" => "csv",
                "bin" => "bin",
                _ => "json"
            };
        }
    }

    public static bool IsDevelopment =>
        Configuration.GetValue("Development:Enabled", false);

    public static string ReportDirectory =>
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Configuration.GetValue<string>
                ("Reports:Directory") ?? "reports"
        );

    public static bool LogToFile =>
        Configuration.GetValue("Logging:File:Enabled", true);

    public static string LogDirectory =>
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Configuration.GetValue<string>
                ("Logging:File:Directory") ?? "log"
        );

    public static int LogRetainDays =>
        Configuration.GetValue("Logging:File:RetainDays", 7);

    public static string LogLevel =>
        Configuration.GetValue<string>("Logging:File:Level") ?? "Error";

    public static string LogOutputTemplate =>
        Configuration.GetValue<string>("Logging:File:OutputTemplate") ??
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}";
}