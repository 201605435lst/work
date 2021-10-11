
export default {
  name: 'SmScheduleApprovalTableFour',
  props: {
    axios: { type: Function, default: null },
    materials: { type: Array, default: () => [] },
  },
  data() {
    return {
      appliances:[], //器具
      mechanics:[], //机械
      securityProducts:[], //安全用品
    };
  },
  computed: {},
  watch: {
    materials: {
      handler: function(value, oldValue) {
        this.appliances = [];
        this.mechanics = [];
        this.securityProducts = [];
        if (value) {
          value.map(item => {
            if(item.type == 2){
              this.appliances.push(item);
            }else if(item.type == 3){
              this.mechanics.push(item);
            }else if(item.type == 4){
              this.securityProducts.push(item);
            }
          });
        }
      },
      
    },
  },
  async created() {},
  methods: {},
  render() {
    return (
      <div>
        <a-row class="row-one" style="border-top: none;text-align: left;">
          <a-col span="24">
              使用器具
          </a-col>
        </a-row>
        
        <a-row class="row-one row-two">   
          <a-col span="9" class="col-left">
            器具名称
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
        {this.appliances.length > 0 ? 
          this.appliances.map(item => {
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

        {/* 使用机械 */}
        <a-row class="row-one" style="border-top: none;text-align: left;">
          <a-col span="24">
              使用机械
          </a-col>
        </a-row>

        <a-row class="row-one row-two">   
          <a-col span="9" class="col-left">
            机械名称
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
        {this.mechanics.length > 0 ? 
          this.mechanics.map(item => {
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

        {/* 安全防护用品 */}
        <a-row class="row-one" style="border-top: none;text-align: left;">
          <a-col span="24">
              安全防护用品
          </a-col>
        </a-row>

        <a-row class="row-one row-two">   
          <a-col span="9" class="col-left">
            名称
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
        {this.securityProducts.length > 0 ? 
          this.securityProducts.map(item => {
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
    );
  },
};
    