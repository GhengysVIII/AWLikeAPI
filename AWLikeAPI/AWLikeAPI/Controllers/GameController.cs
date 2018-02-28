using AWLike.Repository;
using AWLikeAPI.Models;
using AWLikeAPI.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AWLikeAPI.Tools.Mappers;

namespace AWLikeAPI.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        [Route("api/game/getallavailable")]
        public IEnumerable<GameInfo> GetAllAvailable()
        {

            return GameRepository.Instance.GetAllGameInfoAvailable().Select(x => x.ToGameInfo());
        }

        //[HttpPost]
        //[Route("api/user/register")]
        //public UserLight Register(UserRegister UserReg)
        //{

        //    return null;
        //}

    }

}
