using QuickPulse.Explains;

namespace EntityFramework.Explained.Schema
{
    public abstract class SchemaPreTestBase
    {
        protected string ReadFrom(string resourceName)
        {

            using var stream = typeof(SchemaPreTestBase).Assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
                throw new FileNotFoundException($"Resource '{resourceName}' not found.");

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}

/*
    Here is a succinct explanation of the class definition and its method:

    The `SchemaPreTestBase` class is an abstract base class that provides a common method for reading from embedded resources.

    `ReadFrom(string resourceName);`
        This method reads the contents of an embedded resource with the specified resourceName and returns it as a string. 
        If the resource is not found, it throws a `FileNotFoundException`.
*/

/*
How it works:

    1.  `typeof(SchemaPreTestBase).Assembly `
    
    This part of the code obtains a reference to the assembly in which the `SchemaPreTestBase` class is defined. 
    An assembly is the basic unit of deployment and versioning in .NET.

    2.  `GetManifestResourceStream(resourceName)` 
    
    This method searches for a built-in resource named `resourceName` within the assembly and returns a `stream` for reading it.
    Built-in resources are files (such as text files, images, XML) that are compiled directly into the assembly.

    3.  `using var stream = ...` 
    
    The using statement ensures that `stream` will be correctly disposed (released) after use, even if an error occurs.
*/