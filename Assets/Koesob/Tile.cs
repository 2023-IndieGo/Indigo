using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MapManager
{
    public class Tile
    {
        private string id;
        private Stack<Chip> chips;

        public string ID { get { return id; } }
        public Stack<Chip> Chips { get { return chips; } }

        public Tile(string id)
        {
            this.id = id;

            chips = new Stack<Chip>();
        }

        public Chip Pop()
        {
            return chips.Pop();
        }

        public void Push(Chip _chip)
        {
            chips.Push(_chip);
        }
    }
}

