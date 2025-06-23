using System.Runtime.InteropServices;

namespace Example.AzureFunction.xUnit.TestHelpers;

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
