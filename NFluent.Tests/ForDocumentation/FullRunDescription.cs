namespace NFluent.Tests.ForDocumentation
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    public class FullRunDescription
    {
        private Dictionary<string, Dictionary<string, List<CheckDescription>>> runDescription = new Dictionary<string, Dictionary<string, List<CheckDescription>>>();

        public Dictionary<string, Dictionary<string, List<CheckDescription>>> RunDescription
        {
            get
            {
                return this.runDescription;
            }

            set
            {
                this.runDescription = value;
            }
        }

        public void AddEntry(CheckDescription desc)
        {
            if (!this.RunDescription.ContainsKey(desc.CheckedType.Name))
            {
                this.RunDescription.Add(desc.CheckedType.Name, new Dictionary<string, List<CheckDescription>>());
            }

            var typeRun = this.RunDescription[desc.CheckedType.Name];
            
            if (!typeRun.ContainsKey(desc.Check.Name))
            {
                typeRun.Add(desc.Check.Name, new List<CheckDescription>());
            }

            typeRun[desc.Check.Name].Add(desc);
        }

        public void Save(string name)
        {
            var serialier = new XmlSerializer(typeof(FullRunDescription));
            using (var file = new FileStream(name, FileMode.OpenOrCreate))
            {
                serialier.Serialize(file, this);
            }
        }
    }
}