using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWow.Processor
{
    public interface IProcessDocument
    {
        object CleanData();
        ProcessDocument Scrub();
        bool CheckManifest();
        DocumentManifest GetManifest();
    }
}
