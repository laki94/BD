//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BD
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reklamacja
    {
        public int numer_reklamacji { get; set; }
        public string opis { get; set; }
        public Nullable<bool> stan { get; set; }
        public string Kierownik_pesel { get; set; }
        public Nullable<int> id_uczestnictwo { get; set; }
    
        public virtual Kierownik Kierownik { get; set; }
        public virtual Uczestnictwo Uczestnictwo { get; set; }
    }
}
