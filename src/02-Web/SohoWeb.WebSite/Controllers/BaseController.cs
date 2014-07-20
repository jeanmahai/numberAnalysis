using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using Soho.Utility;
using Soho.Utility.Web.Framework;
using SohoWeb.Entity;
using SohoWeb.WebSite.ViewModels;

namespace SohoWeb.WebSite.Controllers
{
    [ResultExecutAttribute]
    public class BaseController : Controller
    {
        protected LoginAuthVM CurrUser = (new AuthMgr()).ReadUserInfo();

        public EntityBase SetEntityBase(EntityBase entity, bool? bIsCreate = null)
        {
            var user = (new AuthMgr()).ReadUserInfo();
            if (bIsCreate.HasValue && bIsCreate.Value)
            {
                entity.InDate = DateTime.Now.ToString();
                entity.InUserSysNo = user.UserSysNo;
                entity.InUserName = user.UserName;
            }
            else if (bIsCreate.HasValue && !bIsCreate.Value)
            {
                entity.EditDate = DateTime.Now.ToString();
                entity.EditUserSysNo = user.UserSysNo;
                entity.EditUserName = user.UserName;
            }
            else
            {
                entity.InDate = DateTime.Now.ToString();
                entity.InUserSysNo = user.UserSysNo;
                entity.InUserName = user.UserName;
                entity.EditDate = DateTime.Now.ToString();
                entity.EditUserSysNo = user.UserSysNo;
                entity.EditUserName = user.UserName;
            }
            return entity;
        }

        private string GetParams()
        {
            var stream = Request.InputStream;
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            return new StreamReader(stream).ReadToEnd();
        }
        protected T GetParams<T>()
        {
            var str = GetParams();
            var obj = new JavaScriptSerializer().Deserialize<T>(str);
            return SerializationUtility.JsonDeserialize2<T>(str);
        }
    }

    [AuthAttribute(NeedAuth = true)]
    public class SSLController : BaseController
    { }

    [AuthAttribute(NeedAuth = false)]
    public class WWWController : BaseController
    { }
}