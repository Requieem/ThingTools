public static class AssemblyUtility
{
    public static List<Assembly> GetUserDefinedAssemblies()
    {
        List<Assembly> userDefinedAssemblies = new List<Assembly>();

        string assetFolderPath = Application.dataPath;
        string[] assemblyFiles = Directory.GetFiles(assetFolderPath, "*.dll", SearchOption.AllDirectories);

        foreach (string assemblyFile in assemblyFiles)
        {
            if (!assemblyFile.Contains("Library") && !assemblyFile.Contains("Temp"))
            {
                Assembly assembly = Assembly.LoadFrom(assemblyFile);
                userDefinedAssemblies.Add(assembly);
            }
        }

        return userDefinedAssemblies;
    }
}
