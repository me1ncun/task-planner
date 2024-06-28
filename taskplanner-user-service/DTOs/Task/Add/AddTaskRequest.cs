using System.ComponentModel.DataAnnotations;
using AutoMapper.Configuration.Annotations;
using Newtonsoft.Json;
using taskplanner_user_service.Helpers.Attributes;

namespace taskplanner_user_service.DTOs;

public class AddTaskRequest{
    [Required] public string Title { get; set; }
    [Required] public string Description { get; set; }
    [Required] public string Status  { get; set; } 
    [SwaggerIgnore] public int UserId { get; set; }
    [JsonIgnore] public DateTime DoneAt  { get; set; }
}