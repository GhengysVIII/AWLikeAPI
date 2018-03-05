using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AWLikeAPI.Models.Game
{
    public class GameInfo
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int MapID { get; set; }
        public int UserTurnNumb { get; set; }
        public List<GameUser> Ref_UserList { get; set; }
        public int NumberOfPlayers { get; set; }

    }
}