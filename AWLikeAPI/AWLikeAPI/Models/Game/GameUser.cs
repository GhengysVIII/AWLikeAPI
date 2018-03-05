using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AWLikeAPI.Models.Game
{
    public class GameUser : ToGameUser
    {
        public int TurnPosition { get; set; }
    }
}