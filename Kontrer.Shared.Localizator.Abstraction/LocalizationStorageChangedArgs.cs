using System;
using System.Collections.Generic;
using System.Text;

namespace Kontrer.Shared.Localizator
{
    public class LocalizationStorageChangedArgs
    {
        public HashSet<string> ChangedSectionUniqueNames { get; }
        public LocalizationStorageChangedArgs(HashSet<string>  sections)
        {
            ChangedSectionUniqueNames = sections;
        }

    }
}
