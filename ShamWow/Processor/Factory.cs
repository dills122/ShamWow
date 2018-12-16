using ShamWow.Constants;
using System;

namespace ShamWow.Processor
{
    public class Factory
    {
        private Func<Object, ScrubMode, ShamWowEngine> _ctorCallerMode;

        public Factory (Func<Object, ScrubMode, ShamWowEngine> ctorCaller)
        {
            _ctorCallerMode = ctorCaller;
        }

        public ShamWowEngine Create(object unScrubbedData, ScrubMode mode)
        {
            if (unScrubbedData != null)
            {
                return _ctorCallerMode(unScrubbedData, mode);
            }
            else
            {
                return null;
            }
        }
    }
}
