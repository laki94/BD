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
    
    public partial class Uczestnictwo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Uczestnictwo()
        {
            this.Opinia = new HashSet<Opinia>();
            this.Reklamacja = new HashSet<Reklamacja>();
        }
    
        public int id_uczestnictwo { get; set; }
        public Nullable<int> liczba_osob { get; set; }
        public Nullable<int> numer_rezerwacji { get; set; }
        public Nullable<decimal> cena_rezerwacji { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Opinia> Opinia { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reklamacja> Reklamacja { get; set; }
        public virtual Rezerwacja Rezerwacja { get; set; }
    }
}
