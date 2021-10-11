
export default {
  name: 'SmScheduleApprovalTableThree',
  props: {
    axios: { type: Function, default: null },
    subsidiaries: { type: Array, default: () => [] },
  },
  data() {
    return {
      FileNames:'',
    };
  },
  computed: {},
  watch: {
    subsidiaries: {
      handler: function(value, oldValue) {
        this.FileNames = '';
        if (value && value[0] && value[0].approvalRltFiles.length > 0) {
          this.FileNames = value[0].approvalRltFiles.length > 0 ? value[0].approvalRltFiles.map((item,ind) => this.FileNames + item.file.name + item.file.type + (value[0].approvalRltFiles.length - 1 == ind ? "" : "，")) : '';
          this.FileNames = this.FileNames.join('');
          this.FileNames = this.FileNames != '' ? (this.FileNames.indexOf(',') != -1 && this.FileNames.lastIndexOf(',') == this.FileNames.length - 1 ? this.FileNames.substring(0,this.FileNames.lastIndexOf(',')) : this.FileNames) : '';
        }
      },
    },
  },
  async created() {
  },
  methods: {},
  render() {
    return (
      <div>
        <a-row class="row-one row-two">   
          <a-col span="4" class="col-left">
            技术资料
          </a-col>
          <a-col span="20">
            {this.FileNames}
          </a-col>
        </a-row>

        <a-row class="row-one row-two">   
          <a-col span="4" class="col-left">
            临时设施
          </a-col>
          <a-col span="20">
            {this.subsidiaries[0] && this.subsidiaries[0].temporaryEquipment ? this.subsidiaries[0].temporaryEquipment.name : null}
          </a-col>
        </a-row>

        <a-row class="row-one row-two">   
          <a-col span="4" class="col-left">
            安全注意事项
          </a-col>
          <a-col span="20">
            {this.subsidiaries[0] && this.subsidiaries[0].safetyCaution ? this.subsidiaries[0].safetyCaution.name : null}
          </a-col>
        </a-row>

        <a-row class="row-one row-two">   
          <a-col span="4" class="col-left">
            备注
          </a-col>
          <a-col span="20">
            {this.subsidiaries[0] && this.subsidiaries[0].remark}
          </a-col>
        </a-row>
      </div>
    );
  },
};
    