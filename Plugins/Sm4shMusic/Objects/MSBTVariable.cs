using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sm4shMusic.Objects
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
            return (Name.StartsWith(SoundMSBTFile.VAR_TITLE) || Name.StartsWith(SoundMSBTFile.VAR_TITLE2) || Name.StartsWith(SoundMSBTFile.VAR_DESCRIPTION) || Name.StartsWith(SoundMSBTFile.VAR_DESCRIPTION2) || Name.StartsWith(SoundMSBTFile.VAR_SOURCE));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
