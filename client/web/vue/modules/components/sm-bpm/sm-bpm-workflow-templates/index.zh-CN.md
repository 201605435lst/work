## API

| 参数  | 说明                         | 类型     | 默认值 |
| ----- | ---------------------------- | -------- | ------ |
| axios | 项目 axios.create 创建的实例 | function |

## 方法

| 名称        | 描述                                    | 参数       |
| ----------- | --------------------------------------- | --------------- |
| view()        | 查看事件                                 | (id) => void    |
| edit()        | 编辑事件                                 | (id) => void    |

------

## 流程选择框API说明

### 1、属性

- axios
- width :弹窗宽度，default:800
- height：弹窗高度，default:600

### 2、方法

- show（）,打开弹窗

### 3、事件

- selected ： 选中的流程模板id



# ❤❤❤❤❤其他模块关联单一流程开发说明❤❤❤❤❤

## 1、设计思路：

1、为了满足实际业务需求，将工作流程分为：表单流程和无表单的流程。

2、表单流程是流程和表单相结合，用于专门的流程审批，结合表单进行相关功能操作；

3、无表单的流程主要用于个别模块业务中使用审批流程使用，即只使用了流程引擎，与表单无关系。

## 2、代码说明

1、需要关联流程的模块中依赖SnAbp.Bpm相关项目。

2、需要关联流程的实体继承类继承`SingleFlowEntity`类

例如：有一个计划类，需要关联审批流程，则写法如下：

```c#
 public class Plan : SingleFlowEntity
 {
     //  some fields
 } 
```

`SingleFlowEntity`类的定义如下：

```c#
 public class SingleFlowEntity : AuditedEntity<Guid>
    {
        /// <summary>
        /// 工作流id
        /// </summary>
        public virtual Guid? WorkflowId { get; set; }
        /// <summary>
        /// 工作流模板id
        /// </summary>
        public virtual Guid? WorkflowTemplateId { get; set; }
        /// <summary>
        /// 工作流
        /// </summary>
        public virtual Workflow Workflow { get; set; }
    }
```

3、给创建的实体类创建一个关联表，用来存储该数据的流程相关的内容，该类要继承`SingleFlowRltEntity`类

例如：计划类关联流程的实体为：`PlanRltSingleFlow`,(表名自己定义即可)，写法如下：

```c#
 public class PlanRltFlowInfo : SingleFlowRltEntity  
    {
        public PlanRltFlowInfo(Guid id) => this.Id = id;
        public virtual Guid PlanId { get; set; }
        public virtual Plan Plan { get; set; }
    }
```

`SingleFlowRltEntity` 类的定义：

```c#
public abstract class SingleFlowRltEntity : AuditedEntity<Guid>
    {
        /// <summary>
        /// 工作流id
        /// </summary>
        public virtual Guid WorkFlowId { get; set; }
        /// <summary>
        /// 工作流的状态
        /// </summary>
        public virtual WorkflowState State { get; set; }
        /// <summary>
        /// 流程审批描述内容，如反馈的消息等；
        /// </summary>
        public virtual string Content { get; set; }
    }
```

### ISingleFlowProcessService 接口使用

ISingleFlowProcessService接口是封装的其他模块操作流程数据的方法，用法如下：

1、在创建一个计划，由于计划关联了流程，所以先根据流程模板创建流程，再创建计划

```c#
 // 先创建一个并启动一个计划
var workflow = await _singleFlowProcess.CreateSingleWorkFlow(input.WorkFlowTemplateId);
var plan = new Plan(_generator.Create());
plan.Name = input.Name;
plan.WorkflowId = workflow.Id;
// 添加数据
await _plans.InsertAsync(plan);
```

2、获取计划关联流程的各个节点的名称及审批人员信息，方法：

```c#
public async Task<List<SingleFlowNodeDto>> GetSingleFlowNodes(Guid id)
        {
            var model = await _plans.GetAsync(id);
            var workflowId = model.WorkflowId.GetValueOrDefault();
            var infos = await _flowInfos.Where(a => a.PlanId == id).ToListAsync();
            var nodes = await _singleFlowProcess.GetWorkFlowNodes(workflowId);
            foreach (var node in nodes)
            {
                node.Comments ??= new List<CommentDto>();
                node.Approvers?.ForEach(a =>
                {
                    var info = infos.FirstOrDefault(b => b.CreatorId == a.Id);
                    var comment = new CommentDto()
                    {
                        User = a,
                        Content = infos.Where(b => b.CreatorId == a.Id).Select(a => a.Content).ToList(),
                        ApproveTime = info?.CreationTime ?? default
                    };
                    node.Comments.Add(comment);
                });
            }
            return await _singleFlowProcess.GetNodeTree(nodes);
        }
```

3、获取计划的列表，由于在前端要显示该条计划是不是审批状态，是不是登录用户有权限审批。

```c#
var plans = await _plans.WithDetails(a=>a.Infos).ToListAsync();
var dtos = ObjectMapper.Map<List<Plan>, List<PlanDto>> (plans);
foreach (var item in dtos)
{
    item.IsWaiting = await _singleFlowProcess.IsWaitingMyApproval(item.WorkflowId.GetValueOrDefault());
    var state = item.Infos.OrderByDescending(a => a.CreationTime).FirstOrDefault()?.State;
    if (state != null)
    {
        item.State = (int) state;
    }
}
```

4、通过审批，在其他的界面中通过流程的审批，示例写法如下：

```c#
public async Task<bool> ApproveTest(Guid id)
{
    var plan = _plans.FirstOrDefault(a => a.Id == id);
    var detial = plan.WorkflowId.GetValueOrDefault();
    var result = await _singleFlowProcess.Approved(detial);// 关键代码
    var info = new PlanRltFlowInfo(_generator.Create());
    info.Content = "666";
    info.PlanId = id;
    info.WorkFlowId = result.Id;
    info.State = result.State;
    await _flowInfos.InsertAsync(info);
    return true;
}
```

