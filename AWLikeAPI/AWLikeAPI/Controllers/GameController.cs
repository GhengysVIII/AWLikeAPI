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

        [HttpGet]
        [Route("api/game/getallGameIn/{UserID}")]
        public IEnumerable<GameInfo> GetAllGameIn(int UserID)
        {

            return GameRepository.Instance.GetAllGameIn(UserID).Select(x => x.ToGameInfo());
        }
        

    }

}
