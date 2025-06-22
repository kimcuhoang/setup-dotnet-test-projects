using Microsoft.Azure.Functions.Worker;

namespace AzFunc.Example.Tests.Helpers;
public class TestFunctionContext(string functionId) : FunctionContext
{
    public override string FunctionId { get; } = functionId;
    public override string InvocationId { get; }
    public override TraceContext TraceContext { get; }
    public override BindingContext BindingContext { get; }
    public override RetryContext RetryContext { get; }
    public override IServiceProvider InstanceServices { get; set; }
    public override FunctionDefinition FunctionDefinition { get; }
    public override IDictionary<object, object> Items { get; set; } = new Dictionary<object, object>();
    public override IInvocationFeatures Features { get; }
}
