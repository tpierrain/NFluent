// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckDescription.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Xml.Serialization;

    // information for one check
    public class CheckDescription
    {
        #region Public Properties

        [XmlIgnore]
        public MethodBase Check { get; set; }

        [XmlIgnore]
        public string CheckName
        {
            get
            {
                return this.Check.Name;
            }
        }

        [XmlIgnore]
        public List<Type> CheckParameters { get; set; }

        public string Signature
        {
            get
            {
                StringBuilder parameters = new StringBuilder(CheckParameters.Count*20);
                // build parameter list
                if (CheckParameters.Count > 0)
                {
                    parameters.Append(CheckParameters[0].Name);
                    for (int i = 1; i < CheckParameters.Count; i++)
                    {
                        parameters.Append(", ");
                        parameters.Append(CheckParameters[i].Name);
                    }
                }
                return string.Format("Check.That({0} sut).{1}({2})", CheckedType.Name, Check.Name, parameters);
            }
            set
            {
                
            }
        }
        [XmlIgnore]
        public Type CheckedType { get; set; }

        public string ErrorSampleMessage { get; set; }

        #endregion
    }

    // information regarding various signatures for one check
    public class CheckList
    {
        #region Fields

        private List<CheckDescription> checks = new List<CheckDescription>();

        #endregion

        #region Public Properties

        [XmlAttribute]
        public string CheckName { get; set; }

        public List<CheckDescription> Checks
        {
            get
            {
                return this.checks;
            }

            set
            {
                this.checks = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void AddCheck(CheckDescription description)
        {
            this.checks.Add(description);
        }

        #endregion
    }

    // information regarding all checks for one type
    public class TypeChecks
    {
        #region Fields

        private readonly Type checkedType;

        private List<CheckList> checks = new List<CheckList>();

        #endregion

        #region Constructors and Destructors

        public TypeChecks(Type checkedType)
        {
            this.checkedType = checkedType;
        }

        public TypeChecks()
        {
        }

        #endregion

        #region Public Properties
        [XmlAttribute]
        public string Typename
        {
            get
            {
                return CheckedType.Name;
            }
            set {}
        }

        public Type CheckedType
        {
            get
            {
                return this.checkedType;
            }
        }

        [XmlArray]
        public List<CheckList> Checks
        {
            get
            {
                return this.checks;
            }

            set
            {
                this.checks = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void AddCheck(CheckDescription description)
        {
            foreach (CheckList checkList in this.checks)
            {
                if (checkList.CheckName == description.CheckName)
                {
                    checkList.AddCheck(description);
                    return;
                }
            }

            var toAdd = new CheckList();
            toAdd.CheckName = description.CheckName;
            toAdd.AddCheck(description);
            this.checks.Add(toAdd);
        }

        #endregion
    }

    public class FullRunDescription
    {
        #region Fields

        private List<TypeChecks> runDescription = new List<TypeChecks>();

        #endregion

        #region Public Properties

        public List<TypeChecks> RunDescription
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

        #endregion

        #region Public Methods and Operators

        public void AddEntry(CheckDescription desc)
        {
            foreach (TypeChecks typeCheckse in this.runDescription)
            {
                if (typeCheckse.CheckedType == desc.CheckedType)
                {
                    typeCheckse.AddCheck(desc);
                    return;
                }
            }

            var addedType = new TypeChecks(desc.CheckedType);
            addedType.AddCheck(desc);
            this.runDescription.Add(addedType);
        }

        public void Save(string name)
        {
            var serialier = new XmlSerializer(typeof(FullRunDescription));
            using (var file = new FileStream(name, FileMode.Create))
            {
                serialier.Serialize(file, this);
            }
        }

        #endregion
    }
}