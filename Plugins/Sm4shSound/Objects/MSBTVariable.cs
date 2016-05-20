using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sm4shSound.Objects
{
    [XmlType(TypeName = "msbt")]
    public class MSBTVariable
    {
        #region Properties
        [XmlElement("n")] 
        public string Name { get; set; }
        [XmlElement("v")]
        public string Value { get; set; }
        #endregion

        public MSBTVariable()
        {
        }

        public MSBTVariable(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public bool IsSoundEntryVariable()
        {
            return Name.StartsWith("Msnd");
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
