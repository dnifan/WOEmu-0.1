using System;
using WO.Core;
using System.Collections.Generic;
using System.Text;

namespace WOEmu.Objects
{
    public class Structure : ObjectBase
    {
        public Structure(long id, string Name) : base()
        {
            ID = id;
            this.Name = Name;
            Type = ObjectType.Structure;

            Nodes = new List<StructureNode>(64);
        }

        public override void OnRemove()
        {
            Sql s = new Sql(Program.sqlData);
            s.ExecuteQuery("DELETE FROM structures WHERE gID = '" + ID + "';");
            s.ExecuteQuery("DELETE FROM structure_nodes WHERE structureID = '" + ID + "';");
            s.Dispose();
            ObjectPool.RemoveObject(this);
        }

        public void SetOrigin(Vector3 origin)
        {
            Position = origin;
        }

        public void saveToDatabase(Player p)
        {
            Sql s = new Sql(Program.sqlData);

            string buf = this.Name;
            string name = buf.Replace("\'", "\\'");
            string q = "INSERT INTO structures VALUES(NULL, " + (int)(8000 + this.ID) + ", '" + name + "', " + this.Position.X + ", " + this.Position.Y + ", " + p.AccountID + ");";
            WO.Core.Logger.Logger.printDebug(q);
            s.ExecuteQuery(q);

            foreach (StructureNode n in Nodes)
            {
                s.ExecuteQuery("INSERT INTO structure_nodes VALUES(NULL, " + (int)(8000+this.ID) + ", " + (int)n.type + ", '" + n.material + "', " + n.relativeOffset.X + ", " + n.relativeOffset.Y + ", " + n.relativeOffset.Z + ", " + (n.flipped == true ? 1 : 0) + ", 0, 0, 0, 0);");
            }
        }

        /// <summary>
        /// Add a node to the structure, (walls etc)
        /// </summary>
        /// <param name="n">The node to add</param>
        public void AddNode(StructureNode n)
        {
            Nodes.Add(n);
        }

        public List<StructureNode> Nodes;
    }
}
