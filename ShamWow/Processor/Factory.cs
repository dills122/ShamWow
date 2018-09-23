using ShamWow.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWow.Processor
{
    public class Factory
    {
        private Func<Object, ScrubTypes, ProcessDocument> _ctorCaller;

        public Factory (Func<Object, ScrubTypes, ProcessDocument> ctorCaller)
        {
            _ctorCaller = ctorCaller;
        }

        public ProcessDocument Create(object unScrubbedData, ScrubTypes scrubType)
        {
            if(unScrubbedData != null)
            {
                return _ctorCaller(unScrubbedData, scrubType);
            }
            else
            {
                return null;
            }
        }
    }
}
