using System.ComponentModel.DataAnnotations;
using Blade;
using Maui.Entities;

namespace Maui.Trading.Binding.Xml
{
    public class Parity
    {
        [Required]
        public string Source
        {
            get;
            set;
        }

        [Required]
        public string Target
        {
            get;
            set;
        }

        [Required]
        public double Value
        {
            get;
            set;
        }

        public bool IsMatch( Currency source, Currency target )
        {
            return Source.EqualsI( source.Name ) && Target.EqualsI( target.Name );
        }
    }
}
