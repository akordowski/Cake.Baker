public static Cake.Core.Configuration.ICakeConfiguration GetCakeConfiguration(this ICakeContext context)
{
    var configProvider = new Cake.Core.Configuration.CakeConfigurationProvider(context.FileSystem, context.Environment);
    var arguments = (IDictionary<string, string>)context.Arguments.GetType().GetProperty("Arguments").GetValue(context.Arguments);

    return configProvider.CreateConfiguration(
        context.Environment.WorkingDirectory,
        arguments);
}

public static bool Exists(this FilePath filePath)
{
    return filePath == null ? false : System.IO.File.Exists(filePath.FullPath);
}

public static string DefaultValue(this string str, string defaultValue)
{
    return String.IsNullOrWhiteSpace(str) ? defaultValue : str;
}