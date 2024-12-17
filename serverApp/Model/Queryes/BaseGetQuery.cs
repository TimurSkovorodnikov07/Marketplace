using System.ComponentModel.DataAnnotations;

[BaseGetQueryValidator]
public class BaseGetQuery
{
    [Required, Range(0, int.MaxValue)] public int From { get; set; }
    [Required, Range(0, int.MaxValue)] public int To { get; set; }    
}