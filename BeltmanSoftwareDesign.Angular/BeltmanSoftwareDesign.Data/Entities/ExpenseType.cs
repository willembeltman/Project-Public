﻿using BeltmanSoftwareDesign.Shared.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeltmanSoftwareDesign.Data.Entities
{
    public class ExpenseType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public long CompanyId { get; set; }
        public string Description { get; set; }

        public bool IsVolledigeKosten { get; set; } // Operationele kosten
        public bool IsRepresentatieKosten { get; set; }
        public bool IsHalfTellen { get; set; }

        public BedrijfsKostenTypeEnum BedrijfsKostenType { get; set; } // Operationele kosten
        public AfschrijfKostenTypeEnum AfschrijfKostenType { get; set; }
        //public virtual Company Company { get; set; }
        //public virtual ICollection<Expense> Expenses { get; set; }

        public override string ToString()
        {
            return Description;
        }

    }
}
