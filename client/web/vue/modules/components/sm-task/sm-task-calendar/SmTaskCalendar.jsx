import moment from 'moment';
import { requestIsSuccess, getTaskGroup, getPriorityType, getTaskColor, getStateType } from '../../_utils/utils';
import { TaskGroup, PriorityType, StateType } from '../../_utils/enum';
import ApiTask from '../../sm-api/sm-task/Task';
let apiTask = new ApiTask();

export default {
  name: 'SmTaskCalendar',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
    dataSource: { type: Array, default: () => [] },
    date: { type: Object, default: null },
  },
  data() {
    return {
      isChecked: false,//是否只查任务
      taskDatasource: [],
    };
  },
  computed: {
    iDatasource() {
      return this.taskDatasource;
    },
    iDate() {
      return moment(this.date).daysInMonth();
    },
    checked() {
      return this.isChecked;
    },

  },
  watch: {
    dataSource: {
      handler: async function (value, oldValue) {
        this.taskDatasource = value;
      },
      immediate: true,
    },
  },
  async created() {
    this.initAxios();
  },
  methods: {
    initAxios() {
    },
  },
  render() {
    let result = [];
    for (let i = 1; i <= this.iDate; i++) {
      let target = this.iDatasource.filter(item => {
        return !this.checked ? (item.state == StateType.Finshed || item.state == StateType.Refused || item.state == StateType.Stop) ?
          moment(item.lastModificationTime).get('date') == i : moment(item.startTime).get('date') <= i && moment(item.endTime).get('date') >= i :
          item.state == StateType.Processing && moment(item.startTime).get('date') <= i && moment(item.endTime).get('date') >= i;
      });
      let time = moment(this.dateTime).set('date', i).format("YYYY-MM-DD dddd");
      if (target.length > 0) {
        result.push(
          <div class="time-content">
            <a-card-grid class="time-date" style="width:20%;text-align:center" hoverable={false}>
              {time}
            </a-card-grid>
            <a-card-grid class="content-item" style="width:80%;text-align:center" hoverable={false}>
              {target.map(item => {
                return <div class="content">
                  <div class="circle" style={{ background: getTaskColor(item.state) }} ></div>
                  <div>{`${item.name} ${getStateType(item.state)}`}</div>
                </div>;
              })
              }
            </a-card-grid>
          </div>,
        );
      }
    }

    return (
      <div class="sm-task-calendar" >
        <a-card
          {...{
            scopedSlots: {
              title: () => {
                let title = (
                  <div class="task-calendar-checkbox">
                    <a-checkbox onChange={(item) => {
                      this.isChecked = item.target.checked;
                      let res=this.dataSource.filter(item=>{
                        return item.state == StateType.Processing;
                      });
                      this.taskDatasource= this.isChecked?res:this.dataSource;
                    }}>
                      只显示有任务数据的日期
                    </a-checkbox>
                  </div>
                );
                return title;
              },
            },
          }}
        >
          {
            result.map(item => {

              return <div><span>{item}</span></div>;
            })}

        </a-card>

      </div>
    );
  },
};