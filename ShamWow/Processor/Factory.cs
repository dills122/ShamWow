using ShamWow.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWow.Processor
{
    public class Factory
    {
        private Func<Object, ScrubType, ProcessDocument> _ctorCaller;

        public Factory (Func<Object, ScrubType, ProcessDocument> ctorCaller)
        {
            _ctorCaller = ctorCaller;
        }

        public ProcessDocument Create(object unScrubbedData, ScrubType scrubType)
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
