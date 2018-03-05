
using AWLike.DAL.Entity;
using AWLike.Repository;
using AWLikeAPI.Models;
using AWLikeAPI.Tools.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AWLikeAPI.Controllers
{
    public class UserController : ApiController
    {

        [HttpPost]
        [Route("api/user/log")]
        public ToGameUser Log(UserCredentials UserCred)
        {
            ToGameUser UL =  UserRepository.Instance.Get(UserCred.Username, UserCred.Password).ToClientUserLight();
            return UL;
        }

        [HttpPost]
        [Route("api/user/register")]
        public ToGameUser Register(UserRegister UserReg)
        {
            int userIDRegistered = UserRepository.Instance.Insert(UserReg.ToUserPoco());
            return UserRepository.Instance.Get(userIDRegistered).ToClientUserLight();
        }

    }
}
