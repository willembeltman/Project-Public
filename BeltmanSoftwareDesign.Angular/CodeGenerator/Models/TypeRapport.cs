using CodeGenerator.Helpers;

namespace CodeGenerator.Models
{
    public class TypeRapport
    {
        static string nulleblestring = "System.Nullable`1[[";
        static string nulleblestringend = "]]";

        static string isliststring = "System.Collections.Generic.List`1[[";
        static string isliststringend = "]]";

        static string isarraystringend = "[]";
        public TypeRapport(Type type, ModelNamespacesList modelNamespaceList)
        {
            FullName = type.FullName;

            if (FullName.StartsWith(nulleblestring))
            {
                Nulleble = true;
                var end = FullName.IndexOf(nulleblestringend);
                var csv = FullName.Substring(nulleblestring.Length, end - nulleblestring.Length);
                var csvlines = csv.Split(new string[] { "," }, StringSplitOptions.None);
                FullName = csvlines.First();
            }

            if (FullName.StartsWith(isliststring))
            {
                List = true;
                var end = FullName.IndexOf(isliststringend);
                var csv = FullName.Substring(isliststring.Length, end - isliststring.Length);
                var csvlines = csv.Split(new string[] { "," }, StringSplitOptions.None);
                FullName = csvlines.First();
            }

            if (FullName.EndsWith(isarraystringend))
            {
                List = true;
                FullName = FullName.Substring(0, FullName.Length - isarraystringend.Length);
            }

            TsName = NameHelper.GetTsType(FullName);
            Name = FullName;
            if (TsName == null)
            {
                var models = modelNamespaceList.List.SelectMany(a => a.Models).ToArray();
                foreach (var model in models)
                {
                    if (model.FullName == FullName)
                    {
                        Model = model;
                        TsName = FullName.Substring(
                            model.ModelsNamespace.Name.Length + 1,
                            FullName.Length - model.ModelsNamespace.Name.Length - 1);
                        Name = TsName;
                        break;
                    }
                }
            }
            if (TsName == null)
                throw new Exception("Cannot find type in models");

            Import = Model != null;

            if (!List && (FullName == "System.String" || Import))
            {
                Nulleble = true;
            }
        }

        public string FullName { get; }
        public string Name { get; }
        public string TsName { get; set; }
        public bool Nulleble { get; set; }
        public bool List { get; set; }
        public bool Import { get; set; }
        public Model? Model { get; set; }

        public string TsFullNameNotNull =>
            TsName + (List ? "[]" : "");
        public string TsFullName =>
            TsFullNameNotNull + (Nulleble ? " | null" : "");
    }
}