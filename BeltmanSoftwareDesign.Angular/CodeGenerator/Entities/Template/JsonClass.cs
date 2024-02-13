using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenerator.Entities.Models;

namespace CodeGenerator.Entities.Template
{
    public class JsonClass
    {
        public JsonClass(EntityInfo entity)
        {
            Entity = entity;
        }

        public EntityInfo Entity { get; }

        public string Get()
        {
            return null;
        }
    }
}
