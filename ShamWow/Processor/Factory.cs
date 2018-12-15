using System;

namespace ShamWow.Processor
{
    public class Factory
    {
        private Func<Object, ShamWowEngine> _ctorCaller;

        public Factory (Func<Object, ShamWowEngine> ctorCaller)
        {
            _ctorCaller = ctorCaller;
        }

        public ShamWowEngine Create(object unScrubbedData)
        {
            if(unScrubbedData != null)
            {
                return _ctorCaller(unScrubbedData);
            }
            else
            {
                return null;
            }
        }
    }
}
