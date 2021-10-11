import SmTaskCalendar from '../sm-task-calendar/SmTaskCalendar';
import { requestIsSuccess, getTaskGroup, getPriorityType, getTaskColor, getStateType } from '../../_utils/utils';
import ApiTask from '../../sm-api/sm-task/Task';
import { TaskGroup, PriorityType, StateType } from '../../_utils/enum';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
let apiTask = new ApiTask();
import moment from 'moment';
import './style';
export default {
  name: 'SmTaskCalendars',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      dataSource: [],//数据源
      isTaskRltDate: false,//是否显示日历和任务
      queryParams: {
        date: moment(),
      },
    };
  },
  computed: {
  },
  watch: {

  },
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiTask = new ApiTask(this.axios);
    },
    async refresh() {
      this.loading = true;
      let date = {
        date: moment(this.queryParams.date).format("YYYY-MM"),
      };
      console.log(date);
      let response = await apiTask.getList(date);
      if (requestIsSuccess(response) && response.data) {
        this.dataSource = response.data.items;
      }
      this.loading = false;
    },
  },
  render() {
    return (
      <div class="sm-task-calendars">
        <a-card
          class="task-calendars-card"
          bordered={false}
          {...{
            scopedSlots: {
              title: () => {
                let result = (
                  <div class="calendar-icons">
                    <div class="calendar-calendar-icon">
                      <a-month-picker
                        // dropdownClassName="month-picker"
                        placeholder="请选择时间"
                        size="small"
                        value={this.queryParams.date}
                        onChange={(val) => {
                          this.queryParams.date = val;
                          this.refresh();
                          console.log(this.queryParams.date);
                        }}

                        onOk="onOk">
                        <span><a-icon
                          type="calendar"
                          class={!this.isTaskRltDate ? "calendar-menu-color" : ''}
                          onClick={() => {
                            this.isTaskRltDate = false;
                          }} /></span>
                      </a-month-picker>
                    </div>
                    <div class="icons">
                      <a-icon
                        type="menu"
                        class={this.isTaskRltDate ? "calendar-menu-color" : ''}
                        onClick={() => {
                          this.isTaskRltDate = true;
                        }} />
                    </div>
                  </div>
                );
                return result;
              },
            },
          }}
        >
          {!this.isTaskRltDate ?
            <a-calendar
              onSelect={(value) => {
                this.queryParams.date = value;
                this.refresh();
              }}
              headerRender={({ value, type, onChange, onTypeChange }) => {
                if (this.queryParams.date) {
                  value = this.queryParams.date;
                }
                onChange(value);
              }}
              {...{
                scopedSlots: {
                  dateCellRender: (value) => {
                    let result = [];
                    let target = this.dataSource.filter(item => {
                      return (item.state == StateType.Finshed || item.state == StateType.Refused || item.state == StateType.Stop) ?
                        moment(item.lastModificationTime).format("YYYY-MM-DD") == value.format("YYYY-MM-DD") : moment(item.startTime).format("YYYY-MM-DD") <= value.format("YYYY-MM-DD") && moment(item.endTime).format("YYYY-MM-DD") >= value.format("YYYY-MM-DD");
                    });
                    target.map(item => {
                      if (item) {
                        result.push((
                          <div class="content">
                            <div class="circle" style={{ background: getTaskColor(item.state) }} ></div>
                            <div>{`${item.name} ${getStateType(item.state)}`}</div>
                          </div>
                        ));
                      }
                    });
                    return result;
                  },
                },
              }}
            /> :
            <SmTaskCalendar
              axios={this.axios}
              dataSource={this.dataSource}
              date={this.queryParams.date}
            />
          }
        </a-card>

      </div >
    );
  },
};