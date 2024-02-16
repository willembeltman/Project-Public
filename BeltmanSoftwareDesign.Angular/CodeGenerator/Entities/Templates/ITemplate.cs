using CodeGenerator.Entities.Models;

namespace CodeGenerator.Entities.Templates
{
    internal interface ITemplate
    {
        string GetContent();
        string GetFullName();
    }
}