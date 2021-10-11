# ScGantt 使用说明(还没有开发完)

## 1、甘特图使用说明

### 属性：

- datas ,数组源格式如下：
  ```javascript
  datas:[
          {
            content: '去永辉买面包', // 任务标题
            parentId: null, // 父id
            children: [ // 子任务
              {
                content: '付钱',
                parentId: '39fdfe09-73ec-daf9-8f0d-1e717ffed971',
                children: [],
                isMilestone: false,
                id: '39fe01db-46d7-0ddb-18fc-4ca3397d97b3',
                startDate: '2021-06-15',
                endDate: '2021-07-15', desc: '',
                duration: 14,
                collapsed: false,
                preTaskIds: [
                  '39fdfe09-73ec-daf9-8f0d-1e717ffed971',
                ],
                disabled: false,
              },
            ],
            isMilestone: false, //是否是里程碑
            id: '39fdfe09-73ec-daf9-8f0d-1e717ffed971', // 任务id
            startDate: '2021-07-01', // 任务开始时间
            endDate: '2021-07-31', // 任务结束时间
            desc: '嘟嘟嘟嘟', // 任务描述
            duration: 30, // 任务工期
            collapsed: false, // 是否闭合(点击三角按钮展开闭合)
            preTaskIds: [], // 前置任务ids
            disabled: false, // 是否禁用
          },
  ],
  ```

## props 说明：

| 字段名称 | 字段说明 | 默认值 | 是否必须 |
| --- | --- | --- | --- |
| width | gantt 图宽度(好像没有啥用,自适应宽度了……) | 1320 | 否 |
| height | gantt 图高度(这个有问题,还没有改好,他会弄的很长) | 1 | 否 |
| data | 数据源(树状列表) 可以看底下的 data 格式 | [] | 否 |
| columns | 自定义列(默认有名称/时间/工期字段 可以自己加别的列) 可以看底下的 columns 格式 | [] | 否 |
| topLvTreeId | 施工计划/总体计划 最顶层树(冠) 对应的 id,比如说现在是施工计划详情树 gantt 图,那么这个 topLvTreeId 应该就是关联的施工计划 Id | null | 否 |
| disableEdit | 是否禁用修改(默认不禁用) | false | 否 |
| showSelection | 是否显示选择框(默认否) | false | 否 |
| selectedIds | 外面传进来的选中的 ids | [] | 否 |
| playSpeed | 播放速度(默认 300ms),这个还没有写完是配合 三维模型生长用的 | 300 | 否 |

## data 数据源 字段说明：

| 字段名称    | 字段说明                       | 字段类型           |
| ----------- | ------------------------------ | ------------------ |
| id          | 主键                           | guid               |
| content     | 任务标题                       | string             |
| startDate   | 开始时间                       | string(2011-02-03) |
| endDate     | 结束时间                       | string(2011-02-03) |
| duration    | 工期                           | number             |
| parentId    | 父 id                          | guid               |
| children    | 子任务                         | Array              |
| isMilestone | 是否里程碑                     | bool               |
| desc        | 任务描述                       | bool               |
| collapsed   | 是否闭合(点击三角按钮展开闭合) | bool               |
| preTaskIds  | 前置任务 ids                   | guid[]             |
| disabled    | 是否禁用                       | bool               |

## columns 说明：

- columns 可以不传,不传 的话 gantt 图 显示 任务名称/开始时间/结束时间/工期
- 如果想显示额外的内容的话,需要写 columns 比如想显示 '工程量' 等字段
  ```javascript
  columns:[   // 这里 customRender 和 antd 的table 用法相同 ,但是想要获取item 的话 应该用 barInfo.task 来获取    { title: '工程量', name: 'material',width:90,customRender:( text,index,barInfo )=>       return  <span color="blue" onClick={event =          this.planContentId = barInfo.task.           this.planMaterialModalVisible = t                  >

            >;
       },
        { title: '工日', name: 'workDay' ,width: 60,customRender:( text,index,ba    >{
          return <span>{te  pan>     }},
        { title: '人工', name: 'workerNumber' ,width: 60,customRender:( text,index     )=>{
          return <span>  </spa;n>;
        }},
  ]
  ```

## columns 字段 说明：

| 字段名称     | 字段说明                                  | 字段类型                             |
| ------------ | ----------------------------------------- | ------------------------------------ |
| title        | 显示在表头的对应列标题                    | string                               |
| name         | 唯一列名                                  | string                               |
| width        | 列宽                                      | number                               |
| minWidth     | 最小宽度                                  | number                               |
| visible      | 是否显示                                  | bool                                 |
| keepVisible  | 拖拽时保持显示                            | bool                                 |
| customRender | 自定义渲染(基本和 antd 的 table 用法一样) | (text,index,barInfo)=> {return html} |
