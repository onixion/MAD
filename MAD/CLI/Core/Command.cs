using System;
using System.Collections.Generic;

namespace MAD.CLICore
{
    public abstract class Command
    {
        #region members

        private List<ParameterOption> _requiredParameter;
        public List<ParameterOption> requiredParameter { get { return _requiredParameter; } }
       
        private List<ParameterOption> _optionalParameter;
        public List<ParameterOption> optionalParameter { get { return _optionalParameter; } }

        public string description { get; set; }
        protected string output = "";

        // This object contains all parameters given by the cli.
        public ParameterInput parameters = new ParameterInput();

        #endregion

        #region constructor

        protected Command()
        {
            _requiredParameter = new List<ParameterOption>();
            _optionalParameter = new List<ParameterOption>();
        }

        #endregion

        #region methodes

        public abstract string Execute(int consoleWidth);

        public Type GetArgumentType(string parameter)
        {
            foreach (Parameter _temp in parameters.parameters)
            {
                if (_temp.parameter == parameter)
                {
                    return _temp.argumentValues[0].GetType();
                }
            }

            return null;
        }

        protected bool OptionalParameterUsed(string parameter)
        {
            foreach (Parameter _temp in parameters.parameters)
            {
                if ((string)_temp.parameter == parameter)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
