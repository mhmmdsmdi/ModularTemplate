using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;

namespace Framework.Api.Logging;

public static class LoggingConfiguration
{
    public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger => (context, configuration) =>
    {
        #region Enriching Logger Context

        var env = context.HostingEnvironment;

        configuration.Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", env.ApplicationName)
            .Enrich.WithProperty("Environment", env.EnvironmentName)
            .Enrich.WithExceptionDetails();

        #endregion Enriching Logger Context

        // todo : use logging settings for select destination type

        configuration.WriteTo.Console().MinimumLevel.Information();

        #region Sql Server

        //var columnOpts = new ColumnOptions();
        //columnOpts.Store.Remove(StandardColumn.Properties);
        //columnOpts.Store.Add(StandardColumn.LogEvent);
        //columnOpts.LogEvent.DataLength = 4096;
        //columnOpts.PrimaryKey = columnOpts.Id;
        //columnOpts.Id.DataType = SqlDbType.Int;
        //    configuration.WriteTo
        //        .MSSqlServer(
        //            connectionString: context.Configuration.GetConnectionString("SqlServer"),
        //            sinkOptions: new MSSqlServerSinkOptions { TableName = "LogEvents", AutoCreateSqlTable = true, SchemaName = "log" })
        //        .MinimumLevel.Warning();

        #endregion Sql Server

        #region File

        //configuration.WriteTo.File("log.txt", rollingInterval: RollingInterval.Day);

        #endregion File

        #region ElasticSearch

        //var elasticUrl = context.Configuration.GetValue<string>("Logging:ElasticUrl");

        //if (!string.IsNullOrEmpty(elasticUrl))
        //{
        //    configuration.WriteTo.Elasticsearch(
        //        new ElasticsearchSinkOptions(new Uri(elasticUrl))
        //        {
        //            AutoRegisterTemplate = true,
        //            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
        //            IndexFormat = "web-logs-{0:yyyy.MM.dd}",
        //            MinimumLogEventLevel = LogEventLevel.Debug
        //        });
        //}

        #endregion ElasticSearch
    };
}