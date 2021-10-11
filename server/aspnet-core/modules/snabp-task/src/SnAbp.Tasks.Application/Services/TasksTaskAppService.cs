using SnAbp.Identity;
using SnAbp.Message.Notice;
using SnAbp.Tasks.Dtos;
using SnAbp.Tasks.Entities;
using SnAbp.Tasks.enums;
using SnAbp.Tasks.Enums;
using SnAbp.Tasks.IServices;
using SnAbp.Utils.EnumHelper;
using SnAbp.Utils.ExcelHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace SnAbp.Tasks.Services
{
    public class TasksTaskAppService : TasksAppService, ITasksTaskAppService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IUnitOfWorkManager _unitOfWork;
        private readonly IRepository<Task, Guid> _taskRepository;
        private readonly IRepository<TaskRltMember, Guid> _taskRltMemberRepository;
        private readonly IRepository<TaskRltFile, Guid> _taskRltFileRepository;
        private readonly IRepository<IdentityUser, Guid> _usersRepository;
        private readonly IdentityUserManager _identityUserManager;
        private IMessageNoticeProvider _iMessageNoticeProvider;

        public TasksTaskAppService(
            IGuidGenerator guidGenerator,
            IRepository<Task, Guid> taskRepository,
            IUnitOfWorkManager unitOfWork,
            IdentityUserManager identityUserManager,
            IRepository<TaskRltMember, Guid> taskRltMemberRepository,
            IRepository<TaskRltFile, Guid> taskRltFileRepository,
            IRepository<IdentityUser, Guid> usersRepository,
            IMessageNoticeProvider iMessageNoticeProvider
            )
        {
            _guidGenerator = guidGenerator;
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
            _identityUserManager = identityUserManager;
            _taskRltMemberRepository = taskRltMemberRepository;
            _taskRltFileRepository = taskRltFileRepository;
            _usersRepository = usersRepository;
            _iMessageNoticeProvider = iMessageNoticeProvider;
        }

        public async Task<TaskDto> Create(TaskCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("任务主题和名称不能为空");
            if (!input.Priority.IsIn(PriorityType.ImportantUrgent, PriorityType.ImportantNoUrgent, PriorityType.NoImportantUrgent, PriorityType.NoImportantNoUrgent)) throw new UserFriendlyException("请选择任务优先级");
            if (input.StartTime == null) throw new UserFriendlyException("请选择任务开始时间");
            if (input.EndTime == null) throw new UserFriendlyException("请选择任务结束时间");
            if (input.EndTime != null)
            {
                if (input.EndTime <= input.StartTime)
                {
                    throw new UserFriendlyException("任务结束时间不能在开始时间之前！");
                }
            }

            await CheckSameName(input.ParentId, null, input.Name);

            using var unow = _unitOfWork.Begin(true, false);

            var task = new Task(_guidGenerator.Create())
            {
                ProjectId = input.ProjectId,
                Name = input.Name,
                Priority = input.Priority,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
                Content = input.Content,
                State = input.State,
            };

            //保存关联人员表
            task.TaskRltMembers = new List<TaskRltMember>();
            foreach (var trm in input.TaskRltMembers)
            {
                task.TaskRltMembers.Add(new TaskRltMember(_guidGenerator.Create())
                {
                    TaskId = task.Id,
                    MemberId = trm.Id,
                    Responsible = trm.Responsible
                });
            }
            task.TaskRltMembers.Add(new TaskRltMember(_guidGenerator.Create())
            {
                TaskId = task.Id,
                MemberId = CurrentUser.Id.Value,
                Responsible = ResponsibleType.Initial
            });

            //保存关联文件表
            task.TaskRltFiles = new List<TaskRltFile>();
            foreach (var trf in input.TaskRltFiles)
            {
                task.TaskRltFiles.Add(new TaskRltFile(_guidGenerator.Create())
                {
                    TaskId = task.Id,
                    FileId = trf.Id,
                    FileType = FileType.Created
                });
            }
            await _taskRepository.InsertAsync(task);
            await unow.SaveChangesAsync();

            if (input.ChildrenTasks.Count > 0)
            {
                foreach (var childrens in input.ChildrenTasks)
                {
                    var childtask = new Task(_guidGenerator.Create())
                    {
                        ParentId = task.Id,
                        Name = childrens.Name,
                        StartTime = task.StartTime, //子任务的开始时间为父级开始时间
                        EndTime = childrens.EndTime,
                        Weight = childrens.Weight,
                        Proportion = childrens.Proportion,
                        Priority = input.Priority,
                        State = input.State,
                    };
                    //保存关联人员表
                    childtask.TaskRltMembers = new List<TaskRltMember>();
                    foreach (var trm in childrens.TaskRltMembers)
                    {
                        childtask.TaskRltMembers.Add(new TaskRltMember(_guidGenerator.Create())
                        {
                            TaskId = childtask.Id,
                            MemberId = trm.Id,
                            Responsible = trm.Responsible
                        });
                    }
                    childtask.TaskRltMembers.Add(new TaskRltMember(_guidGenerator.Create())
                    {
                        TaskId = childtask.Id,
                        MemberId = CurrentUser.Id.Value,
                        Responsible = ResponsibleType.Initial
                    });
                    await _taskRepository.InsertAsync(childtask);
                    await unow.SaveChangesAsync();
                }
            }

            ///保存成功后给通知人员发送消息
            var message = new NoticeMessage();
            message.SendType = Message.MessageDefine.SendModeType.User;
            // 获取通知人员的ids
            List<Guid> userIds = new List<Guid>();
            var taskRltMembers = _taskRltMemberRepository.Where(x => x.TaskId == task.Id && x.Responsible != ResponsibleType.Initial);
            foreach (var taskRltMember in taskRltMembers)
            {
                userIds.Add((Guid)taskRltMember.MemberId);
            }
            message.SetUserIds(userIds.ToArray());
            var content = new NoticeMessageContent();
            content.Type = "task";
            var taskMessageNoticeDto = new TaskMessageNoticeDto();
            //获取提交消息的人
            var user = await _usersRepository.GetAsync(CurrentUser.Id.Value);
            taskMessageNoticeDto.Name = user.Name;
            var taskTypeName = getTaskTypeName(input.State);
            taskMessageNoticeDto.TaskType = taskTypeName;
            content.CreateContent(taskMessageNoticeDto);
            message.SetContent(content);
            await _iMessageNoticeProvider.PushAsync(message.GetBinary());

            return await System.Threading.Tasks.Task.FromResult(ObjectMapper.Map<Task, TaskDto>(task));
        }

        public async Task<PagedResultDto<TaskDto>> GetList(TaskSearchDto input)
        {
            var members = await _identityUserManager.GetUserMembers(CurrentUser.Id.Value);
            var memberIds = members.Select(x => x.Id).ToList();
            var currentUserOrganizations = _identityUserManager.GetOrganizationsAsync(CurrentUser.Id.GetValueOrDefault()).Result;

            //var users = _userRepository.Where(x => userIds.Contains(x.Id)).ToList();
            var list = new List<Task>();
            //var list = (List<Task>)_taskRepository.WithDetails().WhereIf(!string.IsNullOrEmpty(input.KeyWords), x => x.Project.Name.Contains(input.KeyWords) || x.Name.Contains(input.KeyWords));
            var rst = new PagedResultDto<TaskDto>();
            if (input.Date == null)
            {
                switch (input.Group)
                {
                    // 我发起的
                    case TaskGroup.Initial:
                        {
                            list = _taskRepository.WithDetails().Where(y => y.CreatorId == CurrentUser.Id.Value).ToList();
                            if (!input.KeyWords.IsNullOrEmpty())
                            {
                                list = list.Where(x => x.Project != null ? x.Project.Name.Contains(input.KeyWords) : false || x.Name.Contains(input.KeyWords)).ToList();
                            }
                            break;
                        }
                    // 我负责的
                    case TaskGroup.Manage:
                        {
                            list = _taskRltMemberRepository.WithDetails().Where(y =>
                            y.Responsible == ResponsibleType.Manager && y.MemberId == CurrentUser.Id.Value).Select(z => z.Task).ToList();
                            if (!input.KeyWords.IsNullOrEmpty())
                            {
                                list = list.Where(x => x.Project != null ? x.Project.Name.Contains(input.KeyWords) : false || x.Name.Contains(input.KeyWords)).ToList();
                            }
                            break;
                        }
                    // 我参与的
                    case TaskGroup.Cc:
                        {
                            list = _taskRltMemberRepository.WithDetails().Where(y =>
                            y.Responsible == ResponsibleType.Cc && y.MemberId == CurrentUser.Id.Value).Select(z => z.Task).ToList();
                            if (!input.KeyWords.IsNullOrEmpty())
                            {
                                list = list.Where(x => x.Project != null ? x.Project.Name.Contains(input.KeyWords) : false || x.Name.Contains(input.KeyWords)).ToList();
                            }
                            break;
                        }
                    case TaskGroup.All:
                        if (!input.KeyWords.IsNullOrEmpty())
                        {
                            list = list.Where(x => x.Project != null ? x.Project.Name.Contains(input.KeyWords) : false || x.Name.Contains(input.KeyWords)).ToList();
                        }
                        break;
                    default:
                        throw new UserFriendlyException("数据处理异常");
                }
            }
            else
            {
                list = _taskRepository.WithDetails().Where(x => x.StartTime.ToString().Contains(input.Date) || x.EndTime.ToString().Contains(input.Date))
                    .WhereIf(input.IsChecked, x => x.State == StateType.Processing)
                    .ToList();
            }
            rst.TotalCount = list.Count();
            var tasks = input.Date == null ? list.OrderByDescending(s => s.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList() : list;
            rst.Items = ObjectMapper.Map<List<Task>, List<TaskDto>>(tasks);
            return rst;
        }

        public Task<TaskExtendDto> Get(Guid id)
        {
            TaskExtendDto taskExtendDto = null;

            if (id == Guid.Empty || id == null) throw new UserFriendlyException("Id不正确");
            var result = _taskRepository.WithDetails().FirstOrDefault(x => x.Id == id);
            if (result == null) throw new UserFriendlyException("任务信息不存在");

            /*var allResult = _taskRepository.WithDetails().Where(x => x.Id == id || x.ParentId == id);
            foreach (var item in allResult)
            {
                taskExtendDto = ObjectMapper.Map<Task, TaskExtendDto>(result);
                if (item.LastModifierId != null)
                {
                    taskExtendDto.LastModifier = _usersRepository.FirstOrDefault(x => x.Id == item.LastModifierId).Name;
                }
            }*/
            taskExtendDto = ObjectMapper.Map<Task, TaskExtendDto>(result);

            getChildrenTasks(taskExtendDto, id); //递归组装数据

            return System.Threading.Tasks.Task.FromResult(taskExtendDto);
        }

        public async Task<TaskDto> Update(TaskUpdateDto input)
        {
            var task = await _taskRepository.GetAsync(input.Id);
            if (task == null) throw new UserFriendlyException("该项目信息不存在");

            task.ProjectId = input.ProjectId;
            task.Name = input.Name;
            task.Priority = input.Priority;
            task.StartTime = input.StartTime;
            task.EndTime = input.EndTime;
            task.Content = input.Content;

            //清除保存的附件关联表信息
            await _taskRltFileRepository.DeleteAsync(x => x.TaskId == input.Id);
            //清除保存的通知人员关联表信息
            await _taskRltMemberRepository.DeleteAsync(x => x.TaskId == input.Id);

            //保存关联人员表
            task.TaskRltMembers = new List<TaskRltMember>();
            foreach (var trm in input.TaskRltMembers)
            {
                task.TaskRltMembers.Add(new TaskRltMember(_guidGenerator.Create())
                {
                    TaskId = task.Id,
                    MemberId = trm.Id,
                    Responsible = trm.Responsible
                });
            }
            task.TaskRltMembers.Add(new TaskRltMember(_guidGenerator.Create())
            {
                TaskId = task.Id,
                MemberId = CurrentUser.Id.Value,
                Responsible = ResponsibleType.Initial
            });

            //重新保存关联文件表
            task.TaskRltFiles = new List<TaskRltFile>();
            foreach (var trf in input.TaskRltFiles)
            {
                task.TaskRltFiles.Add(new TaskRltFile(_guidGenerator.Create())
                {
                    TaskId = task.Id,
                    FileId = trf.Id,
                    FileType = FileType.Feedback
                });
            }
            await _taskRepository.UpdateAsync(task);

            return await System.Threading.Tasks.Task.FromResult(ObjectMapper.Map<Task, TaskDto>(task));
        }

        public async Task<TaskDto> Feedback(TaskFeedBackDto input)
        {
            if (string.IsNullOrEmpty(input.Description)) throw new UserFriendlyException("请填写任务完成情况描述");

            var task = await _taskRepository.GetAsync(input.Id);
            if (task == null) throw new UserFriendlyException("该项目信息不存在");


            var childTask = _taskRepository.Where(x => x.ParentId == input.Id); //查询此任务的子任务，如果存在未完成的，则不可完成
            foreach (var item in childTask)
            {
                if (item.State != StateType.Finshed || item.Progress != 100)
                {
                    throw new UserFriendlyException("请确认子任务已全部完成！");
                }
            }


            using var unow = _unitOfWork.Begin(true, false);

            task.Description = input.Description;
            task.Progress = input.Progress;
            if (input.Progress == 100) //如果反馈的进度为100，则更改状态为已完成
            {
                task.State = StateType.Finshed;
            }
            await _taskRepository.UpdateAsync(task);
            await unow.SaveChangesAsync();

            //保存关联文件表(我反馈的)
            //清除保存的附件关联表信息（我反馈的）
            await _taskRltFileRepository.DeleteAsync(x => x.TaskId == input.Id && x.FileType == FileType.Feedback);
            await unow.SaveChangesAsync();

            task.TaskRltFiles = new List<TaskRltFile>();
            foreach (var trf in input.TaskRltFiles)
            {
                task.TaskRltFiles.Add(new TaskRltFile(_guidGenerator.Create())
                {
                    TaskId = task.Id,
                    FileId = trf.Id,
                    FileType = FileType.Feedback
                });
            }
            for (var i = 0; i < task.TaskRltFiles.Count; i++)
            {
                await _taskRltFileRepository.InsertAsync(task.TaskRltFiles[i]);
                await unow.SaveChangesAsync();
            }
            /*foreach (var item in task.TaskRltFiles)
            {
                await _taskRltFileRepository.InsertAsync(item);
                await unow.SaveChangesAsync();
            }*/

            //递归更改父级至最顶级的相关进度

            if (task.ParentId.HasValue)
            {
                await UpdateProgress(task.ParentId.Value);
                // var task2 =  _taskRepository.Where( x=>x.Id== task.ParentId.Value).FirstOrDefault();
            }

            return null;
        }

        public Task<bool> UpdateState(TaskUpdateStateDto input)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");

            await _taskRltMemberRepository.DeleteAsync(x => x.TaskId == id);
            await _taskRltFileRepository.DeleteAsync(x => x.TaskId == id);

            await _taskRepository.DeleteAsync(id);

            return true;
        }

        public Task<Stream> Export(EduceDto input)
        {
            var tasks = _taskRepository.WithDetails().Where(x => input.TaskIds.Contains(x.Id)).ToList();
            var list = ObjectMapper.Map<List<Task>, List<Task>>(tasks.OrderBy(x => x.CreationTime).ToList());
            Stream stream = null;
            byte[] sbuf;
            var dt = (DataTable)null;
            var dataColumn = (DataColumn)null;
            var dataRow = (DataRow)null;
            dt = new DataTable();
            //添加表头
            var enumValues = Enum.GetValues(typeof(ExportTasks));
            if (enumValues.Length > 0)
            {
                foreach (int item in enumValues)
                {
                    dataColumn = new DataColumn(Enum.GetName(typeof(ExportTasks), item));
                    dt.Columns.Add(dataColumn);
                }
            }
            //添加内容
            foreach (var row in list)
            {
                dataRow = dt.NewRow();
                dataRow[ExportTasks.项目名称.ToString()] = row.Project?.Name;
                dataRow[ExportTasks.项目编号.ToString()] = row.Project?.Code;
                dataRow[ExportTasks.任务主题.ToString()] = row.Name;
                dataRow[ExportTasks.所属任务.ToString()] = row.Parent?.Name;
                dataRow[ExportTasks.任务状态.ToString()] = row.State.GetDescription();
                dataRow[ExportTasks.任务优先级.ToString()] = row.Priority.GetDescription();
                dataRow[ExportTasks.指派人员.ToString()] = row.TaskRltMembers.FirstOrDefault(x => x.Responsible == ResponsibleType.Initial)?.Member.Name;
                dataRow[ExportTasks.负责人员.ToString()] = getManagetNames(row.TaskRltMembers, ResponsibleType.Manager);
                dataRow[ExportTasks.抄送人员.ToString()] = getManagetNames(row.TaskRltMembers, ResponsibleType.Cc);
                dataRow[ExportTasks.任务创建时间.ToString()] = row.CreationTime;
                dataRow[ExportTasks.开始时间.ToString()] = row.StartTime;
                dataRow[ExportTasks.结束时间.ToString()] = row.EndTime;
                dataRow[ExportTasks.任务进度.ToString()] = row.Progress + "%";
                dataRow[ExportTasks.是否过期.ToString()] = getOverdue(row.EndTime);
                dt.Rows.Add(dataRow);
            }
            sbuf = ExcelHelper.DataTableToExcel(dt, "任务清单表.xlsx");
            stream = new MemoryStream(sbuf);
            return System.Threading.Tasks.Task.FromResult(stream);
        }



        #region 私有放法

        private async System.Threading.Tasks.Task UpdateProgress(Guid id)
        {
            var task = await _taskRepository.GetAsync(id, false);

            if (task != null) //当有父级时，此子级存在占比。
            {
                using var unow = _unitOfWork.Begin(true, false);
                var childTasks = _taskRepository.Where(x => x.ParentId == id).ToList();
                var sum = childTasks.Sum(a => a.Weight);
                var percent = 0d;
                foreach (var child in childTasks)
                {
                    percent += child.Progress * (child.Weight / sum);
                }
                task.Progress = (int)percent;
                await _taskRepository.UpdateAsync(task); //将父级进度更新
                await unow.SaveChangesAsync();

                if (task.ParentId.HasValue)
                {
                    await UpdateProgress(task.ParentId.Value);
                }
            }
        }

        async Task<bool> CheckSameName(Guid? parentId, Guid? id, string name)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                if (parentId == null) return true;

                var sameNames =
                    _taskRepository.FirstOrDefault(a =>
                        a.Name == name && a.ParentId == parentId && a.Id != id);
                if (sameNames != null)
                {
                    throw new UserFriendlyException("当前级别下已存在该名称的任务！");
                }

                return true;
            });
        }

        private void getChildrenTasks(TaskExtendDto taskExtendDto, Guid id)
        {
            var child = _taskRepository.WithDetails().Where(x => x.ParentId == id).ToList();
            foreach (var item in child)
            {
                taskExtendDto.ChildrenTasks.Add(ObjectMapper.Map<Task, TaskExtendDto>(item));
                if (item.LastModifierId != null)
                {
                    taskExtendDto.ChildrenTasks[child.IndexOf(item)].LastModifier = _usersRepository.FirstOrDefault(x => x.Id == item.LastModifierId)?.Name;
                }
                if (_taskRepository.Where(x => x.ParentId == item.Id).Count() > 0)
                {
                    getChildrenTasks(taskExtendDto.ChildrenTasks[child.IndexOf(item)], item.Id);
                }
            }
        }

        private string getTaskTypeName(StateType Type)
        {
            string typeName = null;

            if (Type == StateType.Processing)
            {
                typeName = "正在进行";
            }
            if (Type == StateType.Receive)
            {
                typeName = "已结项";
            }
            if (Type == StateType.Finshed)
            {
                typeName = "已完成";
            }
            if (Type == StateType.Refused)
            {
                typeName = "已驳回";
            }
            if (Type == StateType.NoStart)
            {
                typeName = "未启动";
            }
            if (Type == StateType.Stop)
            {
                typeName = "已暂停";
            }
            return typeName;
        }

        private string getManagetNames(List<TaskRltMember> rltMembers, ResponsibleType responsible)
        {
            string names = "";
            var members = rltMembers.Where(x => x.Responsible == responsible);
            foreach (var member in members)
            {
                names = member.Member?.Name + "," + names;
            }
            return names.Trim(',');
        }

        private string getOverdue(DateTime dateTime)
        {
            string overdue = "";
            DateTime date = DateTime.Now;

            if (DateTime.Compare(dateTime, date) > 0) //dateTime - date > 0
            {
                overdue = "否";
            }
            else
            {
                TimeSpan sp = date.Subtract(dateTime);
                overdue = "超期" + sp.TotalDays.ToString("0.00") + "天";
            }
            return overdue;
        }

        #endregion
    }
}
