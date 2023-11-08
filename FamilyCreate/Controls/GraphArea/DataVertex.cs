using FamilyCreate.Models;
using GraphX.Common.Models;
using System;

namespace FamilyCreate.Controls.GraphArea
{
    public class DataVertex : VertexBase
    {
        public int PersonID { get; set; }
        public string FIO { get; set; }
        public string Age { get; set; }
        public string BornDate { get; set; }
        public string DeathDate { get; set; }
        public string DeathPlace { get; set; }
        public string BornPlace { get; set; }
        public string Gender { get; set; }

        public override string ToString() => FIO;

        public DataVertex() : this(-1) { }

        public DataVertex(int id = -1, string text = "", DateTime? bornDate = null, Place? bornPlace = null,
            DateTime? deathDate = null, Place? deathPlace = null, bool ismale = true)
        {
            PersonID = id;
            FIO = text;
            Gender = ismale ? "мужчина" : "женщина";
            Age = bornDate == null ? "незвестен" : (DateTime.Now.Year - bornDate.Value.Year).ToString();
            BornDate = bornDate == null ? "неизвестна" : bornDate.Value.ToShortDateString();
            DeathDate = deathPlace == null ? "-" : deathDate.Value.ToShortDateString();
            DeathPlace = deathPlace == null ? "-" : deathPlace.Name;
            BornPlace = bornPlace == null ? "-" : bornPlace.Name;
        }
    }
}
