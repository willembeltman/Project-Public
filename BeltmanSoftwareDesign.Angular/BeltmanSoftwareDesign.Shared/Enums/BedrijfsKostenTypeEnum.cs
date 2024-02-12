using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeltmanSoftwareDesign.Shared.Enums
{
    public enum BedrijfsKostenTypeEnum
    {
        Kosten_Van_Grond_En_Hulpstoffen_En_Inkoop_Price_Van_De_Verkopen = 0,
        Kosten_Van_Uitbesteed_Werk_En_Andere_Externe_Kosten = 10,

        Auto_En_Transportkosten = 20,
        Huisvestingkosten = 30,
        Onderhoudkosten_Van_Overige_Materiele_Vaste_Activa = 40,
        Verkoopkosten = 50,
        Andere_Kosten = 60
    }
}
