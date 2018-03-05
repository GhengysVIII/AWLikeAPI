using AWLike.DAL.Entity;
using AWLikeAPI.Models;
using AWLikeAPI.Models.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AWLikeAPI.Tools.Mappers
{
    public static class UserMapper
    {
        //UserLight
        public static UserPOCO ToGlobalUserPoco(this ToGameUser User)
        {
            return new UserPOCO
            {
                Id = User.Id,
                Email = User.Email,
                Username = User.Username

            };
        }

        //UserPOCO
        public static ToGameUser ToClientUserLight(this UserPOCO User)
        {
            return new ToGameUser
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

        //GameUser
        public static GameUser ToGameUser(this UserPOCO User)
        {
            return new GameUser()
            {
                Id = User.Id,
                Email = User.Email,
                TurnPosition = User.TurnPosition,
                Username = User.Username
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