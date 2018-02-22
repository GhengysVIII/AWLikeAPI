using AWLike.DAL.Entity;
using AWLikeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AWLikeAPI.Tools.Mappers
{
    public static class UserMapper
    {
        //UserLight
        public static UserPOCO ToGlobalUserPoco(this UserLight User)
        {
            return new UserPOCO
            {
                Id = User.Id,
                Email = User.Email,
                Username = User.Username

            };
        }

        //UserPOCO
        public static UserLight ToClientUserLight(this UserPOCO User)
        {
            return new UserLight
            {
                Id = User.Id,
                Email = User.Email,
                Username = User.Username

            };
        }
        public static UserRegister ToUserRegister(this UserPOCO User)
        {
            return new UserRegister
            {
                Email = User.Email,
                Username = User.Username,
                Password = User.Password

            };
        }

        //UserRegister
        public static UserPOCO ToUserPoco(this UserRegister User)
        {
            return new UserPOCO
            {
                Email = User.Email,
                Username = User.Username,
                Password = User.Password
            };
        }
    }
}