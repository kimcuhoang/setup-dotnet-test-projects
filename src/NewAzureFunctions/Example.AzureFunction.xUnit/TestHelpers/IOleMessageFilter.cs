using System.Runtime.InteropServices;

namespace Example.AzureFunction.xUnit.TestHelpers;

[ComImport, Guid("00000016-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IOleMessageFilter
{
    [PreserveSig]
    int HandleInComingCall(int dwCallType, nint hTaskCaller, int dwTickCount, nint lpInterfaceInfo);

    [PreserveSig]
    int RetryRejectedCall(nint hTaskCallee, int dwTickCount, int dwRejectType);

    [PreserveSig]
    int MessagePending(nint hTaskCallee, int dwTickCount, int dwPendingType);
}
