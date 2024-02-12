namespace BeltmanSoftwareDesign.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TsControllerMethodAttribute : Attribute
    {
        public string ServiceName { get; }
        public string MethodName { get; }

        public TsControllerMethodAttribute(string serviceName, string methodName)
        {
            ServiceName = serviceName;
            MethodName = methodName;
        }
    }
}
