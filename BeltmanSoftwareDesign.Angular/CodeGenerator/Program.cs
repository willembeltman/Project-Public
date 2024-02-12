
//using BeltmanSoftwareDesign.Data;
//using CodeGenerator.Entities;

//var ns = new CodeGenerator.Entities.DbContextInfo(typeof(ApplicationDbContext));
//var hoofddbsets = ns.DbSetInfos
//    .Where(a => a.Entity.EntityPropertyInfos.Any(b => b.Name == "CompanyId"))
//    .OrderBy(a => a.Name)
//    .ToArray();

//if (hoofddbsets.Any())
//{
//    // Genereer DTO's

//    // Genereer Services

//    // Genereer Angular Interfaces ()


//}

using CodeGenerator;

ApplicationContext app = new ApplicationContext();
app.Run(args);


