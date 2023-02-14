using AntDesign;

namespace Wexflow.BlazorServer.Helper
{

    public class NotifyHelper
    {

        private readonly INotificationService notice;


        public NotifyHelper(INotificationService notice)
        {
            this.notice = notice;
            this.notice.Config(new NotificationGlobalConfig()
            {
                Placement = NotificationPlacement.BottomRight,
            });
        }

        public void NotifySuccessOrFail(bool success, string succMsg, string failMsg) 
        {
            notice.Open(new NotificationConfig()
            {
                Message = "提示",
                Description = success ? succMsg : failMsg,
                NotificationType = success ? NotificationType.Success : NotificationType.Error
            });
        }


        public void Info(string msg)
        {
            notice.Info(new NotificationConfig()
            {
                Message = "提示",
                Description = msg,
            });
        }

        public void Success(string msg)
        {
            notice.Success(new NotificationConfig()
            {
                Message = "成功",
                Description = msg,
            });
        }

        public void Error(string msg)
        {
            notice.Error(new NotificationConfig()
            {
                Message = "错误",
                Description = msg,
            });
        }

        public void Warning(string msg)
        {
            notice.Warning(new NotificationConfig()
            {
                Message = "警告",
                Description = msg,
            });
        }



    }
}
