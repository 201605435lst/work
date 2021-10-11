
export default {
  name: 'SmScheduleApprovalTableTwo',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
    };
  },
  computed: {
    
  },
  async created() {
  },
  methods: {
    initAxios() {
    },
  },
  render() {
    return (
      //<div class="sm-schedule-approval-table">
      <div>
        <a-row class="row-one" style="border-top: none;text-align: left;">
          <a-col span="24">
              主要材料/设备
          </a-col>
        </a-row>

        <a-row class="row-one row-two">   
          <a-col span="9" class="col-left">
            材料/设备名称
          </a-col>
          <a-col span="6">
            规格型号
          </a-col>
          <a-col span="5"class="col-center">
            计划总量
          </a-col>
          <a-col span="4">
            单位
          </a-col>
        </a-row>

        <a-row class="row-one row-two">   
          <a-col span="9" class="col-left">
            暂无
          </a-col>
          <a-col span="6">
            暂无
          </a-col>
          <a-col span="5"class="col-center">
            暂无
          </a-col>
          <a-col span="4">
            暂无
          </a-col>
        </a-row>

        <a-row class="row-one row-two">   
          <a-col span="9" class="col-left">
            暂无
          </a-col>
          <a-col span="6">
            暂无
          </a-col>
          <a-col span="5"class="col-center">
            暂无
          </a-col>
          <a-col span="4">
            暂无
          </a-col>
        </a-row>
      </div>
      //</div>
    );
  },
};
    