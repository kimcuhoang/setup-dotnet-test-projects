using System.Runtime.InteropServices;

namespace Example.AzureFunction.xUnit.DebuggerTools.DevTool;

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

public class MessageFilter : IOleMessageFilter
{
    private const int Handled = 0, RetryAllowed = 2, Retry = 99, Cancel = -1, WaitAndDispatch = 2;

    int IOleMessageFilter.HandleInComingCall(int dwCallType, nint hTaskCaller, int dwTickCount, nint lpInterfaceInfo)
    {
        return Handled;
    }

    int IOleMessageFilter.RetryRejectedCall(nint hTaskCallee, int dwTickCount, int dwRejectType)
    {
        return dwRejectType == RetryAllowed ? Retry : Cancel;
    }

    int IOleMessageFilter.MessagePending(nint hTaskCallee, int dwTickCount, int dwPendingType)
    {
        return WaitAndDispatch;
    }

    public static void Register()
    {
        CoRegisterMessageFilter(new MessageFilter());
    }

    public static void Revoke()
    {
        CoRegisterMessageFilter(null);
    }

    private static void CoRegisterMessageFilter(IOleMessageFilter? newFilter)
    {
        IOleMessageFilter oldFilter;
        CoRegisterMessageFilter(newFilter, out oldFilter);
    }

    [DllImport("Ole32.dll")]
    private static extern int CoRegisterMessageFilter(IOleMessageFilter? newFilter, out IOleMessageFilter oldFilter);
}
