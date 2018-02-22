
using AWLike.DAL.Entity;
using AWLike.Repository;
using AWLikeAPI.Models;
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
        public UserPOCO Log(UserCredentials UserCred)
        {
            return UserRepository.Instance.Get(UserCred.Username, UserCred.Password);
        }

    }
}
