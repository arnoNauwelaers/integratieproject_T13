using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace politiekeBarometer.Models
{
  public class AlertCreateViewModel
  {

    public int Id { get; set; }
    [Display(Name = "Percentage stijging/daling")]
    public double Percentage { get; set; }
    [Display(Name = "Alert instellen op: ")]
    public string ItemName { get; set; }
    public IEnumerable<SelectListItem> Items { get; set; }
    [Display(Name = "Vergelijk met")]
    public string CompareWith { get; set; }
    public int CompareId { get; set; }
    [Display(Name = "Vergelijken op aantal posts of het publieke sentiment")]
    public string CompareOn { get; set; }
    [Display(Name = "Vergeleken met")]
    public string CompareItem { get; set; }

  }
}