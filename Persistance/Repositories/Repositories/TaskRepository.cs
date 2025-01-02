﻿using Core.DTO.TaskDTO;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance;
using Persistance.Repositories.AbstractRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.RepositoriesTb
{
    public class TaskRepository : CrudAbstractions<TaskEntity>, ITaskRepository
    {
        private readonly DataContext context;

        public TaskRepository(DataContext context) : base(context, context.Tasks)
        {
            this.context = context;
        }
        public async Task<List<TaskResponse>> GetTasks()
        {
            return await Get(r => new TaskResponse(r.Id, r.TaskName, r.TaskStatus));
        }
        public async Task<TaskResponse?> GetByIdTask(Guid TaskId)
        {
            var task = await GetById(TaskId, u => u);
            if (task == null)
            {
                return null;
            }
            return new TaskResponse(task.Id, task.TaskName, task.TaskStatus);
        }
        public async Task<List<TaskResponse?>> GetByUserTask(Guid userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }
            var task = await context.Tasks
                .Where(i => i.UserId == userId)
                .Select(t => new TaskResponse(t.Id, t.TaskName, t.TaskStatus))
                .ToListAsync();
            if (task == null || task == null)
            {
                return null;
            }
            return task;
        }
        public async Task<Guid?> CreateTask(Guid TaskId, string taskName, string taskStatus,Guid userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }
            var userEntity = new TaskEntity
            {
                Id = TaskId,
                TaskName = taskName,
                TaskStatus = taskStatus,
                UserId = userId
            };
            return await Create(userEntity);
        }
        public async Task<Guid?> UpdateTask(Guid id, string taskName, string taskStatus)
        {
            var task = await GetById(id, u => u);
            if (task == null)
            {
                return null;
            }
            return await Update(id, task =>
            {
                task.Id = id;
                task.TaskName = taskName;
                task.TaskStatus = taskStatus;
            });
        }
        public async Task<Guid?> DeleteTask(Guid id)
        {
            var task = await GetById(id, u => u);
            if (task == null)
            {
                return null;
            }
            return await Delete(id);
        }
    }
}
