using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.CLI
{
    public abstract class CLIFramework
    {
        // --------------------------------------------------------
        //          CLI Framework
        // --------------------------------------------------------

        #region member

        // cli framework version
        public Version version = new Version(1, 0);

        // cli vars
        protected string cursor = "=> ";

        // cli input vars
        protected string cliInput;
        protected string commandInput;
        protected ParameterInput parameterInput;
        protected string parameterValid;

        // cli command vars
        protected List<CommandOptions> commandOptions;
        protected Command command;
        protected Type inputCommandType;

        // cli usermanagement
        protected List<CLIUser> users;

        #endregion

        #region CLI main methodes

        public void InitCLI()
        {
            // init available commands
            commandOptions = new List<CommandOptions>()
            {
                // GENERAL COMMANDS
                new CommandOptions("help", typeof(HelpCommand)),
                new CommandOptions("versions", typeof(VersionCommand)),
                new CommandOptions("info", typeof(InfoCommand)),
                new CommandOptions("cursor", typeof(CursorCommand)),

                // JOBSYSTEM COMMANDS
                new CommandOptions("jobsystem status", typeof(JobSystemStatusCommand)),
                new CommandOptions("job status", typeof(JobListCommand)),
                new CommandOptions("job remove", typeof(JobSystemRemoveCommand)),
                new CommandOptions("job start", typeof(JobSystemStartCommand)),
                new CommandOptions("job stop", typeof(JobSystemStopCommand)),
                new CommandOptions("job add ping", typeof(JobSystemAddPingCommand)),
                new CommandOptions("job add http", typeof(JobSystemAddHttpCommand)),
                new CommandOptions("job add port", typeof(JobSystemAddPortCommand))
            };
        }

        public virtual void Start()
        {
            InitCLI();
        }

        protected string CLIInfo()
        { 
            return "MAD-CLI (Version " + version + ") \nSERVER-TIME: " + GetTimeStamp();
        }

        #endregion

        #region CLI format methodes

        protected string GetCommand(string input)
        {
            string[] buffer = input.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);

            if (buffer.Length != 0)
            {
                if (buffer[0] == "")
                    return null;

                buffer[0] = buffer[0].Trim();

                return buffer[0];
            }
            else
                return null;
        }

        protected ParameterInput GetParamtersFromInput(string input)
        {
            ParameterInput parameterTemp = new ParameterInput();

            string[] temp = input.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < temp.Length; i++)
                temp[i] = temp[i].Trim();

            for (int i = 1; i < temp.Length; i++)
            {
                string[] temp2 = temp[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (temp2.Length == 1)
                {
                    // parameter argument is null
                    parameterTemp.parameters.Add(new Parameter(temp2[0], null));
                }
                else
                {
                    // more than one argument do NOT get recordnized by the CLI!!!
                    parameterTemp.parameters.Add(new Parameter(temp2[0], temp2[1]));
                }
            }

            return parameterTemp;
        }

        #endregion

        #region CLI internal methodes

        protected bool CommandExists(string command)
        {
            foreach (CommandOptions temp in commandOptions)
                if (temp.command == command)
                    return true;

            return false;
        }

        protected Type GetCommandType(string command)
        {
            foreach (CommandOptions temp in commandOptions)
                if (temp.command == command)
                    return temp.commandType;
            return null;
        }

        #endregion

        #region CLI Usermanagment

        public void InitCLIUsers()
        {
            users = new List<CLIUser>()
            {
                new CLIUser("admin", Encoding.ASCII.GetString(GetMD5Hash(Encoding.ASCII.GetBytes("yolo"))))
            };
        }

        public bool UserExist(string username)
        {
            foreach (CLIUser temp in users)
            {
                if (temp.username == username)
                {
                    return true;
                }
            }

            return false;
        }

        public CLIUser GetUser(string username)
        {
            foreach (CLIUser temp in users)
            {
                if (temp.username == username)
                {
                    return temp;
                }
            }

            return null;
        }

        protected bool CheckLogin(string username, string passwordMD5)
        {
            // get user (if user does not exist: client = null)
            CLIUser client = GetUser(username);

            // check username
            if (client != null)
            {
                // check password
                if (client.passwordMD5 == passwordMD5)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region CLI Securtiy/TimeStamp

        protected string GetTimeStamp()
        {
            return DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        protected byte[] GetMD5Hash(byte[] data)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5CryptoServiceProvider.Create();
            return md5.ComputeHash(data);
        }

        #endregion
    }
}
