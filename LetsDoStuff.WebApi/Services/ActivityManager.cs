using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using LetsDoStuff.WebApi.Services.DTO;
using LetsDoStuff.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
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

        public List<ActivitiesResponse> GetAllActivities()
        {
            var activities = db.Activities.AsNoTracking()
                .OrderBy(o => o.DateStart)
                .Select(a => new ActivitiesResponse()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    DateStart = a.DateStart.ToLongDateString(),
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
                    Name = activity.Creator.FirstName + " " + activity.Creator.LastName,
                    ProfileLink = activity.Creator.ProfileLink
                },
                DateStart = activity.DateStart.ToLongDateString(),
                Tags = activity.Tags
                    .Select(t => t.Name)
                    .ToList()
            };
        }

        public List<TagResponse> GetAvailableTags()
        {
            var tags = db.Tags.AsNoTracking()
                .Select(t => new TagResponse()
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList();

            return tags;
        }

        public void CreateActivity(CreateActivityCommand newActivity, int idUser)
        {
            var creator = db.Users.FirstOrDefault(u => u.Id == idUser);

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

        public void DeleteActivity(int activityId, int userId)
        {
            var activity = db.Activities
                .FirstOrDefault(a => a.Id == activityId)
                ?? throw new ArgumentException($"An Activity with id {activityId} doesn't exist!");
            
            if (activity.CreatorId == userId)
            {
                db.Remove(activity);
                db.SaveChanges();
            }
            else
            {
                throw new ArgumentException($"The activity with id {activityId} doesn't belong to this user");
            }
        }
    }
}
