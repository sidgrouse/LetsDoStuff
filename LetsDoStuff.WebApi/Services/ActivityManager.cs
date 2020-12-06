using System;
using System.Collections.Generic;
using System.Linq;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using LetsDoStuff.Domain.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace LetsDoStuff.WebApi.Services
{
    public class ActivityManager : IActivityService
    {
        private readonly LdsContext db;

        public ActivityManager(LdsContext context)
        {
            db = context;
        }

        public List<ActivityResponse> GetAllActivities()
        {
            var activities = db.Activities.AsNoTracking()
                .Select(a => new ActivityResponse()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Capacity = a.Capacity,
                    Creator = new ActivityCreatorResponse()
                    {
                        Id = a.Creator.Id,
                        Name = a.Creator.Name,
                        Login = a.Creator.Login
                    },
                    Tags = a.Tags.Select(t => t.Name).ToList()
                }).ToList();

            return activities;
        }

        public ActivityResponse GetActivityById(int id)
        {
            var activity = db.Activities.AsNoTracking()
                .Include(a => a.Tags)
                .Include(a => a.Creator)
                .FirstOrDefault(a => a.Id == id)
                ?? throw new ArgumentException($"There is no activity with id {id}");

            return new ActivityResponse()
            {
                Id = activity.Id,
                Name = activity.Name,
                Description = activity.Description,
                Capacity = activity.Capacity,
                Creator = new ActivityCreatorResponse()
                    { 
                        Id = activity.Creator.Id,
                        Name = activity.Creator.Name,
                        Login = activity.Creator.Login
                    },
                Tags = activity.Tags
                    .Select(t => t.Name)
                    .ToList()
            };
        }

        public void CreateActivity(CreateActivityCommand newActivity)
        {
            var creator = db.Users.Find(newActivity.CreatorId)
                ?? throw new ArgumentException($"{nameof(newActivity.CreatorId)} has to point to an existed user");

            var activity = new Activity
            {
                Creator = creator,
                Name = newActivity.Name,
                Description = newActivity.Description,
                Capacity = newActivity.Capacity
            };

            foreach (int id in newActivity.TagIds)
            {
                var tag = db.Tags.Find(id);
                if (tag == null)
                {
                    throw new ArgumentException($"Tag with id {id} has not been found");
                }

                activity.Tags.Add(tag);
            }

            db.Activities.Add(activity);
            db.SaveChanges();
        }
    }
}
