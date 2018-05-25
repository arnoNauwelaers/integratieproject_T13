using BL.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace politiekeBarometer.Models
{
    public class ItemCreateViewModel
    {
        public int ItemId { get; set; }
        [Display(Name = "Type")]
        public string Type { get; set; }
        [Display(Name = "Naam")]
        public string Name { get; set; }
        [Display(Name = "Organisatie")]
        public int SelectedOrganizationId { get; set; }
        public List<int> ProfileIds { get; set; }
        [Display(Name = "Twitter account")]
        public String TwitterUrl { get; set; }
        public IEnumerable<SelectListItem> Organizations { get; set; }
        [Display(Name = "Verwijder trefwoorden")]
        public IEnumerable<int> SelectedKeywords { get; set; }
        public List<SelectListItem> ListKeywords { get; set; }
        [Display(Name = "Toevoegen trefwoorden")]
        public string StringKeywords { get; set; }

    }
}