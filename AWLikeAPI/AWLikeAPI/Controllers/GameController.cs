using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AWLikeAPI.Controllers
{
    public class GameController : ApiController
    {
        
        [HttpGet]
        [Route("api/game/getAllAvailable")]
        public UserLight getAll(UserCredentials UserCred)
        {

            UserLight UL = UserRepository.Instance.Get(UserCred.Username, UserCred.Password).ToClientUserLight();
            return UL;
        }

        [HttpPost]
        [Route("api/user/register")]
        public UserLight Register(UserRegister UserReg)
        {
            int userIDRegistered = UserRepository.Instance.Insert(UserReg.ToUserPoco());
            return UserRepository.Instance.Get(userIDRegistered).ToClientUserLight();
        }

    }

}
