﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="">
//   Copyright 2013 Thomas PIERRAIN
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("NFluent")]
[assembly: AssemblyDescription("NFluent is an ergonomic assertion library which aims to fluent your .NET TDD experience (based on simple Check.That() assertion statements). NFluent aims your tests to be fluent to write (with an happy 'dot' auto completion experience), fluent to read (i.e. as close as possible to plain English expression), but also fluent to troubleshoot, in a less-error-prone way comparing to the classical .NET test frameworks. NFluent is directly, but also freely, inspired by the awesome Java FEST Fluent assertion/reflection library (http://fest.easytesting.org/).")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Thomas PIERRAIN (thomas@pierrain.net), Cyrille DUPUYDAUBY, Marc-Antoine LATOUR and contributors.")]
[assembly: AssemblyProduct("NFluent")]
[assembly: AssemblyCopyright("Copyright ©  2013. Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0).")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("2959f969-9a70-4126-bbb2-ee2a03420e4f")]

// Friend assemblies
[assembly: InternalsVisibleTo("NFluent.Web"), InternalsVisibleTo("NFluent.Tests")]
