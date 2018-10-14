using System.Collections.Generic;
using System.Linq;
using Fody;
using Mono.Cecil;

public class MvxPluginsLoaderWeaver : BaseModuleWeaver
{
    public override void Execute()
    {
        // first: grab plugin assemblies

        var references = References.Split(';');

        // loop through references
        // we can define assembly names or keys to look at using Config xml properties in FodyWeavers.xml
        foreach (var assemblyReference in references.Where(name => name.Contains(".Plugin")))
        {
            LogInfo($"Reference is: {assemblyReference}");

            // load assembly
            var assembly = AssemblyDefinition.ReadAssembly(assemblyReference);

            LogInfo($"Resolved {assembly.Name}. Types are:");
            // here we loop assembly types which implement IMvxPlugin explicitly
            foreach (var t in assembly.MainModule.GetTypes().Where(t => t.HasInterfaces && t.Interfaces.Any(i => i.InterfaceType.Name == "IMvxPlugin")))
            {
                LogInfo($"We have found our plugin! Type is: {t.Name}");
            }
        }

        // second: inject code to somehow register plugins
        // TODO!
    }

    public override IEnumerable<string> GetAssembliesForScanning()
    {
        yield return "netstandard";
        yield return "mscorlib";
    }

    public override bool ShouldCleanReference => true;
}
