

namespace Wexflow.BlazorServer.Data
{
    using Wexflow.Core;

    public class WexflowService
    {

        //public static NameValueCollection Config = ConfigurationManager.AppSettings;

        private string settingsFile;
        private Core.LogLevel logLevel;
        private string superAdminUsername;
        private bool enableWorkflowsHotFolder;
        private bool enableRecordsHotFolder;
        private bool enableEmailNotifications;
        private string smtpHost;
        private int smtpPort;
        private bool smtpEnableSsl;
        private string smtpUser;
        private string smtpPassword;
        private string smtpFrom;

        //private FileSystemWatcher WorkflowsWatcher;
        //private FileSystemWatcher RecordsWatcher;
        //public static WexflowEngine WexflowEngine = new WexflowEngine(
        //    settingsFile
        //    , logLevel
        //    , enableWorkflowsHotFolder
        //    , superAdminUsername
        //    , enableEmailNotifications
        //    , smtpHost
        //    , smtpPort
        //    , smtpEnableSsl
        //    , smtpUser
        //    , smtpPassword
        //    , smtpFrom
        //    );

        public WexflowService(IConfiguration config)
        {
            if (config == null) { 
                throw new ArgumentNullException("config");
            }
            Config = config;

            settingsFile = config["WexflowSettingsFile"];
             logLevel = !string.IsNullOrEmpty(config["LogLevel"]) ? (Core.LogLevel)Enum.Parse(typeof(Core.LogLevel), config["LogLevel"], true) : Core.LogLevel.All;
            superAdminUsername = config["SuperAdminUsername"];
            enableWorkflowsHotFolder = bool.Parse(config["EnableWorkflowsHotFolder"]);
            enableRecordsHotFolder = bool.Parse(config["EnableRecordsHotFolder"]);
            enableEmailNotifications = bool.Parse(config["EnableEmailNotifications"]);
            smtpHost = config["Smtp.Host"];
            smtpPort = int.Parse(config["Smtp.Port"]);
            smtpEnableSsl = bool.Parse(config["Smtp.EnableSsl"]);
            smtpUser = config["Smtp.User"];
            smtpPassword = config["Smtp.Password"];
            smtpFrom = config["Smtp.From"];

            Engine = new WexflowEngine(
                settingsFile
                , logLevel
                , enableWorkflowsHotFolder
                , superAdminUsername
                , enableEmailNotifications
                , smtpHost
                , smtpPort
                , smtpEnableSsl
                , smtpUser
                , smtpPassword
                , smtpFrom
                );


            Thread startThread = new Thread(StartThread) { IsBackground = true };
            startThread.Start();

        }

        public IConfiguration Config { get; private set; }

        public WexflowEngine Engine { get; private set; }

        private void StartThread()
        {
            //if (enableWorkflowsHotFolder)
            //{
            //    //InitializeWorkflowsFileSystemWatcher();
            //}
            //else
            //{
            //    Logger.Info("Workflows hot folder is disabled.");
            //}

            //if (enableRecordsHotFolder)
            //{
            //    // On file found.
            //    foreach (var file in Directory.GetFiles(RecordsHotFolder))
            //    {
            //        var recordId = WexflowEngine.SaveRecordFromFile(file, superAdminUsername);

            //        if (recordId != "-1")
            //        {
            //            Logger.Info($"Record inserted from file {file}. RecordId: {recordId}");
            //        }
            //        else
            //        {
            //            Logger.Error($"An error occured while inserting a record from the file {file}.");
            //        }
            //    }

            //    // On file created.
            //    //InitializeRecordsFileSystemWatcher();
            //}
            //else
            //{
            //    Logger.Info("Records hot folder is disabled.");
            //}

            Engine.Run();
        }

    }
}
