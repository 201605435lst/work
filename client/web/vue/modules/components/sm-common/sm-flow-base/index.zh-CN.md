# 流程节点展示组件API说明

| 参数  | 说明                         | 类型     | 默认值 |
| ----- | ---------------------------- | -------- | ------ |
| axios | 项目 axios.create 创建的实例 | function |
| showDetail | 是否展示审批节点的审批意见 | Boolean | false |
| isView | 是否为查看模式，查看模式下只有节点名称 | Boolean | false |

## 1、任务节点组件属性

- ### nodes

节点数据，由后端传递任务节点数据，有固定的数据格式基础格如下，是一个json格式；

```json
[
        {
          id:1,
          title:'测试任务1',
          subTitle:'',
          perc:40,
          type:'Processing',
          children:[
            {
               id:2,
               title:'子任务1',
               subTitle:'',
               perc:10,
               type:'Processing',
               children:[],
            },
            {
               id:3,
               title:'子任务2',
               subTitle:'',
               perc:10,
               type:'Processing',
               children:[]
            }
          ]
        }
      ]
```

- ### height ：画布的高度，类型为Number，默认 600

- ### width：画布的宽度，类型为Number，默认900

- ### scroller：是否显示滚动条，类型为Boolean,默认为true

- ### filedTrans： 字段转换，类型为：Object

​    当后端返回的数据字段与上述json提供的字段不相同时，可以使用该属性进行转换。例如：

```json
// 后端数据
var dto={
    name:'123',
    people:'zhangsan',
    ...
};
// 数据转换
filedTrans={
    title：'name',
    subTitle:'people',
    ...
}
```



## 2、工作流节点展示组件API说明

### 1、属性

- bpmNode   是否显示为bpm流程节点，boolean 类型，默认是false
- showDetail  是否显示已审核节点的审核评论信息。默认是true
- grid  是否显示流程画布北京方格，默认是true
- nodes 流程数据，当节点类型是bpmNode 时，nodes数据与taskNode数据有区别

### 2、nodes数据

数据格式如下，数据一般有后端直接返回，后端返回接口请查看工作流单一流程开发接口说明

```json
data2:[
        {
          id:1,
          name:'开始节点',
          active:false,
          type:'bpmStart',
          date:'2020-12-01',
          creator:'张三',
          children:[
            {
                id:2,
                name:'审批节点1',
                active:false,
                type:'bpmApprove',
                date:'2020-12-01',
                approvers:[{ name:'张三'}],
                comments:[
                  {
                    approveTime:'2020-01-01',
                    content:'同意',
                  },
                  {
                    approveTime:'2020-01-02',
                    content:'同意666',
                  }
                ],
                children:[
                  {
                    id:3,
                    name:'审批节点2',
                    active:false,
                    type:'bpmApprove',
                    date:'2020-12-02',
                    approvers:[
                      {name:'李四'},
                      {name:'赵六'},
                     ],
                     comments:[
                      {
                       approveTime:'2020-01-01',
                       content:'同意',
                      },
                      {
                        approveTime:'2020-01-02',
                        content:[
                          '你愁啥',
                          '瞅你咋地'
                        ]
                       }
                      ],
                     children:[
                      {
                        id:4,
                        name:'结束节点',
                        type:'bpmEnd',
                        date:'2020-12-02',
                      }
                    ]

                  }

                ],
            },
          ]
        }
      ]
```

