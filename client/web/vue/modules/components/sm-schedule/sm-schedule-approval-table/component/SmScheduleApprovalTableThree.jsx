
export default {
  name: 'SmScheduleApprovalTableThree',
  props: {
    axios: { type: Function, default: null },
    materials: { type: Array, default:() => [] },
  },
  data() {
    return {
      auxiliaryMaterials:[], //辅助材料
    };
  },
  computed: {
  },
  watch: {
    materials: {
      handler: function(value, oldValue) {
        this.auxiliaryMaterials = [];
        if (value) {
          value.map(item => {
            if(item.type == 1){
              this.auxiliaryMaterials.push(item);
            }
          });
        }
      },
    },
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
              辅助材料
          </a-col>
        </a-row>

        <a-row class="row-one row-two">   
          <a-col span="9" class="col-left">
            材料名称
          </a-col>
          <a-col span="6">
            规格型号
          </a-col>
          <a-col span="5"class="col-center">
            数量
          </a-col>
          <a-col span="4">
            单位
          </a-col>
        </a-row>
        {this.auxiliaryMaterials.length > 0 ?
          this.auxiliaryMaterials.map(item => {
            return (
              <a-row class="row-one row-two">   
                <a-col span="9" class="col-left">
                  {item.materialName}
                </a-col>
                <a-col span="6">
                  {item.specModel}
                </a-col>
                <a-col span="5"class="col-center">
                  {item.number}
                </a-col>
                <a-col span="4">
                  {item.unit}
                </a-col>
              </a-row>
            );
          })
          : undefined}
        

      </div>
      //</div>
    );
  },
};
    