using AWLike.Repository;
using AWLikeAPI.Models;
using AWLikeAPI.Models.Game;
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
        public IEnumerable<GameInfo> getAllAvailable()
        {
            return GameRepository.Instance.GetAllGameInfoAvailable();
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
