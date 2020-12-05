using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using LetsDoStuff.Domain.Models.DTO;
using LetsDoStuff.WebApi.SQC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LetsDoStuff.WebApi.Services
{
    public class ActivityManager
    {
        private LdsContext db;

        public ActivityManager(LdsContext context)
        {
            db = context;
        }

        public List<GetActivitiesQueryResult> GetActivities()
        {
            var activities = db.Activities.AsNoTracking()
                .Select(a => new GetActivitiesQueryResult()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Capacity = a.Capacity,
                    Creator = a.Creator,
                    Tags = a.Tags.Select(t => new Tag
                    {
                        Id = t.Id,
                        Name = t.Name
                    }).ToList()
                }).ToList();

            return activities;
        }

        public GetActivitiesQueryResult GetActivityById(int id)
        {
            var activity = db.Activities.AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new GetActivitiesQueryResult()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Capacity = a.Capacity,
                    Creator = a.Creator,
                    Tags = a.Tags.Select(t => new Tag
                    {
                        Id = t.Id,
                        Name = t.Name
                    }).ToList()
                }).FirstOrDefault();

            return activity;
        }

        public IActionResult CreateActivity(CreateActivityCommand newActivity)
        {
            var activity = new Activity(); 

            activity.Name = newActivity.Name;
            activity.Description = newActivity.Description;
            activity.Capacity = newActivity.Capacity;
            activity.Creator = db.Users.Find(newActivity.CreatorId);
            if (activity.Creator == null)
            {
                return new BadRequestResult();
            }

            Tag tag;
            foreach (int id in newActivity.TagIds)
            {
                tag = db.Tags.Find(id);
                if (tag == null)
                {
                    return new BadRequestResult();
                }

                activity.Tags.Add(tag);
            }

            db.Activities.Add(activity);
            db.SaveChanges();

            return new OkResult();
        }
    }
}
