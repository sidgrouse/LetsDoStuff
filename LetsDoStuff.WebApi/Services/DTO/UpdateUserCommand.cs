using System;
using System.Collections.Generic;
using System.Text;
using LetsDoStuff.WebApi.Controllers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LetsDoStuff.WebApi.Services.DTO
{
    public class UpdateUserCommand
    {
        public int IdUser { get; set; }
        
        public JsonPatchDocument<EditUserSettingsRequest> PatchDoc { get; set; }

        public UserController Controller { get; set; }

        public UpdateUserCommand(int idUser, JsonPatchDocument<EditUserSettingsRequest> patchDoc, UserController controller)
        {
            IdUser = idUser;
            PatchDoc = patchDoc;
            Controller = controller;
        }
    }
}
