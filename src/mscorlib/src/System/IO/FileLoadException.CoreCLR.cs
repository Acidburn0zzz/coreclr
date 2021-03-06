// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Security;

namespace System.IO
{
    public partial class FileLoadException
    {
        // Do not delete: this is invoked from native code.
        private FileLoadException(string fileName, string fusionLog, int hResult)
            : base(null)
        {
            HResult = hResult;
            FileName = fileName;
            FusionLog = fusionLog;
            _message = FormatFileLoadExceptionMessage(FileName, HResult);
        }

        internal static string FormatFileLoadExceptionMessage(string fileName, int hResult)
        {
            string format = null;
            GetFileLoadExceptionMessage(hResult, JitHelpers.GetStringHandleOnStack(ref format));

            string message = null;
            if (hResult == System.__HResults.COR_E_BADEXEFORMAT)
                message = SR.Arg_BadImageFormatException;
            else 
                GetMessageForHR(hResult, JitHelpers.GetStringHandleOnStack(ref message));

            return string.Format(CultureInfo.CurrentCulture, format, fileName, message);
        }

        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private static extern void GetFileLoadExceptionMessage(int hResult, StringHandleOnStack retString);

        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private static extern void GetMessageForHR(int hresult, StringHandleOnStack retString);
    }
}
