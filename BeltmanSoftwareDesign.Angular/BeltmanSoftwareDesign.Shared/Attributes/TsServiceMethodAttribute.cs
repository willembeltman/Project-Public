namespace BeltmanSoftwareDesign.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TsServiceMethodAttribute : Attribute
    {
        public string ServiceName { get; }
        public string MethodName { get; }

        public TsServiceMethodAttribute(string serviceName, string methodName)
        {
            ServiceName = serviceName;
            MethodName = methodName;
        }
    }
}
