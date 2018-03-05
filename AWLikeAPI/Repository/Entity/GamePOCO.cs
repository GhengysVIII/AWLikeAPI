using AWLike.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.BaseRepository;

namespace Repository.Entity
{
    public class GamePOCO : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MapID { get; set; }
        public int UserTurnNumb { get; set; }
        public List<UserPOCO> UserList { get; set; }
        public int NumberOfPlayers { get; set; }
    }
}
