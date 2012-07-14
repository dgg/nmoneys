using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("NMoneys")]
[assembly: AssemblyDescription("Implementation of the Money Value Object to support representing moneys in the currencies defined in the ISO 4217 standard.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("NMoneys")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("6008a10f-70bd-400b-8ebb-112069622351")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("3.0.0.0")]
[assembly: AssemblyFileVersion("3.0.0.0")]

[assembly: InternalsVisibleTo("NMoneys.Tests")]
[assembly: InternalsVisibleTo("NMoneys.Tools")]
[assembly: InternalsVisibleTo("NMoneys.Exchange")]
[assembly: System.Windows.Markup.XmlnsDefinition(NMoneys.Serialization.Data.NAMESPACE, "NMoneys")]

[assembly: CLSCompliant(true)]