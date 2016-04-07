using System;
using System.Collections.Generic;
using System.Text;


namespace CodingGame
{
    class ZooT
    {
        [SQLite.PrimaryKey]
        public int ID { get; set; }
        public int Level { get; set; }
        public int Coins { get; set; }
    }
}
