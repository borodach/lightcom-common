///////////////////////////////////////////////////////////////////////////////
//
//  File:           AssemblyInfo
//
//  Facility:       Описание сборки
//
//
//  Abstract:       Описание сборки
//
//  Environment:    VC# 8.0
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  04-03-2007
//
//  Copyright (C) OOO "ЛайтКом", 2005-2006. Все права защищены.
//
///////////////////////////////////////////////////////////////////////////////

/*
 * $History: AssemblyInfo.cs $
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 4.03.07    Time: 12:37
 * Created in $/LightCom/.NET/Common/Properties
  */

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Resources;

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyTitle ("Библиотека классов ЛайтКом")]
[assembly: AssemblyDescription ("Библиотека классов ЛайтКом")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany ("ООО \"ЛайтКом\"")]
[assembly: AssemblyProduct ("Библиотека классов ЛайтКом")]
[assembly: AssemblyCopyright ("© ЛайтКом, 2007.\nВсе права защищены.")]
[assembly: AssemblyTrademark ("Библиотека классов ЛайтКом")]
[assembly: AssemblyCulture("")]
[assembly: CLSCompliant (true)]
[assembly: System.Runtime.InteropServices.ComVisible (false)]	

//
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:

[assembly: AssemblyVersion("1.0.*")]

//
// In order to sign your assembly you must specify a key to use. Refer to the 
// Microsoft .NET Framework documentation for more information on assembly signing.
//
// Use the attributes below to control which key is used for signing. 
//
// Notes: 
//   (*) If no key is specified - the assembly cannot be signed.
//   (*) KeyName refers to a key that has been installed in the Crypto Service
//       Provider (CSP) on your machine. 
//   (*) If the key file and a key name attributes are both specified, the 
//       following processing occurs:
//       (1) If the KeyName can be found in the CSP - that key is used.
//       (2) If the KeyName does not exist and the KeyFile does exist, the key 
//           in the file is installed into the CSP and used.
//   (*) Delay Signing is an advanced option - see the Microsoft .NET Framework
//       documentation for more information on this.
//
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]
[assembly: AssemblyKeyName("")]
[assembly: NeutralResourcesLanguageAttribute ("ru-RU")]
