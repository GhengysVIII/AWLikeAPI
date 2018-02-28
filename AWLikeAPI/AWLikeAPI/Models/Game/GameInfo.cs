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
        public List<UserLight> Ref_UserList { get; set; }
        public int NumberOfPlayers { get; set; }
    }
}