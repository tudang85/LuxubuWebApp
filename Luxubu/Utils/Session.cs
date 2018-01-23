using Luxubu.Types;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Luxubu.Utils
{
    public class Session
    {
        public static SessionData GetSession(HttpContext objHttpCtx)
        {
            try
            {
                string strUserSession = objHttpCtx.Session.GetString(ConstValue.USER_SESSION);
                if(string.IsNullOrEmpty(strUserSession))
                {
                    return null;
                }
                SessionData objSSData = JsonConvert.DeserializeObject<SessionData>(strUserSession);
                if(objSSData == null)
                {
                    throw new Exception("Exception happended when try to deserialize UserSession data.");
                }
                return objSSData;
            }
            catch (Exception objEx)
            {
                throw objEx;
            }
        }

        public static int SetSession(HttpContext objHttpCtx, SessionData objSSData)
        {
            try
            {
                string strSessionData = JsonConvert.SerializeObject(objSSData);
                objHttpCtx.Session.SetString(ConstValue.USER_SESSION, strSessionData);
                return 1;
            }
            catch (Exception objEx)
            {
                throw new Exception("Exception happended in SetSession. Error:" + objEx.ToString());
            }
        }
    }
}
