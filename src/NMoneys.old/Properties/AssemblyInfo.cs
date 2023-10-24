using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("NMoneys")]
[assembly: AssemblyDescription("Implementation of the Money Value Object to support representing moneys in the currencies defined in the ISO 4217 standard.")]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("6008a10f-70bd-400b-8ebb-112069622351")]

[assembly: InternalsVisibleTo("NMoneys.Tests")]
[assembly: InternalsVisibleTo("NMoneys.Tools")]
[assembly: InternalsVisibleTo("NMoneys.Exchange")]

[assembly: CLSCompliant(true)]